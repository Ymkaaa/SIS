using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework.ViewEngineX
{
    public interface IView
    {
        string GetHtml(object Model, ModelStateDictionary modelState, Principal User);
    }
}
