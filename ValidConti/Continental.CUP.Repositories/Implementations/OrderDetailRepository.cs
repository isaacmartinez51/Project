using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.Interfaces;
using Continental.CUP.Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Continental.CUP.Repositories.Implementations
{
    public class OrderDetailRepository : GenericRepository<ApplicationDbContext, OrderDetailEModel>, IOrderDetailRepository
    {
        public bool Exist(Expression<Func<OrderDetailEModel, bool>> expression) => ReadsItems(expression).Count() > 0;


        public IQueryable<OrderDetailVModel> GetQueryOrderDetail()
        {
            return ReadsItems().Select(x => new OrderDetailVModel
            {
                OrderID = x.OrderID,
                OrderDetailID = x.OrderDetailID,
                embarque = x.embarque,
                partida = x.partida,
                total_pallets = x.total_pallets,
                continentalpartnumber = x.continentalpartnumber,
                customerpartnumber = x.customerpartnumber,
                cantidad = x.cantidad,
                delivery = x.delivery,
                traza = x.traza,
                shipment = x.shipment,
                notas = x.notas,
                Leido = x.Leido

            });
        }

        public IQueryable<OrderDetailVModel> GetQueryOrderDetail(int id)
        {
            return ReadsItems(x => x.OrderID == id).Select(x => new OrderDetailVModel
            {
                OrderID = x.OrderID,
                OrderDetailID = x.OrderDetailID,
                embarque = x.embarque,
                partida = x.partida,
                total_pallets = x.total_pallets,
                continentalpartnumber = x.continentalpartnumber,
                customerpartnumber = x.customerpartnumber,
                cantidad = x.cantidad,
                delivery = x.delivery,
                traza = x.traza,
                shipment = x.shipment,
                notas = x.notas,
                Leido = x.Leido
            });
        }
        public OrderDetailEModel GetQueryOrderDetail(int orderId, string partNumber)
        {
            OrderDetailEModel model = ReadsItems(x => x.OrderID == orderId && x.continentalpartnumber == partNumber && x.Leido == false).FirstOrDefault();
            return model;
            //return 
        }
    }
}
