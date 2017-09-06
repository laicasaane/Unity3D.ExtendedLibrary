using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static Quaternion QuaternionField(Rect position, Quaternion value)
        {
            return QuaternionField(position, GUIContent.none, value);
        }

        public static Quaternion QuaternionField(Rect position, GUIContent label, Quaternion value)
        {
            var v4 = new Vector4(value.x, value.y, value.z, value.w);
            v4 = EditorGUI.Vector4Field(position, label, v4);

            return new Quaternion(v4.x, v4.y, v4.z, v4.w);
        }

        public static Quaternion QuaternionField(string label, Quaternion value)
        {
            return QuaternionField(new GUIContent(label), value);
        }

        public static Quaternion QuaternionField(GUIContent label, Quaternion value)
        {
            var v4 = new Vector4(value.x, value.y, value.z, value.w);
            v4 = EditorGUILayout.Vector4Field(label, v4);

            return new Quaternion(v4.x, v4.y, v4.z, v4.w);
        }
    }
}
