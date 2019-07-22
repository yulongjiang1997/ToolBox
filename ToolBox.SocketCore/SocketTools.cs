using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ToolBox.SocketCore
{

    /// <summary>
    /// 套接字工具类
    /// </summary>
    public  class SocketTools
    {

        /// <summary>
        /// 得到二进制字节，已经添加了包头
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string data)
        {

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
            return newBytes;

        }


        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="date">要发送的信息</param>
        public static void SendMsg(System.Net.Sockets.Socket socket , string data)
        {

            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                int dataLength = dataBytes.Length;
                byte[] lengthBytes = BitConverter.GetBytes(dataLength);
                byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();

                if (socket.Connected)
                {
                    socket.Send(newBytes);
                }
                else
                {
                    Console.WriteLine("没有跟服务器连接~");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("发送信息时的报错:" + ex);
            }


        }


        /// <summary>
        /// 是否连接了
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static bool IsSocketConnected(System.Net.Sockets.Socket socket)
        {

            bool part1 = socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (socket.Available == 0);

            if (part1 && part2)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// 得到时间便签
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {

            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }




    }
}
