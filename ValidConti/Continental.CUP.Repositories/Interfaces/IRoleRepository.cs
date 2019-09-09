using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continental.CUP.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<RoleEModel>
    {
        Task<List<RoleVModel>> GetQueryRoleByUserId(int id);
    }
}
