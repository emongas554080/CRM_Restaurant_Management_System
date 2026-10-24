using Microsoft.EntityFrameworkCore;
using CRM.domain.Entities;

namespace CRM.infrastructure.Data
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }

        public DbSet<TenantInfo> Tenants => Set<TenantInfo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantInfo>().HasData(
                new TenantInfo { Id = 1, TenantCode = "A", TenantName = "Tenant A" },
                new TenantInfo { Id = 2, TenantCode = "B", TenantName = "Tenant B" },
                new TenantInfo { Id = 3, TenantCode = "C", TenantName = "Tenant C" }
            );
        }
    }
}