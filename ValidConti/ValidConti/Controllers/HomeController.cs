using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Continental.CUP.Repositories.Classes.Exceptios;
using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Data.Entities;
using Continental.CUP.Repositories.Interfaces;
using Continental.CUP.Repositories.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ValidConti.Models;

namespace ValidConti.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private IOrderRepository _contextOrder;
        private IOrderDetailRepository _contextOrderDetail;
        public HomeController(ApplicationDbContext context, IOrderRepository contextOrder, IOrderDetailRepository contextOrderDetail)
        {
            _context = context;
            _contextOrder = contextOrder;
            _contextOrderDetail = contextOrderDetail;
        }
        #region Get API Embarques Information
        // el parametro debe ser un entero
        public ShipmentVModel test(string shipment)
        {
            try
            {
                var url = "https://continental.xlo.cloud/embarques/aviso/" + shipment;
                var webrequest = (HttpWebRequest)WebRequest.Create(url);

                using (var response = webrequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();
                    //string json = Convert.ToString(result);

                    return JsonConvert.DeserializeObject<ShipmentVModel>(json);
                }
            }
            catch (Exception ex)
            {

                throw new DataValidationException("Error", string.Format("Error de conexión con Embarques: {0}", ex.Message));
            }
        }
        #endregion

        #region Index Inicio
        public IActionResult Index()
        {
            ViewBag.Show = false;
            return View();
        }
        #endregion

        #region Index Enviar
        /// <summary>
        /// This method is called when is need to create an order.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(OrderVModel item)
        {
            OrderVModel orders = new OrderVModel();
            OrderEModel orderModel = new OrderEModel();

            try
            {
                if (ModelState.IsValid)
                {
                    #region Fill select
                    ViewBag.ReaderList = new SelectList(_context.Reader.ToList(), "ReaderID", "Name");
                    #endregion
                    var exist = _contextOrder.GetOrderOnshipment(item.ShipmentNumber);
                    if (exist == null)
                    {
                       
                        #region obtener información de embarques
                        //1.- Obtener el total de piezas
                        ShipmentVModel result = test(item.ShipmentNumber);
                        int index = 0;
                        // 1.1.-Obtener de traza el total de piezas por tarima del número de parte
                        foreach (OrderDetailVModel embarque in result.detalle)
                        {

                            Console.WriteLine(index);
                            //TODO: Conectar con traza y mediante el numero de parte obtener el total de piezas por pallet
                            int pallets = int.Parse(embarque.cantidad) / 48;
                            embarque.OrderID = orderModel.OrderID;
                            embarque.shipment = orderModel.ShipmentNumber;
                            Console.WriteLine(embarque.continentalpartnumber);
                            embarque.total_pallets = pallets;
                            index++;
                        }


                        #endregion

                        #region CreateShipment

                        orders = await _contextOrder.CreateShipment(item, result.detalle);

                        #endregion

                        ViewBag.OrderDetail = orders.ListOrderDetail;
                        ViewBag.Show = true;
                        return View(orders);
                    }
                    else if (exist.OnShipment == true && exist.Finished == false && exist.ReaderID != null)
                    {
                        var mntr = GetInfoMonitor(exist.OrderID);
                        mntr.IdOrden = exist.OrderID;
                        mntr.IdPortal = (int)exist.ReaderID;
                        mntr.Embarque = exist.ShipmentNumber;
                        return RedirectToAction("Monitor", mntr);//Monitor = mntr
                    }
                    else if (exist.Finished == true)
                    {
                        ViewBag.shipment = "Este embarque esta terminado";
                        ViewBag.Show = false;
                        return View();
                    }
                    else {
                        ViewBag.Show = true;
                        orders = _contextOrder.GetQueryOrderComplete(exist.OrderID);
                        ViewBag.OrderDetail = orders.ListOrderDetail;
                        return View(orders);
                    }
                        
                }
                else
                    ViewBag.Show = false;
            }
            catch (DataValidationException dex)
            {
                //TODO: Mostrar el mensaje en la pantala principal
                this.ModelState.AddModelError(dex.PropertyName, dex.ErrorMessage);
            }
            return View();
        }




        //public IActionResult Shipment(MonitorVModel item)
        //{
        //    //TODO: Obtiene los datos de la BD y genera la tabla
        //    var uno = GetInfoMonitor(item.IdOrden);
        //    uno.IdOrden = item.IdOrden;
        //    uno.IdPortal = item.IdPortal;
        //    uno.Portal = item.Portal;
        //    uno.Embarque = item.Embarque;
        //    ViewBag.Onshipment = true;
        //    ViewData["Message"] = "Validación de Pallets en el Anden n.-";

        //    return View(uno);
        //}
        public IActionResult Shipment(MonitorVModel item)
        {
            //TODO: Obtiene los datos de la BD y genera la tabla
            var uno = GetInfoMonitor(item.IdOrden);
            uno.IdOrden = item.IdOrden;
            uno.IdPortal = item.IdPortal;
            uno.Portal = item.Portal;
            uno.Embarque = item.Embarque;
            ViewBag.Onshipment = true;
            ViewData["Message"] = "Validación de Pallets en el";

            return View(uno);
        }






        public IActionResult FinishedShipment(MonitorVModel shipment)
        {
            //TODO: Validar si todos los pallets fueron embarcados
            var finishd = _contextOrderDetail.GetItemByExpression(x => x.embarque.Equals(shipment.Embarque) && x.Leido == false);
            if (finishd == null)
            {
                FinishShipment(shipment.Embarque);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Monitor", shipment);
        }
        private void FinishShipment(string shipment)
        {
            var order = _contextOrder.GetOrderEModel(shipment);
            order.Finished = true;
            _contextOrder.UpdateItem(order);
        }
        #endregion

        #region Monitor
        public IActionResult Monitor(MonitorVModel obj)
        {
            var result = GetInfoMonitor(obj.IdOrden);
            //obj.Order = result.Order;
            obj.OrderDetail = result.OrderDetail;
            ViewData["Message"] = "Your contact page.";
            string spath = @"c:\temp\" + obj.Portal + "\\" + obj.Embarque + ".xml";

            if (System.IO.File.Exists(spath))
            {
                System.IO.File.Delete(spath);
            }
            //TODO: Obtener el número del embarque 
            Console.WriteLine(obj);
            int index = 1;
            var details = _contextOrderDetail.GetQueryOrderDetail(obj.IdOrden);
            if (details != null)
            {
                //TODO:crear el XML
                foreach (var item in details)
                {
                    CreateXMLShipment(item, index, obj);
                    index++;
                }
            }
            return View(obj);
        }

        #endregion

        #region CreateXMLShipment

        private void CreateXMLShipment(OrderDetailVModel item, int Onshipment, MonitorVModel obj)
        {

            string spath = @"c:\temp\" + obj.Portal + "\\" + obj.Embarque + ".xml";
            if (!System.IO.File.Exists(spath))
            {
                using (XmlWriter writer = XmlWriter.Create(spath))
                {
                    writer.WriteStartElement("ArrayOfReadTag");
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }


            //if (1 == Onshipment)
            //{
            //    XDocument docOnshipment = XDocument.Load(spath);
            //    XElement rootOnshipment = new XElement("ReadTag");
            //    rootOnshipment.Add(new XElement("onshipment", "false"));
            //    rootOnshipment.Add(new XElement("shipment", item.embarque));
            //    docOnshipment.Element("ArrayOfReadTag").Add(rootOnshipment);
            //    docOnshipment.Save(spath);
            //}
            XDocument doc = XDocument.Load(spath);
            XElement root = new XElement("ReadTag");
            root.Add(new XElement("continentalpartnumber", item.continentalpartnumber));
            root.Add(new XElement("Tarima", Onshipment));
            root.Add(new XElement("OrderID", item.OrderID));
            root.Add(new XElement("Reading", item.Leido));
            doc.Element("ArrayOfReadTag").Add(root);
            doc.Save(spath);
        }


        #endregion




        #region Asignar Embarque
        [HttpPost]
        public async Task<IActionResult> SetShipment(OrderVModel item)
        {
            MonitorVModel mntr = new MonitorVModel();
            if (item.OrderID > 0)
            {
                //Se pone el embarque activo
                OrderEModel order = _context.Order.FirstOrDefault(x => x.OrderID == item.OrderID);
                order.OnShipment = true;
                order.ReaderID = item.ReaderID;
                var succes = await _contextOrder.UpdateItemAsync(order);

                //Obtengo el nombre de reader asignado
                ReaderEModel reader = _context.Reader.FirstOrDefault(x => x.ReaderID == item.ReaderID);
                mntr.Portal = reader.Name;
                mntr.IdOrden = order.OrderID;
                mntr.Embarque = order.ShipmentNumber;
                mntr.IdPortal = (int)order.ReaderID;
                return RedirectToAction("Monitor", mntr);//Monitor = mntr
            }
            else
                ViewBag.Show = false;
            return null;
        }
        #endregion

        public MonitorVModel GetInfoMonitor(int id)
        {
            MonitorVModel mntr = new MonitorVModel();
            //var order = _context.Order.FirstOrDefault(x=> x.OrderID == id);
            //mntr.IdOrden = order.
            mntr.OrderDetail = _contextOrderDetail.GetQueryOrderDetail(id).ToList();
            return mntr;
        }
        public List<OrderDetailVModel> GetInfoMonitorList(int id)
        {
            List<OrderDetailVModel> list = new List<OrderDetailVModel>();
            //var order = _context.Order.FirstOrDefault(x=> x.OrderID == id);
            //mntr.IdOrden = order.
            list = _contextOrderDetail.GetQueryOrderDetail(id).ToList();
            return list;
        }


        #region Otro
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
    public class ReadTag
    {
        public string continentalpartnumber { get; set; }
        public bool Reading { get; set; }
    }

    public class ReturnPartNumber
    {
        public string PartNumber { get; set; }
        public int Quantity { get; set; }
        public int Success { get; set; }
    }


}
