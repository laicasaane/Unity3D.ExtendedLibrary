using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UObject = UnityEngine.Object;
using MemberFilter = ExtendedLibrary.Events.ExtendedEvent.Listener.MemberFilter;
using VisibilityFilter = ExtendedLibrary.Events.ExtendedEvent.Listener.VisibilityFilter;
using LevelFilter = ExtendedLibrary.Events.ExtendedEvent.Listener.LevelFilter;

namespace ExtendedLibrary.Events
{
    [CustomPropertyDrawer(typeof(ExtendedEvent))]
    public partial class ExtendedEventBaseEditor : PropertyDrawer
    {
        private const string WIZARD = "...";
        private const string HEADER_FORMAT = "{0} () : {1}";
        private const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private string header = string.Empty;

        private Dictionary<string, State> states = new Dictionary<string, State>();
        private List<MemberList> members = new List<MemberList>();
        private SerializedProperty rootProperty;
        private SerializedProperty listenersProperty;
        private ReorderableList reorderableList;
        private int lastSelectedIndex;

        private string returnTypeName;
        //private bool advanced = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            RestoreState(property);

            if (this.reorderableList != null)
                return this.reorderableList.GetHeight();

            return 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            this.rootProperty = property;

            this.header = ObjectNames.NicifyVariableName(property.name);
            var state = RestoreState(property);

            OnGUI(position);

            state.lastSelectedIndex = this.lastSelectedIndex;
            EditorGUI.EndProperty();
        }

        private State RestoreState(SerializedProperty property)
        {
            var state = GetState(property);
            this.listenersProperty = state.reorderableList.serializedProperty;
            this.reorderableList = state.reorderableList;
            this.lastSelectedIndex = state.lastSelectedIndex;
            this.reorderableList.index = state.lastSelectedIndex;

            return state;
        }

        private State GetState(SerializedProperty property)
        {
            State state = null;
            this.states.TryGetValue(property.propertyPath, out state);

            if (state == null)
            {
                var listenersProperty = property.FindPropertyRelative(ExtendedEventFields.Listeners);
                this.returnTypeName = property.FindPropertyRelative(ExtendedEventFields.ReturnTypeName).stringValue;

                state = new State()
                {
                    reorderableList = new ReorderableList(property.serializedObject, listenersProperty, true, true, true, true)
                };

                state.reorderableList.drawHeaderCallback = DrawHeader;
                state.reorderableList.drawElementCallback = DrawListener;
                state.reorderableList.onSelectCallback = SelectListener;
                state.reorderableList.onReorderCallback = EndDragChild;
                state.reorderableList.onAddCallback = AddListener;
                state.reorderableList.onRemoveCallback = RemoveListener;
                //state.reorderableList.elementHeight = this.advanced ? 77f : 59f;
                state.reorderableList.elementHeight = 77f;

                this.states.Add(property.propertyPath, state);
            }

            return state;
        }

        private void DrawHeader(Rect rect)
        {
            rect.height = 16f;
            var returnTypeName = string.IsNullOrEmpty(this.returnTypeName) ? "void" : this.returnTypeName;

            EditorGUI.LabelField(rect, string.Format(HEADER_FORMAT, this.header, returnTypeName));

            //var advancedRect = new Rect(rect)
            //{
            //    xMin = rect.xMax - 75f
            //};

            //var toggle = EditorGUI.ToggleLeft(advancedRect, "Advanced", this.advanced);

            //if (this.advanced != toggle)
            //{
            //    this.advanced = toggle;
            //    var state = GetState(this.rootProperty);
            //    state.reorderableList.elementHeight = toggle ? 77f : 59f;
            //}

            var state = GetState(this.rootProperty);
            state.reorderableList.elementHeight = 77f;
        }

        private void DrawListener(Rect rect, int index, bool isactive, bool isfocused)
        {
            var element = this.listenersProperty.GetArrayElementAtIndex(index);
            var serializedObject = element.serializedObject;
            ++rect.y;

            var targetProperty = element.FindPropertyRelative(ListenerFields.Target);
            var targetNameProperty = element.FindPropertyRelative(ListenerFields.TargetName);
            var indexProperty = element.FindPropertyRelative(ListenerFields.Index);
            var selectedLabelProperty = element.FindPropertyRelative(ListenerFields.SelectedLabel);
            var memberFilterProperty = element.FindPropertyRelative(ListenerFields.MemberFilter);
            var visibilityFilterProperty = element.FindPropertyRelative(ListenerFields.VisibilityFilter);
            var levelFilterProperty = element.FindPropertyRelative(ListenerFields.LevelFilter);
            var memberInfoProperty = element.FindPropertyRelative(ListenerFields.MemberInfo);
            var valuesProperty = element.FindPropertyRelative(ListenerFields.Values);

            var targetRect = new Rect(rect)
            {
                height = 16f
            };

            targetRect.y += 2f;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(targetRect, targetProperty, GUIContent.none);

            if (!ReferenceEquals(targetProperty.objectReferenceValue, null))
                targetNameProperty.stringValue = targetProperty.objectReferenceValue.name;

            serializedObject.ApplyModifiedProperties();

            MemberFilter memberFilter;
            VisibilityFilter visibilityFilter;
            LevelFilter levelFilter;

            //if (this.advanced)
            //{
            var memberFilterRect = new Rect(rect);
            memberFilterRect.width *= 0.3f;
            memberFilterRect.height = 16f;
            memberFilterRect.y += 20f;

            memberFilter = (MemberFilter) EditorGUI.EnumPopup(memberFilterRect, GUIContent.none, (MemberFilter) memberFilterProperty.intValue);
            memberFilterProperty.intValue = (int) memberFilter;
            serializedObject.ApplyModifiedProperties();

            var visibilityFilterRect = new Rect(rect);
            visibilityFilterRect.width *= 0.65f;
            visibilityFilterRect.xMin = memberFilterRect.xMax + 5f;
            visibilityFilterRect.height = 16f;
            visibilityFilterRect.y += 20f;

            visibilityFilter = (VisibilityFilter) EditorGUI.EnumPopup(visibilityFilterRect, GUIContent.none, (VisibilityFilter) visibilityFilterProperty.intValue);
            visibilityFilterProperty.intValue = (int) visibilityFilter;
            serializedObject.ApplyModifiedProperties();

            var levelFilterRect = new Rect(rect);
            levelFilterRect.xMin = visibilityFilterRect.xMax + 5f;
            levelFilterRect.height = 16f;
            levelFilterRect.y += 20f;

            levelFilter = (LevelFilter) EditorGUI.EnumPopup(levelFilterRect, GUIContent.none, (LevelFilter) levelFilterProperty.intValue);
            levelFilterProperty.intValue = (int) levelFilter;
            serializedObject.ApplyModifiedProperties();
            //}
            //else
            //{
            //    memberFilter = (MemberFilter) memberFilterProperty.intValue;
            //    visibilityFilter = (VisibilityFilter) visibilityFilterProperty.intValue;
            //    levelFilter = (LevelFilter) levelFilterProperty.intValue;
            //}

            var members = InsertOrUpdateMembers(targetProperty, index, memberFilter, visibilityFilter, levelFilter, this.returnTypeName);

            if (EditorGUI.EndChangeCheck())
            {
                indexProperty.intValue = -1;
                selectedLabelProperty.stringValue = string.Empty;
                serializedObject.ApplyModifiedProperties();
            }

            var memberRect = new Rect(rect)
            {
                height = 16f,
                xMin = targetRect.xMin
            };

            //if (this.advanced)
            memberRect.y += EditorGUIUtility.singleLineHeight * 2 + 6f;
            //else
            //    memberRect.y += EditorGUIUtility.singleLineHeight + 4f;

            EditorGUI.BeginDisabledGroup(targetProperty.objectReferenceValue == null);
            EditorGUI.BeginChangeCheck();

            if (targetProperty.objectReferenceValue == null)
            {
                indexProperty.intValue = EditorGUI.Popup(memberRect, indexProperty.intValue, new string[0]);
                selectedLabelProperty.stringValue = string.Empty;
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var indexValue = Array.IndexOf(members.labels, selectedLabelProperty.stringValue);

                if (indexValue >= 0)
                {
                    if (indexValue >= members.labels.Length ||
                        (indexValue < members.labels.Length &&
                        !selectedLabelProperty.stringValue.Equals(members.labels[indexValue])))
                    {
                        indexProperty.intValue = -1;
                        selectedLabelProperty.stringValue = string.Empty;
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                indexValue = EditorGUI.Popup(memberRect, indexProperty.intValue, members.labels);

                if (indexValue != indexProperty.intValue)
                {
                    indexProperty.intValue = indexValue;

                    if (indexValue >= 0)
                    {
                        selectedLabelProperty.stringValue = members.labels[indexValue];
                    }

                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                UpdateMemberInfo(memberInfoProperty, indexProperty.intValue, members, serializedObject);

                valuesProperty.arraySize = 0;
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndDisabledGroup();

            var valueRect = new Rect(rect)
            {
                height = 16f
            };

            //if (this.advanced)
            valueRect.y += EditorGUIUtility.singleLineHeight * 3 + 8f;
            //else
            //    valueRect.y += EditorGUIUtility.singleLineHeight * 2 + 6f;

            EditorGUI.BeginDisabledGroup(targetProperty.objectReferenceValue == null || indexProperty.intValue < 0);

            if (targetProperty.objectReferenceValue != null && indexProperty.intValue >= 0)
            {
                var infos = members.GetInfosAt(indexProperty.intValue);

                if (infos != null)
                {
                    switch (infos.memberData.type)
                    {
                        case MemberType.Field:
                        case MemberType.Property:
                            {
                                if (valuesProperty.arraySize == 0 || valuesProperty.arraySize > 1)
                                {
                                    valuesProperty.arraySize = 1;
                                    serializedObject.ApplyModifiedProperties();
                                }

                                var member = infos.memberData;
                                DrawValue(valueRect, infos, member.name, member.assemblyQualifiedName, member.returnType, member.returnTypeName, valuesProperty.GetArrayElementAtIndex(0), serializedObject);
                            }
                            break;

                        case MemberType.Method:
                            {
                                var parameters = infos.memberData.parameters;

                                if (parameters.Length == 1)
                                {
                                    if (valuesProperty.arraySize == 0 || valuesProperty.arraySize > 1)
                                    {
                                        valuesProperty.arraySize = 1;
                                        serializedObject.ApplyModifiedProperties();
                                    }

                                    var param = parameters[0];
                                    DrawValue(valueRect, infos, param.name, param.assemblyQualifiedName, param.returnType, param.typeName, valuesProperty.GetArrayElementAtIndex(0), serializedObject);
                                }
                                else if (parameters.Length > 1)
                                {
                                    if (valuesProperty.arraySize == 0 || valuesProperty.arraySize != parameters.Length)
                                    {
                                        valuesProperty.arraySize = parameters.Length;

                                        for (var i = 0; i < parameters.Length; i++)
                                        {
                                            var param = parameters[i];
                                            var valueProperty = valuesProperty.GetArrayElementAtIndex(i);

                                            var typeProperty = valueProperty.FindPropertyRelative(ValueFields.Type);
                                            var fullTypeNameProperty = valueProperty.FindPropertyRelative(ValueFields.FullTypeName);

                                            typeProperty.intValue = (int)param.returnType;
                                            fullTypeNameProperty.stringValue = param.assemblyQualifiedName;
                                        }

                                        serializedObject.ApplyModifiedProperties();
                                    }

                                    if (GUI.Button(valueRect, WIZARD))
                                    {
                                        var wizard = ScriptableWizard.DisplayWizard<ParameterWizard>(string.Empty, "Save");
                                        wizard.dataList = parameters;
                                        wizard.serializedProperty = valuesProperty.Copy();
                                        wizard.path = valuesProperty.propertyPath;
                                        wizard.onClose = OnParameterWizardClose;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            EditorGUI.EndDisabledGroup();
        }

        private void DrawValue(Rect rect, MemberInfos infos, string name, string assemblyQualifiedName, ObjectType objectType, string typeName, SerializedProperty valueProperty, SerializedObject serializedObject)
        {
            var fieldProp = valueProperty.GetPropertyOfType(objectType);

            if (fieldProp == null)
                return;

            var typeProperty = valueProperty.FindPropertyRelative(ValueFields.Type);
            var fullTypeNameProperty = valueProperty.FindPropertyRelative(ValueFields.FullTypeName);

            typeProperty.intValue = (int) objectType;
            fullTypeNameProperty.stringValue = assemblyQualifiedName;
            serializedObject.ApplyModifiedProperties();

            switch (objectType)
            {
                case ObjectType.Bounds:
                case ObjectType.Rect:
                case ObjectType.Matrix4x4:
                case ObjectType.SerializableType:
                case ObjectType.Array:
                case ObjectType.List:
                    if (GUI.Button(rect, WIZARD))
                    {
                        var wizard = ScriptableWizard.DisplayWizard<PropertyWizard>(string.Empty, "Save");
                        wizard.infos = infos;
                        wizard.type = objectType;
                        wizard.typeOf = Type.GetType(assemblyQualifiedName);
                        wizard.serializedProperty = fieldProp;
                        wizard.path = fieldProp.propertyPath;
                        wizard.onClose = OnPropertyWizardClose;
                    }
                    return;
            }

            var label = new GUIContent(typeName);

            switch (objectType)
            {
                case ObjectType.Boolean:
                case ObjectType.Int32:
                case ObjectType.Int64:
                case ObjectType.Single:
                case ObjectType.Double:
                case ObjectType.String:
                    EditorGUI.PropertyField(rect, fieldProp, label, false);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.UInt64:
                    var oldValue = fieldProp.stringValue;
                    var ulongString = EditorGUI.TextField(rect, label, fieldProp.stringValue);
                    ulong ulongVal;

                    if (!ulong.TryParse(ulongString, out ulongVal))
                        ulongString = oldValue;

                    if (string.IsNullOrEmpty(ulongString))
                        ulongString = "0";

                    fieldProp.stringValue = ulongString;
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.LayerMask:
                case ObjectType.Color:
                case ObjectType.AnimationCurve:
                    EditorGUI.PropertyField(rect, fieldProp, GUIContent.none, false);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Byte:
                    fieldProp.intValue = EditorFields.ByteField(rect, label, (byte) fieldProp.intValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.SByte:
                    fieldProp.intValue = EditorFields.SByteField(rect, label, (sbyte) fieldProp.intValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Char:
                    fieldProp.intValue = EditorFields.CharField(rect, label, (char) fieldProp.intValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Int16:
                    fieldProp.intValue = EditorFields.ShortField(rect, label, (short) fieldProp.intValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.UInt16:
                    fieldProp.intValue = EditorFields.UShortField(rect, label, (ushort) fieldProp.intValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.UInt32:
                    fieldProp.longValue = EditorFields.UIntField(rect, label, (uint) fieldProp.longValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Enum:
                    {
                        var typeOf = Type.GetType(assemblyQualifiedName);
                        var flagsAttribute = typeOf.GetCustomAttributes(typeof(FlagsAttribute), false);
                        Enum enumValue;

                        if (flagsAttribute != null && flagsAttribute.Length >= 1)
                        {
                            enumValue = EditorGUI.EnumMaskPopup(rect, GUIContent.none, (Enum) Enum.ToObject(typeOf, fieldProp.longValue));
                        }
                        else
                        {
                            enumValue = EditorGUI.EnumPopup(rect, (Enum) Enum.ToObject(typeOf, fieldProp.longValue));
                        }

                        try
                        {
                            fieldProp.longValue = (int) Enum.ToObject(typeOf, enumValue);
                            serializedObject.ApplyModifiedProperties();
                        }
                        catch
                        {
                            fieldProp.longValue = (long) Enum.ToObject(typeOf, enumValue);
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;

                case ObjectType.Vector2:
                    fieldProp.vector2Value = EditorGUI.Vector2Field(rect, GUIContent.none, fieldProp.vector2Value);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Vector3:
                    fieldProp.vector3Value = EditorGUI.Vector3Field(rect, GUIContent.none, fieldProp.vector3Value);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Vector4:
                    fieldProp.vector4Value = EditorGUI.Vector4Field(rect, GUIContent.none, fieldProp.vector4Value);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.Quaternion:
                    fieldProp.quaternionValue = EditorFields.QuaternionField(rect, GUIContent.none, fieldProp.quaternionValue);
                    serializedObject.ApplyModifiedProperties();
                    break;

                case ObjectType.UnityObject:
                    {
                        var typeOf = Type.GetType(assemblyQualifiedName);
                        fieldProp.objectReferenceValue = EditorGUI.ObjectField(rect, GUIContent.none, fieldProp.objectReferenceValue, typeOf, true);
                        serializedObject.ApplyModifiedProperties();
                    }
                    break;
            }

        }

        private void SelectListener(ReorderableList list)
        {
            this.lastSelectedIndex = list.index;
        }

        private void EndDragChild(ReorderableList list)
        {
            this.lastSelectedIndex = list.index;
        }

        private void AddListener(ReorderableList list)
        {
            if (this.listenersProperty.hasMultipleDifferentValues)
            {
                foreach (UObject targetObject in this.listenersProperty.serializedObject.targetObjects)
                {
                    var serializedTargetObject = new SerializedObject(targetObject);
                    serializedTargetObject.FindProperty(this.listenersProperty.propertyPath).arraySize += 1;
                    serializedTargetObject.ApplyModifiedProperties();
                }

                this.listenersProperty.serializedObject.SetIsDifferentCacheDirty();
                this.listenersProperty.serializedObject.Update();
                list.index = list.serializedProperty.arraySize - 1;
            }
            else
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
                list.serializedProperty.serializedObject.ApplyModifiedProperties();
            }

            this.lastSelectedIndex = list.index;

            var element = this.listenersProperty.GetArrayElementAtIndex(list.index);
            var serializedObject = element.serializedObject;

            element.FindPropertyRelative(ListenerFields.Target).objectReferenceValue = null;
            element.FindPropertyRelative(ListenerFields.Index).intValue = -1;
            element.FindPropertyRelative(ListenerFields.MemberFilter).intValue = 0;
            element.FindPropertyRelative(ListenerFields.VisibilityFilter).intValue = 0;
            element.FindPropertyRelative(ListenerFields.LevelFilter).intValue = 0;
            element.FindPropertyRelative(ListenerFields.Values).arraySize = 0;
            serializedObject.ApplyModifiedProperties();

        }

        private void RemoveListener(ReorderableList list)
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            list.serializedProperty.serializedObject.ApplyModifiedProperties();

            this.lastSelectedIndex = list.index;
        }

        private void OnGUI(Rect position)
        {
            if (this.listenersProperty == null || !this.listenersProperty.isArray)
                return;

            if (this.reorderableList == null)
                return;

            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            this.reorderableList.DoList(position);

            EditorGUI.indentLevel = indentLevel;
        }

        private MemberList InsertOrUpdateMembers(SerializedProperty property, int index, MemberFilter memberFilter, VisibilityFilter visibilityFilter, LevelFilter levelFilter, string returnTypeName)
        {
            if (property.objectReferenceValue == null)
                return null;

            var flags = BindingFlags.Default;

            flags |= (memberFilter == MemberFilter.Instance) ? BindingFlags.Instance : BindingFlags.Static;
            flags |= (visibilityFilter == VisibilityFilter.Public) ? BindingFlags.Public : BindingFlags.NonPublic;
            flags |= (levelFilter == LevelFilter.Declare) ? BindingFlags.DeclaredOnly : BindingFlags.FlattenHierarchy;

            while (this.members.Count <= index)
            {
                this.members.Insert(this.members.Count, null);
            }

            if (this.members[index] != null)
                this.members[index].Initialize(property.objectReferenceValue, flags, returnTypeName);
            else
                this.members[index] = new MemberList(property.objectReferenceValue, flags, returnTypeName);

            return this.members[index];
        }

        private void UpdateMemberInfo(SerializedProperty property, int index, MemberList members, SerializedObject serializedObject)
        {
            var returnTypeProperty = property.FindPropertyRelative(MemberInfoFields.ReturnType);
            var memberTypeProperty = property.FindPropertyRelative(MemberInfoFields.MemberType);
            var nameProperty = property.FindPropertyRelative(MemberInfoFields.MemberName);
            var parameterTypesProperty = property.FindPropertyRelative(MemberInfoFields.ParameterTypes);
            var targetFullNameProperty = property.FindPropertyRelative(MemberInfoFields.TargetFullName);
            var targetProperty = property.FindPropertyRelative(MemberInfoFields.Target);

            var mem = members.members[index];
            var target = members.targets[index];

            nameProperty.stringValue = mem.name;
            targetFullNameProperty.stringValue = members.dataList[index].fullName;
            targetProperty.objectReferenceValue = target;
            parameterTypesProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();

            switch (mem.type)
            {
                case MemberType.Field:
                case MemberType.Property:
                    {
                        returnTypeProperty.enumValueIndex = (int)mem.returnType;
                        memberTypeProperty.enumValueIndex = (int)mem.type;
                        parameterTypesProperty.InsertArrayElementAtIndex(0);
                        parameterTypesProperty.GetArrayElementAtIndex(0).stringValue = mem.assemblyQualifiedName;
                        serializedObject.ApplyModifiedProperties();
                    }
                    break;

                case MemberType.Method:
                    {
                        returnTypeProperty.enumValueIndex = (int)mem.returnType;
                        memberTypeProperty.enumValueIndex = (int)mem.type;
                        serializedObject.ApplyModifiedProperties();

                        var parameters = mem.parameters;

                        for (var i = 0; i < parameters.Length; ++i)
                        {
                            var param = parameters[i];
                            parameterTypesProperty.InsertArrayElementAtIndex(i);
                            parameterTypesProperty.GetArrayElementAtIndex(i).stringValue = param.assemblyQualifiedName;
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                    break;
            }
        }

        private void OnPropertyWizardClose(PropertyWizardValue value)
        {
            if (value == null)
                return;

            var p = this.rootProperty.serializedObject.FindProperty(value.path);
            var valueProperty = value.serializedObject.FindProperty(FakeObjectFields.Value).GetPropertyOfType(value.type);

            var result = valueProperty.CopyTo(p);

            if (result > 0)
                p.serializedObject.ApplyModifiedProperties();
            else if (result < 0)
                Debug.Log(string.Format("Type `{0}` is not supported.", value.type));
        }

        private void OnParameterWizardClose(ParameterWizardValue value)
        {
            if (value == null)
                return;

            var valuesProperty = this.rootProperty.serializedObject.FindProperty(value.path);

            for (var i = 0; i < value.serializedObjects.Count; i++)
            {
                var type = value.dataList[i].returnType;
                var valueProperty = value.serializedObjects[i].FindProperty(FakeObjectFields.Value).GetPropertyOfType(type);
                var p = valuesProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ValueFields.Get(type));

                var result = valueProperty.CopyTo(p);

                if (result > 0)
                    p.serializedObject.ApplyModifiedProperties();
                else if (result < 0)
                    Debug.Log(string.Format("Type `{0}` is not supported.", type));
            }
        }

        private class State
        {
            public int lastSelectedIndex;
            public ReorderableList reorderableList;
        }
    }

    [CustomPropertyDrawer(typeof(ExtendedEvent))]
    public class ExtendedEventEditor : ExtendedEventBaseEditor
    {
    }
}
