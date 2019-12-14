using System;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object obj)
        {
            string email = Convert.ChangeType(obj, typeof(string)) as string;

            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}
