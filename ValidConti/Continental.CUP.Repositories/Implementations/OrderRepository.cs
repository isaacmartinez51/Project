using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.Interfaces;
using Continental.CUP.Repositories.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Continental.CUP.Repositories.Classes.Exceptios;

namespace Continental.CUP.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<ApplicationDbContext, OrderEModel>, IOrderRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVModel"></param>
        /// <param name="orderDetailVModel"></param>
        /// <returns></returns>
        public async Task<OrderVModel> CreateShipment(OrderVModel order, List<OrderDetailVModel> orderDetailVModel)
        {
            try
            {
                OrderEModel orderModel = new OrderEModel();
                OrderDetailEModel orderDetail = new OrderDetailEModel();
                List<OrderDetailVModel> orderDetailList = new List<OrderDetailVModel>();


                //Exist(x => x.ShipmentNumber == order.ShipmentNumber);
                orderModel = GetItemByExpression(x => x.ShipmentNumber == order.ShipmentNumber);

                var lastItemOrder = Context.Order.LastOrDefault();

                using (var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
                {
                    int idOrder = lastItemOrder == null ? 1 : lastItemOrder.OrderID + 1;
                    if (null == orderModel)
                    {
                        orderModel = new OrderEModel()
                        {
                            OrderID = idOrder,
                            Date = DateTime.Now,
                            OnShipment = true,
                            ReaderID = null,
                            ShipmentNumber = order.ShipmentNumber
                        };

                        Context.Order.Add(orderModel);
                        Context.SaveChanges();
                        order.OrderID = orderModel.OrderID;
                        order.Date = orderModel.Date;
                        order.OnShipment = orderModel.OnShipment;
                    }


                    int pallet = 1;
                    //TODO: Crear los detalles del EMBARQUE
                    foreach (var item in orderDetailVModel)
                    {
                        for (int i = 0; i < item.total_pallets; i++)
                        {
                            var lastItemOrderDetail = Context.OrderDetail.LastOrDefault();
                            int idOrderDetail = lastItemOrderDetail == null ? 1 : lastItemOrderDetail.OrderDetailID + 1;
                            orderDetail = new OrderDetailEModel()
                            {
                                OrderDetailID = idOrderDetail,
                                OrderID = orderModel.OrderID,
                                embarque = item.embarque,
                                partida = item.partida,
                                total_pallets = pallet,
                                continentalpartnumber = item.continentalpartnumber,
                                customerpartnumber = item.customerpartnumber,
                                cantidad = item.cantidad,
                                delivery = item.delivery,
                                traza = item.traza,
                                shipment = item.shipment,
                                notas = item.shipment,
                                Leido = false
                            };
                            pallet++;
                            Context.OrderDetail.Add(orderDetail);
                            Context.SaveChanges();
                            var uno = JsonConvert.DeserializeObject<OrderDetailVModel>(JsonConvert.SerializeObject(orderDetail).ToString());
                            orderDetailList.Add(uno);
                        }
                    }
                    Context.SaveChanges();
                    transaction.Commit();
                }
                order.ListOrderDetail = orderDetailList;
                return order;
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
            }
        }

        public bool Exist(Expression<Func<OrderEModel, bool>> expression) => ReadsItems(expression).Count() > 0;


        public IQueryable<OrderVModel> GetQueryOrder()
        {
            return ReadsItems().Select(x => new OrderVModel
            {
                OrderID = x.OrderID,
                ReaderID = x.ReaderID,
                Number = x.Number,
                ShipmentNumber = x.ShipmentNumber,
                OnShipment = x.OnShipment,
                Date = x.Date,
                Finished = x.Finished
            });
        }

        public IQueryable<OrderVModel> GetQueryOrder(int id)
        {
            return ReadsItems(x => x.OrderID == id).Select(x => new OrderVModel
            {
                OrderID = x.OrderID,
                ReaderID = x.ReaderID,
                Number = x.Number,
                ShipmentNumber = x.ShipmentNumber,
                OnShipment = x.OnShipment,
                Date = x.Date,
                Finished = x.Finished
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrderVModel GetQueryOrderComplete(int id)
        {
            try
            {
                List<OrderDetailVModel> query = Context.OrderDetail
                       .Where(x => x.OrderID == id).Select(x => new OrderDetailVModel
                       {
                           OrderID = x.OrderID,
                           partida = x.partida,
                           total_pallets = x.total_pallets,
                           continentalpartnumber = x.continentalpartnumber,
                           customerpartnumber = x.customerpartnumber,
                           traza = x.traza,
                           notas = x.notas,
                           Leido = x.Leido
                       }).ToList();

                OrderVModel order = ReadsItems(x => x.OrderID == id).Select(x => new OrderVModel
                {
                    OrderID = x.OrderID,
                    ReaderID = x.ReaderID,
                    Number = x.Number,
                    ShipmentNumber = x.ShipmentNumber,
                    OnShipment = x.OnShipment,
                    Date = x.Date,
                    Finished = x.Finished
                }).FirstOrDefault();

                order.ListOrderDetail = query;
                return order;
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
            }
        }
        public OrderVModel GetOrderOnshipment(string embarque)
        {
            try
            {

                OrderVModel order = (from orders in ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true)
                           join reader in Context.Reader on orders.ReaderID equals reader.ReaderID
                           select new
                           {
                               orders,
                               reader
                           }).Select(x => new OrderVModel
                           {
                               OrderID = x.orders.OrderID,
                               ReaderID = x.orders.ReaderID,
                               Number = x.orders.Number,
                               ShipmentNumber = x.orders.ShipmentNumber,
                               OnShipment = x.orders.OnShipment,
                               Date = x.orders.Date,
                               Finished = x.orders.Finished,
                               Portal = x.reader.Name
                           }).FirstOrDefault();

                //OrderVModel order = ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true).Select(x => new OrderVModel
                //{
                //    OrderID = x.OrderID,
                //    ReaderID = x.ReaderID,
                //    Number = x.Number,
                //    ShipmentNumber = x.ShipmentNumber,
                //    OnShipment = x.OnShipment,
                //    Date = x.Date,
                //    Finished = x.Finished
                //}).FirstOrDefault();
                return order;
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
            }
        }
        public OrderEModel GetOrderEModel(string embarque)
        {
            try
            {
                OrderEModel order = ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true).FirstOrDefault();
                return order;
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
            }
        }

    }
}
