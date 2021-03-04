using UnityEngine;

namespace TFrameWork.Component
{
    public class Component : IComponent
    {
        public GameObject GameObject { get; private set; }
        public virtual void Star() { }
        public virtual void Update() { }
        public virtual void Destory() { }

    }
}
