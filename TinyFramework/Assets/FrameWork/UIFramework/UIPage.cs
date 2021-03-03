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
    public class UIPage: UIPageBase
    {
        public UIPage(string pageName, ViewModelBase<ModelBase, ViewBase> viewModel) : base(pageName, viewModel) { }

        [Inject]
        public void Inject(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }
        public override void ShowUIPage()
        {
            ViewModel.Init();
        }
    }
}
