using TFrameWork.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //builder.Register<HelloWorldService>(Lifetime.Singleton);
        builder.Register<IMessageBroker, MessageBroker>(Lifetime.Singleton);
        builder.Register<IEventSystem, EventSystem>(Lifetime.Singleton);
        Assembly dataAccess = Assembly.GetExecutingAssembly();
        foreach (Type item in dataAccess.GetTypes())
        {
            var result = item.GetInterface("IService");
            if (null == result)
                continue;
            IService service = Activator.CreateInstance(item) as IService;
            builder.RegisterInstance(service);
        }
        builder.RegisterEntryPoint<GamePresenter>(Lifetime.Singleton);
    }
}
