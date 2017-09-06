using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace ExtendedLibrary.Events
{
    public static class ExtendedDelegateCreator
    {
        private enum DelegateType
        {
            Action0 = 0,
            Action1,
            Action2,
            Action3,
            Action4,
            UnityAction0,
            UnityAction1,
            UnityAction2,
            UnityAction3,
            UnityAction4,
            Func0,
            Func1,
            Func2,
            Func3,
            Func4
        }

        private static readonly Dictionary<DelegateType, Type> _delegateTypes = new Dictionary<DelegateType, Type>
        {
            { DelegateType.Action0, typeof(ExtendedAction) },
            { DelegateType.Action1, typeof(ExtendedAction<>) },
            { DelegateType.Action2, typeof(ExtendedAction<,>) },
            { DelegateType.Action3, typeof(ExtendedAction<,,>) },
            { DelegateType.Action4, typeof(ExtendedAction<,,,>) },
            { DelegateType.UnityAction0, typeof(ExtendedUnityAction) },
            { DelegateType.UnityAction1, typeof(ExtendedUnityAction<>) },
            { DelegateType.UnityAction2, typeof(ExtendedUnityAction<,>) },
            { DelegateType.UnityAction3, typeof(ExtendedUnityAction<,,>) },
            { DelegateType.UnityAction4, typeof(ExtendedUnityAction<,,,>) },
            { DelegateType.Func0, typeof(ExtendedFunc<>) },
            { DelegateType.Func1, typeof(ExtendedFunc<,>) },
            { DelegateType.Func2, typeof(ExtendedFunc<,,>) },
            { DelegateType.Func3, typeof(ExtendedFunc<,,,>) },
            { DelegateType.Func4, typeof(ExtendedFunc<,,,,>) }
        };

        public static ExtendedDelegate ToExtendedDelegate(this Action action)
        {
            return new ExtendedAction(action);
        }

        public static ExtendedDelegate ToExtendedDelegate<T>(this Action<T> action, T param)
        {
            var type = _delegateTypes[DelegateType.Action1].MakeGenericType(typeof(T));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2>(this Action<T1, T2> action, T1 param1, T2 param2)
        {
            var type = _delegateTypes[DelegateType.Action2].MakeGenericType(typeof(T1), typeof(T2));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param1, param2);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, T3>(this Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
        {
            var type = _delegateTypes[DelegateType.Action3].MakeGenericType(typeof(T1), typeof(T2), typeof(T3));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param1, param2, param3);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            var type = _delegateTypes[DelegateType.Action4].MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param1, param2, param3, param4);
        }

        public static ExtendedDelegate ToExtendedDelegate(this UnityAction action)
        {
            return new ExtendedUnityAction(action);
        }

        public static ExtendedDelegate ToExtendedDelegate<T>(this UnityAction<T> action, T param)
        {
            var type = _delegateTypes[DelegateType.UnityAction1].MakeGenericType(typeof(T));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2>(this UnityAction<T1, T2> action, T1 param1, T2 param2)
        {
            var type = _delegateTypes[DelegateType.UnityAction2].MakeGenericType(typeof(T1), typeof(T2));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param1, param2);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, T3>(this UnityAction<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
        {
            var type = _delegateTypes[DelegateType.UnityAction3].MakeGenericType(typeof(T1), typeof(T2), typeof(T3));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param1, param2, param3);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, T3, T4>(this UnityAction<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            var type = _delegateTypes[DelegateType.UnityAction4].MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

            return (ExtendedDelegate)Activator.CreateInstance(type, action, param1, param2, param3, param4);
        }

        public static ExtendedDelegate ToExtendedDelegate<TResult>(this Func<TResult> func)
        {
            var type = _delegateTypes[DelegateType.Func0].MakeGenericType(typeof(TResult));

            return (ExtendedDelegate)Activator.CreateInstance(type, func);
        }

        public static ExtendedDelegate ToExtendedDelegate<T, TResult>(this Func<T, TResult> func, T param)
        {
            var type = _delegateTypes[DelegateType.Func1].MakeGenericType(typeof(T), typeof(TResult));

            return (ExtendedDelegate)Activator.CreateInstance(type, func, param);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 param1, T2 param2)
        {
            var type = _delegateTypes[DelegateType.Func2].MakeGenericType(typeof(T1), typeof(T2), typeof(TResult));

            return (ExtendedDelegate)Activator.CreateInstance(type, func, param1, param2);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 param1, T2 param2, T3 param3)
        {
            var type = _delegateTypes[DelegateType.Func3].MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(TResult));

            return (ExtendedDelegate)Activator.CreateInstance(type, func, param1, param2, param3);
        }

        public static ExtendedDelegate ToExtendedDelegate<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            var type = _delegateTypes[DelegateType.Func4].MakeGenericType(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(TResult));

            return (ExtendedDelegate)Activator.CreateInstance(type, func, param1, param2, param3, param4);
        }
    }
}
