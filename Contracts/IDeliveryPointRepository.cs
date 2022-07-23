using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDeliveryPointRepository
    {
        void CreateDeliveryPoint(DeliveryPoint deliveryPoint);
        void DeleteDeliveryPoint(DeliveryPoint deliveryPoint);
        Task<PagedList<DeliveryPoint>> GetAllDeliveryPointsAsync(DeliveryPointParameters deliveryPointParameters, bool trackChanges);
        void UpdateDeliveryPoint(DeliveryPoint deliveryPoint);
        Task<DeliveryPoint> GetDeliveryPointByIdAsync(Guid id, bool trackChanges);

    }
}
