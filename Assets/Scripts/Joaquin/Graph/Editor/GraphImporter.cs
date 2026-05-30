using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, GenericGraph.AssetExtension)]
public class GraphImporter : ScriptedImporter
{
    private Dictionary<INode, int> nodeIndexMap = new();

    private void IndexAllNodes(GenericGraph graph)
    {
        int i = 0;
        foreach (var node in graph.GetNodes())
        {
            if (node is not StartNode) nodeIndexMap[node] = i++;
        }
    }

    public override void OnImportAsset(AssetImportContext context)
    {
        GenericGraph graph = GraphDatabase.LoadGraphForImporter<GenericGraph>(context.assetPath);
        StartNode startNode = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
        GenericRuntimeGraph runtimeAsset = ScriptableObject.CreateInstance<GenericRuntimeGraph>();
        INode nextNode = GetNextNode(startNode);
        while (nextNode != null)
        {
            List<GenericRuntimeNode> runtimeNodes = ConvertNodesToRuntimeNodes(nextNode);
            runtimeAsset.nodes.AddRange(runtimeNodes);
            nextNode = GetNextNode(nextNode);
        }
        context.AddObjectToAsset("RuntimeAsset", runtimeAsset);
        context.SetMainObject(runtimeAsset);
    }

    static INode GetNextNode(INode currentNode)
    {
        if (currentNode == null) return null;
        IPort outputPort = currentNode.GetOutputPortByName(GenericNode.OUTPUT_PORT_NAME);
        IPort nextNodePort = outputPort.firstConnectedPort;
        INode nextNode = nextNodePort.GetNode();
        return nextNode;
    }

    static List<GenericRuntimeNode> ConvertNodesToRuntimeNodes(INode currentNode)
    {
        List<GenericRuntimeNode> runtimeNodes = new List<GenericRuntimeNode>();
        switch (currentNode)
        {
            case TextNode textNode:
                runtimeNodes.Add(new TextRuntimeNode
                {
                    text = GetInputPortvalue<string>(textNode.GetInputPortByName(TextNode.INPUT_PORT_TEXT)),
                    waitTime = GetInputPortvalue<int>(textNode.GetInputPortByName(TextNode.INPUT_PORT_WAIT_TIME))
                });
                break;
        }
        return runtimeNodes;

    }

    static T GetInputPortvalue<T>(IPort port)
    {
        T value = default;
        if (port.isConnected)
        {
            switch (port.firstConnectedPort.GetNode())
            {
                case IVariableNode variableNode:
                    variableNode.variable.TryGetDefaultValue<T>(out value);
                    return value;
                    break;
                case IConstantNode constantNode:
                    constantNode.TryGetValue<T>(out value);
                    return value;
                    break;
                default:
                    break;
            }
        }
        else
        {
            port.TryGetValue(out value);
        }
        return value;
    }
}