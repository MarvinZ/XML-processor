using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace GraingerInvoicesSniffer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("I am going to count all the invoices...");
            var lines = new List<string>();

            var baseFolderPath = "C:\\Users\\marv_\\Desktop\\Grainger_invoices\\";
            var list = new List<string>();
            foreach (string file in Directory.GetFiles(baseFolderPath, "*.xml"))
            {
                list.Add(file);
            }






            Decimal total = 0;
            foreach (var invoice in list)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(baseFolderPath, invoice));
                XmlNode root = doc.DocumentElement;
                //https://xmltoolbox.appspot.com/xpath_generator.html 
                XmlNodeList nodes = root.SelectNodes("/cXML/Request/InvoiceDetailRequest/InvoiceDetailSummary/NetAmount");
                var netAmount = nodes[0].InnerText;


                XmlNodeList invoiceIdNode = root.SelectNodes("/cXML/Request/InvoiceDetailRequest/InvoiceDetailRequestHeader");
                var invoiceOuter = invoiceIdNode[0].OuterXml;
                XmlDocument tempInvoiceDoc = new XmlDocument();
                tempInvoiceDoc.LoadXml(invoiceOuter);
                XmlElement tempInvoiceRoot = tempInvoiceDoc.DocumentElement;
                string invoiceId = tempInvoiceRoot.Attributes["invoiceID"].Value;


                XmlNodeList nodes3 = root.SelectNodes("/cXML/Request/InvoiceDetailRequest/InvoiceDetailOrder/InvoiceDetailOrderInfo/OrderReference");
                var totalnode3 = nodes3[0].OuterXml;
                XmlDocument doc5 = new XmlDocument();
                doc.LoadXml(totalnode3);
                XmlElement root5 = doc.DocumentElement;
                string OrderId = root5.Attributes["orderID"].Value;
                var lineResult = invoice + "     " + netAmount + "     " + invoiceId + "     " + OrderId;
                var lineResultExport = "Insert into #GTemp values ('"+invoiceId+"','"+ OrderId+ "','"+netAmount+"')";

                Console.WriteLine(lineResult);
                lines.Add(lineResultExport);

                total = total + Decimal.Parse(netAmount);
            }

            Console.WriteLine("Total invoices: " + list.Count.ToString());
            Console.WriteLine("Total of all the invoices is: " + total.ToString());


            using (System.IO.StreamWriter file =
        new System.IO.StreamWriter(@"C:\\Users\\marv_\\Desktop\\Grainger_invoices\\Result.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }


            Console.ReadLine();

        }




    }
}
