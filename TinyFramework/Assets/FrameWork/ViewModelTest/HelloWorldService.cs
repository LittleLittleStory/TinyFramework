using System.Collections;
using System.Collections.Generic;
using TFrameWork.Events;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class HelloWorldService :IService
{
    private IEventSystem eventSystem;
    private CompositeDisposable disposables;

    public HelloWorldService(IEventSystem eventSystem)
    {
        this.eventSystem = eventSystem;
        disposables = new CompositeDisposable();
    }

    public void Hello()
    {

    }
}
