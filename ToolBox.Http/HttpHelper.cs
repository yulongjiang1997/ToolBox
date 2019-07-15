using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Http
{

    /// <summary>
    /// 带cookie的网络请求
    /// </summary>
    public  class HttpHelper
    {

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpPut(string url,string paramData,CookieContainer cookie=null) {

            return Request(url,paramData,"Put",cookie);
        }

        /// <summary>
        /// 异步put请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<string> AsyncHttpPut(string url, string paramData, CookieContainer cookie=null) {

            return await Task.Run(() =>
            {
                return Request(url, paramData, "Put", cookie);

            });

        }



        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpPost(string url ,string paramData,CookieContainer cookie=null) {

            return Request(url, paramData, "POST", cookie);
        }

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<string> AsyncHttpPost(string url, string paramData, CookieContainer cookie=null) {

            return await Task.Run(() =>
            {
                return Request(url, paramData, "POST", cookie);

            });

        }



        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpGet(string url,CookieContainer cookie=null) {

            return Request(url, "GET", cookie);
        }

        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static  Task<string> AsyncHttpGet(string url, CookieContainer cookie=null) {

            return await Task.Run(() =>
           {
            
               return Request(url, "GET", cookie);

           });


        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string HttpDelete(string url,CookieContainer cookie=null) {

            return Request(url, "DELETE", cookie);
        }


        /// <summary>
        /// 异步Delete
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<string> AsyncHttpDelete(string url, CookieContainer cookie=null) {

            return await Task.Run(() =>
            {
               return   Request(url, "DELETE", cookie);

            }); 

        }



        /// <summary>
        /// 网络请求基方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <param name="mode"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private static string Request(string url, string paramData, string mode, CookieContainer cookie=null)
        {


            try
            {
                //获取提交的字节
                byte[] bs = Encoding.UTF8.GetBytes(paramData);
                //设置提交的相关参数
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.CookieContainer = cookie;
                req.Method = mode;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;
                //提交请求数据
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                //接收返回的页面，必须的，不能省略
                WebResponse wr = req.GetResponse();
                Stream respStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(respStream,Encoding.GetEncoding("utf-8"));
                string t = reader.ReadToEnd();


                wr.Close();

                return t;
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常在getPostRespone:" + ex.Source + ":" + ex.Message);


            }

            return "";
        }


        /// <summary>
        /// 另一个请求基方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mode"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private static string Request(string url,string mode,CookieContainer cookie=null) {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookie;
            request.Method = mode;
            // request.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string re = reader.ReadToEnd();

            stream.Close();
            reader.Close();
            response.Close();

            return re;
        }



 


    }
}
