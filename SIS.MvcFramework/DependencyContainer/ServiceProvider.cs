using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.MvcFramework.DependencyContainer
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly IDictionary<Type, Type> dependencyContainer = new ConcurrentDictionary<Type, Type>();

        public void Add<TSource, TDestination>()
            where TDestination : TSource
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public object CreateInstance(Type type)
        {
            if (dependencyContainer.ContainsKey(type))
            {
                type = dependencyContainer[type]; 
            }

            ConstructorInfo constructor = type
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(c => c.GetParameters().Count())
                .FirstOrDefault();

            if (constructor == null)
            {
                return null;
            }

            ParameterInfo[] parameters = constructor.GetParameters();
            List<object> parameterInstances = new List<object>();

            foreach (ParameterInfo parameter in parameters)
            {
                parameterInstances.Add(CreateInstance(parameter.ParameterType));
            }

            object obj = constructor.Invoke(parameterInstances.ToArray());

            return obj;
        }

        //public T CreateInstance<T>()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
