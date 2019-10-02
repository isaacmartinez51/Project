using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Continental.CUP.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetailEModel>
    {
        /// <summary>
        /// Validate if exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<OrderDetailEModel, bool>> expression);

        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderDetailVModel> GetQueryOrderDetail();


        /// <summary>
        /// Entity model to view model by id
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderDetailVModel> GetQueryOrderDetail(int id);
        /// <summary>
        /// Entity model to view model by id
        /// </summary>
        /// <returns></returns>
        OrderDetailEModel GetQueryOrderDetail(int orderId, string embarque);
    }
}
