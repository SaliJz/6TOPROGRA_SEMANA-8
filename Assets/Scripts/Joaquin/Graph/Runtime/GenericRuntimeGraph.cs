using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenericRuntimeGraph : ScriptableObject
{
    #region Inspector - Data

    [SerializeReference] public List<GenericRuntimeNode> nodes = new List<GenericRuntimeNode>();

    #endregion

    #region Graph Operations

    public ChoiceRuntimeNode[] GetChoicesAfter(int textNodeIndex)
    {
        return nodes
            .OfType<ChoiceRuntimeNode>()
            .Where(c => c.parentNodeIndex == textNodeIndex)
            .ToArray();
    }

    #endregion
}