using System;
using System.Reflection;
using UnityEngine;

namespace ExtendedLibrary.Events
{
    public partial class ExtendedEvent
    {
        [Serializable]
        public class MemberInfo
        {
            private static Type[] NoType = new Type[0];

            [HideInInspector]
            [SerializeField]
            private ObjectType returnType = ObjectType.Void;

            [HideInInspector]
            [SerializeField]
            private MemberType memberType = MemberType.None;

            [HideInInspector]
            [SerializeField]
            private string memberName = string.Empty;

            [HideInInspector]
            [SerializeField]
            private UnityEngine.Object target;

            [HideInInspector]
            [SerializeField]
            private string targetFullName = string.Empty;

            /// <summary>
            /// AssemblyQualifiedName
            /// </summary>
            [HideInInspector]
            [SerializeField]
            private string[] parameterTypes = new string[0];

            private Type typeOf;
            private FieldInfo field;
            private PropertyInfo property;
            private MethodInfo method;
            private object[] parameters;
            private object value;
            private bool initialized = false;

            public object Invoke()
            {
                if (this.target == null)
                {
                    throw new ExtendedEventException(string.Format("No target."));
                }

                switch (this.memberType)
                {
                    case MemberType.Field:
                        return InvokeField();

                    case MemberType.Property:
                        return InvokeProperty();

                    case MemberType.Method:
                        return InvokeMethod();

                    default:
                        return null;
                }
            }

            private object InvokeField()
            {
                if (this.field == null)
                {
                    throw new ExtendedEventException(string.Format("Field {0} cannot be found.", this.memberName));
                }

                if (this.value != null)
                {
                    this.field.SetValue(this.target, this.value);
                }

                return this.value;
            }

            private object InvokeProperty()
            {
                if (this.property == null)
                {
                    throw new ExtendedEventException(string.Format("Property {0} cannot be found.", this.memberName));
                }

                if (this.value != null)
                {
                    this.property.SetValue(this.target, this.value, null);
                }

                return this.value;
            }

            private object InvokeMethod()
            {
                if (this.method == null)
                {
                    throw new ExtendedEventException(string.Format("Method {0} cannot be found.", this.memberName));
                }

                if (this.parameterTypes.Length <= 0)
                {
                    return this.method.Invoke(this.target, null);
                }

                if (this.parameters != null)
                {
                    return this.method.Invoke(this.target, this.parameters);
                }

                return null;
            }

            private UnityEngine.Object GetActualTarget(UnityEngine.Object target)
            {
                try
                {
                    if (target is GameObject)
                    {
                        return ((GameObject)target).GetComponent(this.targetFullName);
                    }

                    return target;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            internal void Initialize(UnityEngine.Object target, Value[] values)
            {
                try
                {
                    if (this.initialized)
                        return;

                    if (this.target == null)
                    {
                        if (this.memberType == MemberType.None || string.IsNullOrEmpty(this.targetFullName) || this.returnType == ObjectType.Void)
                        {
                            return;
                        }

                        this.target = GetActualTarget(target);

                        if (this.target == null)
                        {
                            return;
                        }
                    }

                    if (this.typeOf == null)
                    {
                        this.typeOf = this.target.GetType();
                    }

                    var paramTypes = GetParameterTypes(values);

                    switch (this.memberType)
                    {
                        case MemberType.Field:
                            {
                                this.field = this.typeOf.GetField(this.memberName);

                                if (this.field == null)
                                    this.field = this.typeOf.GetField(this.memberName, BindingFlags.Instance | BindingFlags.NonPublic);

                                if (paramTypes.Length <= 0)
                                {
                                    return;
                                }

                                this.value = values[0].Get();
                            }
                            break;

                        case MemberType.Property:
                            {
                                this.property = this.typeOf.GetProperty(this.memberName);

                                if (this.property == null)
                                    this.property = this.typeOf.GetProperty(this.memberName, BindingFlags.Instance | BindingFlags.NonPublic);

                                if (paramTypes.Length <= 0)
                                {
                                    return;
                                }

                                this.value = values[0].Get();
                            }
                            break;

                        case MemberType.Method:
                            {
                                if (paramTypes.Length != this.parameterTypes.Length)
                                {
                                    return;
                                }

                                this.method = this.typeOf.GetMethod(this.memberName, paramTypes);

                                if (this.method == null)
                                {
                                    this.method = this.typeOf.GetMethod(this.memberName, BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, paramTypes, null);
                                }

                                this.parameters = new object[values.Length];

                                for (var i = 0; i < values.Length; ++i)
                                {
                                    this.parameters[i] = values[i].Get();
                                }
                            }
                            break;
                    }

                    this.initialized = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            private Type[] GetParameterTypes(Value[] values)
            {
                if (values == null)
                    return NoType;

                var paramTypes = new Type[values.Length];

                switch (this.memberType)
                {
                    case MemberType.Field:
                    case MemberType.Property:
                        {
                            if (values == null || values.Length < 1)
                            {
                                return NoType;
                            }

                            if (this.parameterTypes.Length != values.Length)
                            {
                                throw new ExtendedEventException(string.Format("Invalid length: {0} expected {1}, received {2}.",
                                    this.memberName, this.parameterTypes.Length, values.Length));
                            }

                            if (!this.parameterTypes[0].Equals(values[0].FullTypeName))
                            {
                                throw new ExtendedEventException(string.Format("Types mismatched: {0} expected {1}, received {2}.",
                                    this.memberName, this.parameterTypes[0], values[0].FullTypeName));
                            }

                            paramTypes[0] = values[0].TypeOf;
                        }
                        break;

                    case MemberType.Method:
                        {
                            if (this.parameterTypes.Length > 0 &&
                               (values == null || values.Length <= 0 || this.parameterTypes.Length != values.Length))
                            {
                                return NoType;
                            }

                            for (var i = 0; i < values.Length; ++i)
                            {
                                if (values[i].TypeOf == null)
                                {
                                    throw new ExtendedEventException(string.Format("{0} null parameter at {1}: {2}",
                                        this.memberName, i, values[i].FullTypeName));
                                }

                                if (!this.parameterTypes[i].Equals(values[i].FullTypeName))
                                {
                                    throw new ExtendedEventException(string.Format("{0} types mismatched at {1}: expected {2}, received {3}.",
                                        this.memberName, i, this.parameterTypes[i], values[i].FullTypeName));
                                }

                                paramTypes[i] = values[i].TypeOf;
                            }
                        }
                        break;
                }

                return paramTypes;
            }
        }
    }
}
