using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses;

namespace SIS.MvcFramework.Result
{
    public abstract class ActionResult : HttpResponse
    {
        protected ActionResult(HttpResponseStatusCode httpResponseStatusCode) : base(httpResponseStatusCode)
        {

        }
    }
}
