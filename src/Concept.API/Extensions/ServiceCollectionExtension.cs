using Concept.Core.Interfaces;
using Serilog;

namespace Concept.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// --------------------------
        /// 實現自動註冊自己撰寫的服務
        /// --------------------------
        /// 
        /// 此實現的邏輯為，自己寫的Interface要繼承IService介面
        /// </summary>
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // 取得所有已載入的組件
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var baseServices = assembly.GetTypes()
                    .Where(type => typeof(IBaseService).IsAssignableFrom(type))
                    .Where(type => type != typeof(IBaseService))
                    .Where(type => type.IsInterface);

                foreach (Type baseService in baseServices)
                {
                    var implementType = Array.Find(assembly.GetTypes(), type => baseService.IsAssignableFrom(type) && !type.IsInterface);
                    if (implementType == null)
                    {
                        throw new NotImplementedException($"找不到 {baseService.Name} 的實作");
                    }

                    services.AddScoped(baseService, implementType);
                }
            }

            return services;
        }


        public static void AddSerilogConfigure(this IHostBuilder host)
        {
            host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .MinimumLevel.Debug() // 設定預設的日誌級別為 Debug
                                          //.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // 覆蓋 Microsoft 的日誌級別為 Warning
                                          //.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning) // 覆蓋 System 的日誌級別為 Warning
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", hostingContext.HostingEnvironment.ApplicationName) // 添加全域性的屬性 Application
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)
                    .WriteTo.Console();
            });
        }
    }
}
