using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TFrameWork.UI
{
    public class UIPageBase : MonoBehaviour, IUIPage
    {
        public UIManager uiManager;
        public string PageName { get; private set; }
        public ViewModelBase<ModelBase, ViewBase> ViewModel { get; private set; }

        [Inject]
        public void Init(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }
        private void Awake()
        {
            GameLifetimeScope.container.InjectGameObject(gameObject);
        }

        public virtual void ShowUIPage() { }

        public virtual void CloseUIPage() { }

        public virtual void RefreshUIPage() { }

        public virtual void HideUIPage() { }
    }
}
