using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Object
{
    /// <summary>
    /// Object工具类
    /// </summary>
   public  class ObjectHelper
    {

        /// <summary>
        /// 从指定对象创建指定类型的新对象
        /// </summary>
        /// <typeparam name="T">新对象类型</typeparam>
        /// <param name="obj">源对象</param>
        /// <returns>指定类型的新对象</returns>
        /// <param name="notFilds">不拷贝的属性集合</param>
        /// <param name="isCopyVirtual">是否拷贝有Virtual访问修饰符的属性</param>
        /// <returns></returns>
        public static T CreateObject<T>(object obj, string[] notFilds, bool isCopyVirtual) where T : class
        {
            Type type = typeof(T);
            object obj2 = Assembly.GetAssembly(type).CreateInstance(type.FullName);
            PropertyCopy(obj, obj2, notFilds, isCopyVirtual);
            return obj2 as T;
        }

        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="notFilds">不拷贝的属性集合</param>
        /// <returns></returns>
        public static T CreateObject<T>(object obj, string[] notFilds) where T : class
        {
            return CreateObject<T>(obj, notFilds, false);
        }
        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isCopyVirtual">是否拷贝有Virtual访问修饰符的属性</param>
        /// <returns></returns>
        public static T CreateObject<T>(object obj, bool isCopyVirtual) where T : class
        {
            return CreateObject<T>(obj, null, isCopyVirtual);
        }
        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CreateObject<T>(object obj) where T : class
        {
            return CreateObject<T>(obj, null);
        }


        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="notFilds">不拷贝的属性集合</param>
        /// <param name="isCopyVirtual">是否拷贝有Virtual访问修饰符的属性</param>
        /// <returns></returns>
        public static object CreateObject(object obj, string[] notFilds, bool isCopyVirtual)
        {
            Type type = obj.GetType();
            object obj2 = Assembly.GetAssembly(type).CreateInstance(type.FullName);
            PropertyCopy(obj, obj2, notFilds, isCopyVirtual);
            return obj2;
        }
        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="notFilds">不拷贝的属性集合</param>
        /// <returns></returns>
        public static object CreateObject(object obj, string[] notFilds)
        {
            return CreateObject(obj, notFilds, false);
        }
        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isCopyVirtual">是否拷贝有Virtual访问修饰符的属性</param>
        /// <returns></returns>
        public static object CreateObject(object obj, bool isCopyVirtual)
        {
            return CreateObject(obj, null, isCopyVirtual);
        }
        /// <summary>
        /// 拷贝并创建对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object CreateObject(object obj)
        {
            return CreateObject(obj, null);
        }


        /// <summary>
        /// 对象属性拷贝(全匹配拷贝)
        /// </summary>
        /// <param name="obj1">源对象()</param>
        /// <param name="obj2">目标对象(被赋值对象)</param>
        /// <param name="notFilds">不赋值的字段</param>
        /// <returns>目标对象</returns>
        /// <param name="isCopyVirtual">是否拷贝有Virtual访问修饰符的属性</param>
        /// <returns></returns>
        public static object PropertyCopy(object obj1, object obj2, string[] notFilds, bool isCopyVirtual)
        {
            Type souType = obj1.GetType();
            Type tarType = obj2.GetType();
            PropertyInfo[] pis = souType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (null != pis)
            {
                foreach (PropertyInfo pi in pis)
                {
                    string propertyName = pi.Name;
                    if (notFilds != null && notFilds.Count(i => i == propertyName) > 0)
                        continue;
                    PropertyInfo pit = tarType.GetProperty(propertyName);
                    if (pit != null && pi.CanWrite)
                    {
                        if (!isCopyVirtual && pit.GetMethod.IsVirtual) continue;
                        Type ptype = pit.PropertyType;
                        object value = pi.GetValue(obj1, null);
                        if (value != null && pit.CanWrite && types.FirstOrDefault(i => i.Equals(ptype)) != null)
                        {
                            if (ptype == typeof(DateTime) && ((DateTime)value).Year == 1)
                            {
                                continue;
                            }
                            pit.SetValue(obj2, value, null);
                        }
                    }
                }
            }
            return obj2;
        }

        public static object PropertyCopy(object obj1, object obj2, string[] notFilds)
        {
            return PropertyCopy(obj1, obj2, notFilds, false);
        }
        /// <summary>
        /// 对象属性拷贝(全匹配拷贝)
        /// </summary>
        /// <param name="obj1">源对象</param>
        /// <param name="obj2">目标对象(被赋值对象)</param>
        /// <returns>目标对象</returns>
        public static object PropertyCopy(object obj1, object obj2)
        {
            return PropertyCopy(obj1, obj2, null, false);
        }


        private static Type[] types = new Type[] {
            typeof(Int16?),typeof(Int32?),typeof(Int64?),
            typeof(Int16),typeof(Int32),typeof(Int64),
            typeof(string),typeof(float),typeof(double),
            typeof(char),typeof(bool),typeof(decimal),
            typeof(float?),typeof(double?),
            typeof(char?),typeof(bool?),typeof(decimal?),
            typeof(DateTime),typeof(DateTime?),typeof(Byte),typeof(object)
        };

    }
}
