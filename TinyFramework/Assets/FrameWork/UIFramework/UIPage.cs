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
    public class UIPage<TViewModel, TModel, TView> : UIPageBase<TViewModel, TModel, TView>
        where TViewModel : ViewModelBase<TModel, TView>
        where TModel : ModelBase, new()
        where TView : ViewBase, new()
    {
        public UIPage(string pageName, GameObject gameObject) : base(pageName, gameObject) { }

        public override void ShowUIPage()
        {
            ViewModel.Init(GameObject);
        }
    }
}
