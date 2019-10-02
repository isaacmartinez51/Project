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
    public class LocationRepository : GenericRepository<ApplicationDbContext, LocationEModel>, ILocationRepository
    {
        public bool Exist(Expression<Func<LocationEModel, bool>> expression) => ReadsItems(expression).Count() > 0;


        public IQueryable<LocationVModel> GetQueryLocation()
        {
            return ReadsItems().Select(x => new LocationVModel
            {
                LocationID = x.LocationID,
                Name = x.Name,
                Status = x.Status,
                TagID = x.TagID
            });
        }
    }
}
