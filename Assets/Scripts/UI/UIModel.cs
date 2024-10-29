using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIModel<T> where T : MonoBehaviour
    {
        public string Name { get; private set; }
        protected T Context => _controller.Context;
        private readonly UIController<T> _controller;
        public VisualElement View { get; }

        private List<Coroutine> _coroutines = new();
        public bool Created { get; private set; }
        
        public UIModel(UIController<T> ctr, VisualElement view, string name)
        {
            _controller = ctr;
            View = view;
            Name = name;
        }

        protected UIController<T> GetController() => _controller;

        protected void StartCoroutine(IEnumerator coroutine)
        {
            _coroutines.Add(Context.StartCoroutine(coroutine));
        }

        public virtual void OnViewBindOrCreate()
        {
            if (!Created)
                OnViewCreated();
            OnViewBind();
        }

        protected virtual void OnViewCreated()
        {
            Created = true;
        }
        protected virtual void OnViewBind() { }

        public virtual void OnViewUnbind()
        {
            foreach (var coroutine in _coroutines)
            {
                Context.StopCoroutine(coroutine);
            }
        }
        
        public virtual void OnUpdate() { }

        public virtual void OnDestroy() { }

        ~UIModel()
        {
            OnViewUnbind();
            OnDestroy();
        }
    }
}