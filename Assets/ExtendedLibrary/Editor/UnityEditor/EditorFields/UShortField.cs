using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static ushort UShortField(Rect position, ushort value)
        {
            return UShortField(position, GUIContent.none, value);
        }

        public static ushort UShortField(Rect position, GUIContent label, ushort value)
        {
            int intValue = value;
            intValue = EditorGUI.IntField(position, label, intValue);

            if (intValue < ushort.MinValue)
                return ushort.MinValue;

            if (intValue > ushort.MaxValue)
                return ushort.MaxValue;

            unchecked
            {
                return (ushort)intValue;
            }
        }

        public static ushort UShortField(string label, ushort value)
        {
            return UShortField(new GUIContent(label), value);
        }

        public static ushort UShortField(GUIContent label, ushort value)
        {
            int intValue = value;
            intValue = EditorGUILayout.IntField(label, intValue);

            if (intValue < ushort.MinValue)
                return ushort.MinValue;

            if (intValue > ushort.MaxValue)
                return ushort.MaxValue;

            unchecked
            {
                return (ushort)intValue;
            }
        }
    }
}
