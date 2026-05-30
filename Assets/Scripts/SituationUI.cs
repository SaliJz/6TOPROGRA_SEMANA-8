using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SituationUI : MonoBehaviour
{
    #region Inspector - Situation UI

    [SerializeField] private TextMeshProUGUI txtSituationText;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private GameObject optionButtonPrefab;

    #endregion

    #region Inspector - Game Over UI

    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private Button btnRetry;

    #endregion

    #region Inspector - Ending UI

    [SerializeField] private GameObject panelEnding;
    [SerializeField] private TextMeshProUGUI txtEndingTitle;
    [SerializeField] private TextMeshProUGUI txtEndingMessage;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnContinue;

    #endregion

    #region Core UI Logic

    public void DisplaySituation(SituationData data, System.Action<SituationOption> onSelect)
    {
        txtSituationText.text = LocalizationManager.GetText(data.descriptionES, data.descriptionEN);
        ClearOptions();
        foreach (var opt in data.options)
        {
            var captured = opt;
            var go = Instantiate(optionButtonPrefab, optionsContainer);
            go.GetComponentInChildren<TextMeshProUGUI>().text =
                LocalizationManager.GetText(opt.labelES, opt.labelEN);
            go.GetComponent<Button>().onClick.AddListener(() => onSelect(captured));
        }
    }

    public void DisplayChoices(ChoiceRuntimeNode[] choices, System.Action<ChoiceRuntimeNode> onSelect)
    {
        ClearOptions();
        foreach (var choice in choices)
        {
            var captured = choice;
            var go = Instantiate(optionButtonPrefab, optionsContainer);
            go.GetComponentInChildren<TextMeshProUGUI>().text =
                LocalizationManager.GetText(choice.labelES, choice.labelEN);
            go.GetComponent<Button>().onClick.AddListener(() => onSelect(captured));
        }
    }

    public void ClearOptions()
    {
        foreach (Transform child in optionsContainer) Destroy(child.gameObject);
    }

    public void DisplayText(string text) => txtSituationText.text = text;

    #endregion

    #region Game Over & Ending Flows

    public void DisplayGameOver(System.Action onRetry)
    {
        panelGameOver.SetActive(true);
        btnRetry.onClick.RemoveAllListeners();
        btnRetry.onClick.AddListener(() =>
        {
            panelGameOver.SetActive(false);
            onRetry();
        });
    }

    public void HideGameOver() => panelGameOver.SetActive(false);

    public void DisplayEnding(EndingType type, Player player, System.Action onRestart)
    {
        panelEnding.SetActive(true);

        switch (type)
        {
            case EndingType.Good:
                txtEndingTitle.text = LocalizationManager.GetText("Final Bueno", "Good Ending");
                txtEndingMessage.text = LocalizationManager.GetText(
                    $"El oraculo te reconoce, {player.PlayerName}. Compartes su sabiduria con el mundo.",
                    $"The oracle recognizes you, {player.PlayerName}. You share its wisdom with the world.");
                break;
            case EndingType.Neutral:
                txtEndingTitle.text = LocalizationManager.GetText("Final Neutral", "Neutral Ending");
                txtEndingMessage.text = LocalizationManager.GetText(
                    $"El oraculo guarda silencio, {player.PlayerName}. Regresas con las manos vacias.",
                    $"The oracle stays silent, {player.PlayerName}. You return empty-handed.");
                break;
            case EndingType.Bad:
                txtEndingTitle.text = LocalizationManager.GetText("Final Malo", "Bad Ending");
                txtEndingMessage.text = LocalizationManager.GetText(
                    $"El oraculo te maldice, {player.PlayerName}. Las ruinas colapsan a tu alrededor.",
                    $"The oracle curses you, {player.PlayerName}. The ruins collapse around you.");
                break;
        }

        btnRestart.onClick.RemoveAllListeners();
        btnRestart.onClick.AddListener(() => onRestart());
    }

    public void HideEnding() => panelEnding.SetActive(false);

    public void ShowContinueButton(System.Action onContinue)
    {
        btnContinue.gameObject.SetActive(true);
        btnContinue.onClick.RemoveAllListeners();
        btnContinue.onClick.AddListener(() => onContinue());
    }

    public void HideContinueButton()
    {
        btnContinue.gameObject.SetActive(false);
    }

    #endregion
}