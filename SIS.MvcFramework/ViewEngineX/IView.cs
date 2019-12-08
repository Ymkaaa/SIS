using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngineX
{
    public interface IView
    {
        string GetHtml(object Model, Principal User);
    }
}
