using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Continental.CUP.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<OrderEModel>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Embarque"></param>
        /// <returns></returns>
        Task<OrderVModel> GetShipmentV2(string Embarque);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVModel"></param>
        /// <param name="orderDetailVModel"></param>
        /// <returns></returns>
        Task<OrderVModel> CreateShipmentV2(List<OrderDetailVModel> orderDetailVModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVModel"></param>
        /// <param name="orderDetailVModel"></param>
        /// <returns></returns>
        Task<OrderVModel> CreateShipment(OrderVModel orderVModel, List<OrderDetailVModel> orderDetailVModel);
        /// <summary>
        /// Validate if exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<OrderEModel, bool>> expression);

        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderVModel> GetQueryOrder();


        /// <summary>
        /// Entity model to view model by id
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderVModel> GetQueryOrder(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderVModel GetQueryOrderComplete(string shipment);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="embarque"></param>  GetOrderById
        /// <returns></returns>
        OrderVModel GetOrderOnshipment(string embarque);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="embarque"></param>  GetOrderById
        /// <returns></returns>
        OrderLigthVModel GetOrderById(int idOrder);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="embarque"></param>
        /// <returns></returns>
        OrderEModel GetOrderEModel(string embarque);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<OrderEModel> PutOrderEModel(OrderLigthVModel model);


        OrderVModel GetOrderByOrderId(int id);
    }
}
