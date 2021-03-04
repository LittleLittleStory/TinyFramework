using TFrameWork.Events;
using TFrameWork.UI;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class GamePresenter : IStartable, ITickable
{
    private IEventSystem eventSystem;
    private UIManager uiManager;
    private CompositeDisposable disposables;
    public GamePresenter(IEventSystem eventSystem, UIManager uiManager)
    {
        this.eventSystem = eventSystem;
        this.uiManager = uiManager;
        disposables = new CompositeDisposable();
    }

    public void Start()
    {
        IUIPage page = uiManager.CreateUIPage<ViewModelTest, ModelTest, ViewTest>("1111");
        page.ShowUIPage();
        eventSystem.Receive<TestEvent>().Subscribe(e =>
        {
            Debug.Log("Hello Test");
        }).AddTo(disposables);
    }

    public void Tick()
    {
        eventSystem.Publish(new TestEvent());
    }

    public class TestEvent
    {
        public TestEvent()
        {

        }
    }
}

