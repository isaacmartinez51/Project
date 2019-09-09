using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    public class ActionDetailEModel
    {
        [Key]
        //[Alias("ActionDetailID")]
        public int ActionDetailID { get; set; }

        //[References(typeof(Action))]
        public int ActionID { get; set; }

        public string ContainerName { get; set; }

        public string ControlID { get; set; }

    }
}
