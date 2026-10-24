using Microsoft.EntityFrameworkCore;
using CRM.domain.Entities;

namespace CRM.infrastructure.Data
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
    }
}