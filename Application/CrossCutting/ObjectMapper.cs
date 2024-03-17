using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System;
using System.Runtime.ConstrainedExecution;

namespace Application.CrossCutting
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


        /// <summary>
        /// Maps an hierarchy of objects to a equal or similar type
        /// </summary>
        /// <typeparam name="TSource">The source main type</typeparam>
        /// <typeparam name="TResult">The target main type</typeparam>
        /// <param name="objFrom">The source object</param>
        /// <param name="objTo">The target object</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="TargetException"></exception>
        /// <exception cref="TargetInvocationException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns>A new tiped of System.Object</returns>
        public static TResult MapObjectsTo<TSource, TResult>(this TSource objFrom, TResult objTo) where TResult : new()
        {
            var toProperties = objTo.GetType().GetProperties();
            var fromProperties = objFrom.GetType().GetProperties();

            foreach (var toProp in toProperties)
            {
                var fromProp = fromProperties.FirstOrDefault(prop => prop.Name == toProp.Name && toProp.CanWrite);

                if (fromProp != null)
                {
                    var value = fromProp.GetValue(objFrom);

                    if (IsSimpleType(fromProp.PropertyType))
                    {
                        toProp.SetValue(objTo, value);
                    }
                    else if (fromProp.PropertyType.IsClass || IsComplexStruct(fromProp.PropertyType))
                    {
                        if (value != null)
                        {
                            var nestedObjTo = Activator.CreateInstance(toProp.PropertyType);
                            MapObjectsTo(value, nestedObjTo);
                            toProp.SetValue(objTo, nestedObjTo);
                        }
                    }
                }
            }
            return objTo;
        }

        private static bool IsSimpleType(Type type) =>        
            type.IsPrimitive || type == typeof(string) || type == typeof(decimal) ||
                   type == typeof(DateTime) || type == typeof(Guid) || type.IsEnum;        

        private static bool IsComplexStruct(Type type) =>
            type.IsValueType && !type.IsEnum && !IsSimpleType(type);
        
    }
}
