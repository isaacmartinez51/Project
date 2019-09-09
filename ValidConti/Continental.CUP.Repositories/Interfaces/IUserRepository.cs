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
    public interface IUserRepository : IGenericRepository<UserEModel>
    {
        /// <summary>
        /// Validate if exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<UserEModel, bool>> expression);

        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        IQueryable<UserVModel> GetQueryAllUser();
        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        UserVModel GetQueryByUserName(string userName);
        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        UserVModel GetQueryByUserId(int userId);
    }
}
