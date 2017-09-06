using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static char CharField(Rect position, char value)
        {
            return CharField(position, GUIContent.none, value);
        }

        public static char CharField(Rect position, GUIContent label, char value)
        {
            string stringValue = value.ToString();
            stringValue = EditorGUI.TextField(position, label, stringValue);

            if (stringValue.Length < 1)
                return char.MinValue;

            return stringValue[0];
        }

        public static char CharField(string label, char value)
        {
            return CharField(new GUIContent(label), value);
        }

        public static char CharField(GUIContent label, char value)
        {
            string stringValue = value.ToString();
            stringValue = EditorGUILayout.TextField(label, stringValue);

            if (stringValue.Length < 1)
                return char.MinValue;

            return stringValue[0];
        }
    }
}
