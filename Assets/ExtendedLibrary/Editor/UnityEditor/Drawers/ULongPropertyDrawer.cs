using UnityEngine;

namespace UnityEditor
{
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(ulong))]
    public class ULongPropertyDrawer : BasePropertyDrawer<ulong>
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
                return ITEM_HEIGHT;
            }
        }

        protected override void DrawProperty(Rect contentPosition, ref ulong value)
        {
            var ulongStr = value.ToString();
            decimal ulongValue;

            EditorGUI.BeginChangeCheck();

            try
            {
                if (!decimal.TryParse(EditorGUI.DelayedTextField(contentPosition, GUIContent.none, ulongStr), out ulongValue))
                {
                    ulongValue = value;
                }
            }
            catch
            {
                ulongValue = value;
            }

            ulong newValue;

            if (ulongValue < ulong.MinValue)
                newValue = ulong.MinValue;
            else if (ulongValue > ulong.MaxValue)
                newValue = ulong.MaxValue;
            else
                unchecked
                {
                    newValue = (ulong) ulongValue;
                }

            if (EditorGUI.EndChangeCheck())
            {
                SetAndRecord(ref value, ref newValue);
            }
        }
    }
}
