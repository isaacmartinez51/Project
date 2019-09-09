using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Continental.CUP.Repositories.Interfaces
{
    public interface IGenericRepository <EModel> where EModel : class
    {
        /// <summary> Get item by expression </summary>
        /// <returns></returns>
        EModel GetItemByExpression(Expression<Func<EModel, bool>> expression);
        /// <summary>
        /// Reads all items
        /// </summary>
        /// <returns></returns>
        IQueryable<EModel> ReadsItems();
        /// <summary>
        /// Reads items by expreInvon
        /// </summary>
        /// <param name="expression">ExpreInvon to evaluate</param>
        /// <returns></returns>
        IQueryable<EModel> ReadsItems(Expression<Func<EModel, bool>> expression);
        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item"></param>
        EModel CreateItem(EModel item);
        /// <summary>
        /// Create new item async
        /// </summary>
        /// <param name="item"></param>
        Task<EModel> CreateItemAsync(EModel item);
        /// <summary>
        /// Create new item with composed key async
        /// </summary>
        /// <param name="item"></param>
        Task<EModel> CreateItemComposedKeyAsync(EModel item);
        /// <summary>
        /// Create new item with composed key 
        /// </summary>
        /// <param name="item"></param>
        EModel CreateItemComposedKey(EModel item);
        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item"></param>
        EModel UpdateItem(EModel item);
        /// <summary>
        /// Update item async
        /// </summary>
        /// <param name="item"></param>
        Task<EModel> UpdateItemAsync(EModel item);
        /// <summary>
        /// Update item with composed key 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        EModel UpdateItemComposedKey(Expression<Func<EModel, bool>> expression, EModel item);
        /// <summary>
        /// Update item with composed key async
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<EModel> UpdateItemComposedKeyAsync(Expression<Func<EModel, bool>> expression, EModel item);

    }
}
