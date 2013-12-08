using System;
using System.Reflection;

namespace LanExchange.Utils
{
    /// <summary>
    /// Several useful reflection utils.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Sets the class field.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void SetClassPrivateField<TClass,TFieldType>(string name, TFieldType value) where TClass : class
        {
            var fieldInfo = typeof (TClass).GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo != null)
                fieldInfo.SetValue(null, value);
        }

        /// <summary>
        /// Gets the object property.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public static TResult GetObjectProperty<TResult>(object instance, string name) where TResult : class
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            var propInfo = instance.GetType().GetProperty(name);
            return propInfo == null ? null : (TResult)propInfo.GetValue(instance, null);
        }

        /// <summary>
        /// Copies the object properties.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="destObject">The dest object.</param>
        public static void CopyObjectProperties<TClass>(TClass sourceObject, TClass destObject) where TClass : class
        {
            var props = typeof(TClass).GetProperties();
            foreach (var prop in props)
            {
                var obj = prop.GetValue(sourceObject, null);
                prop.SetValue(destObject, obj, null);
            }
        }
    }
}