using EtiquteContinental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtiquteContinental.Classes
{

    public class ConnectionTraza
    {
        #region Print
        public static SerialPrintModel GetInformationSerial(AppSettings appSettings, string serial)
        {
            SerialPrintModel label = new SerialPrintModel();
            string queryString = "SELECT * FROM access$ where d_obj# = 1753";
            //string sql = "SELECT * FROM boxes";
            string oracleConn = appSettings.Traza_Connection;


            #region Cambiar por la version en el server de traza
            //using (OracleConnection connection = new OracleConnection(oracleConn))
            //{
            //    OracleCommand command = new OracleCommand(queryString, connection);
            //    connection.Open();
            //    OracleDataReader reader = command.ExecuteReader();
            //    try
            //    {
            //        while (reader.Read())
            //        {
            //            label.MLFB = reader.GetValue(0).ToString();
            //            label.Order = reader.GetValue(1).ToString();
            //            label.Quantity = 80;
            //            label.Serial = reader.GetValue(2).ToString();
            //            label.PackingType = "Tarima";
            //        }
            //    }
            //    finally
            //    {
            //        // always call Close when done reading.
            //        reader.Close();
            //    } 

            //}
            #endregion

            #region Borrar
            SerialPrintModel labelTest = new SerialPrintModel();
            labelTest.Serial = "1234567890";
            labelTest.MLFB = "A2C1781540095";
            labelTest.Quantity = 100;
            labelTest.PackingType = "Tarima";

            return labelTest;
            #endregion

            //return label;

        }
        #endregion
    }

}
