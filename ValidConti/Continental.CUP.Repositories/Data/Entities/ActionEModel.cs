using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("Action")]
    public class ActionEModel
    {
        [Key]
        //[Alias("ActionID")]
        public int ActionID { get; set; }

        //[References(typeof(ActionType))]
        public int ActionTypeID { get; set; }

        public string Name { get; set; }

        public short Status { get; set; }
    }
}
