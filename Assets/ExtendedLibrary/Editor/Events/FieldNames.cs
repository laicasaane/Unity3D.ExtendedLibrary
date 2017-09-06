namespace ExtendedLibrary.Events
{
    public static class FakeObjectFields
    {
        public const string Value = "value";
    }

    public partial class ExtendedEventBaseEditor
    {
        public static class ExtendedEventFields
        {
            public const string Listeners = "listeners";

            public const string ReturnTypeName = "returnTypeName";
        }

        public static class ListenerFields
        {
            public const string Index = "index";

            public const string Target = "target";

            public const string TargetName = "targetName";

            public const string SelectedLabel = "selectedLabel";

            public const string MemberFilter = "memberFilter";

            public const string VisibilityFilter = "visibilityFilter";

            public const string LevelFilter = "levelFilter";

            public const string MemberInfo = "memberInfo";

            public const string Values = "values";
        }

        public static class MemberInfoFields
        {
            public const string ReturnType = "returnType";

            public const string MemberType = "memberType";

            public const string MemberName = "memberName";

            public const string Target = "target";

            public const string TargetFullName = "targetFullName";

            public const string ParameterTypes = "parameterTypes";
        }

        public static class ValueFields
        {
            public const string FullTypeName = "fullTypeName";

            public const string Type = "type";

            public const string BoolValue = "boolValue";

            public const string ByteValue = "byteValue";

            public const string SByteValue = "sbyteValue";

            public const string CharValue = "charValue";

            public const string ShortValue = "shortValue";

            public const string UShortValue = "ushortValue";

            public const string IntValue = "intValue";

            public const string UIntValue = "uintValue";

            public const string LongValue = "longValue";

            public const string ULongValue = "ulongValue";

            public const string FloatValue = "floatValue";

            public const string DoubleValue = "doubleValue";

            public const string EnumValue = "enumValue";

            public const string StringValue = "stringValue";

            public const string LayerMaskValue = "layerMaskValue";

            public const string Vector2Value = "vector2Value";

            public const string Vector3Value = "vector3Value";

            public const string Vector4Value = "vector4Value";

            public const string ColorValue = "colorValue";

            public const string BoundsValue = "boundsValue";

            public const string RectValue = "rectValue";

            public const string QuaternionValue = "quaternionValue";

            public const string Matrix4x4Value = "matrix4x4Value";

            public const string AnimationCurveValue = "animationCurveValue";

            public const string UnityObjectReference = "unityObjectReference";

            public const string SerializedValue = "serializedValue";

            public static string Get(ObjectType type)
            {
                switch (type)
                {
                    case ObjectType.Boolean:
                        return BoolValue;

                    case ObjectType.Byte:
                        return ByteValue;

                    case ObjectType.SByte:
                        return SByteValue;

                    case ObjectType.Char:
                        return CharValue;

                    case ObjectType.Int16:
                        return ShortValue;

                    case ObjectType.UInt16:
                        return UShortValue;

                    case ObjectType.Int32:
                        return IntValue;

                    case ObjectType.UInt32:
                        return UIntValue;

                    case ObjectType.Int64:
                        return LongValue;

                    case ObjectType.UInt64:
                        return SerializedValue;

                    case ObjectType.Single:
                        return FloatValue;

                    case ObjectType.Double:
                        return DoubleValue;

                    case ObjectType.Enum:
                        return EnumValue;

                    case ObjectType.String:
                        return StringValue;

                    case ObjectType.LayerMask:
                        return LayerMaskValue;

                    case ObjectType.Vector2:
                        return Vector2Value;

                    case ObjectType.Vector3:
                        return Vector3Value;

                    case ObjectType.Vector4:
                        return Vector4Value;

                    case ObjectType.Color:
                        return ColorValue;

                    case ObjectType.Bounds:
                        return BoundsValue;

                    case ObjectType.Rect:
                        return RectValue;

                    case ObjectType.Quaternion:
                        return QuaternionValue;

                    case ObjectType.Matrix4x4:
                        return SerializedValue;

                    case ObjectType.AnimationCurve:
                        return AnimationCurveValue;

                    case ObjectType.UnityObject:
                        return UnityObjectReference;

                    case ObjectType.SerializableType:
                        return SerializedValue;

                    case ObjectType.Array:
                        return SerializedValue;

                    case ObjectType.List:
                        return SerializedValue;

                    default:
                        return string.Empty;
                }
            }
        }
    }
}
