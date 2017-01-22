using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateAssets
{
    static public void CreateAsset(Object asset, string path)
    {
        // Create new.
        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path));
        AssetDatabase.SaveAssets();
        // Focus in explorer.
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Design/Create Gameplay Variables")]
    public static void CreateGameplayVariables()
    {
        CreateAsset(ScriptableObject.CreateInstance<GameplayVariables>(), "Assets/Design/Variables/NewGameplayVariables.asset");
    }

    [MenuItem("Design/Save Assets")]
    public static void SaveAssets()
    {
        AssetDatabase.SaveAssets();
    }
}
