using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace ExtendedLibrary.Events
{
    [Serializable]
    public partial class ExtendedEvent : ISerializationCallbackReceiver
    {
        [HideInInspector]
        [SerializeField]
        protected List<Listener> listeners;

        [HideInInspector]
        [SerializeField]
        protected string returnTypeName = string.Empty;

        protected readonly List<ExtendedDelegate> callbacks = new List<ExtendedDelegate>();

        private readonly List<object> results = new List<object>();

        public bool IsEmpty
        {
            get
            {
                return this.listeners.Count <= 0 && (this.callbacks == null || this.callbacks.Count <= 0);
            }
        }

        public void Invoke()
        {
            for (var i = 0; i < this.listeners.Count; ++i)
            {
                if (this.listeners[i] != null)
                    this.listeners[i].Invoke();
            }

            for (var i = 0; i < this.callbacks.Count; ++i)
            {
                if (this.callbacks[i] != null)
                    this.callbacks[i].Invoke();
            }
        }

        public void Invoke(out object[] results)
        {
            this.results.Clear();

            for (var i = 0; i < this.listeners.Count; ++i)
            {
                if (this.listeners[i] != null)
                    this.results.Add(this.listeners[i].Invoke());
            }

            for (var i = 0; i < this.callbacks.Count; ++i)
            {
                if (this.callbacks[i] != null)
                    this.results.Add(this.callbacks[i].Invoke());
            }

            results = this.results.ToArray();
        }

        public void Invoke(Action<object> resultReceiver)
        {
            if (resultReceiver == null)
            {
                Debug.LogWarning("Result receiver cannot be null.");
                return;
            }

            for (var i = 0; i < this.listeners.Count; ++i)
            {
                if (this.listeners[i] != null)
                    resultReceiver(this.listeners[i].Invoke());
            }

            for (var i = 0; i < this.callbacks.Count; ++i)
            {
                if (this.callbacks[i] != null)
                    resultReceiver(this.callbacks[i].Invoke());
            }
        }

        public void Invoke<T>(Func<object, T> resultReceiver)
        {
            if (resultReceiver == null)
            {
                Debug.LogWarning("Result receiver cannot be null.");
                return;
            }

            for (var i = 0; i < this.listeners.Count; ++i)
            {
                if (this.listeners[i] != null)
                    resultReceiver(this.listeners[i].Invoke());
            }

            for (var i = 0; i < this.callbacks.Count; ++i)
            {
                if (this.callbacks[i] != null)
                    resultReceiver(this.callbacks[i].Invoke());
            }
        }

        public void AddListeners(ExtendedEvent listeners)
        {
            if (listeners != null)
                this.callbacks.AddRange(listeners.callbacks);
        }

        public void AddListeners(IEnumerable<ExtendedDelegate> listeners)
        {
            if (listeners != null)
                this.callbacks.AddRange(listeners);
        }

        public void AddListener(ExtendedDelegate listener)
        {
            if (listener != null)
                this.callbacks.Add(listener);
        }

        public void AddListener(Action callback)
        {
            AddListener(callback.ToExtendedDelegate());
        }

        public void AddListener<T>(Action<T> callback, T param)
        {
            AddListener(callback.ToExtendedDelegate(param));
        }

        public void AddListener<T1, T2>(Action<T1, T2> callback, T1 param1, T2 param2)
        {
            AddListener(callback.ToExtendedDelegate(param1, param2));
        }

        public void AddListener<T1, T2, T3>(Action<T1, T2, T3> callback, T1 param1, T2 param2, T3 param3)
        {
            AddListener(callback.ToExtendedDelegate(param1, param2, param3));
        }

        public void AddListener<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            AddListener(callback.ToExtendedDelegate(param1, param2, param3, param4));
        }

        public void AddListener<TResult>(Func<TResult> callback)
        {
            AddListener(callback.ToExtendedDelegate());
        }

        public void AddListener<T, TResult>(Func<T, TResult> callback, T param)
        {
            AddListener(callback.ToExtendedDelegate(param));
        }

        public void AddListener<T1, T2, TResult>(Func<T1, T2, TResult> callback, T1 param1, T2 param2)
        {
            AddListener(callback.ToExtendedDelegate(param1, param2));
        }

        public void AddListener<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback, T1 param1, T2 param2, T3 param3)
        {
            AddListener(callback.ToExtendedDelegate(param1, param2, param3));
        }

        public void AddListener<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            AddListener(callback.ToExtendedDelegate(param1, param2, param3, param4));
        }

        public void RemoveListener(Action callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T>(Action<T> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2>(Action<T1, T2> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener(UnityAction callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T>(UnityAction<T> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2>(UnityAction<T1, T2> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, T3>(UnityAction<T1, T2, T3> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, T3, T4>(UnityAction<T1, T2, T3, T4> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<TResult>(Func<TResult> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T, TResult>(Func<T, TResult> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, TResult>(Func<T1, T2, TResult> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveListener<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> callback)
        {
            RemoveByMethodInfo(callback.Method);
        }

        public void RemoveAll()
        {
            this.listeners.Clear();
            this.callbacks.Clear();
        }

        protected void RemoveByMethodInfo(MethodInfo method)
        {
            var index = -1;

            for (var i = 0; i < this.callbacks.Count; i++)
            {
                if (this.callbacks[i].Method.Equals(method))
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
                this.callbacks.RemoveAt(index);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
#if !UNITY_EDITOR
            for (var i = 0; i < this.listeners.Count; ++i)
            {
                if (this.listeners[i] != null)
                    this.listeners[i].Initialize();
            }
#endif // !UNITY_EDITOR
        }
    }
}
