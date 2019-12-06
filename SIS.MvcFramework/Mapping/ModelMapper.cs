using System;
using System.Reflection;

namespace SIS.MvcFramework.Mapping
{
    public class ModelMapper
    {
        public static TDestination ProjectTo<TDestination>(object origin)
        {
            TDestination destinationInstance = (TDestination)Activator.CreateInstance(typeof(TDestination));

            foreach (PropertyInfo originProperty in origin.GetType().GetProperties())
            {
                string propertyName = originProperty.Name;
                PropertyInfo destinationProperty = destinationInstance.GetType().GetProperty(propertyName);

                if (destinationProperty != null)
                {
                    if (destinationProperty.PropertyType == typeof(string))
                    {
                        destinationProperty.SetValue(destinationInstance, originProperty.GetValue(origin).ToString());
                    }
                    else
                    {
                        destinationProperty.SetValue(destinationInstance, originProperty.GetValue(origin));
                    }
                }
            }

            return destinationInstance;
        }
    }
}
