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
    public class UIPageBase : IUIPage
    {
        public UIManager uiManager;

        public UIPageBase(string pageName, ViewModelBase<ModelBase, ViewBase> viewModel)
        {
            PageName = pageName;
            ViewModel = viewModel;
        }

        public string PageName { get; private set; }
        public ViewModelBase<ModelBase, ViewBase> ViewModel { get; protected set; }

        public virtual void ShowUIPage() { }

        public virtual void CloseUIPage() { }

        public virtual void RefreshUIPage() { }

        public virtual void HideUIPage() { }
    }
}
