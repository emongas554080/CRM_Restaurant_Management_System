using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CRM.infrastructure.Data;

namespace CRM.infrastructure.Services
{
    public class DbConnectionResolver : IDbConnectionResolver
    {
        private static readonly string[] ValidTenants = { "A", "B", "C" };

        public bool TenantExists(string tenantCode) =>
            ValidTenants.Contains((tenantCode ?? string.Empty).ToUpper());

        public TenantDbContext Resolve(string tenantCode)
        {
            var code = (tenantCode ?? string.Empty).ToUpper();

            if (!TenantExists(code))
            {
                throw new ArgumentException(
                    $"Unknown tenant '{tenantCode}'. Valid tenants: A, B, C."
                );
            }

            var options = new DbContextOptionsBuilder<TenantDbContext>()
                .UseSqlServer(
                    $@"Server=(localdb)\MSSQLLocalDB;Database=CRM_RESTAURANT_Tenant_{code};Trusted_Connection=True;TrustServerCertificate=True;"
                )
                .Options;

            var context = new TenantDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}