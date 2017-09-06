using UnityEditor;

namespace ExtendedLibrary.Events
{
    public static class Utilities
    {
        public static SerializedProperty GetPropertyOfType(this SerializedProperty property, ObjectType type)
        {
            var fieldName = ExtendedEventBaseEditor.ValueFields.Get(type);

            if (type == ObjectType.Void || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }
            else
            {
                return property.FindPropertyRelative(fieldName);
            }
        }

        public static sbyte CopyTo(this SerializedProperty from, SerializedProperty to)
        {
            if (from == null || to == null)
                return 0;

            switch (from.propertyType)
            {
                case SerializedPropertyType.Integer:
                    if (from.intValue != to.intValue)
                    {
                        to.intValue = from.intValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Boolean:
                    if (from.boolValue != to.boolValue)
                    {
                        to.boolValue = from.boolValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Float:
                    if (from.floatValue != to.floatValue)
                    {
                        to.floatValue = from.floatValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.String:
                    if (!from.stringValue.Equals(to.stringValue))
                    {
                        to.stringValue = from.stringValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Color:
                    if (from.colorValue != to.colorValue)
                    {
                        to.colorValue = from.colorValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Character:
                    if (from.intValue != to.intValue)
                    {
                        to.intValue = from.intValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.ObjectReference:
                    if (from.objectReferenceValue != to.objectReferenceValue)
                    {
                        to.objectReferenceValue = from.objectReferenceValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Vector2:
                    if (from.vector2Value != to.vector2Value)
                    {
                        to.vector2Value = from.vector2Value;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Vector3:
                    if (from.vector3Value != to.vector3Value)
                    {
                        to.vector3Value = from.vector3Value;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Vector4:
                    if (from.vector4Value != to.vector4Value)
                    {
                        to.vector4Value = from.vector4Value;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Rect:
                    if (from.rectValue != to.rectValue)
                    {
                        to.rectValue = from.rectValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.AnimationCurve:
                    to.animationCurveValue = from.animationCurveValue;
                    return 1;

                case SerializedPropertyType.Bounds:
                    if (from.boundsValue != to.boundsValue)
                    {
                        to.boundsValue = from.boundsValue;
                        return 1;
                    }
                    return 0;

                case SerializedPropertyType.Quaternion:
                    if (from.quaternionValue != to.quaternionValue)
                    {
                        to.quaternionValue = from.quaternionValue;
                        return 1;
                    }
                    return 0;

                default:
                    return -1;
            }
        }
    }
}
