using System;
using ToolBox.Http;
using ToolBox.DateTimeTool;
using ToolBox.Log;
using System.Threading.Tasks;
using ToolBox.Socket;
using ToolBox.Phone;

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


            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("PCAM00"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("Nokia X6"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("TAS-AN00"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("LIO-AN00"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("TNY-AL00"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("vivo X21A"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("MI CC9 Pro Premium Edition"));
            Console.WriteLine(new PhoneModelNumberTool().ModelNumberToName("WLZ-AL10"));
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
