using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Assets.UIComponent;
using System.IO;

public class UXMLPreprocessor : AssetPostprocessor
{

    // Called when importing assets
    static void OnPostprocessAllAssets(
        string[] importedAssets, 
        string[] deletedAssets, 
        string[] movedAssets, 
        string[] movedFromAssetPaths)
    {
        Debug.Log($"Postprocessing {importedAssets.Length} assets");
        foreach (string assetPath in importedAssets)
        {
            // Check if the imported asset is a VisualTreeAsset (UXML file)
            if (assetPath.EndsWith(".uxml"))
            {
                Debug.Log($"UXML file modified: {assetPath}");

                // Load the asset into memory as a VisualTreeAsset
                VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);

                if (visualTree == null)
                {
                    Debug.LogWarning("Failed to load asset");
                    return;
                }
                var root = visualTree.CloneTree();
                var gen = new UIComponentBackingGenerator(assetPath, root);
                string code = gen.Generate();
                //Debug.Log(code);
                GenerateCSharpScript(assetPath, code);
            }
        }
    }

    static void GenerateCSharpScript(string assetPath, string content)
    {
        // Define a name for the new C# script
        string scriptName = Path.GetFileNameWithoutExtension(assetPath) + "_Fields.cs";
        string directoryPath = "Assets/Scripts/Generated/";  // Where to save the script
        string scriptPath = Path.Combine(directoryPath, scriptName);

        // Ensure the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Write the content to a new .cs file
        File.WriteAllText(scriptPath, content);

        // Refresh the AssetDatabase to register the new script
        AssetDatabase.Refresh();

        Debug.Log($"Generated C# script at: {scriptPath}");
    }
}
