using System;
using System.Collections.Generic;
using System.Text;

namespace Continental.CUP.Repositories.ViewModels
{
    public class MonitorVModel
    {
        public int IdOrden { get; set; }
        public string Embarque { get; set; }
        public string Portal { get; set; }
        public int IdPortal { get; set; }
        public List<OrderDetailVModel> OrderDetail { get; set; }
    }
}
