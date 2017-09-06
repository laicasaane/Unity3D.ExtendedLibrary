using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorFields
    {
        public static ulong ULongField(Rect position, ulong value)
        {
            return ULongField(position, GUIContent.none, value);
        }

        public static ulong ULongField(Rect position, GUIContent label, ulong value)
        {
            var ulongStr = value.ToString();
            decimal ulongValue;

            try
            {
                if (!decimal.TryParse(EditorGUI.DelayedTextField(position, label, ulongStr), out ulongValue))
                {
                    ulongValue = value;
                }
            }
            catch
            {
                ulongValue = value;
            }

            if (ulongValue < ulong.MinValue)
                return ulong.MinValue;

            if (ulongValue > ulong.MaxValue)
                return ulong.MaxValue;

            unchecked
            {
                return (ulong)ulongValue;
            }
        }

        public static ulong ULongField(string label, ulong value)
        {
            return ULongField(new GUIContent(label), value);
        }

        public static ulong ULongField(GUIContent label, ulong value)
        {
            var ulongStr = value.ToString();
            decimal ulongValue;

            try
            {
                if (!decimal.TryParse(EditorGUILayout.DelayedTextField(label, ulongStr), out ulongValue))
                {
                    ulongValue = value;
                }
            }
            catch
            {
                ulongValue = value;
            }

            if (ulongValue < ulong.MinValue)
                return ulong.MinValue;

            if (ulongValue > ulong.MaxValue)
                return ulong.MaxValue;

            unchecked
            {
                return (ulong)ulongValue;
            }
        }
    }
}
