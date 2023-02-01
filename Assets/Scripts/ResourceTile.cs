using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourceTile : Tile
{
    public string resourceColor;

    public string GetResourceColor() {
        return resourceColor;
    }
    
    #if UNITY_EDITOR
    [MenuItem("Assets/Create/ResourceTile")]
    public static void CreateResourceTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Resource Tile", "New Resource Tile", "Asset", "Save Resource Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ResourceTile>(), path);
    }
    #endif
}
