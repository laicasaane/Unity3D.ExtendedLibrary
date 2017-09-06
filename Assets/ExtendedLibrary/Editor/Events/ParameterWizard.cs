using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ExtendedLibrary.Events
{
    public class ParameterWizard : BaseWizard<ParameterWizardValue>
    {
        public ParameterData[] dataList;

        private List<FakeObject> fakeObjects;
        private List<SerializedObject> serializedFakeObjects;
        private List<SerializedProperty> serializedFakeProperties;
        private List<GUIContent> labels;

        private ScriptableObject[] cloneObjects;
        private FieldInfo[] cloneValueFieldInfos;
        private SerializedObject[] serializedCloneObjects;
        private SerializedProperty[] serializedCloneProperties;

        protected override sealed bool DrawWizardGUI()
        {
            if (this.fakeObjects == null || this.serializedFakeObjects == null || this.labels == null)
            {
                this.fakeObjects = new List<FakeObject>();
                this.serializedFakeObjects = new List<SerializedObject>();
                this.serializedFakeProperties = new List<SerializedProperty>();
                this.labels = new List<GUIContent>();

                for (var i = 0; i < this.dataList.Length; i++)
                {
                    this.fakeObjects.Add(CreateInstance<FakeObject>());
                    this.serializedFakeObjects.Add(new SerializedObject(this.fakeObjects[i]));

                    var param = this.dataList[i];
                    var valueProperty = this.serializedProperty.GetArrayElementAtIndex(i).GetPropertyOfType(param.returnType);
                    var fakeProperty = this.serializedFakeObjects[i].FindProperty(FakeObjectFields.Value).GetPropertyOfType(param.returnType);
                    valueProperty.CopyTo(fakeProperty);

                    string label;

                    switch (param.returnType)
                    {
                        case ObjectType.LayerMask:
                        case ObjectType.Color:
                        case ObjectType.AnimationCurve:
                            label = param.name;
                            break;

                        default:
                            label = string.Format("{0} : {1}", param.name, param.typeName);
                            break;
                    }

                    this.labels.Add(new GUIContent(label));
                    this.serializedFakeProperties.Add(fakeProperty);
                }
            }

            if (this.cloneObjects == null || this.cloneObjects.Length != this.dataList.Length)
            {
                this.cloneObjects = new ScriptableObject[this.dataList.Length];
                this.cloneValueFieldInfos = new FieldInfo[this.dataList.Length];
                this.serializedCloneObjects = new SerializedObject[this.dataList.Length];
                this.serializedCloneProperties = new SerializedProperty[this.dataList.Length];
            }

            for (var i = 0; i < this.dataList.Length; i++)
            {
                DrawMember(this.serializedFakeProperties[i], ref i, this.dataList[i]);
            }

            return true;
        }

        private void DrawMember(SerializedProperty property, ref int index, ParameterData parameter)
        {
            var label = this.labels[index];

            switch (parameter.returnType)
            {
                case ObjectType.Boolean:
                case ObjectType.Int32:
                case ObjectType.Int64:
                case ObjectType.Single:
                case ObjectType.Double:
                case ObjectType.String:
                case ObjectType.LayerMask:
                case ObjectType.Color:
                case ObjectType.AnimationCurve:
                    EditorGUILayout.PropertyField(property, label, false);
                    break;

                case ObjectType.UInt64:
                    var oldString = property.stringValue;
                    var ulongString = EditorGUILayout.TextField(label, property.stringValue);
                    ulong ulongVal;

                    if (!ulong.TryParse(ulongString, out ulongVal))
                        ulongString = oldString;

                    if (string.IsNullOrEmpty(ulongString))
                        ulongString = "0";

                    property.stringValue = ulongString;
                    break;

                case ObjectType.Byte:
                    property.intValue = EditorFields.ByteField(label, (byte) property.intValue);
                    break;

                case ObjectType.SByte:
                    property.intValue = EditorFields.SByteField(label, (sbyte) property.intValue);
                    break;

                case ObjectType.Char:
                    property.intValue = EditorFields.CharField(label, (char) property.intValue);
                    break;

                case ObjectType.Int16:
                    property.intValue = EditorFields.ShortField(label, (short) property.intValue);
                    break;

                case ObjectType.UInt16:
                    property.intValue = EditorFields.UShortField(label, (ushort) property.intValue);
                    break;

                case ObjectType.UInt32:
                    property.longValue = EditorFields.UIntField(label, (uint) property.longValue);
                    break;

                case ObjectType.Enum:
                    {
                        var typeOf = Type.GetType(parameter.assemblyQualifiedName);
                        var flagsAttribute = typeOf.GetCustomAttributes(typeof(FlagsAttribute), false);
                        Enum enumValue;

                        if (flagsAttribute != null && flagsAttribute.Length >= 1)
                        {
                            enumValue = EditorGUILayout.EnumMaskPopup(label, (Enum) Enum.ToObject(typeOf, property.longValue));
                        }
                        else
                        {
                            enumValue = EditorGUILayout.EnumPopup(label, (Enum) Enum.ToObject(typeOf, property.longValue));
                        }

                        try
                        {
                            property.longValue = (int) Enum.ToObject(typeOf, enumValue);
                        }
                        catch
                        {
                            property.longValue = (long) Enum.ToObject(typeOf, enumValue);
                        }
                    }
                    break;

                case ObjectType.Vector2:
                    property.vector2Value = EditorGUILayout.Vector2Field(label, property.vector2Value);
                    break;

                case ObjectType.Vector3:
                    property.vector3Value = EditorGUILayout.Vector3Field(label, property.vector3Value);
                    break;

                case ObjectType.Vector4:
                    property.vector4Value = EditorGUILayout.Vector4Field(label, property.vector4Value);
                    break;

                case ObjectType.Bounds:
                    property.boundsValue = EditorGUILayout.BoundsField(label, property.boundsValue);
                    break;

                case ObjectType.Rect:
                    property.rectValue = EditorGUILayout.RectField(label, property.rectValue);
                    break;

                case ObjectType.Quaternion:
                    property.quaternionValue = EditorFields.QuaternionField(label, property.quaternionValue);
                    break;

                case ObjectType.UnityObject:
                    {
                        var typeOf = Type.GetType(parameter.assemblyQualifiedName);
                        property.objectReferenceValue = EditorGUILayout.ObjectField(label, property.objectReferenceValue, typeOf, true);
                    }
                    break;

                case ObjectType.Matrix4x4:
                    {
                        var matrix = property.stringValue.ToObject<Matrix4x4>();
                        matrix = EditorFields.Matrix4x4Field(label, matrix);
                        property.stringValue = matrix.ToJson();
                    }
                    break;

                case ObjectType.SerializableType:
                case ObjectType.Array:
                case ObjectType.List:
                    {
                        var typeOf = Type.GetType(parameter.assemblyQualifiedName);

                        if (this.cloneObjects[index] == null)
                        {
                            var cloneObjectType = CreateCloneObjectType("CloneObject", FakeObjectFields.Value, typeOf);
                            this.cloneObjects[index] = CreateInstance(cloneObjectType);
                            this.cloneValueFieldInfos[index] = cloneObjectType.GetField(FakeObjectFields.Value,
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        }

                        var cloneObject = this.cloneObjects[index];
                        var cloneValueFieldInfo = this.cloneValueFieldInfos[index];

                        var data = property.stringValue.ToObject(typeOf);

                        if (data == null)
                        {
                            if (parameter.returnType == ObjectType.Array)
                                data = Activator.CreateInstance(typeOf, 0);
                            else
                                data = Activator.CreateInstance(typeOf);
                        }

                        cloneValueFieldInfo.SetValue(cloneObject, data);

                        if (this.serializedCloneObjects[index] == null)
                        {
                            this.serializedCloneObjects[index] = new SerializedObject(cloneObject);
                            this.serializedCloneProperties[index] = this.serializedCloneObjects[index].FindProperty(FakeObjectFields.Value);
                        }

                        var serializedCloneObject = this.serializedCloneObjects[index];
                        var serializedCloneProperty = this.serializedCloneProperties[index];

                        EditorGUI.BeginChangeCheck();

                        serializedCloneObject.Update();
                        EditorGUILayout.PropertyField(serializedCloneProperty, label, true);

                        if (EditorGUI.EndChangeCheck())
                        {
                            serializedCloneObject.ApplyModifiedPropertiesWithoutUndo();
                            data = cloneValueFieldInfo.GetValue(cloneObject);

                            property.stringValue = data.ToJson(typeOf);
                        }
                    }
                    break;
            }
        }

        private void OnWizardCreate()
        {
            this.onClose(new ParameterWizardValue()
            {
                dataList = this.dataList,
                serializedObjects = this.serializedFakeObjects,
                path = this.path
            });
        }
    }

    public class ParameterWizardValue
    {
        public ParameterData[] dataList;
        public List<SerializedObject> serializedObjects;
        public string path;
    }
}
