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
        if (null== Instance)
            Instance = this;
        Builder = builder;

        builder.Register<IMessageBroker, MessageBroker>(Lifetime.Singleton);
        builder.Register<IEventSystem, EventSystem>(Lifetime.Singleton);

        Assembly dataAccess = Assembly.GetExecutingAssembly();
        InitBindInterface<IService>(dataAccess, "IService");

        builder.Register(typeof(ViewModelTest), Lifetime.Singleton);
        builder.RegisterEntryPoint(typeof(GamePresenter), Lifetime.Singleton);
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
