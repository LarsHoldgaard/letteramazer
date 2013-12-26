using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace LetterAmazer.Business.Services.Model
{
    public enum OrderDirection
    {
        Asc,
        Desc
    }

    public class OrderBy
    {
        private string propertyName;
        private OrderDirection orderDirection;

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        public OrderDirection OrderDirection
        {
            get { return orderDirection; }
            set { orderDirection = value; }
        }

        public OrderBy(string propertyName, OrderDirection orderDirection)
        {
            this.propertyName = propertyName;
            this.orderDirection = orderDirection;
        }

        public static OrderBy Asc(string propertyName)
        {
            return new OrderBy(propertyName, OrderDirection.Asc);
        }

        public static OrderBy Asc<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var propertyInfo = GetPropertyInfo(propertyLambda);
            return new OrderBy(propertyInfo.Name, OrderDirection.Asc);
        }

        public static OrderBy Desc(string propertyName)
        {
            return new OrderBy(propertyName, OrderDirection.Desc);
        }

        public static OrderBy Desc<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var propertyInfo = GetPropertyInfo(propertyLambda);
            return new OrderBy(propertyInfo.Name, OrderDirection.Desc);
        }

        private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));

            System.Reflection.PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", propertyLambda.ToString()));

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format("Expresion '{0}' refers to a property that is not from type {1}.", propertyLambda.ToString(), type));

            return propInfo;
        }

        public override string ToString()
        {
            return this.PropertyName + " " + this.OrderDirection;
        }
    }
}
