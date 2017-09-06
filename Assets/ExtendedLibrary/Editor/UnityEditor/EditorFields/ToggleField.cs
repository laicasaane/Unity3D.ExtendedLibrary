using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static readonly Color FalseColor = Color.white;
        public static readonly Color TrueColor = Color.green;

        public static bool ToggleField(bool value, GUIContent label, GUIStyle style = null, params GUILayoutOption[] options)
        {
            Color color = value ? TrueColor : FalseColor;
            Color defaultColor = GUI.backgroundColor;

            GUI.backgroundColor = color;
            if (GUILayout.Button(label, style == null ? GUI.skin.button : style, options))
            {
                value = !value;
            }
            GUI.backgroundColor = defaultColor;

            return value;
        }

        public static bool ToggleField(bool value, string text, GUIStyle style = null, params GUILayoutOption[] options)
        {
            Color color = value ? TrueColor : FalseColor;
            Color defaultColor = GUI.backgroundColor;

            GUI.backgroundColor = color;

            if (GUILayout.Button(text, style == null ? GUI.skin.button : style, options))
            {
                value = !value;
            }

            GUI.backgroundColor = defaultColor;

            return value;
        }
    }
}
