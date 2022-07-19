using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IOrderRepository Order { get; }
        IProductRepository Product { get; }
        IWarehouseRepository Warehouse { get; }
        IFeedbackRepository Feedback { get; }
        IDeliveryPointRepository DeliveryPoint { get; }

        void Save();
        Task SaveAsync();

    }
}
