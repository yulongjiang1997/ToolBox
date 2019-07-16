using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ToolBox.Http
{
    /// <summary>
    /// 请求结果
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// cookite
        /// </summary>
        public string Cookie { get; set; }


        /// <summary>
        /// html
        /// </summary>
        public string Html { get; set; } = string.Empty;

        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte { get; set; }

        /// <summary>
        /// header对象
        /// </summary>
        public HttpContentHeaders Header { get; set; }


        /// <summary>
        ///http 是否响应成功
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }


        /// <summary>
        /// http响应 描述信息
        /// </summary>
        public string StatusDescription { get; set; }


        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }


        /// <summary>
        /// 最后访问的URl
        /// </summary>
        public string ResponseUri { get; set; }


        /// <summary>
        /// 获取重定向的URl
        /// </summary>
        public string RedirectUrl
        {
            get; set;
        }



    }
}
