using System;

namespace SIS.MvcFramework.Attributes
{
    public abstract class BaseHttpAttribute : Attribute
    {
        public string ActionName { get; set; }

        public string Url { get; set; }
    }
}
