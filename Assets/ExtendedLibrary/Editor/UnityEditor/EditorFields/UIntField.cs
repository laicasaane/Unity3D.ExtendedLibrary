using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static uint UIntField(Rect position, uint value)
        {
            return UIntField(position, GUIContent.none, value);
        }

        public static uint UIntField(Rect position, GUIContent label, uint value)
        {
            long longValue = value;
            longValue = EditorGUI.LongField(position, label, longValue);

            if (longValue < uint.MinValue)
                return uint.MinValue;

            if (longValue > uint.MaxValue)
                return uint.MaxValue;

            unchecked
            {
                return (uint)longValue;
            }
        }

        public static uint UIntField(string label, uint value)
        {
            return UIntField(new GUIContent(label), value);
        }

        public static uint UIntField(GUIContent label, uint value)
        {
            long longValue = value;
            longValue = EditorGUILayout.LongField(label, longValue);

            if (longValue < uint.MinValue)
                return uint.MinValue;

            if (longValue > uint.MaxValue)
                return uint.MaxValue;

            unchecked
            {
                return (uint)longValue;
            }
        }
    }
}
