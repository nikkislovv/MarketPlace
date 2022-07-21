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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateProduct(Product product)
        {
            Create(product);
        }
        public void DeleteProduct(Product product)
        {
            Delete(product);
        }
        public async Task<Product> GetProductByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
        public async Task<PagedList<Product>> GetAllProductsAsync(ProductParameters productParameters,bool trackChanges)
        {
            var _products = await FindAll(trackChanges)
                .Skip((productParameters.PageNumber - 1) * productParameters.PageSize)
                .Take(productParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(false).CountAsync();
            return new PagedList<Product>(_products, count, productParameters.PageNumber, productParameters.PageSize);
        }
        //need to change on searching
        //public async Task<IEnumerable<Product>> GetProductsByAccountAsync(string userId, bool trackChanges)
        //{
        //    return await FindByCondition(e => e.UserId.Equals(userId), trackChanges).ToListAsync();
        //}
        //public async Task<IEnumerable<Product>> GetProductsByWarehouseAsync(Guid WarehouseId, bool trackChanges)
        //{
        //    return await FindByCondition(e => e.WarehouseId.Equals(WarehouseId), trackChanges).ToListAsync();
        //}
        public void UpdateProduct(Product product)
        {
            Update(product);
        }
    }
}
