using System;
using System.Collections.Generic;
using VContainer.Unity;

namespace TFrameWork.UI
{
    public class UIManager :  IUIManager,IService
    {
        public Dictionary<string, UIPage> UIPages { get; private set; }

        public bool HasUIPage(string UIPageName)
        {
            return UIPages.ContainsKey(UIPageName);
        }

        public bool GetUIPage(string UIPageName, out UIPage page)
        {
            UIPages.TryGetValue(UIPageName, out page);
            return page == null ? false : true;
        }

        public bool GetUIPage<T>(out UIPage page) where T : UIPage, new()
        {
            UIPages.TryGetValue(typeof(T).ToString(), out page);
            return page == null ? false : true;
        }

        public UIPage GetUIPage(string UIPageName)
        {
            UIPage page = new UIPage();
            UIPages.TryGetValue(UIPageName, out page);
            return page;
        }

        public T GetUIPage<T>() where T : UIPage, new()
        {
            UIPage page = new UIPage();
            UIPages.TryGetValue(typeof(T).ToString(), out page);
            return (T)page;
        }

        public UIPage CreateUIPage(string UIPageName)
        {
            throw new NotImplementedException();
        }

        public UIPage ShowUIPage(string UIPageName)
        {
            UIPage page;
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
            UIPage page;
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
            UIPage page;
            if (GetUIPage(UIPageName, out page))
            {
                page.RefreshUIPage();
            }
        }

        public bool HideUIPage(string UIPageName)
        {
            UIPage page;
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
