using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("OrderDetail")]
    public class OrderDetailEModel
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Display(Name = "")]
        public int OrderID { get; set; }
        public string embarque { get; set; }

        public string partida { get; set; }
        public string continentalpartnumber { get; set; }
        //[Display(Name = "")]
        public string customerpartnumber { get; set; }

        public string cantidad { get; set; }

        public string delivery { get; set; }

        public string traza { get; set; }

        public string shipment { get; set; }

        public string notas { get; set; }

        public int total_pallets { get; set; }
        public bool Leido { get; set; }
    }
}
