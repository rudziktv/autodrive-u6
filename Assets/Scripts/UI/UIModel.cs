using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        protected List<Coroutine> Coroutines = new();
        public bool Created { get; private set; }
        
        public UIModel(UIController<T> ctr, VisualElement view, string name)
        {
            _controller = ctr;
            View = view;
            Name = name;
        }

        protected UIController<T> GetController() => _controller;

        protected Coroutine StartCoroutine(IEnumerator enumerator)
        {
            Coroutines = Coroutines.Where(c => c != null).ToList();
            Coroutines.Add(Context.StartCoroutine(enumerator));
            return Coroutines[^1];
        }

        // ReSharper disable Unity.PerformanceAnalysis
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
            Coroutines = Coroutines.Where(c => c != null).ToList();
            foreach (var coroutine in Coroutines)
            {
                Context.StopCoroutine(coroutine);
            }
        }
        
        public virtual void OnUpdate() { }

        public virtual void OnDestroy()
        {
            OnViewUnbind();
            View.Clear();
            // Context.Destroy();
        }

        ~UIModel()
        {
            OnDestroy();
        }
    }
}