using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Contracts
{
    public interface IProductManager
    {
        void ClearProductsInCollection(Order order);
        Task<bool> AddProductCollection(Order order, IEnumerable<Guid> productsIds);
        Task<bool> CheckForAvailability(IEnumerable<Guid> productsIds);
        Task<bool> ControlOfQuantity(IEnumerable<Product> productsIds);

    }
}
