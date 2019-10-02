using Continental.CUP.Repositories.Data;
using Continental.CUP.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ValidConti.Hubs
{
    public class ShipmentHub : Hub
    {
        private IOrderDetailRepository _context;
        private IOrderRepository _orderContext;
        #region Test
        List<ReadTag> readTag;
        XmlDocument xmlDoc;
        public ShipmentHub(IOrderDetailRepository context, IOrderRepository orderContext)
        {
            _context = context;
            _orderContext = orderContext;
            readTag = new List<ReadTag>();
        }

        public async Task Pallets(string portal, string embarque)
        {
            SuccessTag ret = new SuccessTag()
            {
                Success = 0
            };
            int valid = ret.Success;
            try
            {
                //Obtener la info del XML del Reader
                var uno = getTagReader();
                var dos = getTagList(portal, embarque);
                ReadTag tag = dos.Find(x => x.Reading == false);
                ReadTag tagTrue = dos.Find(x => x.Reading == true);



                if (tag != null)//Valida si aún quedan pallets por embarcar
                {
                    //Si el último tag leido au no es validado entra en la condición
                    if (uno.Reading == false)
                    {
                        //Busca en el xml del embarque el último pallet leido
                        tag = dos.Find(x => x.continentalpartnumber == uno.continentalpartnumber);
                        //Si se encuentra coincidencia en el paso anterior entra en la condición
                        if (tag != null)
                        {
                            valid = checkReadTag(tag, portal, embarque);
                            if (valid == 1)
                            {
                                ret.Success = 1;
                            }
                        }
                        else
                            ret.Success = 2;//No valido
                    }
                    else if (tagTrue == null)
                    {
                        ret.Success = 5; //Esperando a iniciar el embarque
                    }
                    else
                        ret.Success = 1;// Debe regresar un estado donde el ultimo tag al leído fue valido
                }
                else if (uno.Reading == false)
                {
                    ret.Success = 4;//No pertenece al embarque, ademas de estar terminado el embarque
                }
                else
                {
          
                    ret.Success = 3;// Embarque terminado
                }


                var json = JsonConvert.SerializeObject(uno);

                //TODO: Enviar el número de embarque
                await Clients.Caller.SendAsync("ReceivePallet", ret.Success, json);

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

       


        public int checkReadTag(ReadTag item, string portal, string embarque)
        {
            var po = "\\" + portal;
            var em = "\\" + embarque;
            var path = "C:\\temp" + po + em + ".xml";
            int flag = 0;
            string partNumber = item.continentalpartnumber;
            xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList userNodes = xmlDoc.SelectNodes("//ReadTag");
            foreach (XmlNode userNode in userNodes)
            {
                var prtNbr = userNode["continentalpartnumber"].InnerText;
                var rdng = bool.Parse(userNode["Reading"].InnerText);
                var ordr = int.Parse(userNode["OrderID"].InnerText);
                if (prtNbr.Equals(partNumber))
                {
                    if (rdng.Equals(false))
                    {
                        userNode.SelectSingleNode("Reading").InnerText = "true";
                        var orderDtl = _context.GetQueryOrderDetail(ordr, prtNbr);
                        orderDtl.Leido = true;
                        _context.UpdateItem(orderDtl);
                        xmlDoc.Save(path);
                        readingTag();
                        flag = 1;
                        break;
                    }
                }
            }
            return flag;
        }
        public void readingTag()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\temp\\tagCUP.xml");
            xmlDoc.SelectSingleNode("//ReadTag/Reading").InnerText = "true";
            xmlDoc.Save("C:\\temp\\tagCUP.xml");
        }

        //TODO:Generar path por reader
        public ReadTag getTagReader()
        {
            //TODO: Validar en el reader el PATH donde se almacenara la info
            string path = @"C:\Temp\tagCUP.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ReadTag>));
            StreamReader reader = new StreamReader(path);
            var uno = (List<ReadTag>)serializer.Deserialize(reader);
            reader.Close();
            return uno[0];
        }
        public List<ReadTag> getTagList(string portal, string embarque)
        {
            var po = "\\" + portal;
            var em = "\\" + embarque;
            var pa = "C:\\temp" + po + em + ".xml";
            string path = @pa;
            XmlSerializer serializer = new XmlSerializer(typeof(List<ReadTag>));
            StreamReader reader = new StreamReader(path);
            var uno = (List<ReadTag>)serializer.Deserialize(reader);
            reader.Close();
            return uno;
        }

        public void getSaveTag(List<ReadTag> tagList)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load("C:\\Temp\\tagCUP2.xml");
            XmlNode userNodes = xmlDoc.SelectSingleNode("//ArrayOfReadTag"); //ReadTag
            //XElement contacts =
            //    new XElement add = xmlDoc.CreateElement("ReadTag"),
            //        new XElement("TagNumber", tagList[0].TagNumber);//name= \"Nueva\"  

            XElement shippingUnit =
                new XElement("ArrayOfReadTag",
                    //new XElement("ReadTag", tagList[0].TagNumber),
                    new XElement("PartNumber", tagList[0].continentalpartnumber)
                    //new XElement("Quantity", tagList[0].Quantity)
                    );

            // userNodes.AppendChild();
        }

        #endregion
    }
    public class ReadTag
    {
        public string continentalpartnumber { get; set; }
        public bool Reading { get; set; }
    }
    public class SuccessTag
    {
        public int Success { get; set; }
    }
}
