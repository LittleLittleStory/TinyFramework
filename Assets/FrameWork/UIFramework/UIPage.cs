using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TFrameWork.UI
{
    public class UIPage : MonoBehaviour, IUIPage
    {
        public string PageName { get; private set; }

        public ViewModelBase<ModelBase, ViewBase> ViewModel { get; private set; }

        public void ShowUIPage()
        {
            throw new NotImplementedException();
        }

        public void CloseUIPage()
        {
            throw new NotImplementedException();
        }

        public void RefreshUIPage()
        {
            throw new NotImplementedException();
        }

        public void HideUIPage()
        {
            throw new NotImplementedException();
        }
    }
}
