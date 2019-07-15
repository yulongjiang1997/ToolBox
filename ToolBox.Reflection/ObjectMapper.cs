using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToolBox.Reflection
{
    /// <summary>
    /// Object对象工具类
    /// </summary>
    public class ObjectMapper
    {
        public static IList<PropertyMapper> GetMapperProperties(Type sourceType, Type targetType)
        {
            var sourceProperties = sourceType.GetProperties();
            var targetProperties = targetType.GetProperties();

            return (from s in sourceProperties
                    from t in targetProperties
                    where s.Name == t.Name && s.CanRead && t.CanWrite && s.PropertyType == t.PropertyType
                    select new PropertyMapper
                    {
                        SourceProperty = s,
                        TargetProperty = t
                    }).ToList();
        }

        /// <summary>
        /// 复制对象，将source对象赋值到target对象中
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        public static void CopyProperties(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();
            var mapperProperties = GetMapperProperties(sourceType, targetType);

            for (int index = 0, count = mapperProperties.Count; index < count; index++)
            {
                var property = mapperProperties[index];
                var sourceValue = property.SourceProperty.GetValue(source, null);
                property.TargetProperty.SetValue(target, sourceValue, null);
            }
        }
    }

    public class PropertyMapper
    {
        public PropertyInfo SourceProperty { get; set; }
        public PropertyInfo TargetProperty { get; set; }
    }
}
