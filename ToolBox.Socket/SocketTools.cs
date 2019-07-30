using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ToolBox.Socket
{

    /// <summary>
    /// 套接字工具类
    /// </summary>
    public static  class SocketTools
    {

        /// <summary>
        /// 得到二进制字节，已经添加了包头
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string data)
        {

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
            return newBytes;

        }


        /// <summary>
        /// Get the binary byte, the header has been added
        /// </summary>
        /// <param name="data"></param>
        /// <param name="IsEncryption"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string data, bool IsEncryption = true) {

            if (IsEncryption)
            {
                return GetBytes(data.Encrypt());
            }
            else {

                return GetBytes(data);
            }

        }


        /// <summary>
        /// 字符串解密（Stringdecryption）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IsEncryption"></param>
        public static string  StringDecrypt(this string str, bool IsEncryption) {

            if (IsEncryption)
            {
                return str.Decrypt();
            }
            else {

                return str;
            }

        }



        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="IsEncryption"></param>
        public static void SendMsg(System.Net.Sockets.Socket socket,string data,bool IsEncryption=true) {

            if (IsEncryption)
            {
                SendMsg(socket, data.Encrypt());
            }
            else {

                SendMsg(socket, data);
            }


        }



        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="data">要发送的信息</param>
        private static void SendMsg(System.Net.Sockets.Socket socket , string data)
        {

            try
            {
                byte[] newBytes =  GetBytes(data);

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
        public static bool IsSocketConnected(this System.Net.Sockets.Socket socket)
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
