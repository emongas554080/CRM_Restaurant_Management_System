namespace CRM.domain.Entities
{
    public class TenantInfo
    {
        public int Id { get; set; }
        public string TenantCode { get; set; } = string.Empty; // "A", "B", "C"
        public string TenantName { get; set; } = string.Empty; // "Tenant A"
    }
}