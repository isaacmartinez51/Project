using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Continental.CUP.Repositories.Interfaces
{
    public interface IReaderKindRepository : IGenericRepository<ReaderKindEModel>
    {
        /// <summary>
        /// Validate if exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<ReaderKindEModel, bool>> expression);

        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        IQueryable<ReaderKindVModel> GetQueryReaderKind();
    }
}
