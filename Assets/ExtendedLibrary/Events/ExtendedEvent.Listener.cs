using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedLibrary.Events
{
    public partial class ExtendedEvent
    {
        [Serializable]
        public class Listener
        {
#if UNITY_EDITOR
            public enum MemberFilter
            {
                Instance,
                Static
            }

            public enum VisibilityFilter
            {
                Public,
                NonPublic
            }

            public enum LevelFilter
            {
                Declare,
                Inherit
            }

#pragma warning disable CS0414

            [HideInInspector]
            [SerializeField]
            private MemberFilter memberFilter;

            [HideInInspector]
            [SerializeField]
            private VisibilityFilter visibilityFilter;

            [HideInInspector]
            [SerializeField]
            private LevelFilter levelFilter;

#pragma warning restore CS0414
#endif // UNITY_EDITOR

            [HideInInspector]
            [SerializeField]
            private string selectedLabel;

            [HideInInspector]
            [SerializeField]
            private int index;

            [HideInInspector]
            [SerializeField]
            private UnityEngine.Object target;

            [HideInInspector]
            [SerializeField]
            private string targetName;

            [HideInInspector]
            [SerializeField]
            private MemberInfo memberInfo;

            [HideInInspector]
            [SerializeField]
            private List<Value> values;

            private Value[] valueArray;

            public object Invoke()
            {
                if (this.index < 0)
                    return null;

                try
                {
#if UNITY_EDITOR
                    if (this.valueArray == null)
                    {
                        this.valueArray = this.values == null ? new Value[0] : this.values.ToArray();
                    }

                    this.memberInfo.Initialize(this.target, this.valueArray);
#endif // UNITY_EDITOR

                    return this.memberInfo.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("{0}: {1}\n{2}\n{3}", this.targetName, this.selectedLabel, e.Message, e.StackTrace));
                    return null;
                }
            }

            internal void Initialize()
            {
                if (this.index < 0)
                    return;

                try
                {
                    if (this.valueArray == null)
                    {
                        this.valueArray = this.values == null ? new Value[0] : this.values.ToArray();
                    }

                    this.memberInfo.Initialize(this.target, this.valueArray);
                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("{0}: {1}\n{2}\n{3}", this.targetName, this.selectedLabel, e.Message, e.StackTrace));
                    return;
                }
            }
        }
    }
}
