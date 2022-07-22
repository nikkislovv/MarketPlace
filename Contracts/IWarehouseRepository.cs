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
        void CreateWarehouse(Warehouse warehouse);
        void DeleteWarehouse(Warehouse warehouse);
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges);
        void UpdateWarehouse(Warehouse warehouse);
        Task<Warehouse> GetWarehouseByIdAsync(Guid id, bool trackChanges);
    }
}
