using System;

namespace SIS.SandBox
{
    public class StringLengthAttribute : ValidationAttribute
    {
        private readonly int minLength;
        private readonly int maxLength;

        public StringLengthAttribute(int minLength, int maxLength, string errorMessage) : base(errorMessage)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public override bool IsValid(object obj)
        {
            string value = Convert.ChangeType(obj, typeof(string)) as string;

            if (value.Length >= minLength && value.Length <= maxLength)
            {
                return true;
            }

            return false;
        }
    }
}
