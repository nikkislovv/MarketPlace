using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
        Task<PagedList<Order>> GetAllOrdersAsync(OrderParameters orderParameters, bool trackChanges);
        void UpdateOrder(Order order);
        Task<Order> GetOrderByIdAsync(Guid id, bool trackChanges);

    }
}
