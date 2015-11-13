using Microsoft.ServiceBus.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Xaml.Blend.Tutorial.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SendMPNS();
            Console.ReadKey();
        }

        static async void SendMPNS()
        {
            string text = "Hello World!";
            NotificationHubClient hub = NotificationHubClient
                    .CreateClientFromConnectionString("Endpoint=sb://karolzakpip.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=J2Rrw4Xt67NADIOprmmEiyPX89LCOrcP+8WkCBMsuXw=", "karolzakpip");
            string toast = prepareToastPayload(text, text);
            var x = await hub.SendMpnsNativeNotificationAsync(toast);
            
        }
        private static string prepareToastPayload(string text1, string text2)
        {
            // Create encoding manually in order to prevent
            // creation of leading BOM (Byte Order Mark) xFEFF at start
            // of string created from the XML
            Encoding Utf8 = new UTF8Encoding(false); // Prevents creation of BOM
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = false,
                //   Encoding = Encoding.UTF8    !!NO-> adds Unicode BOM to start
                Encoding = Utf8,    // Use manually created UTF8 encoding
            };
            XmlWriter writer = XmlWriter.Create(stream, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("wp", "Notification", "WPNotification");
            writer.WriteStartElement("wp", "Toast", "WPNotification");
            writer.WriteStartElement("wp", "Text1", "WPNotification");
            writer.WriteValue(text1);
            writer.WriteEndElement();
            writer.WriteStartElement("wp", "Text2", "WPNotification");
            writer.WriteValue(text2);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
