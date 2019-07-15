using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Json
{
    public static class JsonHelper
    {

        /// <summary>
        /// 对象转json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// json转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T JsonToModel<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }



    }
}
