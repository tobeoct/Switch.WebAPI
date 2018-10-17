using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using System.Xml.Serialization;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using Switch.WebAPI.Models;


namespace Switch.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static List<SourceNode> getSourceNodes()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            try
            {
                var sourceNode = _context.SourceNodes.Include(c => c.Scheme).Include(c => c.Scheme.Fee)
                    .Include(c => c.Scheme.Route).Include(c => c.Scheme.Route.SinkNode).Include(c => c.Scheme.Channel)
                    .Include(c => c.Scheme.TransactionType).ToList();
                return sourceNode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        [EnableCors("*", "*", "*")]
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);
//            string serverInfo = "127.0.0.1:85";
//            string serverInfo = "127.0.0.1:90";
            string serverIp = "127.0.0.1";
//            int serverPort = 87;
            int serverPort = 99;
//            int serverPort = 8080;
            var messageToSend = new Message()
            {
                Amount = 2000.00,
                Channel = 10,
                TransactionType = 01
            };
           
            
            var nodes = getSourceNodes();
//            var message = SerializeObject<List<SourceNode>>(nodes);
            var message = SerializeObject<Message>(messageToSend);
//            var message = "Hey!!!, I got a message";
            ConnectToServer(serverIp, serverPort, message);
//            System.Net.IPAddress remoteIpAddress = System.Net.IPAddress.Parse("0.0.0.0");
//            System.Net.IPEndPoint localEndPoint = new System.Net.IPEndPoint(remoteIpAddress,  System.Convert.ToInt16("83", 10));

//            ConfigureSockets(messageToSend, serverInfo);
        }

        public static void ConnectToServer(string serverIp, int serverPort,string message)
        {
                       
            
            
            TcpClient client = new TcpClient();
            var serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            Int64 bytecount = Encoding.ASCII.GetByteCount(message);
            byte[] sendData = new byte[bytecount+1];
            sendData = Encoding.ASCII.GetBytes(message+";");
            Console.WriteLine("Connecting to Server");

            client.Connect(serverEndPoint);
            NetworkStream stream = client.GetStream();
            Console.WriteLine("Connected to Server");
            stream.Write(sendData, 0, sendData.Length);
            stream.Close();
            client.Close();
        }
        public static void ConfigureSockets(Message messageToSend, string serverInfo)
        {
            //Request server IP and port number
//            Console.WriteLine("Please enter the server IP and port in the format 192.168.0.1:10000 and press return:");
            

            //Parse the necessary information out of the provided string
            string serverIP = serverInfo.Split(':').First();
            int serverPort = int.Parse(serverInfo.Split(':').Last());


            //Write some information to the console window


//            NetworkComms.SendObject("Message", serverIP, serverPort, "OK");
            //            NetworkComms.SendObject("Message", serverIP, serverPort, SerializeObject<Message>(messageToSend));
            var message = getSourceNodes();
            NetworkComms.SendObject("Message", serverIP, serverPort, SerializeObject<List<SourceNode>>(message));


            //We have used comms so we make sure to call shutdown
            NetworkComms.Shutdown();
        }

        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
