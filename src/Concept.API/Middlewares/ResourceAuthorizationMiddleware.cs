namespace Concept.API.Middlewares
{
    public class ResourceAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public ResourceAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // TODO：實現資源授權邏輯
        }
    }
}
