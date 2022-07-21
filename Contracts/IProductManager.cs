using Entities.DataTransferObjects.OrderDTO;
using Entities.Models;
using System.Threading.Tasks;

namespace ApiApplication.Contracts
{
    public interface IProductManager
    {
        void ClearProductsInCollection(Order order);
        Task<bool> AddProductCollection(Order order, OrderToHandleDto orderDto);
    }
}
