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
    public class DeliveryPointRepository : RepositoryBase<DeliveryPoint>, IDeliveryPointRepository
    {
        public DeliveryPointRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void CreateDeliveryPoint(DeliveryPoint deliveryPoint)
        {
            Create(deliveryPoint);
        }
        public void DeleteDeliveryPoint(DeliveryPoint deliveryPoint)
        {
            Delete(deliveryPoint);
        }
        public async Task<DeliveryPoint> GetDeliveryPointByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<DeliveryPoint>> GetAllDeliveryPointsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }
        
        public void UpdateDeliveryPoint(DeliveryPoint deliveryPoint)
        {
            Update(deliveryPoint);
        }
    }
}
