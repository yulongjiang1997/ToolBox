using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ToolBox.Reflection
{
    /// <summary>
    /// 反射工具类
    /// </summary>
    public static class ReflectionUtil
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _cachedProperties = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        public static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> expression)
        {
            string propertyPath = null;
            if (expression.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression.Body;
                if (unaryExpression.NodeType == ExpressionType.Convert)
                    propertyPath = unaryExpression.Operand.ToString();
            }

            if (propertyPath == null)
                propertyPath = expression.Body.ToString();

            propertyPath = propertyPath.Replace(expression.Parameters[0] + ".", string.Empty);

            return propertyPath;
        }

        public static List<string> GetPropertyNamesFromExpressions<T>(Expression<Func<T, object>>[] expressions)
        {
            var propertyNames = new List<string>();
            foreach (var expression in expressions)
            {
                var propertyName = GetPropertyNameFromExpression(expression);
                propertyNames.Add(propertyName);
            }
            return propertyNames;
        }

        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <param name="item">将返回其属性值的对象。</param>
        /// <param name="property"> 对象的PropertyInfo信息（发现属性 (Property) 的属性 (Attribute) 并提供对属性 (Property) 元数据的访问）</param>
        /// <returns>object</returns>
        public static object GetPropertyValue(object item, PropertyInfo property)
        {
            var value = property.GetValue(item, null);

            return value;
        }

        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <param name="item">将返回其属性值的对象。</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>object</returns>
        public static object GetPropertyValue(object item, string propertyName)
        {
            PropertyInfo property;
            foreach (var part in propertyName.Split('.'))
            {
                if (item == null)
                    return null;

                var type = item.GetType();

                property = type.GetProperty(part);
                if (property == null)
                    return null;

                item = GetPropertyValue(item, property);
            }
            return item;
        }

        public static object GetPropertyValueDynamic(object item, string name)
        {
            var dictionary = (IDictionary<string, object>)item;

            return dictionary[name];
        }

        public static Dictionary<string, PropertyInfo> GetProperties(Type type)
        {
            var properties = _cachedProperties.GetOrAdd(type, BuildPropertyDictionary);

            return properties;
        }

        private static Dictionary<string, PropertyInfo> BuildPropertyDictionary(Type type)
        {
            var result = new Dictionary<string, PropertyInfo>();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                result.Add(property.Name.ToLower(), property);
            }
            return result;
        }

        public static bool IsList(object item)
        {
            if (item is ICollection)
                return true;

            return false;
        }

        public static bool IsNullable(PropertyInfo property)
        {
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;

            return false;
        }

        /// <summary>
        /// Includes a work around for getting the actual type of a Nullable type.
        /// </summary>
        public static Type GetPropertyType(PropertyInfo property)
        {
            if (IsNullable(property))
                return property.PropertyType.GetGenericArguments()[0];

            return property.PropertyType;
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        public static bool IsBasicClrType(Type type)
        {
            if (type.IsEnum
                || type.IsPrimitive
                || type.IsValueType
                || type == typeof(string)
                || type == typeof(DateTime))
                return true;

            return false;
        }
    }
}
