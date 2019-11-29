using System;

namespace SIS.WebServer.Attributes
{
    public abstract class BaseHttpAttribute : Attribute
    {
        public string ActionName { get; set; }

        public string Url { get; set; }
    }
}
