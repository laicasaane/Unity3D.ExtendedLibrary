using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace UnityEditor
{
    public abstract class BasePropertyDrawer<T> : PropertyDrawer
    {
        protected const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        protected static readonly System.Type IListType = typeof(IList);

        protected const float ITEM_HEIGHT = 16f;
        protected const float EXTENDED_HEIGHT = 18f;
        protected const float ITEM_OFFSET = 6f;
        protected const float RIGHTMOST_MARGIN = 4.5f;

        protected Object target;

        protected abstract float PrimaryHeight { get; }

        protected abstract float SecondaryHeight { get; }

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.target = property.serializedObject.targetObject;
            var targetMonoBehaviour = true;

            var targets = new List<object>();
            var infos = new List<IndexOrInfo>();
            var values = new List<object>();

            var value = default(T);

            try
            {
                value = (T) this.fieldInfo.GetValue(target);
            }
            catch
            {
                targetMonoBehaviour = false;
                if (!TryParsePropertyPath(property.propertyPath, this.target, ref targets, ref infos, ref values))
                    return;

                try
                {
                    value = (T) values[values.Count - 1];
                }
                catch
                {
                    return;
                }
            }

            label = EditorGUI.BeginProperty(position, label, property);
            var contentPosition = EditorGUI.PrefixLabel(position, label, GUI.skin.label);

            contentPosition = AdjustContentPosition(position, contentPosition);
            EditorGUI.indentLevel = 0;

            DrawProperty(contentPosition, ref value);

            if (targetMonoBehaviour)
            {
                this.fieldInfo.SetValue(this.target, value);
            }
            else
            {
                SetValue(ref value, targets, infos, values);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Screen.width < 333 ? this.SecondaryHeight : this.PrimaryHeight;
        }

        protected virtual Rect AdjustContentPosition(Rect position, Rect contentPosition)
        {
            if (position.height > this.PrimaryHeight)
            {
                position.height = ITEM_HEIGHT;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.y += EXTENDED_HEIGHT;
            }
            else
            {
                contentPosition.height = ITEM_HEIGHT;
            }

            return contentPosition;
        }

        protected abstract void DrawProperty(Rect contentPosition, ref T value);

        protected void SetAndRecord<U>(ref U source, ref U value) where U : System.IEquatable<U>
        {
            if (!source.Equals(value))
            {
                Undo.RecordObject(this.target, "SetValue");
                source = value;
            }
        }

        protected void SetAndRecord<U>(Rect rect, string label, ref U source, System.Func<Rect, string, U, U> display)
        {
            if (this.target == null)
                return;

            EditorGUI.BeginChangeCheck();
            var newValue = display(rect, label, source);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this.target, "SetValue");
                source = newValue;
            }
        }

        private bool TryParsePropertyPath(string propertyPath, Object firstTarget, ref List<object> targets, ref List<IndexOrInfo> infos, ref List<object> values)
        {
            targets.Add(firstTarget);

            var path = propertyPath.Replace("Array.data", string.Empty);
            var levels = path.Split(new[] { '.' });
            var index = -1;
            var lastLevelIndex = levels.Length - 1;

            for (var i = 0; i < levels.Length; ++i)
            {
                var level = levels[i];
                var target = targets[targets.Count - 1];

                try
                {
                    var targetType = target.GetType();

                    if (level[0] == '[' && level[level.Length - 1] == ']')
                    {
                        if (!targetType.GetInterfaces().Contains(IListType))
                            return false;

                        if (targetType.IsArray && targetType.GetArrayRank() > 1)
                            return false;

                        var indexStr = level.Substring(1, level.Length - 2);

                        if (!int.TryParse(indexStr, out index))
                        {
                            return false;
                        }

                        var list = (IList) target;

                        if (list == null)
                            return false;

                        if (index >= list.Count)
                            return false;

                        infos.Add(IndexOrInfo.New(index));
                        values.Add(list[index]);

                        if (i < lastLevelIndex)
                            targets.Add(values[values.Count - 1]);
                    }
                    else
                    {
                        var info = targetType.GetField(level, FLAGS);

                        if (info == null)
                            return false;

                        infos.Add(IndexOrInfo.New(info));
                        values.Add(info.GetValue(target));

                        if (i < lastLevelIndex)
                            targets.Add(values[values.Count - 1]);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogErrorFormat("{0}\n{1}", ex.Message, ex.StackTrace);
                    return false;
                }
            }

            return true;
        }

        private void SetValue(ref T newValue, List<object> targets, List<IndexOrInfo> infos, List<object> values)
        {
            var last = targets.Count - 1;
            values[last] = newValue;

            object val = values[last];

            for (var i = last; i >= 0; --i)
            {
                var target = targets[i];
                var info = infos[i];

                if (info.IsIndex)
                {
                    var list = (IList) target;
                    list[info.index] = val;
                }
                else if (info.info != null)
                {
                    info.info.SetValue(target, val);
                }

                val = target;
            }
        }

        protected struct IndexOrInfo
        {
            public int index { get; private set; }

            public FieldInfo info { get; private set; }

            public bool IsIndex
            {
                get { return this.index >= 0; }
            }

            public bool IsEmpty
            {
                get { return this.index < 0 && this.info == null; }
            }

            private IndexOrInfo(int index, FieldInfo info)
            {
                this.index = index;
                this.info = info;
            }

            public static IndexOrInfo New(int index)
            {
                return new IndexOrInfo(index, null);
            }

            public static IndexOrInfo New(FieldInfo info)
            {
                return new IndexOrInfo(-1, info);
            }
        }
    }
}
