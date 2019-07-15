using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ToolBox.Reflection
{
    public class PropertyExpressionParser<T>
    {
        private readonly object _item;
        private PropertyInfo _property;

        public PropertyExpressionParser(object item, Expression<Func<T, object>> propertyExpression)
        {
            _item = item;
            _property = GetProperty(propertyExpression);
        }

        private static PropertyInfo GetProperty(Expression<Func<T, object>> exp)
        {
            PropertyInfo result;
            if (exp.Body.NodeType == ExpressionType.Convert)
                result = ((MemberExpression)((UnaryExpression)exp.Body).Operand).Member as PropertyInfo;
            else result = ((MemberExpression)exp.Body).Member as PropertyInfo;

            if (result != null)
                return typeof(T).GetProperty(result.Name);

            throw new ArgumentException(string.Format("Expression '{0}' does not refer to a property.", exp.ToString()));
        }

        public object Value
        {
            get { return ReflectionUtil.GetPropertyValue(_item, _property); }
        }

        public string Name
        {
            get { return _property.Name; }
        }

        public Type Type
        {
            get { return ReflectionUtil.GetPropertyType(_property); }
        }
    }
}
