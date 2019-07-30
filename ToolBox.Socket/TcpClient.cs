using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ToolBox.Socket
{
    /// <summary>
    /// TCP客户端
    /// </summary>
    public  class TcpClient
    {
        /// <summary>
        /// 客户端套接字
        /// </summary>
        public System.Net.Sockets.Socket  mySocket;

        /// <summary>
        /// 接收线程
        /// </summary>
        public Thread recThread;

        /// <summary>
        /// 本地ip
        /// </summary>
        public string EndPointIp;


        private bool IsReceive = false;


        /// <summary>
        /// 心跳检查间隔（Heartbeat check interval）
        /// </summary>
        public int HeartbeatCheckInterval { get; set; } = 3000;


        /// <summary>
        ///  (是否打开des加密，默认是打开)Whether to open des encryption, open by default
        /// </summary>
        public bool IsOpenDesEnc { get; set; } = true;

        /// <summary>
        /// 开始连接服务器
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ip"></param>
        public void StartConnect(int port, string ip = "127.0.0.1")
        {

            try
            {
                mySocket = new System.Net.Sockets.Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(ip);
                IPEndPoint endPoint = new IPEndPoint(address, port);

                //开始异步连接
                mySocket.BeginConnect(endPoint, asyncResult =>
                {

                    try
                    {
                        mySocket.EndConnect(asyncResult);                        //结束异步连接
                       // localEndPointIp = mySocket.LocalEndPoint.ToString();     //得到ip地址

                        OnSuccess?.Invoke(this);   //连接成功的回调

                        recThread = new Thread(RecMsg);
                        recThread.IsBackground = true;
                        recThread.Start(mySocket);


                        Task.Run(() =>
                        {
                            while (true) {

                                if (mySocket != null && IsReceive)
                                {
                                    string ss = EndPointIp;
                                    SendMsg("hear," + ss);

                                }
                                Thread.Sleep(HeartbeatCheckInterval);

                            }

                        });

   
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(ex);
                    }

                }, null);

            }
            catch (Exception ex)
            {
      
                OnError?.Invoke(ex);    //报错的回调
            }


        }



        /// <summary>
        /// Set key,the key is length must >=8 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void SetEncryptKey(string key)
        {

            if (!SocketDesHelper.SetEncryptKey(key))
            {

                throw new Exception("The key is set incorrectly, the length is greater than or equal to 8, and cannot contain Chinese.");

            }

        }




        /// <summary>
        /// 接收信息的线程
        /// </summary>
        private void RecMsg(object socket)
        {
            int headSize = 4;
            byte[] surplusBuffer = null;

            System.Net.Sockets.Socket  mySocket = socket as System.Net.Sockets.Socket ;

            while (true)
            {

                int count = -1;

                try
                {
                    byte[] vs = new byte[1024];
                    count = mySocket.Receive(vs);
                    int bytesRead = vs.Length;

                    if (bytesRead > 0)
                    {
                        surplusBuffer = vs;
                    }
                    else
                    {
                        surplusBuffer = surplusBuffer.Concat(vs).ToArray();
                    }

                    int haveRead = 0;
                    int totalLen = surplusBuffer.Length;

                    while (haveRead <= totalLen)
                    {

                        if (totalLen - haveRead < headSize)
                        {
                            byte[] byteSub = new byte[totalLen - haveRead];
                            Buffer.BlockCopy(surplusBuffer, haveRead, byteSub, 0, totalLen - haveRead);
                            surplusBuffer = byteSub;
                            totalLen = 0;
                            break;
                        }

                        //如果是够一个完整包，读取包头的数据
                        byte[] headByte = new byte[headSize];
                        Buffer.BlockCopy(surplusBuffer, haveRead, headByte, 0, headSize);

                        int bodySize = BitConverter.ToInt32(headByte, 0);

                        if (bodySize == 0)
                        {

                            surplusBuffer = null;
                            totalLen = 0;
                            break;

                        }


                        if (haveRead + headSize + bodySize > totalLen)
                        {

                            byte[] byteSub = new byte[totalLen - haveRead];
                            Buffer.BlockCopy(surplusBuffer, haveRead, byteSub, 0, totalLen - haveRead);
                            surplusBuffer = byteSub;
                            // Console.WriteLine("不够一个包，拆出来保存");
                            break;

                        }
                        else
                        {

                            string strc = Encoding.UTF8.GetString(surplusBuffer, haveRead + headSize, bodySize).StringDecrypt(IsOpenDesEnc);

                            if (IsReceive == false)
                            {
                                string[] ss = strc.Split(',');
                                if (ss.Count()== 2 && ss[0].ToString().Equals("YouIP")) {
                                    EndPointIp = ss[1].ToString().Trim();
                                }
                                IsReceive = true;
                            }
                            else {

                                // Console.WriteLine("------------结果：" + strc);
                                OnRecMessage?.Invoke(strc, this);

                            }

                            haveRead = haveRead + headSize + bodySize;

                            if (headSize + bodySize == bytesRead)
                            {
                                surplusBuffer = null;
                                totalLen = 0;
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                  //  Console.WriteLine("接收的线程组错误信息：" + ex);

                    OnError?.Invoke(ex);
                    break;

                }

            }

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {

            if (mySocket != null && recThread != null)
            {
                mySocket.Close();
                recThread = null;
            }

        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {

            if (mySocket.Connected)
            {
                mySocket.Send(SocketTools.GetBytes(msg,IsOpenDesEnc));
            }
            else
            {
                 OnMessage?.Invoke("Not connected to the server~");

            }
        }



        /// <summary>
        /// 连接成功
        /// </summary>
        public Action<TcpClient> OnSuccess { get; set; }    //连接成功


        /// <summary>
        /// 接收消息
        /// </summary>
        public Action<string, TcpClient> OnRecMessage { get; set; }     //接收消息

       
        /// <summary>
        /// 错误处理
        /// </summary>
        public Action<Exception> OnError { get; set; }

        /// <summary>
        /// 接收信息
        /// </summary>
        public Action<string> OnMessage { get; set; }
 



    }
}
