using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository
    {
        void CreateProduct(Product order);
        void DeleteProduct(Product order);//может только кто создавал карточку товара
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        Task<IEnumerable<Product>> GetProductsByAccountAsync(Guid userId, bool trackChanges);//просмотр всех товаров одного продавца
        Task<IEnumerable<Product>> GetProductsByWarehouseAsync(Guid userId, bool trackChanges);//просмотр всех товаров определенного склада
        void UpdateProduct(Product order);//может только кто создавал карточку товара
        Task<Product> GetProductByIdAsync(Guid id, bool trackChanges);
    }
}
