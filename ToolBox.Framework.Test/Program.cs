using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using ToolBox.Files;
using ToolBox.Socket;

namespace ToolBox.Framework.Test
{
    public class Laputa : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            
            Send(e.Data);
         
        }     
    }


    class Program
    {
        static void Main(string[] args)
        {

            UdpServer udpServer = new UdpServer("127.0.0.1",1256);

            udpServer.OnSuccess += (a, b) =>
            {

                Console.WriteLine(b.message);

            };

            udpServer.OnReciveMsg += (a, b) => {

                Console.WriteLine(b.message);
            };

            udpServer.Connect();

            

            //var wssv = new WebSocketServer("ws://127.0.0.1:8081");

            
            
            //wssv.AddWebSocketService<Laputa>("/");
           
            //wssv.Start();


            Console.ReadKey(true);
            //wssv.Stop();
        }

   
    }
}
