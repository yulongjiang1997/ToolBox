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
    public partial class TcpServer
    {

        #region 变量


        /// <summary>
        /// 服务端监听套接字
        /// </summary>
        private System.Net.Sockets.Socket socketWatch = null;

        /// <summary>
        /// 客户端的字典
        /// </summary>
        private Dictionary<string, ClientMode> dictsocket = new Dictionary<string, ClientMode>();

        /// <summary>
        /// 读写线程锁
        /// </summary>
        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// Heartbeat time (by default, the client is cleared if no heartbeat event is received for more than 7 seconds)
        /// 心跳时间（默认超过7秒没收到心跳事件就把客户端清除）
        /// </summary>
        public long HearTime { get; set; } = 7;

        /// <summary>
        /// 心跳检查间隔（Heartbeat check interval）
        /// </summary>
        public int HeartbeatCheckInterval { get; set; } = 3000;

        /// <summary>
        ///  (是否打开des加密，默认是打开)Whether to open des encryption, open by default
        /// </summary>
        public bool IsOpenDesEnc { get; set; } = true;

        #endregion


        /// <summary>
        /// Set key,the key is length must >=8 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void SetEncryptKey(string key) {

            if (!SocketDesHelper.SetEncryptKey(key)) {

                throw new Exception("The key is set incorrectly, the length is greater than or equal to 8, and cannot contain Chinese.");

            }           
   
        }


        #region 开始服务器

        /// <summary>
        /// 开始服务器
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="count">连接队列总数（默认50）</param>
        /// <param name="ip">ip地址（默认本机ip）</param>
        public void StartServer(int port, int count = 50, string ip = "127.0.0.1")
        {
            socketWatch = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAdr = IPAddress.Parse(ip);
            IPEndPoint iPEnd = new IPEndPoint(ipAdr, port);

            try
            {
                socketWatch.Bind(iPEnd);
            }
            catch (Exception ex)
            {

                OnError?.Invoke($"Startup exception : {ex.ToString()}");
                OnSuccess?.Invoke(false);
                return;
            }

            socketWatch.Listen(count);

            //开启心脏检测
            Task.Run(() =>
            {
                while (true)
                {
                    HearBeat();
                    Thread.Sleep(HeartbeatCheckInterval);
                }

            });

            // 监听客户端请求的方法，线程
            Task.Run(() =>
            {

                while (true)
                {

                    System.Net.Sockets.Socket conn = socketWatch.Accept();
                    string socketip = conn.RemoteEndPoint.ToString();
                 
                    conn.Send(SocketTools.GetBytes("YouIP," + socketip, IsOpenDesEnc));

                    Thread thr = new Thread(RecMsg);
                    thr.IsBackground = true;
                    thr.Start(conn);

                    string id;

                    AddSocketClient(socketip, conn, thr, out id);

                    OnClientAdd?.Invoke(this, new SocketArgs(new ClientInfo() { ip = socketip, id = id }));

                }

            });

            OnSuccess?.Invoke(true);

        }

        #endregion





        #region 接收信息

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="socket"></param>
        private void RecMsg(object socket)
        {
            int headSize = 4;
            byte[] surplusBuffer = null;
            System.Net.Sockets.Socket sokClient = socket as System.Net.Sockets.Socket;
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
                                string strc = Encoding.UTF8.GetString(surplusBuffer, haveRead + headSize, bodySize).StringDecrypt(IsOpenDesEnc);

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

                                            OnDebug?.Invoke($"Update timestamp：{SocketTools.GetTimeStamp()} -  ss[1].ToString()");

                                            socketClient.lastTickTime = SocketTools.GetTimeStamp();

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        OnError?.Invoke($"Heartbeat error: {ex.ToString()}");
                                    }
                                    finally
                                    {
                                        lockSlim.ExitWriteLock();
                                    }

                                }
                                else
                                {
                                    OnRecMessage?.Invoke(socketip, strc);
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

                    //OnError?.Invoke($"Client thread error:{socketip} " );
                    break;
                }


            }

        }

        #endregion

    }



}
