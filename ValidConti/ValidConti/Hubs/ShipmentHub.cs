using Continental.CUP.Repositories.Classes.Exceptios;
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
        #region XML
        List<ReadTag> readTag;
        //XmlDocument xmlDoc;
        public ShipmentHub(IOrderDetailRepository context, IOrderRepository orderContext)
        {
            _context = context;
            _orderContext = orderContext;
            readTag = new List<ReadTag>();
        }

        public async Task Pallets(string portal, string embarque, int srt)
        {

            SuccessTag ret = new SuccessTag()
            {
                Success = srt
            };
            int valid = ret.Success;
            try
            {
                //Obtener la info del XML del Reader
                var uno = getTagReader(portal);
                // Obtiene la lista de todos los pallets a embarcar
                var listTags = getTagList(portal, embarque);
                // Metodo que obtiene si en la lista de los pallets existe alguno por embarcar
                ReadTag tag = listTags.Find(x => x.Reading == false);
                // Metodo para validar si ya se embarco algun pallet
                ReadTag tagTrue = listTags.Find(x => x.Reading == true);



                if (uno != null)
                {
                    if (tag != null)//Valida si aún quedan pallets por embarcar
                    {
                        //Si el último tag leido au no es validado entra en la condición
                        if (uno.Reading == false)
                        {
                            //Busca en el xml del embarque el último pallet leido
                            tag = listTags.Find(x => x.continentalpartnumber == uno.continentalpartnumber);
                            //Si se encuentra coincidencia en el paso anterior entra en la condición
                            if (tag != null && tag.Reading == false)
                            {
                                valid = await checkReadTag(tag, portal, embarque);
                                if (valid == 1)
                                    ret.Success = 1;
                            }
                            else
                                ret.Success = 2;//No valido
                        }
                        else if (tagTrue == null)
                            ret.Success = 5; //Esperando a iniciar el embarque
                        else
                            ret.Success = 1;// Debe regresar un estado donde el ultimo tag al leído fue valido
                    }
                    else if (uno.Reading == false)
                        ret.Success = 4;//No pertenece al embarque, ademas de estar terminado el embarque
                    else
                        ret.Success = 3;// Embarque terminado
                }
                else
                    ret.Success = 5; //Esperando a iniciar el embarque

                var json = JsonConvert.SerializeObject(uno);
                if (ret.Success != srt)
                    await Clients.Caller.SendAsync("ReceivePallet", ret.Success, json);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }




        public async Task<int> checkReadTag(ReadTag item, string portal, string embarque)
        {
            var po = "\\" + portal;
            var em = "\\" + embarque;
            var path = "C:\\temp" + po + em + ".xml";
            int flag = 0;
            string partNumber = item.continentalpartnumber; //item.continentalpartnumber
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList userNodes = xmlDoc.SelectNodes("//ReadTag");
            foreach (XmlNode userNode in userNodes)
            {
                var prtNbr = userNode["continentalpartnumber"].InnerText;
                var rdng = bool.Parse(userNode["Reading"].InnerText);
                var ordr = int.Parse(userNode["OrderID"].InnerText);
                //if (prtNbr.Equals(item.continentalpartnumber))
                if (prtNbr.Equals(partNumber))
                {
                    if (rdng.Equals(false))
                    {

                        userNode.SelectSingleNode("Reading").InnerText = "true";
                        var orderDtl = _context.GetQueryOrderDetail(ordr, prtNbr);
                        orderDtl.Leido = true;
                        _context.UpdateItem(orderDtl);
                        xmlDoc.Save(path);
                        await ReadingTag(po);
                        flag = 1;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ReadingTag(string portal)
        {
            // TODO2: Probar el path
            try
            {
                await Task.Run(() =>
                {
                    XmlDocument xmlDocTag = new XmlDocument();
                    xmlDocTag.Load("C:\\temp\\" + portal + "Tag\\tagCUP.xml");
                    xmlDocTag.SelectSingleNode("//ReadTag/Reading").InnerText = "true";
                    xmlDocTag.Save("C:\\temp\\" + portal + "Tag\\tagCUP.xml");
                });
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Error", string.Format("Error de conexión con Embarques: {0}", ex.Message));
            }

        }

        //TODO:Generar path por reader
        public ReadTag getTagReader(string portal)
        {
            //TODO: Validar en el reader el PATH donde se almacenara la info
            string path = @"C:\Temp\" + portal + "Tag" + "\\tagCUP.xml";
            if (System.IO.File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ReadTag>));
                StreamReader reader = new StreamReader(path);
                var uno = (List<ReadTag>)serializer.Deserialize(reader);
                reader.Close();
                return uno[0];
            }
            return null;
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
