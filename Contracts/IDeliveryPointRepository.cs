using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDeliveryPointRepository
    {
        void CreateDeliveryPoint(DeliveryPoint order);
        void DeleteDeliveryPoint(DeliveryPoint order);
        Task<IEnumerable<DeliveryPoint>> GetAllDeliveryPointsAsync( bool trackChanges);
        void UpdateDeliveryPoint(DeliveryPoint order);
        Task<DeliveryPoint> GetDeliveryPointByIdAsync(Guid id, bool trackChanges);

    }
}
