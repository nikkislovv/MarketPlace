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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateProductForWarehouse(Guid warehouseId,Product product)//
        {
            product.WarehouseId = warehouseId;
            Create(product);
        }
        public void DeleteProduct(Product product)
        {
            Delete(product);
        }
        public async Task<Product> GetProductByIdAsync(Guid id, bool trackChanges)//
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges)//
        {
            return await FindAll(trackChanges).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByAccountAsync(string userId, bool trackChanges)//
        {
            return await FindByCondition(e => e.UserId.Equals(userId), trackChanges).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByWarehouseAsync(Guid WarehouseId, bool trackChanges)
        {
            return await FindByCondition(e => e.WarehouseId.Equals(WarehouseId), trackChanges).ToListAsync();
        }
        public void UpdateProduct(Product product)
        {
            Update(product);
        }
    }
}
