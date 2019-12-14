using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.SandBox
{
    public class Program
    {
        public static void Main()
        {
            User user = new User() { Username = "Ivan" };

            PropertyInfo[] objProperties = user.GetType().GetProperties();

            foreach (PropertyInfo property in objProperties)
            {
                List<ValidationAttribute> validationAttributes = property
                    .GetCustomAttributes()
                    .Where(type => type is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList();

                foreach (ValidationAttribute validationAttribute in validationAttributes)
                {
                    if (validationAttribute.IsValid(property.GetValue(user)))
                    {
                        Console.WriteLine(true);
                    }
                    else
                    {
                        Console.WriteLine(false);
                    }
                }
            }

        }
    }
}
