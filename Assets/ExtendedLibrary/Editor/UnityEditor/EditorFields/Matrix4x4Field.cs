using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static Matrix4x4 Matrix4x4Field(Rect position, Matrix4x4 value)
        {
            return Matrix4x4Field(position, GUIContent.none, value);
        }

        public static Matrix4x4 Matrix4x4Field(Rect position, GUIContent label, Matrix4x4 value)
        {
            position = EditorGUI.PrefixLabel(position, label);

            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 28f;

            var contentPosition = position;
            var firstX = contentPosition.x;
            var itemWidth = (Screen.width - contentPosition.x) / 4f - 5.7f;
            var itemOffset = 6f;
            contentPosition.width = itemWidth;

            // Row 0
            {
                value.m00 = EditorGUI.FloatField(contentPosition, "M00", value.m00);

                contentPosition.x += itemWidth + itemOffset;
                value.m01 = EditorGUI.FloatField(contentPosition, "M01", value.m01);

                contentPosition.x += itemWidth + itemOffset;
                value.m02 = EditorGUI.FloatField(contentPosition, "M02", value.m02);

                contentPosition.x += itemWidth + itemOffset;
                value.m03 = EditorGUI.FloatField(contentPosition, "M03", value.m03);
            }

            contentPosition = EditorGUILayout.GetControlRect();
            contentPosition.width = itemWidth;

            // Row 1
            {
                value.m10 = EditorGUI.FloatField(contentPosition, "M10", value.m10);

                contentPosition.x += itemWidth + itemOffset;
                value.m11 = EditorGUI.FloatField(contentPosition, "M11", value.m11);

                contentPosition.x += itemWidth + itemOffset;
                value.m12 = EditorGUI.FloatField(contentPosition, "M12", value.m12);

                contentPosition.x += itemWidth + itemOffset;
                value.m13 = EditorGUI.FloatField(contentPosition, "M13", value.m13);
            }

            contentPosition = EditorGUILayout.GetControlRect();
            contentPosition.width = itemWidth;

            // Row 2
            {
                value.m20 = EditorGUI.FloatField(contentPosition, "M20", value.m20);

                contentPosition.x += itemWidth + itemOffset;
                value.m21 = EditorGUI.FloatField(contentPosition, "M21", value.m21);

                contentPosition.x += itemWidth + itemOffset;
                value.m22 = EditorGUI.FloatField(contentPosition, "M22", value.m22);

                contentPosition.x += itemWidth + itemOffset;
                value.m23 = EditorGUI.FloatField(contentPosition, "M23", value.m23);
            }

            contentPosition = EditorGUILayout.GetControlRect();
            contentPosition.width = itemWidth;

            // Row 3
            {
                value.m30 = EditorGUI.FloatField(contentPosition, "M30", value.m30);

                contentPosition.x += itemWidth + itemOffset;
                value.m31 = EditorGUI.FloatField(contentPosition, "M31", value.m31);

                contentPosition.x += itemWidth + itemOffset;
                value.m32 = EditorGUI.FloatField(contentPosition, "M32", value.m32);

                contentPosition.x += itemWidth + itemOffset;
                value.m33 = EditorGUI.FloatField(contentPosition, "M33", value.m33);
            }

            EditorGUIUtility.labelWidth = originalLabelWidth;

            return value;
        }

        public static Matrix4x4 Matrix4x4Field(string label, Matrix4x4 value)
        {
            return Matrix4x4Field(new GUIContent(label), value);
        }

        public static Matrix4x4 Matrix4x4Field(GUIContent label, Matrix4x4 value)
        {
            var originalLabelWidth = EditorGUIUtility.labelWidth;

            EditorGUILayout.BeginVertical();

            EditorGUIUtility.labelWidth = float.MaxValue;
            EditorGUILayout.PrefixLabel(label);

            EditorGUIUtility.labelWidth = 28f;

            // Row 0
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            value.m00 = EditorGUILayout.FloatField("M00", value.m00);
            value.m01 = EditorGUILayout.FloatField("M01", value.m01);
            value.m02 = EditorGUILayout.FloatField("M02", value.m02);
            value.m03 = EditorGUILayout.FloatField("M03", value.m03);
            EditorGUILayout.EndHorizontal();

            // Row 1
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            value.m10 = EditorGUILayout.FloatField("M10", value.m10);
            value.m11 = EditorGUILayout.FloatField("M11", value.m11);
            value.m12 = EditorGUILayout.FloatField("M12", value.m12);
            value.m13 = EditorGUILayout.FloatField("M13", value.m13);
            EditorGUILayout.EndHorizontal();

            // Row 2
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            value.m20 = EditorGUILayout.FloatField("M20", value.m20);
            value.m21 = EditorGUILayout.FloatField("M21", value.m21);
            value.m22 = EditorGUILayout.FloatField("M22", value.m22);
            value.m23 = EditorGUILayout.FloatField("M23", value.m23);
            EditorGUILayout.EndHorizontal();

            // Row 3
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            value.m30 = EditorGUILayout.FloatField("M30", value.m30);
            value.m31 = EditorGUILayout.FloatField("M31", value.m31);
            value.m32 = EditorGUILayout.FloatField("M32", value.m32);
            value.m33 = EditorGUILayout.FloatField("M33", value.m33);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUIUtility.labelWidth = originalLabelWidth;

            return value;
        }
    }
}
