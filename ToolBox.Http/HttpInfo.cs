using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Http
{
    public class HttpInfo
    {
        public bool Is_bool { get; set; } = false;//成功为true 
        public string Html { get; set; } = string.Empty;//成功后的 返回数据
        public string J_code { get; set; } = string.Empty;//其它数据
        public byte[] Img { get; set; } = null;//图片数据
    }
}
