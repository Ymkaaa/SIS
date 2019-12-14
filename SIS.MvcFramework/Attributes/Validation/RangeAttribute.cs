using System;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class RangeAttribute : ValidationAttribute
    {
        private readonly object minValue;
        private readonly object maxValue;
        private readonly Type objectType;

        public RangeAttribute(int minValue, int maxValue, string errorMessage)
            : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.objectType = typeof(int);
        }

        public RangeAttribute(double minValue, double maxValue, string errorMessage)
            : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.objectType = typeof(double);
        }

        public RangeAttribute(Type type, string minValue, string maxValue, string errorMessage)
            : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.objectType = type;
        }

        public override bool IsValid(object obj)
        {
            if (this.objectType == typeof(int))
            {
                return (int)obj >= (int)this.minValue && (int)obj <= (int)this.maxValue;
            }

            if (this.objectType == typeof(double))
            {
                return (double)obj >= (double)this.minValue && (double)obj <= (double)this.maxValue;
            }

            if (this.objectType == typeof(decimal))
            {
                return (decimal)obj >= (decimal)this.minValue && (decimal)obj <= (decimal)this.maxValue;
            }

            return false;
        }
    }
}
