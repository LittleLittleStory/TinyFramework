using TFrameWork.Events;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class GamePresenter : IStartable,ITickable
{
    private IEventSystem eventSystem;
    private CompositeDisposable disposables;
    public GamePresenter(IEventSystem eventSystem)
    {
        this.eventSystem = eventSystem;

        disposables = new CompositeDisposable();
    }

    public void Start()
    {
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

