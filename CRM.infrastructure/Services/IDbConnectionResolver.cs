using CRM.infrastructure.Data;

namespace CRM.infrastructure.Services
{
    public interface IDbConnectionResolver
    {
        TenantDbContext Resolve(string tenantCode);
        bool TenantExists(string tenantCode);
    }
}