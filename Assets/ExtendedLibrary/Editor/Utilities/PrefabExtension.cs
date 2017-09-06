using UnityEngine;
using UnityEditor;

public static class PrefabExtension
{
	[MenuItem("Tools/Apply Prefab Changes %#p")]
	public static void ApplyPrefabChanges()
	{
		ApplyPrefabChanges(Selection.activeGameObject);
	}

	public static void ApplyPrefabChanges(this GameObject gameObject)
	{
		if (gameObject == null)
		{
			Debug.Log("Nothing selected.");
			return;
		}

		var prefabInstance = PrefabUtility.FindPrefabRoot(gameObject);
		var targetPrefab = PrefabUtility.GetPrefabParent(prefabInstance);

		if (targetPrefab == null)
		{
			Debug.Log("Selected object isn't an instance of any prefab.");
			return;
		}

		PrefabUtility.ReplacePrefab(prefabInstance, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
	}
}
