using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Continental.CUP.Repositories.ViewModels
{
    public class OrderVModel
    {
        public int OrderID { get; set; }

        [Display(Name = "Reader")]
        public int? ReaderID { get; set; }

        [Display(Name = "Número de orden")]
        [StringLength(50)]
        public string Number { get; set; }

        public System.DateTime Date { get; set; }

        [Display(Name = "Numero de embarque")]
        [StringLength(20)]
        [Required]
        public string ShipmentNumber { get; set; }

        public bool? OnShipment { get; set; }
        public bool? Finished { get; set; }

        public List<OrderDetailVModel> ListOrderDetail { get; set; }
    }
}
