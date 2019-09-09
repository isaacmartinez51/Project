using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.Interfaces;
using Continental.CUP.Repositories.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continental.CUP.Repositories.Implementations
{
    public class UserRoleRepository : GenericRepository<ApplicationDbContext, UserRoleEModel>, IUserRoleRepository
    {

        public async Task<List<UserRoleVModel>> GetQueryUserRoleByUserId(int id)
        {
            IEnumerable<UserRoleVModel> dos = await Task.FromResult((from r in Context.Role
                                                                     join ur in Context.UserRole on new { A = r.RoleID, B = id } equals new { A = ur.RoleID, B = ur.UserID }
                                                                     into uroles
                                                                     from rl in uroles.DefaultIfEmpty()
                                                                     select new UserRoleVModel
                                                                     {
                                                                         UserRoleID = (int?)rl.UserRoleID ?? 0,
                                                                         UserID = rl.UserID,
                                                                         RoleID = r.RoleID,
                                                                         Name = r.Name,
                                                                         Active = rl.Active == false ? false : rl.Active

                                                                     }).ToList());
            return dos.ToList();
        }

        public async Task<bool> UpdateUserRoleTODO(UserVModel model)
        {
            List<UserRoleEModel> usro = new List<UserRoleEModel>();
            UserEModel us = new UserEModel
            {
                UserID = model.UserID,
                FirtsName = model.FirtsName,
                LastName = model.LastName,
                UserName = model.UserName,
                Password = model.Password,
                Status = model.Status,
                IsSuperAdmin = model.IsSuperAdmin
            };
            foreach (var item in model.UserRoleModelList)
            {
                UserRoleEModel userRole = new UserRoleEModel
                {
                    UserID = (int)item.UserID,
                    RoleID = item.RoleID,
                    Active = item.Active
                };
                usro.Add(userRole);
            }


            // Initialize transaction
            using (var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
            {
                Context.Set<UserEModel>().Add(us);
                Context.Set<UserRoleEModel>().AddRange(usro);
                Console.WriteLine("transaction");
                //await Context.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
    }
}