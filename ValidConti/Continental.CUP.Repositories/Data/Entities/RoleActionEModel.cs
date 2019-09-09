using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("RoleAction")]
    public class RoleActionEModel
    {
        [Key]
        //[Alias("RoleActionID")]
        public int ID { get; set; }

        //[References(typeof(Role))]
        public int RoleID { get; set; }

        //[References(typeof(Action))]
        public int ActionID { get; set; }

        public bool Active { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
