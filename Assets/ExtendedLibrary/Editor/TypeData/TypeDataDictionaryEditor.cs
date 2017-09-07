using Rotorz.ReorderableList;
using UnityEditor;
using UnityEngine;
using ExtendedLibrary.Editor;

namespace ExtendedLibrary
{
    [CustomEditor(typeof(TypeDataDictionary))]
    public class TypeDataDictionaryEditor : UnityEditor.Editor
    {
        private const string DEFAULT_NAMESPACE_LABEL = "Default Namespace";
        private const string CUSTOM_NAMESPACES_LABEL = "Custom Namespaces";
        private const string UPDATE_BUTTON_LABEL = "Update";
        private const string CLEAR_BUTTON_LABEL = "Clear Database";
        private const string SHOW_DATA_PANEL_LABEL = "Show Saved Type List";

        private static GUIContent EmptyNamespaceLabel = new GUIContent("Empty Namespace", "Include types that have no namespace");

        private SerializedProperty customNamespacesProperty;
        private SerializedProperty includeEmptyNamespaceTypesProperty;

        private void OnEnable()
        {
            this.customNamespacesProperty = this.serializedObject.FindProperty("customNamespaces");
            this.includeEmptyNamespaceTypesProperty = this.serializedObject.FindProperty("includeEmptyNamespaceTypes");
        }

        public override void OnInspectorGUI()
        {
            var asset = this.target as TypeDataDictionary;

            EditorGUILayout.TextField(DEFAULT_NAMESPACE_LABEL, TypeExtension.UNITY_NAMESPACE);

            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.includeEmptyNamespaceTypesProperty, EmptyNamespaceLabel, true);

            ReorderableListGUI.Title(CUSTOM_NAMESPACES_LABEL);
            ReorderableListGUI.ListField(this.customNamespacesProperty, ReorderableListFlags.ShowIndices);

            this.serializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(UPDATE_BUTTON_LABEL))
            {
                asset.UpdateData();
            }

            if (GUILayout.Button(CLEAR_BUTTON_LABEL))
            {
                asset.ClearData();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10f);

            if (GUILayout.Button(SHOW_DATA_PANEL_LABEL))
            {
                SavedTypeListWindow.ShowWindow();
            }
        }
    }
}
