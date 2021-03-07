using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TFramework.UI
{
    public class UIPageBase<TViewModel, TModel, TView> : IUIPage
        where TViewModel : ViewModelBase<TModel, TView>
        where TModel : ModelBase, new()
        where TView : ViewBase, new()
    {
        [Inject]
        protected UIManager uiManager { get; set; }
        [Inject]
        public TViewModel ViewModel { get; set; }
        public string PageName { get; private set; }
        public GameObject GameObject { get; private set; }
        public UIPageBase(string pageName,GameObject gameObject)
        {
            PageName = pageName;
            GameObject = gameObject;
        }

        public virtual void ShowUIPage() { }

        public virtual void CloseUIPage() { }

        public virtual void RefreshUIPage() { }

        public virtual void HideUIPage() { }
    }
}
