using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ToolBox.SocketCore
{
    /// <summary>
    /// 服务器端的客户
    /// </summary>
    public class ClientMode
    {

        /// <summary>
        /// 客户端构造函数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="thread"></param>
        /// <param name="socket"></param>
        /// <param name="id"></param>
        public ClientMode(string ip, Thread thread, Socket socket, string id)
        {

            this.ip = ip;
            this.socket = socket;
            this.thr = thread;
            this.id = id;
            lastTickTime = SocketTools.GetTimeStamp();

        }


        public string ip { get; set; }                                         //客户端ip
        public string id { get; set; }                                         //客户端id
        public Thread thr { get; set; }                                           //客户端线程
        public System.Net.Sockets.Socket socket { get; set; }                                   //客户端套接字
        public long lastTickTime { get; set; } = long.MinValue;                 //时间标签



    }
}
