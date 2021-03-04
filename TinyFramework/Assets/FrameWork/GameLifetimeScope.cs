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
    public static IContainerBuilder Builder;
    public new static IObjectResolver Container;
    protected override void Configure(IContainerBuilder builder)
    {
        GameLifetimeScope.Builder = builder;
        builder.Register<IMessageBroker, MessageBroker>(Lifetime.Singleton);
        builder.Register<IEventSystem, EventSystem>(Lifetime.Singleton);
        Assembly dataAccess = Assembly.GetExecutingAssembly();
        InitBindInterface<IService>(dataAccess, "IService");
        //Builder.Register(typeof(ViewModelTest), Lifetime.Singleton);
        //Builder.RegisterEntryPoint(typeof(GamePresenter), Lifetime.Singleton);
        GameLifetimeScope.Container = GameLifetimeScope.Builder.Build();
        //builder.RegisterEntryPoint<GamePresenter>(Lifetime.Singleton);
    }

    private void InitBindInterface<T>(Assembly dataAccess, string interfaceName) where T : class
    {
        foreach (Type item in dataAccess.GetTypes())
        {
            var result = item.GetInterface(interfaceName);
            if (null == result)
                continue;
            Builder.Register(item, Lifetime.Singleton);
        }
    }

    private void InitBindEntryPoint(Assembly dataAccess, string interfaceName)
    {
        /*foreach (Type item in dataAccess.GetTypes())
        {
            if (item == typeof(IStartable))
                continue;
            var result = item.GetInterface(interfaceName);
            if (null == result)
                continue;

        }*/
    }
}
