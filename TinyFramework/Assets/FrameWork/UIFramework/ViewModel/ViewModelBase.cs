using UnityEngine;

namespace TFramework.UI
{
    public class ViewModelBase<TModel, TView> : IViewModelBase
        where TModel : ModelBase, new()
        where TView : ViewBase, new()
    {
        private TView view;
        private TModel model;

        public ViewModelBase()
        {
            view = new TView();
            model = new TModel();
        }

        public string PageName { get; private set; }
        protected UIManager uiManager { get; private set; }

        public void Init(GameObject gameObject)
        {
            view.Init(gameObject);
            SetupModel(view, model);
            SetupEvent(view);
        }

        public virtual void SetupModel(TView view, TModel model)
        {

        }

        public virtual void SetupEvent(TView model)
        {

        }

        public void Destory()
        {

        }
    }
}
