using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using ToolBox.Log;
using ToolBox.Socket;


namespace ToolBox.Framework.Test
{



    class Program
    {
        static void Main(string[] args)
        {

            TcpClient tcpClient = new TcpClient();
            tcpClient.StartConnect(1985);

            tcpClient.OnSuccess = (s) =>
            {

                Console.WriteLine("成功");
            };

            tcpClient.OnMessage = (s) =>
            {

                Console.WriteLine(s);

            };

            tcpClient.OnError = (s) =>
            {
                Console.WriteLine(s);
            };


            Console.ReadLine();
            //wssv.Stop();
        }

   
    }
}
