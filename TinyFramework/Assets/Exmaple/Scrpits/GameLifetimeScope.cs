using TFrameWork.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using TFrameWork.UI;

public class GameLifetimeScope : LifetimeScope
{
    public static IContainerBuilder builder;
    public static IObjectResolver container;
    protected override void Configure(IContainerBuilder builder)
    {
        GameLifetimeScope.builder = builder;
        builder.Register<IMessageBroker, MessageBroker>(Lifetime.Singleton);
        builder.Register<IEventSystem, EventSystem>(Lifetime.Singleton);
        Assembly dataAccess = Assembly.GetExecutingAssembly();
        InitBindinterface<IService>(dataAccess, "IService");
        GameLifetimeScope.container = GameLifetimeScope.builder.Build();
    }

    private void InitBindinterface<T>(Assembly dataAccess, string interfaceName) where T : class
    {
        foreach (Type item in dataAccess.GetTypes())
        {
            var result = item.GetInterface(interfaceName);
            if (null == result)
                continue;
            T service = Activator.CreateInstance(item) as T;
            builder.RegisterInstance(service);
        }
    }
}
