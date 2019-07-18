using System;
using ToolBox.Http;
using ToolBox.DateTimeTool;
using ToolBox.Log;

namespace ToolBox.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine(((long)1561815867).UnixTimeStamp10ToBeijingTime());
            Console.WriteLine(((long)1563073506).UnixTimeStamp10ToBeijingTime());

            LogUtil log = new LogUtil("logs/");
            log.Debug("这是debug信息");
            log.Info("这是Info信息");
            log.Error("这是Error信息");

            Console.ReadKey();
        }

        public enum test
        {
            test1,
            test2
        }
    }
}
