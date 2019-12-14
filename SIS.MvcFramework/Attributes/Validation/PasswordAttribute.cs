using System;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public override bool IsValid(object obj)
        {
            string value = Convert.ChangeType(obj, typeof(string)) as string;

            if (value.Length <= 3)
            {
                return false;
            }

            return true;
        }
    }
}
