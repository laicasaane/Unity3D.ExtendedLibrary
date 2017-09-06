using UnityEditor;
using UnityEngine;

namespace ExtendedLibrary.Editor
{
    public class SavedTypeListWindow : EditorWindow
    {
        private static SavedTypeListWindow _instance;
        private static GUIStyle _indexLabelSkin;

        [MenuItem("Tools/Extended Library/Saved Type List", priority = 1)]
        public static void ShowWindow()
        {
            if (_instance != null)
                return;

            var dictionary = AssetDatabase.LoadAssetAtPath<TypeDataDictionary>(TypeDataDictionary.AssetPath);

            if (dictionary == null)
            {
                TypeDataDictionary.LocateOrCreateAsset();
                dictionary = Selection.activeObject as TypeDataDictionary;
            }

            _instance = CreateInstance<SavedTypeListWindow>();
            _instance.titleContent = new GUIContent("Type List");
            _instance.savedTypes = dictionary.GetSavedTypes();
            _instance.Show();
        }

        private string[][] savedTypes;
        private Vector2 scrollPosition = Vector2.zero;

        private void OnGUI()
        {
            if (_indexLabelSkin == null)
            {
                _indexLabelSkin = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
            }

            GUILayout.Space(10f);
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);

            if (GUILayout.Button("Locate Or Create Asset"))
            {
                TypeDataDictionary.LocateOrCreateAsset();
            }

            var length = this.savedTypes == null ? 0 : this.savedTypes.Length;
            EditorGUILayout.LabelField(string.Format("Saved Type List [{0}]", length));

            if (length > 0)
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
