using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Continental.CUP.Repositories.ViewModels
{
    public class OrderLigthVModel
    {
        public int OrderID { get; set; }

        [Display(Name = "Reader")]
        public int? ReaderID { get; set; }

        public string Portal { get; set; }

        public string Embarque { get; set; }
    }
}
