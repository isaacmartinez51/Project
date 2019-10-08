using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.Interfaces;
using Continental.CUP.Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidConti.Business
{
    public class BusinessPlatform
    {
        private ApplicationDbContext _context;
        private IOrderRepository _contextOrder;

        public BusinessPlatform(ApplicationDbContext context, IOrderRepository contextOrder)
        {
            _context = context;
            _contextOrder = contextOrder;

        }
        public List<ReaderEModel> GetPlatforms()
        {
            return _context.Reader.ToList();
        }

        public OrderLigthVModel GetOrderByIdOrder(int id) {


            return _contextOrder.GetOrderById(id);
        }

        public async Task<OrderEModel> PutPlatform(OrderLigthVModel item)
        {
            OrderVModel orderV = _contextOrder.GetOrderByOrderId(item.OrderID);
            string spath = @"c:\temp\" + orderV.Portal + "\\" + orderV.ShipmentNumber + ".xml";
            if (System.IO.File.Exists(spath))
            {
                System.IO.File.Delete(spath);
            }
            OrderEModel order = _context.Order.FirstOrDefault(x => x.OrderID == item.OrderID);
            order.OnShipment = true;
            order.ReaderID = item.ReaderID;
            var succes = await _contextOrder.UpdateItemAsync(order);

            return succes;
        }
    }
}
