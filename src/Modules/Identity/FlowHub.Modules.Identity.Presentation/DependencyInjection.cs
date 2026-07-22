using FlowHub.Modules.Identity.Presentation.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace FlowHub.Modules.Identity.Presentation
{
    public static class DependencyInjection
    {
        public static WebApplication AddIdentityPresentation(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
