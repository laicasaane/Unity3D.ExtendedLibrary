using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ExtendedLibrary.Editor
{
    public class NamespacesWindow : EditorWindow
    {
        private static NamespacesWindow _instance;
        private static GUIStyle _indexLabelSkin;

        [MenuItem("Tools/Extended Library/Available Namespaces", priority = 1)]
        public static void ShowWindow()
        {
            if (_instance != null)
                return;

            var namespaces = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => a.GetTypes())
                            .Select(t => t.Namespace).Distinct()
                            .OrderBy(ns => ns).ToArray();

            _instance = CreateInstance<NamespacesWindow>();
            _instance.titleContent = new GUIContent("Namespaces");
            _instance.namespaces = namespaces;
            _instance.Show();
        }

        private string[] namespaces;
        private Vector2 scrollPosition;

        private void OnGUI()
        {
            if (this.namespaces == null)
                return;

            if (_indexLabelSkin == null)
            {
                _indexLabelSkin = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
            }

            GUILayout.Space(10f);
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);

            EditorGUILayout.LabelField("Available Namespaces", GUI.skin.label);

            for (var i = 0; i < this.namespaces.Length; i++)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(i.ToString(), _indexLabelSkin, GUILayout.MaxWidth(50f));
                EditorGUILayout.TextField(this.namespaces[i]);

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            GUILayout.Space(10f);
        }
    }
}
