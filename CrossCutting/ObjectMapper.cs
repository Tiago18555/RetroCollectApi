using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System;

namespace RetroCollectApi.CrossCutting
{
    public static class ObjectMapper
    {
        /// <summary>
        /// Maps an object to a equal or similar type
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <typeparam name="TResult">The target type</typeparam>
        /// <param name="objFrom">The source object</param>
        /// <param name="objTo">The target object</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="TargetException"></exception>
        /// <exception cref="TargetInvocationException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns>A new tiped of System.Object</returns>
        public static TResult MapObjectTo<TSource, TResult>(this TSource objFrom, TResult objTo)
        {
            PropertyInfo[] ToProperties = objTo.GetType().GetProperties();
            PropertyInfo[] FromProperties = objFrom.GetType().GetProperties();

            ToProperties.ToList().ForEach(objToProp =>
            {
                PropertyInfo FromProp = FromProperties.FirstOrDefault(objFromProp =>
                    objFromProp.Name == objToProp.Name && objFromProp.PropertyType == objToProp.PropertyType
                );

                if (FromProp != null) objToProp.SetValue(objTo, FromProp.GetValue(objFrom));

            });
            return objTo;
        }

        public static dynamic MapObjectToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }
}
