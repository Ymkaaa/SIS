using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.MvcFramework.Mapping
{
    public static class ModelMapper
    {
        public static TDestination ProjectTo<TDestination>(object origin)
        {
            return (TDestination)MapObject(origin, typeof(TDestination));
        }

        private static object MapObject(object origin, Type declarationType)
        {
            object destinationInstance = Activator.CreateInstance(declarationType);

            foreach (PropertyInfo originProperty in origin.GetType().GetProperties())
            {
                string propertyName = originProperty.Name;
                PropertyInfo destinationProperty = destinationInstance.GetType().GetProperty(propertyName);

                MapProperty(origin, destinationInstance, originProperty, destinationProperty);
            }

            return destinationInstance;
        }

        private static void MapProperty(object origin, object destinationInstance, PropertyInfo originProperty, PropertyInfo destinationProperty)
        {
            if (destinationProperty == null)
            {
                return;
            }

            object originPropertyValue = originProperty.GetValue(origin);

            if (destinationProperty.PropertyType == typeof(string) || destinationProperty.PropertyType == typeof(decimal) || destinationProperty.PropertyType.IsPrimitive) // Support only string, decimal and primitive types
            {
                if (originProperty.PropertyType == destinationProperty.PropertyType) // Support only map to equal types
                {
                    destinationProperty.SetValue(destinationInstance, originPropertyValue);
                }
            }
            else if (destinationProperty.PropertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))) // Support IEnumerable collections (Recursive)
            {
                IEnumerable originCollection = (IEnumerable) originPropertyValue;
                IList destinationCollection = (IList)Activator.CreateInstance(destinationProperty.PropertyType);

                Type destinationElementType = destinationProperty.GetValue(destinationInstance).GetType().GetGenericArguments()[0];

                foreach (object originElement in originCollection)
                {
                    destinationCollection.Add(MapObject(originElement, destinationElementType));
                }

                destinationProperty.SetValue(destinationInstance, destinationCollection);
            }
            else // Support objects (Recursive)
            {
                object value = MapObject(originProperty.GetValue(origin), destinationProperty.PropertyType);

                destinationProperty.SetValue(destinationInstance, value);
            }
        }
    }
}
