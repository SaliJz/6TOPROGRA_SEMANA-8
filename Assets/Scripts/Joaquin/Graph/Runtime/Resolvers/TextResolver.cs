using System.Threading.Tasks;

public class TextResolver : INodeResolver<TextRuntimeNode>
{
    public async Task Resolve(DialogController controller, TextRuntimeNode node)
    {
        controller.DisplayText(node.text);

        if (node.waitTime > 0) await Task.Delay(node.waitTime);
        else await controller.WaitForContinue();
    }
}