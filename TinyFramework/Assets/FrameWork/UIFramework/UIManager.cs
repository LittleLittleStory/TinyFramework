﻿using System;
using System.Collections.Generic;
using TFramework.VContainer;
using UnityEngine;

namespace TFramework.UI
{
    public class UIManager : IUIManager, IService
    {
        public static Transform MainCanvas { get; private set; }
        public UIManager()
        {
            UIPages = new Dictionary<string, IUIPage>();
            MainCanvas = GameObject.Find("MainCanvas").transform;
        }

        public Dictionary<string, IUIPage> UIPages { get; private set; }

        public bool HasUIPage(string UIPageName)
        {
            return UIPages.ContainsKey(UIPageName);
        }

        public bool GetUIPage(string UIPageName, out IUIPage page)
        {
            UIPages.TryGetValue(UIPageName, out page);
            return page == null ? false : true;
        }

        public bool GetUIPage(out IUIPage pageType)
        {
            throw new NotImplementedException();
        }

        public IUIPage CreateUIPage<TViewModel, TModel, TView>(string UIPageName, Transform canvas = null)
            where TViewModel : ViewModelBase<TModel, TView>
            where TModel : ModelBase, new()
            where TView : ViewBase, new()
        {
            var prefabs = Resources.Load<GameObject>(UIPageName);
            canvas = canvas == null ? MainCanvas : canvas;
            var ui = GameObject.Instantiate(prefabs, canvas);
            UIPage<TViewModel, TModel, TView> page = new UIPage<TViewModel, TModel, TView>(UIPageName, ui);
            GameLanucher.Instance.Container.Inject(page);
            return page;
        }

        public IUIPage ShowUIPage(string UIPageName)
        {
            IUIPage page;
            if (GetUIPage(UIPageName, out page))
            {
                page.ShowUIPage();
                return page;
            }
            else
            {
                return page;
            }
        }

        public bool CloseUIPage(string UIPageName)
        {
            IUIPage page;
            if (GetUIPage(UIPageName, out page))
            {
                page.CloseUIPage();
                UIPages.Remove(UIPageName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RefreshUIPage(string UIPageName)
        {
            IUIPage page;
            if (GetUIPage(UIPageName, out page))
            {
                page.RefreshUIPage();
            }
        }

        public bool HideUIPage(string UIPageName)
        {
            IUIPage page;
            if (GetUIPage(UIPageName, out page))
            {
                page.HideUIPage();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ShowAllUIPages()
        {
            throw new NotImplementedException();
        }

        public void CloseAllUIPage(bool notExpect)
        {
            throw new NotImplementedException();
        }

        public void HideAllUIPages(bool notExpect)
        {
            throw new NotImplementedException();
        }

        public int GetCurUIPagesCount()
        {
            return UIPages.Count;
        }
    }
}
