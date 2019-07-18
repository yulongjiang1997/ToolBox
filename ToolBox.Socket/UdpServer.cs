using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ToolBox.Socket
{

    public class MsgArgs : EventArgs {

        public string message { get; set; }

        public MsgArgs(string msg) {

            message = msg;
        }

    }




    /// <summary>
    /// udp服务器
    /// </summary>
    public class UdpServer
    {
        /// <summary>
        /// 套接字
        /// </summary>
        public System.Net.Sockets.Socket server;

        /// <summary>
        /// 错误输出
        /// </summary>
        public event EventHandler<MsgArgs> OnError;

        /// <summary>
        /// 接收信息输出
        /// </summary>
        public event EventHandler<MsgArgs> OnReciveMsg;

        /// <summary>
        /// 成功输出
        /// </summary>
        public event EventHandler<MsgArgs> OnSuccess;


        private string ip { get; set; }

        private int port { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public UdpServer(string ip,int port) {

            this.ip=ip;
            this.port = port;

        }


        /// <summary>
        /// 开始udp连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect() {

            try
            {
                server = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                server.Bind(new IPEndPoint(IPAddress.Parse(ip), port));//绑定端口号和IP
                Thread t2 = new Thread(ReciveMsg);
                t2.Start();
                
                OnSuccess?.BeginInvoke(this, new MsgArgs("成功启动"), null, null);
            }
            catch (Exception ex)
            {        
                OnError?.BeginInvoke(this, new MsgArgs(ex.ToString()), null, null);
            }


        }

   

        /// <summary>
        ///  向特定ip的主机的端口发送数据报
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void sendMsg(string msg, string ip, int port)
        {

            try
            {
                EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);

                server.SendTo(Encoding.UTF8.GetBytes(msg), point);
            }
            catch (Exception ex)
            {

                OnError?.BeginInvoke(this, new MsgArgs(ex.ToString()), null, null);
            }


            

        }


        /// <summary>
        /// 接收到的数据
        /// </summary>
        public void ReciveMsg() {


            try
            {
                while (true)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号

                    byte[] buffer = new byte[1024];
                    int length = server.ReceiveFrom(buffer, ref point);//接收数据报
                    string message = Encoding.UTF8.GetString(buffer, 0, length);

                    //  Console.WriteLine(point.ToString() + message);

                    OnReciveMsg?.BeginInvoke(this, new MsgArgs(point.ToString()+","+ message), null, null);

                }
            }
            catch (Exception ex)
            {

                OnError?.BeginInvoke(this, new MsgArgs(ex.ToString()), null, null);
            }

   
        }




    }



}
