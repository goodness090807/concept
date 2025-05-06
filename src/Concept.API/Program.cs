using Concept.API.Authorizations.ResourceAccess;
using Concept.API.Extensions;
using Concept.Core.Interfaces;
using Concept.Core.Interfaces.Repositories;
using Concept.Infrastructure;
using Concept.Infrastructure.Data;
using Concept.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Configs;
using Shared.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
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
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceAccessHandler>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ResourceAccessPolicyProvider>();

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
        };
    }
);

builder.Services.AddCoreServices();

// 添加健康檢查服務
builder.Services.AddHealthChecks();

var app = builder.Build();

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

await app.RunAsync();
