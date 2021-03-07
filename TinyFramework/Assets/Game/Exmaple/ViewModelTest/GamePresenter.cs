using TFramework.Events;
using TFramework.UI;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class TestEvent
{
    public int hp;

    public TestEvent(int hp)
    {
        this.hp = hp;
    }
}

public class HPChange
{
    public int hp;

    public HPChange(int hp)
    {
        this.hp = hp;
    }
}

public class GamePresenter : IStartable, ITickable
{
    private HelloWorldService helloWorldService;
    private IEventSystem eventSystem;
    private UIManager uiManager;
    public GamePresenter(HelloWorldService helloWorldService,IEventSystem eventSystem, UIManager uiManager)
    {
        this.helloWorldService = helloWorldService;
        this.eventSystem = eventSystem;
        this.uiManager = uiManager;
    }

    public void Start()
    {
        IUIPage page = uiManager.CreateUIPage<ViewModelTest, ModelTest, ViewTest>("PanelTest");
        page.ShowUIPage();
        eventSystem.Receive<TestEvent>().Subscribe(_ =>
        {
            Debug.Log(_.hp);
        });
    }

    public void Tick()
    {

    }
}

