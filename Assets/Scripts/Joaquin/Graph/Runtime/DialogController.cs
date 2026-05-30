using Assets.Scripts.Joaquin.Core;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    #region Inspector - References

    [SerializeField] private SituationUI situationUI;
    [SerializeField] private GenericRuntimeGraph dialogGraph;
    [SerializeField] private SituationManager situationManager;

    #endregion

    #region Internal State

    private int currentIndex = 0;
    private TaskCompletionSource<ChoiceRuntimeNode> choiceTcs;
    private TaskCompletionSource<CombatOutcome> combatTcs;
    private TaskCompletionSource<bool> continueTcs;

    #endregion

    #region Public Properties & Events

    public SituationManager SituationManager => situationManager;

    #endregion

    #region Dialog Control & Flow

    public void DisplayText(string text)
    {
        situationUI.DisplayText(text);
    }

    public async void RunFrom(int index)
    {
        currentIndex = index;
        var node = dialogGraph.nodes[currentIndex];

        switch (node)
        {
            case TextRuntimeNode text:
                await new TextResolver().Resolve(this, text);
                ChoiceRuntimeNode[] choices = dialogGraph.GetChoicesAfter(currentIndex);
                ChoiceRuntimeNode selected = await PresentChoice(choices);
                situationManager.ApplyEffectFromGraph(selected);
                RunFrom(selected.nextNodeIndex);
                break;
            case CombatRuntimeNode combat:
                await new CombatResolver().Resolve(this, combat);
                break;
            case EndingRuntimeNode ending:
                situationManager.TriggerEndingFromGraph(ending);
                break;
        }
    }

    public void OnChoiceSelected(ChoiceRuntimeNode choice)
    {
        situationManager.ApplyEffectFromGraph(choice);
        RunFrom(choice.nextNodeIndex);
    }

    public async Task<ChoiceRuntimeNode> PresentChoice(ChoiceRuntimeNode[] choices)
    {
        choiceTcs = new TaskCompletionSource<ChoiceRuntimeNode>();
        situationUI.DisplayChoices(choices, OnChoiceButtonPressed);
        return await choiceTcs.Task;
    }

    public async Task<CombatOutcome> TriggerCombat(CombatRuntimeNode node)
    {
        combatTcs = new TaskCompletionSource<CombatOutcome>();

        situationManager.StartCombatFromGraph(node, OnCombatFinished);

        return await combatTcs.Task;
    }

    public async Task WaitForContinue()
    {
        continueTcs = new TaskCompletionSource<bool>();
        situationUI.ShowContinueButton(OnContinuePressed);
        await continueTcs.Task;
    }

    #endregion

    #region Event Callbacks

    private void OnCombatFinished(CombatOutcome outcome)
    {
        combatTcs?.SetResult(outcome);
    }

    private void OnChoiceButtonPressed(ChoiceRuntimeNode choice)
    {
        situationUI.ClearOptions();
        choiceTcs?.SetResult(choice);
    }

    private void OnContinuePressed()
    {
        situationUI.HideContinueButton();
        continueTcs?.SetResult(true);
    }

    #endregion
}