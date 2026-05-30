using System;

[Serializable]
public class TextNode : GenericNode
{
    public static readonly string INPUT_PORT_TEXT = "Text";
    public static readonly string INPUT_PORT_WAIT_TIME = "WaitTime";
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort<string>(INPUT_PORT_TEXT).Build();
        context.AddInputPort<int>(INPUT_PORT_WAIT_TIME).Build();

        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }
}