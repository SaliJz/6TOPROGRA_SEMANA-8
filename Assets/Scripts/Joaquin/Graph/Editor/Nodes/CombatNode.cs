using System;

[Serializable]
public class CombatNode : GenericNode
{
    public static readonly string INPUT_PORT_ENEMY_NAME = "EnemyName";
    public static readonly string INPUT_PORT_ENEMY_HP = "EnemyHP";
    public static readonly string INPUT_PORT_ENEMY_DAMAGE = "EnemyDamage";
    public static readonly string OUTPUT_PORT_WIN = "OnWin";
    public static readonly string OUTPUT_PORT_LOSE = "OnLose";
    public static readonly string OUTPUT_PORT_FLEE = "OnFlee";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort(INPUT_PORT_ENEMY_NAME).Build();
        context.AddInputPort(INPUT_PORT_ENEMY_HP).Build();
        context.AddInputPort(INPUT_PORT_ENEMY_DAMAGE).Build();
        context.AddOutputPort(OUTPUT_PORT_WIN).Build();
        context.AddOutputPort(OUTPUT_PORT_LOSE).Build();
        context.AddOutputPort(OUTPUT_PORT_FLEE).Build();
    }
}