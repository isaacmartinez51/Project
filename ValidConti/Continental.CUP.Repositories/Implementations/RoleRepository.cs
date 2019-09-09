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
    public class RoleRepository : GenericRepository<ApplicationDbContext, RoleEModel>, IRoleRepository
    {
        public async Task<List<ActionDetailVModel>> GetQueryActionDetailByIdUser(int id) {


            //IEnumerable<ActionDetailVModel> dos = await Task.FromResult((from actionDetail in ReadsItems()
            //           join action in Context.Action on actionDetail.ActionID equals action.ActionID
            //           join roleAction in Context.RoleAction on action.ActionID equals roleAction.ActionID
            //           join role in Context.Role on roleAction.RoleID equals role.RoleID
            //           join userRole in Context.UserRole on role.RoleID equals userRole.RoleID
            //           where userRole.UserID == id
            //           select new ActionDetailVModel {
            //               ActionDetailID = actionDetail.ActionDetailID,
            //               ActionID = actionDetail.ActionID,
            //               ContainerName = actionDetail.ContainerName,
            //               ControlID = actionDetail.ControlID
            //           }).ToList().Distinct());
            //return dos.ToList();

            return null;
        }

        public async Task<List<RoleVModel>> GetQueryRoleByUserId(int id)
        {
            //IEnumerable<RoleVModel> dos = await Task.FromResult((from role in ReadsItems()
            //                                                     join userRole in Context.UserRole on role.RoleID equals userRole.RoleID into roles
            //                                                     from rl in roles.DefaultIfEmpty()
            //                                                     select new RoleVModel {
            //                                                         RoleID = role.RoleID,
            //                                                         UserRoleID = (roles == null)
            //                                                     }).Where(x => x.UserID == id));

            //return dos.ToList();

            return null;
        }

        //Task<List<ActionDetailVModel>> IActionDetailRepository.GetQueryActionDetailByIdUser(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
