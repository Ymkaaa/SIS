using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngineX
{
    public interface IViewEngine
    {
        string Execute<T>(string viewContent, T model, Principal user = null);
    }
}
