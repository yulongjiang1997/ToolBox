using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Socket
{

    public partial class TcpServer
    {

        /// <summary>
        /// all debug info Output
        /// </summary>
        public Action<string> OnDebug { get; set; }

        /// <summary>
        /// 成功启动后的回调函数
        /// </summary>
        public Action<bool> OnSuccess { get; set; }             



        /// <summary>
        /// 处理接收消息的回调函数
        /// </summary>
        public Action<string , string> OnRecMessage { get; set; }                     

        /// <summary>
        /// 处理消息回调函数
        /// </summary>
        public Action<string> OnMessage { get; set; }                                 

        /// <summary>
        /// 客户端关闭的回调函数
        /// </summary>
        public Action<string > OnClientClose { get; set; }


        /// <summary>
        /// 添加一个用客户端后的回调函数
        /// </summary>
        public Action<string> OnClientAdd { get; set; }


        /// <summary>
        /// 异常输出的回调函数
        /// </summary>
        public Action<string> OnError { get; set; }




    }
}
