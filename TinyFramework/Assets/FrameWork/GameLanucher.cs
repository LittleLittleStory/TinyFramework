using TFrameWork.Events;
using System;
using System.Reflection;
using UniRx;
using VContainer;
using VContainer.Unity;
using TFrameWork.UI;
using TFrameWork.VContainer;
using TFrameWork.IEnumeratorTool;

public class GameLanucher : LifetimeScope
{
    public static GameLanucher Instance;
    public IContainerBuilder Builder;

    protected override void Awake()
    {
        base.Awake();
        gameObject.AddComponent<IEnumeratorTool>();
    }
    protected override void Configure(IContainerBuilder builder)
    {
        if (null == Instance)
            Instance = this;
        Builder = builder;

        builder.Register<IMessageBroker, MessageBroker>(Lifetime.Singleton);
        builder.Register<IEventSystem, EventSystem>(Lifetime.Singleton);
        builder.Register<UIManager>(Lifetime.Singleton);
        Assembly assembly = Assembly.Load("Game");
        InitAutoBindByInterface<IService>(assembly);
        InitAutoBindByInterface<IViewModelBase>(assembly);
        InitBindEntryPoint(assembly);
    }

    private void InitAutoBindByInterface<T>(Assembly dataAccess)
    {
        foreach (Type item in dataAccess.GetTypes())
        {
            var result = item.GetInterface(typeof(T).ToString());
            if (null == result)
                continue;
            Builder.Register(item, Lifetime.Singleton);
        }
    }

    private void InitBindEntryPoint(Assembly dataAccess)
    {
        foreach (Type item in dataAccess.GetTypes())
        {
            var startable = item.GetInterface(typeof(IStartable).ToString());
            var tickable = item.GetInterface(typeof(ITickable).ToString());
            if (null != startable
            || null != tickable)
                Builder.RegisterEntryPoint(item, Lifetime.Singleton);
        }
    }
}
