using ApiApplication.Contracts;
using Contracts;
using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using System;
using System.Threading.Tasks;

namespace ProductService
{
    public class ProductManager : IProductManager
    {
        IRepositoryManager _repository;
        public ProductManager(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public void ClearProductsInCollection(Order order)
        {
            order.Products.Clear();
        }
        public async Task<bool> AddProductCollection(Order order, OrderToHandleDto orderDto)
        {
            foreach (Guid item in orderDto.ProductsIds)
            {
                var product = await _repository.Product.GetProductByIdAsync(item, true);
                if (product != null)
                {
                    order.Products.Add(product);
                }
            }
            return order.Products.Count > 0;
        }
    }
}
