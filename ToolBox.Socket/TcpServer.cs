using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace ToolBox.Socket
{
    public partial class TcpServer
    {
     
            private Thread threadWatch = null;    //负责监听连接的线程
            private Socket socketWatch = null;    //服务端监听套接字

            //客户端的字典
            private Dictionary<string, ClientMode> dictsocket = new Dictionary<string, ClientMode>();

            //线程锁
            private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

            System.Timers.Timer timer;        //计时器
            private long HearTime = 10;         //心跳时间


            /// <summary>
            /// 开始服务器
            /// </summary>
            /// <param name="port">端口号</param>
            /// <param name="count">连接队列总数（默认50）</param>
            /// <param name="ip">ip地址（默认本机ip）</param>
            public void StartServer(int port, int count = 50, int time = 2000,long hearTime=10, string ip = "127.0.0.1")
            {

              HearTime = hearTime;
             socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAdr = IPAddress.Parse(ip);
                IPEndPoint iPEnd = new IPEndPoint(ipAdr, port);

                try
                {
                    socketWatch.Bind(iPEnd);
                }
                catch (Exception ex)
                {
                    writeMsg("启动服务时的异常：" + ex.ToString());
                    return;

                }

                socketWatch.Listen(count);

                timer = new System.Timers.Timer(time);

                threadWatch = new Thread(WatchConnecting);
                threadWatch.IsBackground = true;
                threadWatch.Start();

                TimerInit();
                writeMsg("服务器启动监听成功~");

            }


        #region 心跳事件方法

        /// <summary>
        /// 初始化计时函数
        /// </summary>
        private void TimerInit()
        {

            timer.Elapsed += new ElapsedEventHandler(HandleMainTimer);
            timer.AutoReset = false;
            timer.Enabled = true;

        }

        /// <summary>
        /// 线程handleMainTimer函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleMainTimer(object sender, ElapsedEventArgs e)
        {

            HearBeat();
            timer.Start();

        }

        #endregion


        /// <summary>
        /// 写入输出信息
        /// </summary>
        /// <param name="msg"></param>
        private void writeMsg(string msg)
        {

            HandleMsg?.BeginInvoke(msg, null, null);
        }


        /// <summary>
        /// 监听客户端请求的方法，线程
        /// </summary>
        private void WatchConnecting()
        {

            while (true)
            {
                Socket conn = socketWatch.Accept();
                string socketip = conn.RemoteEndPoint.ToString();

                writeMsg("进来新的客户端ip:" + socketip);

                Thread thr = new Thread(RecMsg);
                thr.IsBackground = true;
                thr.Start(conn);

                AddSocketClient(socketip, conn, thr);

            }

        }


        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="socket"></param>
        private void RecMsg(object socket)
        {
            int headSize = 4;
            byte[] surplusBuffer = null;
            Socket sokClient = socket as Socket;
            string socketip = sokClient.RemoteEndPoint.ToString();

            while (true)
            {
                int count = -1;
                try
                {
                    byte[] vs = new byte[1024];
                    count = sokClient.Receive(vs); // 接收数据，并返回数据的长度；
                    int bytesRead = vs.Length;

                    if (bytesRead > 0)
                    {

                        if (surplusBuffer == null)
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
                                //Console.WriteLine("不够一个包~");
                                byte[] byteSub = new byte[totalLen - haveRead];
                                Buffer.BlockCopy(surplusBuffer, haveRead, byteSub, 0, totalLen - haveRead);
                                surplusBuffer = byteSub;
                                totalLen = 0;
                                break;
                            }

                            //如果是够一个完整包了，帽读取包头的数据
                            byte[] headByte = new byte[headSize];
                            Buffer.BlockCopy(surplusBuffer, haveRead, headByte, 0, headSize);

                            int bodySize = BitConverter.ToInt32(headByte, 0);   //得到长度

                            if (bodySize == 0)
                            {
                                surplusBuffer = null;
                                totalLen = 0;
                                break;
                            }

                            //这里的 haveRead=等于N个数据包的长度 从0开始；0,1,2,3....N
                            //如果自定义缓冲区拆解N个包后的长度 大于 总长度，说最后一段数据不够一个完整的包了，拆出来保存
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
                                string strc = Encoding.UTF8.GetString(surplusBuffer, haveRead + headSize, bodySize);

                                string[] ss = strc.Split(',');

                                //心跳事件，更新客户端的最后登陆时间
                                if (ss.Count() == 2 && ss[0].ToString().Equals("hear"))
                                {

                                    // 心跳事件 0=hert,1=ip
                                    lockSlim.EnterWriteLock();

                                    try
                                    {

                                        ClientMode socketClient;
                                        if (dictsocket.TryGetValue(ss[1].ToString(), out socketClient))
                                        {
                                            socketClient.lastTickTime = SocketTools.GetTimeStamp();

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("心跳事件报错：:" + ex.ToString());

                                    }
                                    finally
                                    {
                                        lockSlim.ExitWriteLock();
                                    }

                                }
                                else
                                {
                                    HandleRecMsg?.BeginInvoke(null, strc, null, null);
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

                }
                catch (Exception ex)
                {
                    ReMoveSocketClient(socketip);
                    Console.WriteLine("接收客户端的线程报错" + socketip);
                    break;
                }


            }

        }






    }


    
}
