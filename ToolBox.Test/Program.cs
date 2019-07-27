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

            tcpServer.StartServer(1985);
    
            Console.ReadKey();
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
