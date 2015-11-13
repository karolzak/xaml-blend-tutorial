using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.ServiceBus.Notifications;
using System.Text;
using System.IO;
using System.Xml;

namespace karolzakpipService.Controllers
{
    public class BroadcasterController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/Custom
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello";
        }

        [AuthorizeLevel(AuthorizationLevel.User)]
        public async Task<bool> Post(JObject data)
        {

            
            try
            {
                //TODO: Trzeba rozpoznawać użytkowników i wysyłać do właściwych urzadzeń
                string text = data.GetValue("message").Value<string>();
                NotificationHubClient hub = NotificationHubClient
                    .CreateClientFromConnectionString("Endpoint=sb://karolzakpip.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=J2Rrw4Xt67NADIOprmmEiyPX89LCOrcP+8WkCBMsuXw=", "karolzakpip");
                string toast = PrepareToastPayload(text, text);
                await hub.SendMpnsNativeNotificationAsync(toast);
              


                //Tak np. wygląda wysyłanie notyfikacji dla Apple
                //NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString("<connection string with full access>", "<hub name>");
                //var alert = "{\"aps\":{\"alert\":\"Hello from .NET!\"}}";
                //await hub.SendAppleNativeNotificationAsync(alert);

                string wnsToast = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">{0}</text></binding></visual></toast>", data.GetValue("message").Value<string>());
                WindowsPushMessage message = new WindowsPushMessage();
                message.XmlPayload = wnsToast;
                //await Services.Push.SendAsync(message);
                await hub.SendWindowsNativeNotificationAsync(wnsToast);

                return true;
            }
            catch (Exception e)
            {
                Services.Log.Error(e.ToString());
            }
            return false;
        }

        private string PrepareToastPayload(string text1, string text2)
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
