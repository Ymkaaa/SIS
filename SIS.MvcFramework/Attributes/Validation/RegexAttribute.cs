using System;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class RegexAttribute : ValidationAttribute
    {
        private readonly string pattern;

        public RegexAttribute(string pattern, string errorMessage) 
            : base(errorMessage)
        {
            this.pattern = pattern;
        }

        public override bool IsValid(object obj)
        {
            return Regex.IsMatch(Convert.ChangeType(obj, typeof(string)) as string, pattern);
        }
    }
}
