using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    public class LocationEModel
    {
        [Key]
        //Alias("LocationID")]
        public int LocationID { get; set; }

        public string Name { get; set; }

        //[References(typeof(Tag))]
        public int TagID { get; set; }

        public short Status { get; set; }
    }
}
