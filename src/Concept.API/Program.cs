using Concept.API.Extensions;
using Concept.API.Middlewares;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure;
using Concept.Infrastructure.Data;
using Concept.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shared;
using Shared.Configs;
using Shared.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogConfigure();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddOptions<EmailServiceConfig>()
    .Bind(builder.Configuration.GetSection(EmailServiceConfig.SectionName));

builder.Services.AddOptions<TokenServiceConfig>()
    .Bind(builder.Configuration.GetSection(TokenServiceConfig.SectionName));

// TODO：Optimize 應實現自動註冊
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// 添加基於權限的授權
builder.Services.AddPermissionBasedAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("TokenService:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("TokenService:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("TokenService:SecretKey").Value!)),
            ClockSkew = TimeSpan.Zero // 這個設置可以避免 Token 時區問題導致的提前過期
        };
        
        // 添加事件處理程序，以便能夠記錄驗證過程中的錯誤
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"驗證失敗: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token 驗證成功");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"權限挑戰: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddCoreServices();

// 添加健康檢查服務
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 映射健康檢查端點 (放在app.UseAuthorization()之後或app.MapControllers()附近)
app.MapHealthChecks("/health");

// 記錄應用程式啟動訊息
Log.Information("Starting up {Application} in {Environment} environment",
    builder.Environment.ApplicationName, builder.Environment.EnvironmentName);

await app.RunAsync();

// 應用程式關閉紀錄
Log.Information("Shutting down {Application}", builder.Environment.ApplicationName);
await Log.CloseAndFlushAsync();