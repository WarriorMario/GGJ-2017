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

    [MenuItem("Design/GameplayVariables")]
    public static void CreatePlayerConfig()
    {
        CreateAsset(ScriptableObject.CreateInstance<GameplayVariables>(), "Assets/Design/Variables/NewGameplayVariables.asset");
    }
}
