using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static sbyte SByteField(Rect position, sbyte value)
        {
            return SByteField(position, GUIContent.none, value);
        }

        public static sbyte SByteField(Rect position, GUIContent label, sbyte value)
        {
            int intValue = value;
            intValue = EditorGUI.IntField(position, label, intValue);

            if (intValue < sbyte.MinValue)
                return sbyte.MinValue;

            if (intValue > sbyte.MaxValue)
                return sbyte.MaxValue;

            unchecked
            {
                return (sbyte)intValue;
            }
        }

        public static sbyte SByteField(string label, sbyte value)
        {
            return SByteField(new GUIContent(label), value);
        }

        public static sbyte SByteField(GUIContent label, sbyte value)
        {
            int intValue = value;
            intValue = EditorGUILayout.IntField(label, intValue);

            if (intValue < sbyte.MinValue)
                return sbyte.MinValue;

            if (intValue > sbyte.MaxValue)
                return sbyte.MaxValue;

            unchecked
            {
                return (sbyte)intValue;
            }
        }
    }
}
