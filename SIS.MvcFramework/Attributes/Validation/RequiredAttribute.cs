namespace SIS.MvcFramework.Attributes.Validation
{
    public class RequiredAttribute : ValidationAttribute
    {
        public RequiredAttribute()
        {
        }

        public RequiredAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public override bool IsValid(object obj)
        {
            return obj != null;
        }
    }
}
