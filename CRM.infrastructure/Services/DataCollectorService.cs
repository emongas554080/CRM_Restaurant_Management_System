using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRM.domain.Entities;
using CRM.domain.Dtos;
using CRM.infrastructure.Data;

namespace CRM.infrastructure.Services
{
    public class DataCollectorService : IDataCollectorService
    {
        private readonly IDbConnectionResolver _resolver;
        private readonly MasterDbContext _masterDb;

        public DataCollectorService(
            IDbConnectionResolver resolver,
            MasterDbContext masterDb)
        {
            _resolver = resolver;
            _masterDb = masterDb;
        }

        // CREATE
        public async Task<Customer> CollectAsync(
            string tenantCode,
            CustomerDto data)
        {
            using var tenantDb = _resolver.Resolve(tenantCode);

            var customer = new Customer
            {
                FullName = data.FullName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Address = data.Address,
                CustomerType = data.CustomerType,
                FavoriteFood = data.FavoriteFood,
                VisitFrequency = data.VisitFrequency,
                Feedback = data.Feedback,
                DateCollected = DateTime.Now,
                IsArchived = false
            };

            tenantDb.Customers.Add(customer);

            await tenantDb.SaveChangesAsync();

            return customer;
        }

        // READ
        public async Task<List<Customer>> GetCollectedAsync(
            string tenantCode)
        {
            using var tenantDb = _resolver.Resolve(tenantCode);

            return await tenantDb.Customers
                .Where(c => !c.IsArchived)
                .OrderByDescending(c => c.DateCollected)
                .ToListAsync();
        }

        // UPDATE
        public async Task<Customer?> UpdateAsync(
            string tenantCode,
            int id,
            CustomerDto data)
        {
            using var tenantDb = _resolver.Resolve(tenantCode);

            var customer = await tenantDb.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return null;

            customer.FullName = data.FullName;
            customer.Email = data.Email;
            customer.PhoneNumber = data.PhoneNumber;
            customer.Address = data.Address;
            customer.CustomerType = data.CustomerType;
            customer.FavoriteFood = data.FavoriteFood;
            customer.VisitFrequency = data.VisitFrequency;
            customer.Feedback = data.Feedback;

            await tenantDb.SaveChangesAsync();

            return customer;
        }

        // DELETE = ARCHIVE
        public async Task<bool> ArchiveAsync(
            string tenantCode,
            int id)
        {
            using var tenantDb = _resolver.Resolve(tenantCode);

            var customer = await tenantDb.Customers
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return false;

            customer.IsArchived = true;
            customer.DateArchived = DateTime.Now;

            await tenantDb.SaveChangesAsync();

            return true;
        }

        // MASTER VIEW
        public async Task<List<TenantCustomersDto>> GetAllCollectedAsync()
        {
            var tenants = await _masterDb.Tenants
                .OrderBy(t => t.TenantCode)
                .ToListAsync();

            var result = new List<TenantCustomersDto>();

            foreach (var tenant in tenants)
            {
                var customers = await GetCollectedAsync(
                    tenant.TenantCode);

                result.Add(new TenantCustomersDto
                {
                    TenantCode = tenant.TenantCode,
                    TenantName = tenant.TenantName,
                    Customers = customers
                });
            }

            return result;
        }
    }
}