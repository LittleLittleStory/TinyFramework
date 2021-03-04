using TFrameWork.Events;
using UnityEngine;

namespace TFrameWork.UI
{
    public class ViewModelTest: ViewModelBase<ModelTest,ViewTest >
    {
        private IEventSystem eventSystem;

        public ViewModelTest(IEventSystem eventSystem)
        {
            this.eventSystem = eventSystem;
        }

        public override void SetupModel(ViewTest view, ModelTest model)
        {
            base.SetupModel(view, model);
            Debug.Log("1111112225");
        }

        public override void SetupEvent(ModelTest model)
        {

        }
    }
}
