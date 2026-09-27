using FlowHub.Modules.Identity.Application.Interfaces;
using FlowHub.Modules.Identity.Infrastructure.Configuration;
using FlowHub.Modules.Identity.Infrastructure.Database;
using FlowHub.Modules.Identity.Infrastructure.Identity.Entities;
using FlowHub.Modules.Identity.Infrastructure.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowHub.Modules.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            //DataBase
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<IdentityDbContext>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
