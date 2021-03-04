namespace TFrameWork.UI
{
    public interface IViewModelBase
    {
        string PageName { get; }
        void Init();
        void Destory();
    }
}
