using ApiApplication.Contracts;
using Contracts;
using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
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
        public async Task<bool> AddProductCollection(Order order, IEnumerable<Guid> productsIds)
        {
            foreach (Guid item in productsIds)
            {
                var product = await _repository.Product.GetProductByIdAsync(item, true);
                if (product != null)
                {
                    order.Products.Add(product);
                }
            }
            return order.Products.Count > 0;
        }
        public async Task<bool> CheckForAvailability(IEnumerable<Guid> productsIds)
        {
            foreach (Guid item in productsIds)
            {
                var product = await _repository.Product.GetProductByIdAsync(item,true);
                if (product ==null||product.Quantity==0)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> ControlOfQuantity(IEnumerable<Product> products)
        {
            foreach (Product item in products)
            {
                item.Quantity--;
                _repository.Product.UpdateProduct(item);
            }
            await _repository.SaveAsync();
            return true;
        }

        public bool CheckProductInOrders(IEnumerable<Order> orders,string userId)
        {
            foreach (Order item in orders)
            {
                if (item.UserId==userId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
