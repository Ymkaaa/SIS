using System;

namespace SIS.MvcFramework.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        protected ValidationAttribute(string errorMessage = "Default error message")
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public abstract bool IsValid(object obj);
    }
}
