using System.Collections.Generic;
using CRM.domain.Entities;

namespace CRM.domain.Dtos
{
    public class TenantCustomersDto
    {
        public string TenantCode { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public List<Customer> Customers { get; set; } = new();
    }
}