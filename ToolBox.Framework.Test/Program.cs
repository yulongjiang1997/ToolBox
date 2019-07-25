using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using ToolBox.Log;
using ToolBox.Security;


namespace ToolBox.Framework.Test
{



    class Program
    {
        static void Main(string[] args)
        {

            DesHelper.encryptKey = "abc1231a";


           string aa=  DesHelper.Encrypt("aidenxl55445465");


            Console.WriteLine(aa);

          string ss=   DesHelper.Decrypt(aa);

            Console.WriteLine();
            Console.WriteLine(ss);

            Console.ReadLine();
            //wssv.Stop();
        }

   
    }
}
