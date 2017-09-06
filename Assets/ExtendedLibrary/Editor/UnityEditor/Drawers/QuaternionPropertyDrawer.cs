using UnityEngine;

namespace UnityEditor
{
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(Quaternion))]
    public class QuaternionPropertyDrawer : BasePropertyDrawer<Quaternion>
    {
        protected override float PrimaryHeight
        {
            get
            {
                return ITEM_HEIGHT;
            }
        }

        protected override float SecondaryHeight
        {
            get
            {
                return ITEM_HEIGHT + EXTENDED_HEIGHT;
            }
        }

        protected override void DrawProperty(Rect contentPosition, ref Quaternion value)
        {
            var itemWidth = contentPosition.width / 4f - RIGHTMOST_MARGIN;
            contentPosition.width = itemWidth;
            EditorGUIUtility.labelWidth = 15f;

            SetAndRecord(contentPosition, "X", ref value.x, EditorGUI.FloatField);

            contentPosition.x += contentPosition.width + ITEM_OFFSET;
            SetAndRecord(contentPosition, "Y", ref value.y, EditorGUI.FloatField);

            contentPosition.x += contentPosition.width + ITEM_OFFSET;
            SetAndRecord(contentPosition, "Z", ref value.z, EditorGUI.FloatField);

            contentPosition.x += contentPosition.width + ITEM_OFFSET;
            SetAndRecord(contentPosition, "W", ref value.w, EditorGUI.FloatField);
        }
    }
}
