using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.Interfaces;
using Continental.CUP.Repositories.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continental.CUP.Repositories.Implementations
{
    public class ActionDetailRepository : GenericRepository<ApplicationDbContext, ActionDetailEModel>, IActionDetailRepository
    {
        public async Task<List<ActionDetailVModel>> GetQueryActionDetailByIdUser(int id) {


            IEnumerable<ActionDetailVModel> dos = await Task.FromResult((from actionDetail in ReadsItems()
                       join action in Context.Action on actionDetail.ActionID equals action.ActionID
                       join roleAction in Context.RoleAction on action.ActionID equals roleAction.ActionID
                       join role in Context.Role on roleAction.RoleID equals role.RoleID
                       join userRole in Context.UserRole on role.RoleID equals userRole.RoleID
                       where userRole.UserID == id
                       select new ActionDetailVModel {
                           ActionDetailID = actionDetail.ActionDetailID,
                           ActionID = actionDetail.ActionID,
                           ContainerName = actionDetail.ContainerName,
                           ControlID = actionDetail.ControlID
                       }).ToList().Distinct());
            return dos.ToList();
        }

    }
}
