using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ExtendedLibrary.Events
{
    public class PropertyWizard : BaseWizard<PropertyWizardValue>
    {
        public Type typeOf;
        public ObjectType type;
        public MemberInfos infos;

        private FakeObject fakeObject;
        private SerializedObject serializedFakeObject;
        private SerializedProperty serializedFakeProperty;

        private ScriptableObject cloneObject;
        private FieldInfo cloneValueFieldInfo;
        private SerializedObject serializedCloneObject;
        private SerializedProperty serializedCloneProperty;

        protected override sealed bool DrawWizardGUI()
        {
            if (this.fakeObject == null)
            {
                this.fakeObject = CreateInstance<FakeObject>();
                this.serializedFakeObject = new SerializedObject(this.fakeObject);

                this.serializedFakeProperty = this.serializedFakeObject.FindProperty(FakeObjectFields.Value).GetPropertyOfType(this.type);
                this.serializedProperty.CopyTo(this.serializedFakeProperty);
            }

            DrawMember(this.serializedFakeProperty);

            return true;
        }

        private void DrawMember(SerializedProperty property)
        {
            switch (this.type)
            {
                case ObjectType.Bounds:
                    property.boundsValue = EditorGUILayout.BoundsField(this.infos.memberData.guiLabel, property.boundsValue);
                    break;

                case ObjectType.Rect:
                    property.rectValue = EditorGUILayout.RectField(this.infos.memberData.guiLabel, property.rectValue);
                    break;

                case ObjectType.Matrix4x4:
                    {
                        var matrix = property.stringValue.ToObject<Matrix4x4>();
                        matrix = EditorFields.Matrix4x4Field(this.infos.memberData.guiLabel, matrix);
                        property.stringValue = matrix.ToJson();
                    }
                    break;

                case ObjectType.SerializableType:
                case ObjectType.Array:
                case ObjectType.List:
                    {
                        if (this.cloneObject == null)
                        {
                            var cloneObjectType = CreateCloneObjectType("CloneObject", FakeObjectFields.Value, this.typeOf);
                            this.cloneObject = CreateInstance(cloneObjectType);
                            this.cloneValueFieldInfo = cloneObjectType.GetField(FakeObjectFields.Value,
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        }

                        var data = property.stringValue.ToObject(this.typeOf);

                        if (data == null)
                        {
                            if (this.type == ObjectType.Array)
                                data = Activator.CreateInstance(this.typeOf, 0);
                            else
                                data = Activator.CreateInstance(this.typeOf);
                        }

                        this.cloneValueFieldInfo.SetValue(this.cloneObject, data);

                        if (this.serializedCloneObject == null)
                        {
                            this.serializedCloneObject = new SerializedObject(this.cloneObject);
                            this.serializedCloneProperty = this.serializedCloneObject.FindProperty(FakeObjectFields.Value);
                        }

                        EditorGUI.BeginChangeCheck();

                        this.serializedCloneObject.Update();
                        EditorGUILayout.PropertyField(this.serializedCloneProperty, this.infos.memberData.guiLabel, true);

                        if (EditorGUI.EndChangeCheck())
                        {
                            this.serializedCloneObject.ApplyModifiedPropertiesWithoutUndo();
                            data = this.cloneValueFieldInfo.GetValue(this.cloneObject);

                            property.stringValue = data.ToJson(this.typeOf);
                        }
                    }
                    break;
            }
        }

        private void OnWizardCreate()
        {
            this.onClose(new PropertyWizardValue()
            {
                path = this.path,
                typeOf = this.typeOf,
                type = this.type,
                serializedObject = this.serializedFakeObject
            });
        }
    }

    public class PropertyWizardValue
    {
        public Type typeOf;
        public ObjectType type;
        public SerializedObject serializedObject;
        public string path;
    }
}
