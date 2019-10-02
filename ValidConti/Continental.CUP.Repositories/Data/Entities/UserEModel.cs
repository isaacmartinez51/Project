using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("User")]
    public class UserEModel
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirtsName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsSuperAdmin { get; set; }

        public short Status { get; set; }
    }
}
