using UnityEngine;

namespace TFrameWork.Component
{
    public interface IComponent
    {
        GameObject GameObject { get; }
        void Star();
        void Update();
        void Destory();
    }
}