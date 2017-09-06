using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static byte ByteField(Rect position, byte value)
        {
            return ByteField(position, GUIContent.none, value);
        }

        public static byte ByteField(Rect position, GUIContent label, byte value)
        {
            int intValue = value;
            intValue = EditorGUI.IntField(position, label, intValue);

            if (intValue < byte.MinValue)
                return byte.MinValue;

            if (intValue > byte.MaxValue)
                return byte.MaxValue;

            unchecked
            {
                return (byte)intValue;
            }
        }

        public static byte ByteField(string label, byte value)
        {
            return ByteField(new GUIContent(label), value);
        }

        public static byte ByteField(GUIContent label, byte value)
        {
            int intValue = value;
            intValue = EditorGUILayout.IntField(label, intValue);

            if (intValue < byte.MinValue)
                return byte.MinValue;

            if (intValue > byte.MaxValue)
                return byte.MaxValue;

            unchecked
            {
                return (byte)intValue;
            }
        }
    }
}
