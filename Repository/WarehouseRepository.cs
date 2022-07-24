using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateWarehouse(Warehouse deliveryPoint)
        {
            Create(deliveryPoint);
        }
        public void DeleteWarehouse(Warehouse deliveryPoint)
        {
            Delete(deliveryPoint);
        }
        public async Task<Warehouse> GetWarehouseByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
        public async Task<PagedList<Warehouse>> GetAllWarehousesAsync(WarehouseParameters warehouseParameters,bool trackChanges)
        {
            var warehouses = await FindAll(trackChanges)
               .OrderBy(e => e.Address)//sorting
               .Skip((warehouseParameters.PageNumber - 1) * warehouseParameters.PageSize)//paging
               .Take(warehouseParameters.PageSize)
               .ToListAsync();
            var count = await FindAll(false).CountAsync();
            return new PagedList<Warehouse>(warehouses, count, warehouseParameters.PageNumber, warehouseParameters.PageSize);
        }

        public void UpdateWarehouse(Warehouse deliveryPoint)
        {
            Update(deliveryPoint);
        }
    }
}
