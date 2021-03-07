using UnityEngine;

namespace TFramework.UI
{
    public interface IViewModelBase
    {
        string PageName { get; }
        void Init(GameObject gameObject);
        void Destory();
    }
}
