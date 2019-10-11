using EtiquteContinental.Models;
using Oracle.ManagedDataAccess.Client;
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

            string oracleConn = "Data Source= tqdb002x.tq.mx.conti.de:1521/tqtrazapdb.tq.mx.conti.de; User Id=consulta; Password= solover";

            #region Connection Traza
            string query = $"SELECT MLFB, Serial, ordernumber, datetimestamp from ETGDL.boxes WHERE SERIAL = '{serial}'"; //9974S432B1136138
            string query2 = $"SELECT aunitsperbox * aboxperpallet FROM ETGDL.products WHERE MLFB = (Select MLFB from ETGDL.boxes WHERE SERIAL = '{serial}')";
            using (OracleConnection connection = new OracleConnection(oracleConn))
            {
                OracleCommand command = new OracleCommand(query, connection);
                connection.Open();
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    label.MLFB = reader.GetOracleString(0).ToString();
                    label.Serial = reader.GetOracleString(1).ToString(); //String
                    label.Order = reader.GetOracleValue(2).ToString(); //String
                }


                OracleCommand command2 = new OracleCommand(query2, connection);

                OracleDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    //label.Quantity = reader2.GetOracleValue(0);
                    var uno = reader2.GetOracleValue(0).ToString();
                    label.Quantity = int.Parse(uno);

                }
                Console.WriteLine("Label: " + label);
                reader.Close();
                reader2.Close();
            }

            #endregion


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
            //SerialPrintModel labelTest = new SerialPrintModel();
            //labelTest.Serial = "1234567890";
            //labelTest.MLFB = "A2C1781540095";
            //labelTest.Quantity = 100;
            //labelTest.PackingType = "Tarima";

            //return labelTest;
            #endregion

            return label;

        }
        #endregion
    }

}
