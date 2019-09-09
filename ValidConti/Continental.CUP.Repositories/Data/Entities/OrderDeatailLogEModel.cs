using System;
using System.Collections.Generic;
using System.Text;

namespace Continental.CUP.Repositories.Data.Entities
{
    public class OrderDeatailLogEModel
    {
        public int OrderDeatailLogID { get; set; }
        public int OrderDeatailID { get; set; }
        public string Continentalpartnumber { get; set; }
        public bool Leido { get; set; }
        public int Partida { get; set; }

    }
}
