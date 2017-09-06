using UnityEditor;
using System.Collections.Generic;

public static class AssetExtension
{
    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        var assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).ToString().Replace("UnityEngine.", "")));

        for (int i = 0; i < guids.Length; i++)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (asset != null)
            {
                assets.Add(asset);
            }
        }

        return assets;
    }
}