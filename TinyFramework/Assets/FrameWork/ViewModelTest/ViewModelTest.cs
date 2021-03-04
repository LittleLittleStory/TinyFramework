using TFrameWork.Events;
using UnityEngine;
using UniRx;
using TFrameWork.UI;

public class ViewModelTest : ViewModelBase<ModelTest, ViewTest>
{
    private IEventSystem eventSystem;

    public ViewModelTest(IEventSystem eventSystem)
    {
        this.eventSystem = eventSystem;
    }

    public override void SetupModel(ViewTest view, ModelTest model)
    {
        base.SetupModel(view, model);
        view.text.text = "555555555";
    }

    public override void SetupEvent(ViewTest view)
    {
        base.SetupEvent(view);
        eventSystem.Receive<TestEvent>().Subscribe(e =>
        {
            Debug.Log("Hello Test");
        }).AddTo(view.Disposables);
    }
}

