using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework
{
    public interface IControllerState
    {
        ModelStateDictionary ModelState { get; set; }

        void Reset();

        void Initialize(Controller controller);

        void SetState(Controller controller);
    }
}
