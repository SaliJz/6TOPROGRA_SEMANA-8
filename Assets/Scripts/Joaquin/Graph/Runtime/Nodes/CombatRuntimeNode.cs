using System;
using UnityEngine;

[Serializable]
public class CombatRuntimeNode : GenericRuntimeNode
{
    public string enemyNameES;
    public string enemyNameEN;
    public int enemyHP;
    public int enemyDamage;
    public int onWinNextIndex;
    public int onLoseNextIndex;
    public int onFleeNextIndex;
}