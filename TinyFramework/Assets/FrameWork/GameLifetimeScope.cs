using TFrameWork.Events;
using System;
using System.Reflection;
using UniRx;
using VContainer;
using VContainer.Unity;
using TFrameWork.UI;
using TFrameWork.VContainer;

public class GameLifetimeScope : LifetimeScope
{
    public static GameLifetimeScope Instance;
    public IContainerBuilder Builder;
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
        InitBindEntryPoint<ITickable>(assembly);
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

    private void InitBindEntryPoint<T>(Assembly dataAccess)
    {
        foreach (Type item in dataAccess.GetTypes())
        {
            var result = item.GetInterface(typeof(T).ToString());
            if (null == result)
                continue;
            Builder.RegisterEntryPoint(item, Lifetime.Singleton);
        }
    }
}

