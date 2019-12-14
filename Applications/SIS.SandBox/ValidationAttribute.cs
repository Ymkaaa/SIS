using System;

namespace SIS.SandBox
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        protected ValidationAttribute(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public abstract bool IsValid(object obj);
    }
}
