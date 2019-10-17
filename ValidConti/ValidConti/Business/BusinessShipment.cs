using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidConti.Business
{
    public class BusinessShipment
    {
        private ApplicationDbContext _context;
        private IOrderRepository _contextOrder;

        public BusinessShipment(ApplicationDbContext context, IOrderRepository contextOrder)
        {
            _context = context;
            _contextOrder = contextOrder;
        }

        //Metodo que valida si el embarque ya esta asignado  
        public bool OrderExistAssign(string shipment)
        {
            var result =_contextOrder.Exist(x => x.ShipmentNumber == shipment && x.OnShipment == true);

            return result;
        }
        //Metodo que valida si el embarque ya esta asignado  
        public bool OrderExist(string shipment)
        {
            var result = _contextOrder.Exist(x => x.ShipmentNumber == shipment);

            return result;
        }

        public bool OrderFinished(string shipment)
        {
            var result = _contextOrder.Exist(x => x.ShipmentNumber == shipment && x.OnShipment == true && x.Finished == true);

            return result;
        }
    }
   
}
