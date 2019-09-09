using System.ComponentModel.DataAnnotations;

namespace Continental.CUP.Repositories.ViewModels
{

    public class OrderDetailVModel
    {
        public int OrderDetailID { get; set; }

        //[Display(Name = "")]
        public int OrderID { get; set; }
        [Display(Name = "Embarque")]
        public string embarque { get; set; }
        [Display(Name = "Partida")]
        public string partida { get; set; }
        [Display(Name = "Número de parte")]
        public string continentalpartnumber { get; set; }
        [Display(Name = "Entrega")]
        public string customerpartnumber { get; set; }
        [Display(Name = "Piezas")]
        public string cantidad { get; set; }

        public string delivery { get; set; }
        [Display(Name = "Traza")]
        public string traza { get; set; }

        public string shipment { get; set; }
        [Display(Name = "Nota")]
        public string notas { get; set; }
        [Display(Name = "Pallets")]
        public int total_pallets { get; set; }
        public bool Leido { get; set; }

        //public int OrderDetailID { get; set; }

        //[Display(Name = "")]
        //public int OrderID { get; set; }

        //public int Traza { get; set; }

        //[Display(Name = "")]
        //public int UnitID { get; set; }

        //public string PartNumber { get; set; }

        //public int Quantity { get; set; }
    }
}
