using UnityEngine;

namespace UnityEditor
{
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(Matrix4x4))]
    public class Matrix4x4PropertyDrawer : BasePropertyDrawer<Matrix4x4>
    {
        protected override float PrimaryHeight
        {
            get
            {
                return ITEM_HEIGHT + EXTENDED_HEIGHT * 3 - 6f;
            }
        }

        protected override float SecondaryHeight
        {
            get
            {
                return ITEM_HEIGHT + EXTENDED_HEIGHT * 4 - 6f;
            }
        }

        protected override void DrawProperty(Rect contentPosition, ref Matrix4x4 value)
        {
            var firstX = contentPosition.x;
            var itemWidth = contentPosition.width / 4f - RIGHTMOST_MARGIN;
            contentPosition.width = itemWidth;
            EditorGUIUtility.labelWidth = 28f;

            // Row 0
            {
                SetAndRecord(contentPosition, "M00", ref value.m00, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M01", ref value.m01, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M02", ref value.m02, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M03", ref value.m03, EditorGUI.FloatField);
            }

            contentPosition.x = firstX;
            contentPosition.y += ITEM_HEIGHT;
            contentPosition = EditorGUI.IndentedRect(contentPosition);

            // Row 1
            {
                SetAndRecord(contentPosition, "M10", ref value.m10, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M11", ref value.m11, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M12", ref value.m12, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M13", ref value.m13, EditorGUI.FloatField);
            }

            contentPosition.x = firstX;
            contentPosition.y += ITEM_HEIGHT;
            contentPosition = EditorGUI.IndentedRect(contentPosition);

            // Row 2
            {
                SetAndRecord(contentPosition, "M20", ref value.m20, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M21", ref value.m21, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M22", ref value.m22, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M23", ref value.m23, EditorGUI.FloatField);
            }

            contentPosition.x = firstX;
            contentPosition.y += ITEM_HEIGHT;
            contentPosition = EditorGUI.IndentedRect(contentPosition);

            // Row 3
            {
                SetAndRecord(contentPosition, "M30", ref value.m30, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M31", ref value.m31, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M32", ref value.m32, EditorGUI.FloatField);

                contentPosition.x += contentPosition.width + ITEM_OFFSET;
                SetAndRecord(contentPosition, "M33", ref value.m33, EditorGUI.FloatField);
            }
        }
    }
}
