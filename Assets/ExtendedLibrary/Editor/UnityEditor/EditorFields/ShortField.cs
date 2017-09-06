using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static short ShortField(Rect position, short value)
        {
            return ShortField(position, GUIContent.none, value);
        }

        public static short ShortField(Rect position, GUIContent label, short value)
        {
            int intValue = value;
            intValue = EditorGUI.IntField(position, label, intValue);

            if (intValue < short.MinValue)
                return short.MinValue;

            if (intValue > short.MaxValue)
                return short.MaxValue;

            unchecked
            {
                return (short)intValue;
            }
        }

        public static short ShortField(string label, short value)
        {
            return ShortField(new GUIContent(label), value);
        }

        public static short ShortField(GUIContent label, short value)
        {
            int intValue = value;
            intValue = EditorGUILayout.IntField(label, intValue);

            if (intValue < short.MinValue)
                return short.MinValue;

            if (intValue > short.MaxValue)
                return short.MaxValue;

            unchecked
            {
                return (short)intValue;
            }
        }
    }
}
