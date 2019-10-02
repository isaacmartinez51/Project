using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("UserRole")]
    public class UserRoleEModel
    {
        [Key]
        //[Alias("UserRoleID")]
        public int UserRoleID { get; set; }

        //[References(typeof(User))]
        public int UserID { get; set; }

        //[References(typeof(Role))]
        public int RoleID { get; set; }

        public bool Active { get; set; }
    }
}
