using Unity.GraphToolkit.Editor;
using System;

[Serializable]
public class StartNode : GenericNode
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }

}