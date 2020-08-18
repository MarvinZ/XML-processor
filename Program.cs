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
                XmlNodeList nodes = root.SelectNodes("/cXML/Request/InvoiceDetailRequest/InvoiceDetailSummary/NetAmount");
                var totalnode = nodes[0].InnerText;
                Console.WriteLine(invoice+ "     " + totalnode);
                total = total + Decimal.Parse(totalnode);

            }

            Console.WriteLine("Total invoices: " + list.Count.ToString());
            Console.WriteLine("Total of all the invoices is: " + total.ToString());

            Console.ReadLine();

        }
    }
}
