using System;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EtiquteContinental.Models;
using Microsoft.Extensions.Options;
using EtiquteContinental.Classes;

namespace EtiquteContinental.Controllers
{
    public class HomeController : Controller
    {
        public AppSettings AppSettings { get; set; }
        public HomeController(IOptions<AppSettings> appSettings)
        {
            this.AppSettings = appSettings.Value;
        }
        public IActionResult Index(SerialPrintModel item)
        {
            bool print = false;
            if (null != item.Serial)
            {
                item = ConnectionTraza.GetInformationSerial(this.AppSettings, item.Serial);
                string zpl = replaceZPL(this.AppSettings, item);
                print = printLabel(this.AppSettings, zpl);
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region ZPL
        private string replaceZPL(AppSettings appSettings, SerialPrintModel label)
        {

            if (label != null)
            {
                string zplBase = appSettings.ZPL;
                StringBuilder zpl = new StringBuilder(zplBase);
                zpl.Replace("*RFID*", Mlfb16char(label.MLFB));
                zpl.Replace("*tittle*", "Blackflush de Producto Terminado");
                zpl.Replace("*em*", "Etiqueta Master");
                zpl.Replace("*mlfb*", label.MLFB);
                zpl.Replace("*sh*", label.PackingType);
                zpl.Replace("*ta*", "Tarima");
                zpl.Replace("*qn*", label.Quantity.ToString());
                zpl.Replace("sr", label.Serial);
                zpl.Replace("*date*", DateTime.Now.ToString());

                return zpl.ToString();
            }
            return null;
        }

        private string Mlfb16char(string mlfb) {
            int total = mlfb.Length;
            if (total < 16)
            {
                StringBuilder sbMlfb = new StringBuilder(mlfb);
                for (int i = total; i < 16; i++)
                {
                    sbMlfb.Insert(i, '@');
                }
                return sbMlfb.ToString();
            }
            else
                return mlfb;
        }

        private bool printLabel(AppSettings appSettings, string zpl)
        {
            return RawPrinterHelper.SendStringToPrinter(appSettings.PrintNames, zpl);
        }
        #endregion
    }
}
