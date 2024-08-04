using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BYOJoystick
{
    public static class CompiledExpressions
    {
        public static Func<TClass, TField> CreateFieldGetter<TClass, TField>(string fieldName)
        {
            var fieldInfo = typeof(TClass).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                throw new ArgumentException($"Field {fieldName} not found in {typeof(TClass).Name}");
            var instance = Expression.Parameter(typeof(TClass), "instance");
            var field    = Expression.Field(instance, fieldInfo);
            var lambda   = Expression.Lambda<Func<TClass, TField>>(field, instance);
            return lambda.Compile();
        }

        public static Func<TField> CreateStaticFieldGetter<TClass, TField>(string fieldName)
        {
            var fieldInfo = typeof(TClass).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (fieldInfo == null)
                throw new ArgumentException($"Field {fieldName} not found in {typeof(TClass).Name}");
            var field  = Expression.Field(null, fieldInfo);
            var lambda = Expression.Lambda<Func<TField>>(field);
            return lambda.Compile();
        }

        public static Action<TClass, TField> CreateFieldSetter<TClass, TField>(string fieldName)
        {
            var fieldInfo = typeof(TClass).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                throw new ArgumentException($"Field {fieldName} not found in {typeof(TClass).Name}");
            var instance = Expression.Parameter(typeof(TClass), "instance");
            var value    = Expression.Parameter(typeof(TField), "value");
            var field    = Expression.Field(instance, fieldInfo);
            var assign   = Expression.Assign(field, value);
            var lambda   = Expression.Lambda<Action<TClass, TField>>(assign, instance, value);
            return lambda.Compile();
        }
        
        public static Action<TClass, TProperty> CreatePropertySetter<TClass, TProperty>(string propertyName)
        {
            var propertyInfo = typeof(TClass).GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo == null)
                throw new ArgumentException($"Property {propertyName} not found in {typeof(TClass).Name}");
    
            var instance = Expression.Parameter(typeof(TClass), "instance");
            var value    = Expression.Parameter(typeof(TProperty), "value");
            var property = Expression.Property(instance, propertyInfo);
            var assign   = Expression.Assign(property, value);
            var lambda   = Expression.Lambda<Action<TClass, TProperty>>(assign, instance, value);
    
            return lambda.Compile();
        }


        public static Action<TClass, int> CreateEventInvoker<TClass>(string eventName)
        {
            var eventInfo = typeof(TClass).GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
            if (eventInfo == null)
                throw new ArgumentException($"Event {eventName} not found in {typeof(TClass).Name}");
            var instance         = Expression.Parameter(typeof(TClass), "instance");
            var argument         = Expression.Parameter(typeof(int), "argument");
            var delegateType     = eventInfo.AddMethod.GetParameters()[0].ParameterType;
            var delegateInstance = Expression.Variable(delegateType, "delegateInstance");
            var block = Expression.Block(new[] { delegateInstance }, Expression.Assign(delegateInstance, Expression.Field(instance, eventInfo.Name)),
                                         Expression.IfThen(Expression.NotEqual(delegateInstance, Expression.Constant(null)), Expression.Invoke(delegateInstance, argument)));
            var lambda = Expression.Lambda<Action<TClass, int>>(block, instance, argument);
            return lambda.Compile();
        }
    }
}