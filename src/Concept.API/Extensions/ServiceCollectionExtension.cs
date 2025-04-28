using Concept.Core.Interfaces;

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
    }
}
