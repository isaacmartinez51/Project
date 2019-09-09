using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    [Table("Order")]
    public class OrderEModel
    {
        [Key]
        public int OrderID { get; set; }

        public int? ReaderID { get; set; }
        [Required]
        public string Number { get; set; }

        public DateTime Date { get; set; }

        public string ShipmentNumber { get; set; }

        public Boolean OnShipment { get; set; }
        public Boolean Finished { get; set; }
    }
}
