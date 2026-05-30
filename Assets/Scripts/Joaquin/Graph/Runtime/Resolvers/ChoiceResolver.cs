using UnityEngine;
using System.Threading.Tasks;

public class ChoiceResolver : INodeResolver<ChoiceRuntimeNode>
{
    public Task Resolve(DialogController controller, ChoiceRuntimeNode node)
    {
        controller.SituationManager.ApplyEffectFromGraph(node);
        return Task.CompletedTask;
    }
}