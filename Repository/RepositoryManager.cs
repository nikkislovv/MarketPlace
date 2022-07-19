using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        IWarehouseRepository _warehouseRepository;
        IDeliveryPointRepository _deliveryPointRepository;
        IFeedbackRepository _feedbackRepository;
        RepositoryContext _context;
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }
        public IOrderRepository Order
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(_context);
                return _orderRepository;
            }
        }
        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }
        public IWarehouseRepository Warehouse
        {
            get
            {
                if (_warehouseRepository == null)
                    _warehouseRepository = new WarehouseRepository(_context);
                return _warehouseRepository;
            }
        }
        public IDeliveryPointRepository DeliveryPoint
        {
            get
            {
                if (_deliveryPointRepository == null)
                    _deliveryPointRepository = new DeliveryPointRepository(_context);
                return _deliveryPointRepository;
            }
        }
        public IFeedbackRepository Feedback
        {
            get
            {
                if (_feedbackRepository == null)
                    _feedbackRepository = new FeedbackRepository(_context);
                return _feedbackRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
