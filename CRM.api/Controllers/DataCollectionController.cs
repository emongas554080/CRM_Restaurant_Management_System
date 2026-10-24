using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.domain.Dtos;
using CRM.infrastructure.Data;
using CRM.infrastructure.Services;

namespace CRM.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataCollectionController : ControllerBase
    {
        private readonly IDataCollectorService _collector;
        private readonly IDbConnectionResolver _resolver;
        private readonly MasterDbContext _masterDb;

        public DataCollectionController(
            IDataCollectorService collector,
            IDbConnectionResolver resolver,
            MasterDbContext masterDb)
        {
            _collector = collector;
            _resolver = resolver;
            _masterDb = masterDb;
        }

        // READ TENANTS
        [HttpGet("tenants")]
        public async Task<IActionResult> GetTenants()
        {
            var tenants = await _masterDb.Tenants
                .OrderBy(t => t.TenantCode)
                .ToListAsync();

            return Ok(tenants);
        }

        // CREATE / REGISTER
        [HttpPost("{tenantCode}/customers")]
        public async Task<IActionResult> Collect(
            string tenantCode,
            [FromBody] CustomerDto data)
        {
            if (!_resolver.TenantExists(tenantCode))
            {
                return NotFound(
                    $"Tenant '{tenantCode}' does not exist. Valid tenants: A, B, C.");
            }

            if (string.IsNullOrWhiteSpace(data.FullName))
            {
                return BadRequest("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(data.Email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(data.PhoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(data.Address))
            {
                return BadRequest("Address is required.");
            }

            if (string.IsNullOrWhiteSpace(data.CustomerType))
            {
                return BadRequest("Customer type is required.");
            }

            if (string.IsNullOrWhiteSpace(data.VisitFrequency))
            {
                return BadRequest("Visit frequency is required.");
            }

            var saved = await _collector.CollectAsync(
                tenantCode,
                data);

            return Ok(saved);
        }

        // READ
        [HttpGet("{tenantCode}/customers")]
        public async Task<IActionResult> GetByTenant(
            string tenantCode)
        {
            if (!_resolver.TenantExists(tenantCode))
            {
                return NotFound(
                    $"Tenant '{tenantCode}' does not exist.");
            }

            var customers = await _collector.GetCollectedAsync(
                tenantCode);

            return Ok(customers);
        }

        // UPDATE
        [HttpPut("{tenantCode}/customers/{id}")]
        public async Task<IActionResult> Update(
            string tenantCode,
            int id,
            [FromBody] CustomerDto data)
        {
            if (!_resolver.TenantExists(tenantCode))
            {
                return NotFound(
                    $"Tenant '{tenantCode}' does not exist.");
            }

            if (string.IsNullOrWhiteSpace(data.FullName))
            {
                return BadRequest("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(data.Email))
            {
                return BadRequest("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(data.PhoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(data.Address))
            {
                return BadRequest("Address is required.");
            }

            if (string.IsNullOrWhiteSpace(data.CustomerType))
            {
                return BadRequest("Customer type is required.");
            }

            if (string.IsNullOrWhiteSpace(data.VisitFrequency))
            {
                return BadRequest("Visit frequency is required.");
            }

            var updated = await _collector.UpdateAsync(
                tenantCode,
                id,
                data);

            if (updated == null)
            {
                return NotFound(
                    $"Customer with ID {id} was not found.");
            }

            return Ok(updated);
        }

        // DELETE = ARCHIVE
        [HttpDelete("{tenantCode}/customers/{id}")]
        public async Task<IActionResult> Archive(
            string tenantCode,
            int id)
        {
            if (!_resolver.TenantExists(tenantCode))
            {
                return NotFound(
                    $"Tenant '{tenantCode}' does not exist.");
            }

            var archived = await _collector.ArchiveAsync(
                tenantCode,
                id);

            if (!archived)
            {
                return NotFound(
                    $"Customer with ID {id} was not found.");
            }

            return Ok(new
            {
                message = "Customer archived successfully."
            });
        }

        // MASTER VIEW
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _collector.GetAllCollectedAsync();

            return Ok(all);
        }
    }
}