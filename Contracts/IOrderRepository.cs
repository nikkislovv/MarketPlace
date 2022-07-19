using Entities.Models;
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
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges);
        Task<IEnumerable<Order>> GetOrdersByAccountAsync(Guid userId,bool trackChanges);//получение истории заказов определенного аккаунта
        void UpdateOrder(Order order);
        Task<Order> GetOrderByIdAsync(Guid id, bool trackChanges);

    }
}
