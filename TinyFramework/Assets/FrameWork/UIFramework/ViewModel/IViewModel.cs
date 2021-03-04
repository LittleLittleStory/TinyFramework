using UnityEngine;

namespace TFrameWork.UI
{
    public interface IViewModelBase
    {
        string PageName { get; }
        void Init(GameObject gameObject);
        void Destory();
    }
}
