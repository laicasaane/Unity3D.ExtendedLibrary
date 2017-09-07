using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace ExtendedLibrary.Editor
{
    public class SavedTypeListWindow : EditorWindow
    {
        private const string DEFAULT_NAMESPACE_LABEL = "Default Namespace";
        private const string LOCATE_CREATE_LABEL = "Locate Or Create Database";
        private const string CUSTOM_NAMESPACES_LABEL = "Custom Namespaces";
        private const string NAMESPACE_SETTINGS_LABEL = "Namespace Settings";
        private const string NAMESPACE_BUTTON_LABEL = "Update Namespace Settings";
        private const string UPDATE_BUTTON_LABEL = "Update Database";
        private const string CLEAR_BUTTON_LABEL = "Clear Database";


        private static GUIContent EmptyNamespaceLabel = new GUIContent("Empty Namespace", "Include types that have no namespace");
        private static SavedTypeListWindow _instance;
        private static GUIStyle _indexLabelSkin;

        [MenuItem("Tools/Extended Library/Type Database", priority = 1)]
        public static void ShowWindow()
        {
            if (_instance != null)
                return;

            var sobj = CreateInstance<TypeDataDictionary>();

            _instance = CreateInstance<SavedTypeListWindow>();
            _instance.titleContent = new GUIContent("Type Database");
            _instance.serializedObject = new SerializedObject(sobj);
            _instance.customNamespacesProperty = _instance.serializedObject.FindProperty("customNamespaces");
            _instance.includeEmptyNamespaceTypesProperty = _instance.serializedObject.FindProperty("includeEmptyNamespaceTypes");
            _instance.Show();
        }

        private TypeDataDictionary dictionary;
        private string[][] savedTypes;
        private List<string> customNamespaces = new List<string>();

        private SerializedObject serializedObject;
        private SerializedProperty customNamespacesProperty;
        private SerializedProperty includeEmptyNamespaceTypesProperty;
        private Vector2 scrollPosition = Vector2.zero;
        private bool showTypeList = false;

        private void OnGUI()
        {
            if (_indexLabelSkin == null)
            {
                _indexLabelSkin = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
            }

            GUILayout.Space(10f);
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);

            if (GUILayout.Button(LOCATE_CREATE_LABEL))
            {
                TypeDataDictionary.LocateOrCreateAsset();
                this.dictionary = Selection.activeObject as TypeDataDictionary;
                Selection.activeObject = null;
                this.savedTypes = this.dictionary.GetSavedTypes();
                this.includeEmptyNamespaceTypesProperty.boolValue = this.dictionary.IncludeEmptyNamespaceTypes;

                var namespaces = this.dictionary.CustomNamespaces;
                this.customNamespacesProperty.arraySize = namespaces.Count;
                this.customNamespacesProperty.ClearArray();

                for (var i = 0; i < namespaces.Count; i++)
                {
                    this.customNamespacesProperty.InsertArrayElementAtIndex(i);
                    this.customNamespacesProperty.GetArrayElementAtIndex(i).stringValue = namespaces[i];
                }
            }

            if (this.dictionary != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(NAMESPACE_SETTINGS_LABEL, EditorStyles.boldLabel);
                EditorGUILayout.TextField(DEFAULT_NAMESPACE_LABEL, TypeExtension.UNITY_NAMESPACE);
                EditorGUILayout.PropertyField(this.includeEmptyNamespaceTypesProperty, EmptyNamespaceLabel, true);

                ReorderableListGUI.Title(CUSTOM_NAMESPACES_LABEL);
                ReorderableListGUI.ListField(this.customNamespacesProperty, ReorderableListFlags.ShowIndices);

                if (GUILayout.Button(NAMESPACE_BUTTON_LABEL))
                {
                    Selection.activeObject = this.dictionary;
                    this.dictionary.SetIncludeEmptyNamespaceTypes(this.includeEmptyNamespaceTypesProperty.boolValue);

                    this.customNamespaces.Clear();

                    for (var i = 0; i < this.customNamespacesProperty.arraySize; i++)
                    {
                        var customNamespace = this.customNamespacesProperty.GetArrayElementAtIndex(i).stringValue;
                        this.customNamespaces.Add(customNamespace);
                    }

                    this.dictionary.SetCustomNamespaces(this.customNamespaces);
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(UPDATE_BUTTON_LABEL))
                {
                    Selection.activeObject = this.dictionary;
                    this.dictionary.UpdateData();
                    this.savedTypes = this.dictionary.GetSavedTypes();
                }

                if (GUILayout.Button(CLEAR_BUTTON_LABEL))
                {
                    Selection.activeObject = this.dictionary;
                    this.dictionary.ClearData();
                    this.savedTypes = this.dictionary.GetSavedTypes();
                }
                GUILayout.EndHorizontal();

                Selection.activeObject = null;
            }

            EditorGUILayout.Space();
            var length = this.savedTypes == null ? 0 : this.savedTypes.Length;
            EditorGUILayout.LabelField(string.Format("Types [{0}]", length), EditorStyles.boldLabel);
            this.showTypeList = EditorGUILayout.Foldout(this.showTypeList, "");

            if (length > 0 && this.showTypeList)
            {
                GUILayout.Space(5f);
                for (var i = 0; i < this.savedTypes.Length; i++)
                {
                    var names = this.savedTypes[i];

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(i.ToString(), _indexLabelSkin, GUILayout.MaxWidth(50f));

                    for (var n = 0; n < names.Length; n++)
                    {
                        EditorGUILayout.TextField(names[n]);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.Space(10f);
        }
    }
}
