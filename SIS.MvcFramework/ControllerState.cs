using System;
using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework
{
    public class ControllerState : IControllerState
    {
        public ControllerState()
        {
            this.Reset();
        }

        public ModelStateDictionary ModelState { get; set; }

        public void Initialize(Controller controller)
        {
            this.ModelState = controller.ModelState;
        }

        public void SetState(Controller controller)
        {
            controller.ModelState = this.ModelState;
        }

        public void Reset()
        {
            this.ModelState = new ModelStateDictionary();
        }
    }
}
