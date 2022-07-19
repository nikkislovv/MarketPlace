using Contracts;
using Entities.Models;
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
        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

        public void UpdateWarehouse(Warehouse deliveryPoint)
        {
            Update(deliveryPoint);
        }
    }
}
