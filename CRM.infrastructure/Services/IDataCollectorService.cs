using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.domain.Entities;
using CRM.domain.Dtos;

namespace CRM.infrastructure.Services
{
    public interface IDataCollectorService
    {
        Task<Customer> CollectAsync(
            string tenantCode,
            CustomerDto data);

        Task<List<Customer>> GetCollectedAsync(
            string tenantCode);

        Task<List<TenantCustomersDto>> GetAllCollectedAsync();

        Task<Customer?> UpdateAsync(
            string tenantCode,
            int id,
            CustomerDto data);

        Task<bool> ArchiveAsync(
            string tenantCode,
            int id);
    }
}