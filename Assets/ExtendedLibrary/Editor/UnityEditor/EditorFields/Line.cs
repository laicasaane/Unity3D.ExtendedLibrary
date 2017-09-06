using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static void Line(float height = 1f)
        {
            Color defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.black;
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));
            GUI.backgroundColor = defaultColor;
        }
    }
}
