using Assets.Scripts.Joaquin.Core;
using Assets.Scripts.Joaquin.Interfaces;
using UnityEngine;

public class SituationManager : MonoBehaviour, ICombatEventListener
{
    #region Inspector - Datos Del Juego

    [Header("Datos del juego")]
    [SerializeField] private SituationData[] situations;

    #endregion

    #region Inspector - Referencias De UI

    [Header("Referencias de UI")]
    [SerializeField] private GameObject panelSetup;
    [SerializeField] private GameObject panelSituation;
    [SerializeField] private SituationUI situationUI;
    [SerializeField] private CombatController combatController;

    #endregion

    #region Internal State

    private Player player;
    private int currentIndex = 0;
    private int karma = 0;
    private System.Action<CombatOutcome> pendingCombatCallback;

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        panelSituation.SetActive(false);
    }

    #endregion

    #region Initialization & State Reset

    public void InitPlayer(string name, int maxHealth, int damage)
    {
        player = new Player(name, maxHealth, damage);
        player.Inventory.AddItem(ItemFactory.CreateHealthPotion());
        player.OnPlayerDied += TriggerGameOver;

        panelSetup.SetActive(false);
        panelSituation.SetActive(true);

        karma = 0;
        LoadSituation(0);
    }

    private void ResetToStart()
    {
        panelSituation.SetActive(false);
        combatController.gameObject.SetActive(false);
        situationUI.HideGameOver();
        situationUI.HideEnding();

        panelSetup.SetActive(true);

        karma = 0;
        currentIndex = 0;
        player = null;
    }

    #endregion

    #region Situation Core Logic

    private void LoadSituation(int index)
    {
        if (index >= situations.Length) { TriggerEnding(); return; }

        currentIndex = index;
        var data = situations[index];

        if (data.type == SituationType.Combat)
        {
            panelSituation.SetActive(false);
            TriggerCombat(BuildEnemy(data.enemyData));
        }
        else
        {
            panelSituation.SetActive(true);
            situationUI.DisplaySituation(data, OnOptionSelected);
        }
    }

    private void OnOptionSelected(SituationOption option)
    {
        ApplyEffect(option.effect);
        karma += option.effect.karmaChange;
        if (!player.IsAlive) { TriggerGameOver(); return; }
        if (option.nextSituationIndex == -1) TriggerEnding();
        else LoadSituation(option.nextSituationIndex);
    }

    private void ApplyEffect(SituationEffect effect)
    {
        if (effect.hpChange > 0) player.Heal(effect.hpChange);
        else if (effect.hpChange < 0) player.TakeDamage(-effect.hpChange);

        if (effect.damageChange > 0) player.IncreaseDamage(effect.damageChange);

        if (effect.itemsGiven != null)
        {
            foreach (var itemData in effect.itemsGiven)
            {
                player.Inventory.AddItem(itemData.Build());
            }
        }
    }

    public void ApplyEffectFromGraph(ChoiceRuntimeNode choice)
    {
        if (choice.hpChange > 0) player.Heal(choice.hpChange);
        else if (choice.hpChange < 0) player.TakeDamage(-choice.hpChange);

        if (choice.damageChange > 0) player.IncreaseDamage(choice.damageChange);

        karma += choice.karmaChange;

        if (!player.IsAlive) TriggerGameOver();
    }

    #endregion

    #region Combat Management

    private void TriggerCombat(Enemy enemy)
    {
        combatController.gameObject.SetActive(true);
        combatController.StartCombat(player, enemy, this);
    }

    public void StartCombatFromGraph(CombatRuntimeNode node, System.Action<CombatOutcome> onFinished)
    {
        string enemyName = LocalizationManager.GetText(node.enemyNameES, node.enemyNameEN);
        var enemy = new Enemy(enemyName, node.enemyHP, node.enemyDamage);

        pendingCombatCallback = onFinished;
        TriggerCombat(enemy);
    }

    private Enemy BuildEnemy(EnemyData data)
    {
        string name = LocalizationManager.GetText(data.enemyNameES, data.enemyNameEN);
        var enemy = new Enemy(name, data.health, data.damage);
        foreach (var dropData in data.drops)
        {
            enemy.AddDrop(dropData.Build());
        }
        return enemy;
    }

    public void OnCombatEnded(CombatOutcome outcome)
    {
        combatController.gameObject.SetActive(false);

        pendingCombatCallback?.Invoke(outcome);
        pendingCombatCallback = null;

        switch (outcome)
        {
            case CombatOutcome.PlayerWon:
                LoadSituation(currentIndex + 1);
                break;
            case CombatOutcome.PlayerFled:
                karma -= 1;
                LoadSituation(currentIndex + 1);
                break;
            case CombatOutcome.PlayerLost:
                TriggerGameOver();
                break;
        }
    }

    #endregion

    #region Game Over & Endings

    private void TriggerEnding()
    {
        panelSituation.SetActive(false);
        EndingType ending = karma >= 5 ? EndingType.Good :
                            karma >= 0 ? EndingType.Neutral :
                                         EndingType.Bad;

        situationUI.DisplayEnding(ending, player, OnRestartFromEnding);
    }

    public void TriggerEndingFromGraph(EndingRuntimeNode node)
    {
        panelSituation.SetActive(false);
        situationUI.DisplayEnding(node.endingType, player, OnRestartFromEnding);
    }

    private void TriggerGameOver() => situationUI.DisplayGameOver(OnRetryFromGameOver);

    private void OnRetryFromGameOver()
    {
        ResetToStart();
    }

    public void OnRestartFromEnding()
    {
        ResetToStart();
    }

    #endregion
}