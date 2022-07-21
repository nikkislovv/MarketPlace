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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateOrder(Order order)
        {
            Create(order);
        }
        public void DeleteOrder(Order order)
        {
            Delete(order);
        }
        public async Task<Order> GetOrderByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();         
        }
        //public async Task<IEnumerable<Order>> GetOrdersByAccountAsync(Guid userId, bool trackChanges)
        //{
        //    return await FindByCondition(e => e.UserId.Equals(userId), trackChanges).ToListAsync();
        //}
        public void UpdateOrder(Order order)
        {
            Update(order);
        }
    }
}
