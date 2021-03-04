using TFrameWork.Events;
using TFrameWork.UI;
using UniRx;
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
        //helloWorldService.Hello();
    }

    public void Tick()
    {
        eventSystem.Publish(new TestEvent(10) {});
    }
}

