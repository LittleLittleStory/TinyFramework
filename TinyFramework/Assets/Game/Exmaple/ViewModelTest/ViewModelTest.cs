using TFramework.Events;
using UnityEngine;
using UniRx;
using TFramework.UI;

public class ViewModelTest : ViewModelBase<ModelTest, ViewTest>
{
    private IEventSystem eventSystem;
    private HelloWorldService helloWorldService;

    public ViewModelTest(HelloWorldService helloWorldService, IEventSystem eventSystem)
    {
        this.eventSystem = eventSystem;
        this.helloWorldService = helloWorldService;
    }

    public override void SetupModel(ViewTest view, ModelTest model)
    {
        base.SetupModel(view, model);
        view.text.text = helloWorldService.playerHP.ToString();
    }

    public override void SetupEvent(ViewTest view)
    {
        base.SetupEvent(view);
        /*eventSystem.Receive<TestEvent>().Subscribe(message =>
        {
            view.text.text = message.hp.ToString();
        }).AddTo(view.Disposables);*/
    }
}

