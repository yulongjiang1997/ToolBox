using System;
using ToolBox.Http;
using ToolBox.DateTimeTool;
using ToolBox.Log;
using ToolBox.Socket;

namespace ToolBox.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            TcpClient tcpClient = new TcpClient();

            tcpClient.StartConnect(1956);

         
            tcpClient.OnSuccess = new Action<TcpClient>((c) =>
            {
             
                tcpClient.SendMsg("6666");



            });

            tcpClient.OnMessage = (s) =>
            {
                Console.WriteLine(s);
            };

            tcpClient.OnRecMessage = (s, c) =>
            {

                Console.WriteLine("接收回来的"+s);
            };


            tcpClient.OnError = (ex) =>
            {
                Console.WriteLine(ex);
            };


        

            Console.ReadLine();


        }

        public enum test
        {
            test1,
            test2
        }
    }
}
