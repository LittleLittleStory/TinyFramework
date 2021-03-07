using System;

namespace TFramework.Events
{
    public interface IEventSystem
    {
        void Publish<T>(T message);
        IObservable<T> Receive<T>();
    }
}