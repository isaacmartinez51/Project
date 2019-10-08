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
using Oracle.ManagedDataAccess.Client;
using ValidConti.Business;
using ValidConti.Models;

namespace ValidConti.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private IOrderRepository _contextOrder;
        private IOrderDetailRepository _contextOrderDetail;
        private BusinessPlatform platform;
        public HomeController(ApplicationDbContext context, IOrderRepository contextOrder, IOrderDetailRepository contextOrderDetail)
        {
            _context = context;
            _contextOrder = contextOrder;
            _contextOrderDetail = contextOrderDetail;
            platform = new BusinessPlatform(_context, _contextOrder);
        }
        #region Get API Embarques Information
        // el parametro debe ser un entero
        public ShipmentVModel GetShipment(string shipment)
        {
            try
            {
                var url = "https://continental.xlo.cloud/embarques/aviso/" + shipment;
                var webrequest = (HttpWebRequest)WebRequest.Create(url);

                using (var response = webrequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();
                    ShipmentVModel result = JsonConvert.DeserializeObject<ShipmentVModel>(json);
                    if (int.Parse(result.cancelado) != 0)
                        return result;
                    else
                        throw new DataValidationException("Error", string.Format("Embarque cancelado"));
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

        #region Exceptions
        [HttpPost]
        public IActionResult Exceptions()
        {

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
            int palletBox = 0;

            try
            {
                if (ModelState.IsValid)
                {
                    #region Fill select
                    ViewBag.ReaderList = new SelectList(platform.GetPlatforms(), "ReaderID", "Name");
                    #endregion
                    var exist = _contextOrder.GetOrderOnshipment(item.ShipmentNumber);
                    if (exist == null)
                    {
                        #region obtener información de embarques
                        //1.- Obtener el total de piezas
                        ShipmentVModel result = GetShipment(item.ShipmentNumber);

                        int index = 0;
                        // 1.1.-Obtener de traza el total de piezas por tarima del número de parte
                        foreach (OrderDetailVModel embarque in result.detalle)
                        {
                            #region Connection Traza
                            string oracleConn = "Data Source= tqdb002x.tq.mx.conti.de:1521/tqtrazapdb.tq.mx.conti.de; User Id=consulta; Password= solover";
                            string query = $"SELECT aunitsperbox * aboxperpallet FROM ETGDL.products WHERE MLFB = '{embarque.continentalpartnumber}' ";
                            using (OracleConnection connection = new OracleConnection(oracleConn))
                            {
                                OracleCommand command = new OracleCommand(query, connection);
                                connection.Open();
                                OracleDataReader reader = command.ExecuteReader();

                                if (reader.Read())
                                {
                                    palletBox = reader.GetInt32(0);
                                }
                                reader.Close();
                            }

                            #endregion
                            Console.WriteLine(index);
                            //TODO: Conectar con traza y mediante el numero de parte obtener el total de piezas por pallet
                            //int pallets = int.Parse(embarque.cantidad) / 48;

                            int pallets = int.Parse(embarque.cantidad) / palletBox;
                            if (pallets == 0)
                                throw new DataValidationException("Error", string.Format("Error de conexión con Embarques"));
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
                        //OrderLigthVModel mntr = new OrderLigthVModel();
                        var mntr = GetInfoMonitor(exist.OrderID);
                        mntr.IdOrden = exist.OrderID;
                        mntr.IdPortal = (int)exist.ReaderID;
                        mntr.Embarque = exist.ShipmentNumber;
                        mntr.Portal = exist.Portal;
                        return RedirectToAction("Monitor", mntr);//Monitor = mntr
                    }
                    else if (exist.Finished == true)
                    {
                        ViewBag.shipment = "Este embarque esta terminado";
                        ViewBag.Show = false;
                        return View();
                    }
                    else
                    {
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
                ViewBag.Show = false;
                ViewBag.Alert = false;
                return RedirectToAction("Error");
                //TODO: Mostrar el mensaje en la pantala principal
                //this.ModelState.AddModelError(dex.PropertyName, dex.ErrorMessage);
            }
            return View();
        }

        public IActionResult Shipment(MonitorVModel item)
        {
            try
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
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }


        public IActionResult FinishedShipment(MonitorVModel shipment)
        {
            try
            {
                //TODO: Validar si todos los pallets fueron embarcados
                var finishd = _contextOrderDetail.GetItemByExpression(x => x.embarque.Equals(shipment.Embarque) && x.Leido == false);
                if (finishd == null)
                {
                    FinishShipment(shipment.Embarque, shipment.Portal);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Monitor", shipment);
            }
            catch (Exception)
            {

                return RedirectToAction("Error");
            }
        }
        private void FinishShipment(string shipment, string portal)
        {
            try
            {
                //TODO1: Validar el path
                string spath = @"c:\temp\" + portal + "Tag\tagCUP.xml";
                var order = _contextOrder.GetOrderEModel(shipment);
                order.Finished = true;
                if (System.IO.File.Exists(spath))
                {
                    System.IO.File.Delete(spath);
                }
                _contextOrder.UpdateItem(order);

            }
            catch (Exception ex)
            {

                throw new DataValidationException("Error", string.Format("Error de conexión con Embarques: {0}", ex.Message));
            }
        }
        #endregion



        #region CreateXMLShipment

        private void CreateXMLShipment(OrderDetailVModel item, int Onshipment, MonitorVModel obj)
        {

            try
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
                #region MyRegion
                //if (1 == Onshipment)
                //{
                //    XDocument docOnshipment = XDocument.Load(spath);
                //    XElement rootOnshipment = new XElement("ReadTag");
                //    rootOnshipment.Add(new XElement("onshipment", "false"));
                //    rootOnshipment.Add(new XElement("shipment", item.embarque));
                //    docOnshipment.Element("ArrayOfReadTag").Add(rootOnshipment);
                //    docOnshipment.Save(spath);
                //} 
                #endregion
                XDocument doc = XDocument.Load(spath);
                XElement root = new XElement("ReadTag");
                root.Add(new XElement("continentalpartnumber", item.continentalpartnumber));
                root.Add(new XElement("Tarima", Onshipment));
                root.Add(new XElement("OrderID", item.OrderID));
                root.Add(new XElement("Reading", item.Leido));
                doc.Element("ArrayOfReadTag").Add(root);
                doc.Save(spath);
            }
            catch (Exception ex)
            {

                throw new DataValidationException("Error", string.Format("Error de conexión con Embarques: {0}", ex.Message));
            }
        }

        #endregion

        #region Asignar Embarque
        [HttpPost]
        public async Task<IActionResult> SetShipment(OrderVModel item)
        {
            try
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
                    string spath = @"c:\temp\" + mntr.Portal + "Tag" + "\\tagCUP" + ".xml";
                    if (System.IO.File.Exists(spath))
                    {
                        System.IO.File.Delete(spath);
                    }
                    return RedirectToAction("Monitor", mntr);//Monitor = mntr
                }
                else
                    ViewBag.Show = false;
            }
            catch (Exception)
            {
                //TODO: Generar un platilla para errores
                return RedirectToAction("Error");
            }
            return null;
        }
        #endregion

        #region Monitor
        //public IActionResult Monitor(OrderLigthVModel obj)
        //{
        //    MonitorVModel list = new MonitorVModel();
        //    try
        //    {

        //        var result = GetInfoMonitor(obj.OrderID);
        //        //obj.Order = result.Order;
        //        list.OrderDetail = result.OrderDetail;
        //        ViewData["Message"] = "Your contact page.";
        //        string spath = @"c:\temp\" + obj.Portal + "\\" + list.Embarque + ".xml";

        //        if (System.IO.File.Exists(spath))
        //        {
        //            System.IO.File.Delete(spath);
        //        }
        //        //TODO: Obtener el número del embarque 
        //        Console.WriteLine(obj);
        //        int index = 1;
        //        var details = _contextOrderDetail.GetQueryOrderDetail(list.IdOrden);
        //        if (details != null)
        //        {
        //            //TODO:crear el XML
        //            foreach (var item in details)
        //            {
        //                CreateXMLShipment(item, index, list);
        //                index++;
        //            }
        //        }
        //        return View(list);
        //    }
        //    catch (Exception ex)
        //    {

        //        return RedirectToAction("Error");
        //    }
        //}
        public IActionResult Monitor(MonitorVModel obj)
        {
            try
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
            catch (Exception ex)
            {

                return RedirectToAction("Error");
            }
        }

        #endregion

        public MonitorVModel GetInfoMonitor(int id)
        {
            try
            {
                MonitorVModel mntr = new MonitorVModel();
                //var order = _context.Order.FirstOrDefault(x=> x.OrderID == id);
                //mntr.IdOrden = order.
                mntr.OrderDetail = _contextOrderDetail.GetQueryOrderDetail(id).ToList();
                return mntr;
            }
            catch (Exception ex)
            {

                throw new DataValidationException("Error", string.Format("Error de conexión con Embarques: {0}", ex.Message));
            }
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

        #region Cambiar ánden

        public IActionResult Platform(OrderLigthVModel obj)
        {
            #region Fill select
            ViewBag.ReaderList = new SelectList(platform.GetPlatforms(), "ReaderID", "Name");
            #endregion
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> PlatformChange(OrderLigthVModel item)
        {
            try
            {
                var uno = platform.GetOrderByIdOrder(item.OrderID);
                if (uno.ReaderID != item.ReaderID)
                {
                    var result = await platform.PutPlatform(item);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Alert = "Seleccionaste el mismo anden";
                    ViewBag.ReaderList = new SelectList(platform.GetPlatforms(), "ReaderID", "Name");
                    return View("Platform", item);
                }
            }
            catch (Exception)
            {
                //TODO: Generar un platilla para errores
                return RedirectToAction("Error");
            }
        }
        #endregion
    }

    #region Modelos

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
    #endregion

}
