using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;

namespace ExtendedLibrary
{
    public class TypeDataDictionary : ScriptableObject
    {
        private const string ROOT_FOLDER = "Assets";
        private const string SECONDARY_FOLDER = "Editor";
        private const string LIBRARY_FOLDER = "ExtendedLibrary";
        private const string KEY_FORMAT = "{0} {1}";

        public static readonly string AssetPath = string.Format("{0}/{1}/{2}/TypeDataDictionary.asset", ROOT_FOLDER, SECONDARY_FOLDER, LIBRARY_FOLDER);

        [MenuItem("Assets/Create/TypeData Dictionary", priority = 1)]
        public static void LocateOrCreateAsset()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TypeDataDictionary>(AssetPath);

            if (asset == null)
            {
                var secondaryPath = string.Format("{0}/{1}", ROOT_FOLDER, SECONDARY_FOLDER);

                if (!AssetDatabase.IsValidFolder(secondaryPath))
                {
                    AssetDatabase.CreateFolder(ROOT_FOLDER, SECONDARY_FOLDER);
                }

                var libraryPath = string.Format("{0}/{1}", secondaryPath, LIBRARY_FOLDER);
                if (!AssetDatabase.IsValidFolder(libraryPath))
                {
                    AssetDatabase.CreateFolder(secondaryPath, LIBRARY_FOLDER);
                }

                asset = CreateInstance<TypeDataDictionary>();
                asset.INTERNAL_UpdateData();

                AssetDatabase.CreateAsset(asset, AssetPath);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
            else
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
        }

        public static string ComposeKey(Type type, BindingFlags flags)
        {
            return string.Format(KEY_FORMAT, (int)flags, type.FullName);
        }

        [SerializeField]
        private List<string> customNamespaces;

        [SerializeField]
        private bool includeEmptyNamespaceTypes;

        [SerializeField]
        private List<string> types;

        [HideInInspector]
        [SerializeField]
        private List<string> keys;

        [HideInInspector]
        [SerializeField]
        private List<TypeData> values;

        public string[][] GetSavedTypes()
        {
            var savedTypes = new string[this.types.Count][];

            for (var i = 0; i < this.types.Count; i++)
            {
                savedTypes[i] = this.types[i].Split(new[] { ';' });
            }

            return savedTypes;
        }

        public void ClearData()
        {
            if (!IsValidSelection())
                return;

            INTERNAL_ClearData();

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(Selection.activeObject);
            AssetDatabase.SaveAssets();
        }

        public void UpdateData()
        {
            if (!IsValidSelection())
                return;

            INTERNAL_UpdateData();

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(Selection.activeObject);
            AssetDatabase.SaveAssets();
        }

        public TypeData GetValue(Type type, BindingFlags flags)
        {
            var key = this.keys.IndexOf(ComposeKey(type, flags));

            if (key >= 0 && key < this.values.Count)
                return this.values[key];

            return null;
        }

        private bool IsValidSelection()
        {
            return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(TypeDataDictionary);
        }

        private void INTERNAL_ClearData()
        {
            if (this.types == null)
                this.types = new List<string>();
            else
                this.types.Clear();

            if (this.keys == null)
                this.keys = new List<string>();
            else
                this.keys.Clear();

            if (this.values == null)
                this.values = new List<TypeData>();
            else
                this.values.Clear();
        }

        private void INTERNAL_UpdateData()
        {
            INTERNAL_ClearData();

            var memberFilters = new[] { BindingFlags.Instance, BindingFlags.Static };
            var visibilityFilters = new[] { BindingFlags.Public, BindingFlags.NonPublic };
            var levelFilters = new[] { BindingFlags.DeclaredOnly, BindingFlags.FlattenHierarchy };
            var filters = new List<BindingFlags>();

            foreach (var mf in memberFilters)
            {
                foreach (var vf in visibilityFilters)
                {
                    foreach (var lf in levelFilters)
                    {
                        filters.Add(mf | vf | lf);
                    }
                }
            }

            var typesTemp = new List<Type>() { typeof(GameObject) };
            var namespaces = new List<string> { TypeExtension.UNITY_NAMESPACE };

            if (this.customNamespaces != null)
                namespaces.AddRange(this.customNamespaces);

            foreach (var nsp in namespaces)
            {
                var componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(a => a.GetTypes())
                                    .Where(t => IsValidType(t, nsp));

                typesTemp.AddRange(componentTypes);
            }

            var types = typesTemp.OrderBy(t => t.FullName);

            foreach (var type in types)
            {
                this.types.Add(string.Format("{0};{1}", type.Namespace, type.Name));

                foreach (var filter in filters)
                {
                    var key = ComposeKey(type, filter);
                    var typeData = TypeData.GetTypeData(type, filter);

                    this.keys.Add(key);
                    this.values.Add(typeData);
                }
            }
        }

        private bool IsValidType(Type type, string typeNamespace)
        {
            if (string.IsNullOrEmpty(type.Namespace))
            {
                if (!this.includeEmptyNamespaceTypes)
                    return false;
            }
            else
            {
                if (!type.Namespace.Contains(typeNamespace))
                    return false;
            }

            return type.IsClass && !type.IsAbstract && type.IsSubclassOf(TypeExtension.ComponentType);
        }
    }
}