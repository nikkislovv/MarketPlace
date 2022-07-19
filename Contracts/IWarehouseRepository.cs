using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IWarehouseRepository
    {
        void CreateWarehouse(Warehouse order);
        void DeleteWarehouse(Warehouse order);
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges);
        void UpdateWarehouse(Warehouse order);
        Task<Warehouse> GetWarehouseByIdAsync(Guid id, bool trackChanges);
    }
}
