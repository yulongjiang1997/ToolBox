using System;
using ToolBox.Http;
using ToolBox.DateTimeTool;
using ToolBox.Log;
using ToolBox.SocketCore;

namespace ToolBox.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //Console.WriteLine(((long)1561815867).UnixTimeStamp10ToBeijingTime());
            //Console.WriteLine(((long)1563073506).UnixTimeStamp10ToBeijingTime());
            //string a = "";
            //Console.WriteLine(a == "" ? "0" : a);
            //Console.ReadKey();


            TcpServer tcpServer = new TcpServer();



            //tcpServer.OnSuccess = new Action<string>((s) =>
            //{

            //    Console.WriteLine(s);
            //});


            //tcpServer.OnMessage = new Action<string>((s) =>
            //{

            //    Console.WriteLine(s);
            //});

            tcpServer.StartServer(1669);

            Console.WriteLine("5555");

            Console.ReadLine();


        }

        public enum test
        {
            test1,
            test2
        }
    }
}
