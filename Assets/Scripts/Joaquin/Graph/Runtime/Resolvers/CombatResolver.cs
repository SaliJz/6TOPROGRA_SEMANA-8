using Assets.Scripts.Joaquin.Core;
using System.Threading.Tasks;
using UnityEngine;

public class CombatResolver : INodeResolver<CombatRuntimeNode>
{
    public async Task Resolve(DialogController controller, CombatRuntimeNode node)
    {
        CombatOutcome outcome = await controller.TriggerCombat(node);

        int nextIndex = outcome switch
        {
            CombatOutcome.PlayerWon => node.onWinNextIndex,
            CombatOutcome.PlayerLost => node.onLoseNextIndex,
            CombatOutcome.PlayerFled => node.onFleeNextIndex,
            _ => node.onWinNextIndex
        };

        controller.RunFrom(nextIndex);
    }
}