using ApiApplication.Repository.RepositoryPhoneExtensions;
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
        public async Task<PagedList<Order>> GetAllOrdersAsync(OrderParameters orderParameters, bool trackChanges)
        {
            var orders = await FindAll(trackChanges)
               .Search(orderParameters.SearchByField, orderParameters.SearchTerm)//searching
               .OrderBy(e => e.FullName)//sorting
               .Skip((orderParameters.PageNumber - 1) * orderParameters.PageSize)//paging
               .Take(orderParameters.PageSize)
               .ToListAsync();
            var count = await FindAll(true)
                .Search(orderParameters.SearchByField, orderParameters.SearchTerm)//searching
                .CountAsync();
            return new PagedList<Order>(orders, count, orderParameters.PageNumber, orderParameters.PageSize);
        }
        public void UpdateOrder(Order order)
        {
            Update(order);
        }
    }
}
