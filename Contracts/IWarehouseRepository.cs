using Entities.Models;
using Entities.RequestFeatures;
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
        Task<PagedList<Warehouse>> GetAllWarehousesAsync(WarehouseParameters warehouseParameters, bool trackChanges);
        void UpdateWarehouse(Warehouse warehouse);
        Task<Warehouse> GetWarehouseByIdAsync(Guid id, bool trackChanges);
    }
}
