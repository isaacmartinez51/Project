using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Impinj.OctaneSdk;
using Anden_2.Classes;
using static Impinj.OctaneSdk.ImpinjReader;

namespace Anden_2
{
    public partial class MainForm : Form
    {

        ImpinjReader reader;

        public MainForm()
        {
            InitializeComponent();
            reader = new ImpinjReader();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string ipReader = txtIPReader.Text;
                reader.Connect(ipReader);

                Settings settings = reader.QueryDefaultSettings();
                settings.Report.IncludeAntennaPortNumber = true;
                settings.Session = 2;
                settings.SearchMode = SearchMode.SingleTarget;
                settings.Report.IncludeLastSeenTime = true;

                for (ushort a = 1; a <= 4; a++)
                {
                    settings.Antennas.GetAntenna(a).TxPowerInDbm = Convert.ToDouble(numericUpDown1.Value);
                    settings.Antennas.GetAntenna(a).RxSensitivityInDbm = -70;
                }
                reader.ApplySettings(settings);
                reader.TagsReported += new TagsReportedHandler((sReader, report) =>
                {
                    string hexascci = string.Empty;
                    List<ReadTag> lista = new List<ReadTag>();
                    foreach (Tag tag in report)
                    {
                        hexascci = EpcConvertHexAsc.HexToAscii(tag.Epc.ToString());
                        
                        lista.Add(new ReadTag()
                        {
                            
                            continentalpartnumber = hexascci,
                            Reading = false
                        });
                    }
                    if (lista[0].continentalpartnumber != "")//null || lista[0].continentalpartnumber !=string.Empty
                    {
                        saveTagList(lista);
                        reader.DeleteAllOpSequences();
                    }

                });
                reader.Start();
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnStart.Enabled = true;
            }

        }

        private void saveTagList(List<ReadTag> lista)
        {
            try
            {


                char[] charsToTrim = { '@', ' ' };
                string result = lista[0].continentalpartnumber.Trim(charsToTrim);

                #region Nueva Prueba
                string spath = txtFile.Text;

                if (System.IO.File.Exists(spath))
                {
                    System.IO.File.Delete(spath);
                }
                using (XmlWriter writer = XmlWriter.Create(spath))
                {
                    writer.WriteStartElement("ArrayOfReadTag");
                    writer.WriteEndElement();
                    writer.Flush();
                }
                XDocument doc = XDocument.Load(spath);
                XElement root = new XElement("ReadTag");
                root.Add(new XElement("continentalpartnumber", result));
                root.Add(new XElement("Reading", lista[0].Reading));
                doc.Element("ArrayOfReadTag").Add(root);
                doc.Save(spath);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (reader.IsConnected)
                {
                    reader.Stop();
                    reader.Disconnect();
                }
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



      
    }
    [Serializable]
    public class ReadTag
    {
        public string continentalpartnumber { get; set; }
        public bool Reading { get; set; }
    }

    [Serializable]
    public class ReadTag2
    {
        public string PartNumber { get; set; }
        public string Quantity { get; set; }
    }
}
