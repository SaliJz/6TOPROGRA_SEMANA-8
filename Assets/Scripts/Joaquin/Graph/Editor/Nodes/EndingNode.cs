using System;

[Serializable]
public class EndingNode : GenericNode
{
    public static readonly string INPUT_PORT_ENDING_TYPE = "EndingType";
    public static readonly string INPUT_PORT_TEXT = "EndingText";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort(INPUT_PORT_ENDING_TYPE).Build();
        context.AddInputPort(INPUT_PORT_TEXT).Build();
    }
}