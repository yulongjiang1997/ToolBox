using System;
using ToolBox.Http;
using ToolBox.DateTimeTool;
using ToolBox.Log;
using System.Threading.Tasks;
using ToolBox.Socket;

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
            tcpServer.IsOpenDesEnc = false;
            tcpServer.SetEncryptKey("ddccbbaa");

            tcpServer.OnSuccess = (s) =>
            {
                if (s)
                {
                    Console.WriteLine("运行成功~");
                }

            };

            

            tcpServer.StartServer(1988);

            tcpServer.OnRecMessage += TcpServer_OnRecMessage;
   

            tcpServer.OnClientAdd += TcpServer_OnClientAdd;

            tcpServer.OnClientClose += TcpServer_OnClientClose;

          


    
            Console.ReadKey();
        }

        private static void TcpServer_OnRecMessage(object sender, SocketMsgArgs e)
        {
            Console.WriteLine($"接收到{e.clientRecInfo.ip} + {e.clientRecInfo.msg}");


        }

        private static void TcpServer_OnClientClose(object sender, SocketArgs e)
        {
            Console.WriteLine($"退出了{e.ClientInfo.id} + {e.ClientInfo.ip}");
        }

        private static void TcpServer_OnClientAdd(object sender, SocketArgs e)
        {
            Console.WriteLine($"添加了{e.ClientInfo.id} + {e.ClientInfo.ip}");
        }

        public enum test
        {
            test1,
            test2
        }

        private static void RunTask()
        {
            Task.Run(() =>
            {
                "test1".WriteErrorLog();
                "test2".WriteErrorLog();
                "test3".WriteErrorLog();
                "test4".WriteErrorLog();
                "test5".WriteErrorLog();
                "test6".WriteErrorLog();
                "test7".WriteErrorLog();
                "test8".WriteErrorLog();
                "test9".WriteErrorLog();
                "test10".WriteErrorLog();
                "test11".WriteErrorLog();
                "test1".WriteDebugLog();
                "test2".WriteDebugLog();
                "test3".WriteDebugLog();
                "test4".WriteDebugLog();
                "test5".WriteDebugLog();
                "test6".WriteDebugLog();
                "test7".WriteDebugLog();
                "test8".WriteDebugLog();
                "test9".WriteDebugLog();
                "test10".WriteDebugLog();
                "test11".WriteDebugLog();
            });
        }
    }
}
