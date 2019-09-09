using System;
using System.Collections.Generic;
using System.Text;

namespace Continental.CUP.Repositories.ViewModels
{
    public class LocationVModel
    {
        //[Alias("LocationID")]
        public int LocationID { get; set; }

        public string Name { get; set; }

        public int TagID { get; set; }

        public short Status { get; set; }
    }
}
