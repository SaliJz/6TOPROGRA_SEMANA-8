using System;
using Unity.GraphToolkit.Editor;

[Serializable]
public class ChoiceNode : GenericNode
{
    public static readonly string INPUT_PORT_CHOICE_LABEL = "ChoiceLabel";
    public static readonly string INPUT_PORT_KARMA = "KarmaChange";
    public static readonly string INPUT_PORT_HP = "HpChange";
    public static readonly string INPUT_PORT_DAMAGE = "DamageChange";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort(INPUT_PORT_CHOICE_LABEL).Build();
        context.AddInputPort(INPUT_PORT_KARMA).Build();
        context.AddInputPort(INPUT_PORT_HP).Build();
        context.AddInputPort(INPUT_PORT_DAMAGE).Build();
        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }
}