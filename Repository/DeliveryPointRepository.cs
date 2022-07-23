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
        public async Task<PagedList<DeliveryPoint>> GetAllDeliveryPointsAsync(DeliveryPointParameters deliveryPointParameters,bool trackChanges)
        {
            var deliveryPoints = await FindAll(trackChanges)
               .OrderBy(e => e.Address)//sorting
               .Skip((deliveryPointParameters.PageNumber - 1) * deliveryPointParameters.PageSize)//paging
               .Take(deliveryPointParameters.PageSize)
               .ToListAsync();
            var count = await FindAll(false).CountAsync();
            return new PagedList<DeliveryPoint>(deliveryPoints, count, deliveryPointParameters.PageNumber, deliveryPointParameters.PageSize);
        }
        
        public void UpdateDeliveryPoint(DeliveryPoint deliveryPoint)
        {
            Update(deliveryPoint);
        }
    }
}
