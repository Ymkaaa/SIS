using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework.ViewEngineX
{
    public interface IViewEngine
    {
        string Execute<T>(string viewContent, T model, ModelStateDictionary modelState, Principal user = null);
    }
}
