using System;
using UnityEngine;

[Serializable]
public class ChoiceRuntimeNode : GenericRuntimeNode
{
    public int parentNodeIndex;
    public string labelES;
    public string labelEN;
    public int karmaChange;
    public int hpChange;
    public int damageChange;
    public int nextNodeIndex;
}