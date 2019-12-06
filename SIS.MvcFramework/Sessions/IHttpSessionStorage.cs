using SIS.HTTP.Sessions.Contracts;

namespace SIS.MvcFramework.Sessions
{
    public interface IHttpSessionStorage
    {
        IHttpSession GetSession(string id);

        bool ContainsSession(string id);
    }
}
