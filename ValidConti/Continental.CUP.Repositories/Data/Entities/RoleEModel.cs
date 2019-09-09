using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("Role")]
    public class RoleEModel
    {
        [Key]
        //[Alias("RoleID")]
        public int RoleID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
