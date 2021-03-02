namespace TFrameWork.UI
{
    public interface IUIPage
    {
        /// <summary>
        /// 界面名称
        /// </summary>
        string PageName { get; }

        /// <summary>
        /// 界面名称
        /// </summary>
        ViewModelBase<ModelBase,ViewBase> ViewModel  { get; }

        /// <summary>
        /// 展示UIPage
        /// </summary>
        void ShowUIPage();

        /// <summary>
        /// 关闭UIPage
        /// </summary>
        void CloseUIPage();

        /// <summary>
        /// 刷新UIPage
        /// </summary>
        void RefreshUIPage();

        /// <summary>
        /// 隐藏UIPage
        /// </summary>
        void HideUIPage();
    }
}
