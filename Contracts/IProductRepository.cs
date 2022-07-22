using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository
    {
        void CreateProduct(Product product);//создается товар 
        void DeleteProduct(Product product);//может только кто создавал карточку товара

        Task<PagedList<Product>> GetAllProductsAsync(ProductParameters productParameters,bool trackChanges);
        //Task<IEnumerable<Product>> GetProductsByAccountAsync(string userId, bool trackChanges);//просмотр всех товаров одного продавца
        //Task<IEnumerable<Product>> GetProductsByWarehouseAsync(Guid userId, bool trackChanges);//просмотр всех товаров определенного склада
        void UpdateProduct(Product product);//может только кто создавал карточку товара
        Task<Product> GetProductByIdAsync(Guid id, bool trackChanges);
    }
}
