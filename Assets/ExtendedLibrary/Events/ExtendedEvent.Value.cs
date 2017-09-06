using System;
using UnityEngine;

namespace ExtendedLibrary.Events
{
    public partial class ExtendedEvent
    {
        [Serializable]
        public class Value
        {
            private Type typeOf;
            private object value = null;
            private bool hasValue = false;

            /// <summary>
            /// AssemblyQualifiedName
            /// </summary>
            [HideInInspector]
            [SerializeField]
            private string fullTypeName = string.Empty;

            [HideInInspector]
            [SerializeField]
            private ObjectType type = ObjectType.Void;

            [HideInInspector]
            [SerializeField]
            private bool boolValue;

            [HideInInspector]
            [SerializeField]
            private byte byteValue;

            [HideInInspector]
            [SerializeField]
            private sbyte sbyteValue;

            [HideInInspector]
            [SerializeField]
            private char charValue;

            [HideInInspector]
            [SerializeField]
            private short shortValue;

            [HideInInspector]
            [SerializeField]
            private ushort ushortValue;

            [HideInInspector]
            [SerializeField]
            private int intValue;

            [HideInInspector]
            [SerializeField]
            private uint uintValue;

            [HideInInspector]
            [SerializeField]
            private long longValue;

            [HideInInspector]
            [SerializeField]
            private float floatValue;

            [HideInInspector]
            [SerializeField]
            private double doubleValue;

            [HideInInspector]
            [SerializeField]
            private long enumValue;

            [HideInInspector]
            [SerializeField]
            private string stringValue = string.Empty;

            [HideInInspector]
            [SerializeField]
            private LayerMask layerMaskValue;

            [HideInInspector]
            [SerializeField]
            private Vector2 vector2Value;

            [HideInInspector]
            [SerializeField]
            private Vector3 vector3Value;

            [HideInInspector]
            [SerializeField]
            private Vector4 vector4Value;

            [HideInInspector]
            [SerializeField]
            private Color colorValue;

            [HideInInspector]
            [SerializeField]
            private Bounds boundsValue;

            [HideInInspector]
            [SerializeField]
            private Rect rectValue;

            [HideInInspector]
            [SerializeField]
            private Quaternion quaternionValue;

            [HideInInspector]
            [SerializeField]
            private AnimationCurve animationCurveValue;

            [HideInInspector]
            [SerializeField]
            private UnityEngine.Object unityObjectReference;

            [HideInInspector]
            [SerializeField]
            private string serializedValue = string.Empty;

            public ObjectType Type
            {
                get { return this.type; }
            }

            /// <summary>
            /// AssemblyQualifiedName
            /// </summary>
            public string FullTypeName
            {
                get { return this.fullTypeName; }
            }

            public Type TypeOf
            {
                get
                {
                    if (this.typeOf == null)
                    {
                        if (this.type == ObjectType.Void)
                            return null;

                        if (string.IsNullOrEmpty(this.fullTypeName))
                            return null;

                        try
                        {
                            this.typeOf = System.Type.GetType(this.fullTypeName);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
                            return null;
                        }
                    }

                    return this.typeOf;
                }
            }

            public Value()
            {
            }

            public Value(object value)
            {
                Set(value);
            }

            public object Get()
            {
                if (this.hasValue)
                    return this.value;

                var typeOf = this.TypeOf;

                if (typeOf == null)
                {
                    this.hasValue = true;
                    return this.value;
                }

                switch (this.type)
                {
                    case ObjectType.Boolean:
                        this.value = this.boolValue;
                        break;

                    case ObjectType.Byte:
                        this.value = this.byteValue;
                        break;

                    case ObjectType.SByte:
                        this.value = this.sbyteValue;
                        break;

                    case ObjectType.Char:
                        this.value = this.charValue;
                        break;

                    case ObjectType.Int16:
                        this.value = this.shortValue;
                        break;

                    case ObjectType.UInt16:
                        this.value = this.ushortValue;
                        break;

                    case ObjectType.Int32:
                        this.value = this.intValue;
                        break;

                    case ObjectType.UInt32:
                        this.value = this.uintValue;
                        break;

                    case ObjectType.Int64:
                        this.value = this.longValue;
                        break;

                    case ObjectType.UInt64:
                        ulong ulongValue;
                        ulong.TryParse(this.serializedValue, out ulongValue);
                        this.value = ulongValue;
                        break;

                    case ObjectType.Single:
                        this.value = this.floatValue;
                        break;

                    case ObjectType.Double:
                        this.value = this.doubleValue;
                        break;

                    case ObjectType.Enum:
                        try
                        {
                            this.value = Enum.ToObject(typeOf, this.enumValue);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
                            return null;
                        }

                    case ObjectType.String:
                        this.value = this.stringValue;
                        break;

                    case ObjectType.LayerMask:
                        this.value = this.layerMaskValue;
                        break;

                    case ObjectType.Vector2:
                        this.value = this.vector2Value;
                        break;

                    case ObjectType.Vector3:
                        this.value = this.vector3Value;
                        break;

                    case ObjectType.Vector4:
                        this.value = this.vector4Value;
                        break;

                    case ObjectType.Color:
                        this.value = this.colorValue;
                        break;

                    case ObjectType.Bounds:
                        this.value = this.boundsValue;
                        break;

                    case ObjectType.Rect:
                        this.value = this.rectValue;
                        break;

                    case ObjectType.Quaternion:
                        this.value = this.quaternionValue;
                        break;

                    case ObjectType.Matrix4x4:
                        this.value = this.serializedValue.ToObject<Matrix4x4>();
                        break;

                    case ObjectType.AnimationCurve:
                        this.value = this.animationCurveValue;
                        break;

                    case ObjectType.UnityObject:
                        this.value = this.unityObjectReference;
                        break;

                    case ObjectType.SerializableType:
                    case ObjectType.Array:
                    case ObjectType.List:
                        try
                        {
                            var value = this.serializedValue.ToObject(typeOf);

                            if (value == null)
                            {
                                value = Activator.CreateInstance(typeOf);
                            }

                            this.value = value;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
                            return null;
                        }
                }

                this.hasValue = true;
                return this.value;
            }

            public void Set(object value)
            {
                this.typeOf = value.GetType();
                this.type = this.typeOf.GetSerializableObjectType();

                if (this.type == ObjectType.Void)
                    return;

                this.fullTypeName = this.typeOf.GetSerializableAssemblyQualifiedName();

                try
                {
                    switch (this.type)
                    {
                        case ObjectType.Void:
                            break;

                        case ObjectType.Boolean:
                            this.boolValue = (bool) value;
                            break;

                        case ObjectType.Byte:
                            this.byteValue = (byte) value;
                            break;

                        case ObjectType.SByte:
                            this.sbyteValue = (sbyte) value;
                            break;

                        case ObjectType.Char:
                            this.charValue = (char) value;
                            break;

                        case ObjectType.Int16:
                            this.shortValue = (short) value;
                            break;

                        case ObjectType.UInt16:
                            this.ushortValue = (ushort) value;
                            break;

                        case ObjectType.Int32:
                            this.intValue = (int) value;
                            break;

                        case ObjectType.UInt32:
                            this.uintValue = (uint) value;
                            break;

                        case ObjectType.Int64:
                            this.longValue = (long) value;
                            break;

                        case ObjectType.UInt64:
                            this.serializedValue = value.ToString();
                            break;

                        case ObjectType.Single:
                            this.floatValue = (float) value;
                            break;

                        case ObjectType.Double:
                            this.doubleValue = (double) value;
                            break;

                        case ObjectType.Enum:
                            this.enumValue = (long) value;
                            break;

                        case ObjectType.String:
                            this.stringValue = (string) value;
                            break;

                        case ObjectType.LayerMask:
                            this.layerMaskValue = (LayerMask) value;
                            break;

                        case ObjectType.Vector2:
                            this.vector2Value = (Vector2) value;
                            break;

                        case ObjectType.Vector3:
                            this.vector3Value = (Vector3) value;
                            break;

                        case ObjectType.Vector4:
                            this.vector4Value = (Vector4) value;
                            break;

                        case ObjectType.Color:
                            this.colorValue = (Color) value;
                            break;

                        case ObjectType.Bounds:
                            this.boundsValue = (Bounds) value;
                            break;

                        case ObjectType.Rect:
                            this.rectValue = (Rect) value;
                            break;

                        case ObjectType.Quaternion:
                            this.quaternionValue = (Quaternion) value;
                            break;

                        case ObjectType.AnimationCurve:
                            this.animationCurveValue = (AnimationCurve) value;
                            break;

                        case ObjectType.UnityObject:
                            this.unityObjectReference = (UnityEngine.Object) value;
                            break;

                        case ObjectType.Matrix4x4:
                        case ObjectType.SerializableType:
                        case ObjectType.Array:
                        case ObjectType.List:
                            this.serializedValue = value.ToJson(this.typeOf);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
    }
}
