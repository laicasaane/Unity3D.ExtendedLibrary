using System;
using System.Reflection;
using UnityEngine.Events;

namespace ExtendedLibrary.Events
{
    public abstract class ExtendedDelegate
    {
        public MethodInfo Method { get; private set; }

        public object Target { get; private set; }

        public abstract object Invoke();

        protected ExtendedDelegate(MethodInfo method, object target)
        {
            this.Method = method;
            this.Target = target;
        }
    }

    public sealed class ExtendedAction : ExtendedDelegate
    {
        private Action action;

        public ExtendedAction(Action action)
            : base(action.Method, action.Target)
        {
            this.action = action;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke();
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null Action.");
        }
    }

    public sealed class ExtendedAction<T> : ExtendedDelegate
    {
        private Action<T> action;

        private T param;

        public ExtendedAction(Action<T> action, T param)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param = param;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null Action.");
        }
    }

    public sealed class ExtendedAction<T1, T2> : ExtendedDelegate
    {
        private Action<T1, T2> action;

        private T1 param1;

        private T2 param2;

        public ExtendedAction(Action<T1, T2> action, T1 param1, T2 param2)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param1, this.param2);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null Action.");
        }
    }

    public sealed class ExtendedAction<T1, T2, T3> : ExtendedDelegate
    {
        private Action<T1, T2, T3> action;

        private T1 param1;

        private T2 param2;

        private T3 param3;

        public ExtendedAction(Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param1, this.param2, this.param3);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null Action.");
        }
    }

    public sealed class ExtendedAction<T1, T2, T3, T4> : ExtendedDelegate
    {
        private Action<T1, T2, T3, T4> action;

        private T1 param1;

        private T2 param2;

        private T3 param3;

        private T4 param4;

        public ExtendedAction(Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            this.param4 = param4;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param1, this.param2, this.param3, this.param4);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null Action.");
        }
    }

    public sealed class ExtendedUnityAction : ExtendedDelegate
    {
        private UnityAction action;

        public ExtendedUnityAction(UnityAction action)
            : base(action.Method, action.Target)
        {
            this.action = action;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke();
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null UnityAction.");
        }
    }

    public sealed class ExtendedUnityAction<T> : ExtendedDelegate
    {
        private UnityAction<T> action;

        private T param;

        public ExtendedUnityAction(UnityAction<T> action, T param)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param = param;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null UnityAction.");
        }
    }

    public sealed class ExtendedUnityAction<T1, T2> : ExtendedDelegate
    {
        private UnityAction<T1, T2> action;

        private T1 param1;

        private T2 param2;

        public ExtendedUnityAction(UnityAction<T1, T2> action, T1 param1, T2 param2)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param1, this.param2);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null UnityAction.");
        }
    }

    public sealed class ExtendedUnityAction<T1, T2, T3> : ExtendedDelegate
    {
        private UnityAction<T1, T2, T3> action;

        private T1 param1;

        private T2 param2;

        private T3 param3;

        public ExtendedUnityAction(UnityAction<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param1, this.param2, this.param3);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null UnityAction.");
        }
    }

    public sealed class ExtendedUnityAction<T1, T2, T3, T4> : ExtendedDelegate
    {
        private UnityAction<T1, T2, T3, T4> action;

        private T1 param1;

        private T2 param2;

        private T3 param3;

        private T4 param4;

        public ExtendedUnityAction(UnityAction<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
            : base(action.Method, action.Target)
        {
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            this.param4 = param4;
        }

        public override object Invoke()
        {
            if (this.action != null)
            {
                this.action.Invoke(this.param1, this.param2, this.param3, this.param4);
                return null;
            }
            else
                throw new InvalidOperationException("Cannot invoke null UnityAction.");
        }
    }

    public abstract class ExtendedFuncBase<TResult> : ExtendedDelegate
    {
        protected ExtendedFuncBase(MethodInfo method, object target)
            : base(method, target)
        {
        }
    }

    public sealed class ExtendedFunc<TResult> : ExtendedFuncBase<TResult>
    {
        private Func<TResult> func;

        public ExtendedFunc(Func<TResult> func)
            : base(func.Method, func.Target)
        {
            this.func = func;
        }

        public override object Invoke()
        {
            if (this.func != null)
                return this.func.Invoke();

            throw new InvalidOperationException("Cannot invoke null Func.");
        }
    }

    public sealed class ExtendedFunc<T, TResult> : ExtendedFuncBase<TResult>
    {
        private Func<T, TResult> func;

        private T param;

        public ExtendedFunc(Func<T, TResult> func, T param)
            : base(func.Method, func.Target)
        {
            this.func = func;
            this.param = param;
        }

        public override object Invoke()
        {
            if (this.func != null)
                return this.func.Invoke(this.param);

            throw new InvalidOperationException("Cannot invoke null Func.");
        }
    }

    public sealed class ExtendedFunc<T1, T2, TResult> : ExtendedFuncBase<TResult>
    {
        private Func<T1, T2, TResult> func;

        private T1 param1;

        private T2 param2;

        public ExtendedFunc(Func<T1, T2, TResult> func, T1 param1, T2 param2)
            : base(func.Method, func.Target)
        {
            this.func = func;
            this.param1 = param1;
            this.param2 = param2;
        }

        public override object Invoke()
        {
            if (this.func != null)
                return this.func.Invoke(this.param1, this.param2);

            throw new InvalidOperationException("Cannot invoke null Func.");
        }
    }

    public sealed class ExtendedFunc<T1, T2, T3, TResult> : ExtendedFuncBase<TResult>
    {
        private Func<T1, T2, T3, TResult> func;

        private T1 param1;

        private T2 param2;

        private T3 param3;

        public ExtendedFunc(Func<T1, T2, T3, TResult> func, T1 param1, T2 param2, T3 param3)
            : base(func.Method, func.Target)
        {
            this.func = func;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
        }

        public override object Invoke()
        {
            if (this.func != null)
                return this.func.Invoke(this.param1, this.param2, this.param3);

            throw new InvalidOperationException("Cannot invoke null Func.");
        }
    }

    public sealed class ExtendedFunc<T1, T2, T3, T4, TResult> : ExtendedFuncBase<TResult>
    {
        private Func<T1, T2, T3, T4, TResult> func;

        private T1 param1;

        private T2 param2;

        private T3 param3;

        private T4 param4;

        public ExtendedFunc(Func<T1, T2, T3, T4, TResult> func, T1 param1, T2 param2, T3 param3, T4 param4)
            : base(func.Method, func.Target)
        {
            this.func = func;
            this.param1 = param1;
            this.param2 = param2;
            this.param3 = param3;
            this.param4 = param4;
        }

        public override object Invoke()
        {
            if (this.func != null)
                return this.func.Invoke(this.param1, this.param2, this.param3, this.param4);

            throw new InvalidOperationException("Cannot invoke null Func.");
        }
    }
}
