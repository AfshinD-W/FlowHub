using FlowHub.Modules.Identity.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlowHub.Modules.Identity.Infrastructure.Database
{
    public class IdentityDbContext : IdentityDbContext<User, Role, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
    }
}
