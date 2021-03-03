using System.Collections.Generic;

namespace  TFrameWork.UI
{
    public interface IUIManager<T> where T:IUIPage
    {
        /// <summary>
        /// UIPage管理容器
        /// </summary>
        Dictionary<string, T> UIPages { get; }

        /// <summary>
        /// 通过UIPageName判断对应UIPage是否存在
        /// </summary>
        /// <param name="UIPageName"></param>
        /// <returns></returns>
        bool HasUIPage(string UIPageName);

        /// <summary>
        /// 通过Name得到对应UIPage
        /// </summary>
        bool GetUIPage(out T page);

        /// <summary>
        /// 创建UIPage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="UILayer"></param>
        /// <returns></returns>
        T CreateUIPage(string UIPageName);

        /// <summary>
        /// 展示指定UIPage
        /// </summary>
        /// <param name="UIPageName"></param>
        /// <returns></returns>
        T ShowUIPage(string UIPageName);

        /// <summary>
        /// 关闭指定UIPage
        /// </summary>
        /// <param name="UIPageName"></param>
        /// <returns></returns>
        bool CloseUIPage(string UIPageName);

        /// <summary>
        /// 刷新指定UIPage
        /// </summary>
        /// <param name="UIPageName"></param>
        /// <returns></returns>
        void RefreshUIPage(string UIPageName);

        /// <summary>
        /// 隐藏指定UIPage
        /// </summary>
        /// <param name="UIPageName"></param>
        /// <returns></returns>
        bool HideUIPage(string UIPageName);

        /// <summary>
        /// 展示所有UIPage
        /// </summary>
        void ShowAllUIPages();

        /// <summary>
        /// 关闭所有UIPage
        /// </summary>
        /// <param name="notExpect"></param>
        void CloseAllUIPage(bool notExpect);

        /// <summary>
        /// 隐藏所有UIPage
        /// </summary>
        /// <param name="notExpect"></param>
        void HideAllUIPages(bool notExpect);

        /// <summary>
        /// 得到当前所有UIPage的数量
        /// </summary>
        /// <param name="notExpect"></param>
        int GetCurUIPagesCount();
    }
}
