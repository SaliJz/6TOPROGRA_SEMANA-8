using UnityEditor;
using Unity.GraphToolkit.Editor;
using System;

[Graph(AssetExtension)]
[Serializable]
public class GenericGraph : Graph
{
    public const string AssetExtension = "graph";

    [MenuItem("Assets/Create/Graphs/GenericGraph")]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<GenericGraph>();
    }
}