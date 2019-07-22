using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Socket
{

    public partial class TcpServer
    {




        /// <summary>
        /// 成功启动后的回调函数
        /// </summary>
        public Action<string> OnSuccess { get; set; }             

        /// <summary>
        /// 添加一个用客户端后的回调函数
        /// </summary>
        public Action<string> OnClientAdd { get; set; }

        /// <summary>
        /// 服务端异常回调
        /// </summary>
        public Action<ClientMode, string> OnServerError { get; set; }


        /// <summary>
        /// 处理接收消息的回调函数
        /// </summary>
        public Action<ClientMode, string> OnRecMessage { get; set; }                     

        /// <summary>
        /// 处理消息回调函数
        /// </summary>
        public Action<string> OnMessage { get; set; }                                 

        /// <summary>
        /// 客户端关闭的回调函数
        /// </summary>
        public Action<string > OnClientClose { get; set; }


        /// <summary>
        /// 异常输出的回调函数
        /// </summary>
        public Action<string> OnError { get; set; }




    }
}
