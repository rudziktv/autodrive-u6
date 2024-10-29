using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public abstract class UIController<T> where T : MonoBehaviour
    {
        public VisualElement Root { get; private set; }
        public T Context { get; private set; }
        private UIModel<T> _currentModel;

        protected virtual void OnInitialize(T context, VisualElement root, UIModel<T> model)
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
            _currentModel?.OnUpdate();
        }

        public virtual void NavigateTo(UIModel<T> newModel)
        {
            _currentModel?.OnViewUnbind();
            AssignViewModel(newModel);
        }

        protected virtual void AssignViewModel(UIModel<T> model)
        {
            _currentModel = model;
            Root.Clear();
            model.View.style.flexGrow = 1;
            Root.Add(model.View);
            _currentModel.OnViewBindOrCreate();
        }
    }
}