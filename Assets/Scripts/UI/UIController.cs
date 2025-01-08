using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIController<T> where T : MonoBehaviour
    {
        public VisualElement Root { get; private set; }
        public T Context { get; private set; }
        protected UIModel<T> CurrentModel;

        public virtual void Initialize(T context, VisualElement root, UIModel<T> model)
        {
            Context = context;
            Root = root;
            OnViewCreate(model);
        }

        protected virtual void OnViewCreate(UIModel<T> model)
        {
            AssignViewModel(model);
            OnViewCreated();
        }

        protected virtual void OnViewCreated() { }

        public virtual void OnUpdate()
        {
            CurrentModel?.OnUpdate();
        }

        /// <summary>
        /// Navigates to new model and its View.
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns>Previous model.</returns>
        public virtual UIModel<T> NavigateTo(UIModel<T> newModel)
        {
            var oldModel = CurrentModel;
            CurrentModel?.OnViewUnbind();
            AssignViewModel(newModel);
            return oldModel;
        }
        
        /// <summary>
        /// Navigates to new model and their View. The old model is destroyed and unbind.
        /// </summary>
        /// <param name="newModel"></param>
        public virtual void NavigateToAndDestroy(UIModel<T> newModel)
        {
            CurrentModel?.OnViewUnbind();
            CurrentModel?.OnDestroy();
            CurrentModel = null;
            AssignViewModel(newModel);
        }

        protected virtual void AssignViewModel(UIModel<T> model)
        {
            CurrentModel = model;
            AssignView(model.View);
            CurrentModel.OnViewBindOrCreate();
        }

        protected virtual void AssignView(VisualElement view)
        {
            Root.Clear();
            view.style.flexGrow = 1;
            Root.Add(view);
        }

        public virtual void Dispose()
        {
            CurrentModel?.OnDestroy();
            CurrentModel = null;
            Root.Clear();
        }
    }
}