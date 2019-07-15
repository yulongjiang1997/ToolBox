using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// http工具类
/// </summary>
namespace ToolBox.Http
{

    /// <summary>
    /// 带cookie的网络请求
    /// </summary>
    public  class HttpHelper
    {
        /// <summary>
        /// 默认的编码
        /// </summary>
        private Encoding Encoding = Encoding.Default;

        private Encoding Postencoding = Encoding.Default;

        /// <summary>
        /// HttpWebRequest对象用来发起请求
        /// </summary>
        private HttpClient Client = null;

        HttpClientHandler Handler = null;

        private HttpRequestMessage RequestHeads = null;

        //这些头数据 不能加在head 中 得加在content.head中
        private static List<string> _list = new List<string>() { "Content-Disposition".ToLower(), "Content-Encoding".ToLower(), "Content-Language".ToLower(), "Content-Location".ToLower(), "Content-MD5".ToLower(), "Content-Range".ToLower(), "Expires".ToLower(), "LastModified".ToLower() };



        /// <summary>
        /// 网络请求
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="httpMethod">请求方法</param>
        /// <param name="cookie">cookie</param>
        /// <returns></returns>
        public async static Task<HttpResult> RequestAsync(string url, HttpMethod httpMethod,string Postdata=null, string cookie=null)
        {

            HttpItem item = new HttpItem();
            item.URL = url;
            item.Method = httpMethod;
            item.Cookie = cookie;
            item.Postdata = Postdata;
            

            return await new HttpHelper().GetHtml(item);

        }



        /// <summary>
        /// 网络请求
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async static Task<HttpResult> RequestAsync(HttpItem item) {

           

           return await new HttpHelper().GetHtml(item);

        }




        /// <summary>
        /// Gethtml
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<HttpResult> GetHtml(HttpItem item)
        {
            string html = string.Empty;
            try
            {

                SetRequest(item);

                try
                {

                    var response = await Client.SendAsync(RequestHeads);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = new HttpResult();
                        result.StatusCode = response.StatusCode;


                        #region base
                        //获取StatusCode

                        //获取StatusDescription
                        result.IsSuccessStatusCode = response.IsSuccessStatusCode;
                        //获取Headers
                        result.Header = response.Content.Headers;


                        //获取set-cookie
                        if (response.Headers.Contains("Set-Cookie"))
                        {
                            var cookies = response.Headers.GetValues("Set-Cookie");
                            string reCook = string.Empty;

                            if (item.AutoHanderCookie)
                            {
                                if (cookies != null) reCook = CookieHelper.GetSmallCookie(cookies);

                            }
                            else
                            {
                                reCook = string.Join(";", cookies);
                            }


                            if (item.AutoFixCookie)
                            {
                                result.Cookie = CookieHelper.UpdateMergeCookie(item.Cookie, reCook);
                            }
                            else
                            {
                                result.Cookie = reCook;
                            }



                        }


                        #endregion

                        //获取跳转地址
                        if (response.Headers.Location != null)
                        {
                            result.RedirectUrl = response.Headers.Location.ToString();
                        }

                        if (item.ResultType == ResultType.Byte)
                        {
                            result.ResultByte = await response.Content.ReadAsByteArrayAsync();
                        }
                        else
                        {
                            using (Stream myResponseStream = await response.Content.ReadAsStreamAsync())
                            {
                                using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding))
                                {
                                    result.Html = myStreamReader.ReadToEnd();

                                }

                            }


                        }

                        return result;

                    }

                    return new HttpResult() { Cookie = string.Empty, Header = null, Html = "", StatusCode = response.StatusCode, StatusDescription = "服务器响应：" + response.StatusCode };


                }
                catch (Exception ex)
                {
                    return new HttpResult() { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "请求时出错：" + ex.Message };
                }


            }
            catch (Exception ex)
            {

                //配置参数时出错
                return new HttpResult() { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "配置参数时出错：" + ex.Message };
            }
        }


        /// <summary>
        /// 为请求准备参数
        /// </summary>
        /// <param name="item"></param>
        private void SetRequest(HttpItem item) {

            try
            {
                Handler = new HttpClientHandler();

                if (item.URL.StartsWith("https")) {

                   ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }

                RequestHeads = new HttpRequestMessage(item.Method, item.URL);

                //设置压缩 解压 方式
                Handler.AutomaticDecompression = item.Decompression;

                //Authorization 认证
                if (!string.IsNullOrEmpty(item.Authorization))
                {
                    RequestHeads.Headers.Authorization = new AuthenticationHeaderValue("Basic", item.Authorization);
                }

                //设置Post数据
                if (item.Method != HttpMethod.Get)
                {
                    SetPostData(item);
                }


                //设置Header参数
                if (item.Header != null && item.Header.Count > 0)
                {
                    foreach (string key in item.Header.AllKeys)
                    {
                        if (_list.Contains(key.ToLower()))//这些头不能加在请求头里面 得加在 content.head 里面
                        {
                            if (RequestHeads.Content != null)
                            {
                                RequestHeads.Content.Headers.TryAddWithoutValidation(key, item.Header[key]);
                            }
                        }
                        else
                        {

                            RequestHeads.Headers.Add(key, item.Header[key]);
                        }

                    }
                }

                // 设置代理
                SetProxy(item);


                if (item.ProtocolVersion != null) { RequestHeads.Version = item.ProtocolVersion; }


                if (!string.IsNullOrWhiteSpace(item.Host))
                {
                    RequestHeads.Headers.Host = item.Host;
                }

                RequestHeads.Headers.Add("Accept", item.Accept.Trim());

                if (item.KeepAlive)
                {
                    RequestHeads.Headers.ConnectionClose = false;
                }
                else
                {
                    RequestHeads.Headers.ConnectionClose = true;//非持久连接
                }

                //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
                RequestHeads.Headers.Add("User-Agent", item.UserAgent);

                if (item.Encoding != null)
                {
                    Encoding = item.Encoding;
                }

                //设置安全凭证
                Handler.Credentials = item.ICredentials;

                //设置cookie
                SetCookie(item);

                //来源地址

                if (!string.IsNullOrEmpty(item.Referer)) RequestHeads.Headers.Referrer = new Uri(item.Referer);

                //是否执行跳转功能
                Handler.AllowAutoRedirect = item.Allowautoredirect;
                if (item.MaximumAutomaticRedirections > 0)
                {
                    Handler.MaxAutomaticRedirections = item.MaximumAutomaticRedirections;
                }


                //最后设置  clinet
                Client = new System.Net.Http.HttpClient(Handler);

                Client.Timeout = TimeSpan.FromSeconds(item.Timeout);




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);

            }

        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="item"></param>
        private void SetCookie(HttpItem item) {

            if (!string.IsNullOrWhiteSpace(item.Cookie)) {

                Handler.UseCookies = false;
                RequestHeads.Headers.Add("Cookie", item.Cookie);

            }

        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="item"></param>
        private void SetPostData(HttpItem item) {

            if (item.PostEncoding != null) { Postencoding = item.PostEncoding; }

            byte[] buffer = null;

            //写入Byte类型
            if (item.postDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0)
            {

                //验证在得到结果时否有传入数据
                buffer = item.PostdataByte;
            }
            else if (item.postDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(item.Postdata)) {

                StreamReader r = new StreamReader(item.Postdata, Postencoding);
                buffer = Postencoding.GetBytes(r.ReadToEnd());
                r.Close();

            } else if (!string.IsNullOrWhiteSpace(item.Postdata)) {

                buffer = Postencoding.GetBytes(item.Postdata);

            }

            if (buffer != null) {

                RequestHeads.Content = new ByteArrayContent(buffer, 0, buffer.Length);
                RequestHeads.Content.Headers.Add("Content-Length", buffer.Length.ToString());
            }

            if (RequestHeads.Content != null) {

                RequestHeads.Content.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);

            }


        }


        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="item"></param>
        private void SetProxy(HttpItem item) {

            bool isIeProxy = false;

            if (!string.IsNullOrWhiteSpace(item.ProxyIp))
            {
                isIeProxy = item.ProxyIp.ToLower().Contains("ieproxy");
            }

            if (!string.IsNullOrWhiteSpace(item.ProxyIp) && !isIeProxy)
            {
                //设置代理服务器
                if (item.ProxyIp.Contains(":"))
                {
                    string[] plist = item.ProxyIp.Split(':');
                    WebProxy myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    Handler.UseProxy = true;
                    Handler.Proxy = myProxy;
                }
                else
                {
                    WebProxy myProxy = new WebProxy(item.ProxyIp, false);
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    Handler.UseProxy = true;
                    Handler.Proxy = myProxy;
                }
            }
            else if (isIeProxy)
            {
                //设置为IE代理
                Handler.UseProxy = true;// defaultProxy 	
                                         // 使用默认（系统）代理时，获取或设置要提交到默认代理服务器进行身份验证的凭据。 只有在 UseProxy 设置为 true 且 Proxy 设置为 null 时才使用默认代理。
            }
            else
            {
                Handler.Proxy = item.WebProxy;
            }



        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~HttpHelper() {

            if (Handler != null)
            {
                Handler.Dispose();
            }

            if (RequestHeads != null)
            {
                RequestHeads.Dispose();
            }


            if (Client != null)
            {
                Client.Dispose();
            }


        }





        #region 旧的方法

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
        ///异步Post请求
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




        #endregion

    }
}
