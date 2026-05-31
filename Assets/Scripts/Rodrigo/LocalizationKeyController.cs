using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class LocalizationKeyController : MonoBehaviour
{
    [Header("Texto que quiero cambiar")]
    [SerializeField] private GameObject targetTextObject;

    [Header("Nombre de la tabla")]
    [SerializeField] private string tableName = "tablas";

    [Header("Keys disponibles para este texto")]
    [SerializeField] private List<string> keys = new List<string>();

    public static int localizationIndex = 0;

    private LocalizeStringEvent localizeStringEvent;

    private void Awake()
    {
        if (targetTextObject == null)
        {
            Debug.LogError("No asignaste el GameObject del texto.");
            return;
        }

        localizeStringEvent = targetTextObject.GetComponent<LocalizeStringEvent>();

        if (localizeStringEvent == null)
        {
            Debug.LogError("El GameObject no tiene LocalizeStringEvent.");
            return;
        }

        ApplyLocalization();
    }

    public void AddOne()
    {
        localizationIndex++;
        ApplyLocalization();
    }

    public void ResetIndex()
    {
        localizationIndex = 0;
        ApplyLocalization();
    }

    public void ApplyLocalization()
    {
        if (localizeStringEvent == null) return;

        if (keys == null || keys.Count == 0)
        {
            Debug.LogWarning("La lista de keys está vacía.");
            return;
        }

        int listIndex = GetListIndexFromStaticValue();
        if (listIndex >= keys.Count)
        {
            listIndex = keys.Count - 1;
        }
        if (listIndex < 0)
        {
            listIndex = 0;
        }

        string key = keys[listIndex];

        localizeStringEvent.StringReference.SetReference(tableName, key);
    }

    private int GetListIndexFromStaticValue()
    {
        switch (localizationIndex)
        {
            case 19:
                return 0;

            case 29:
                return 1;

            case 39:
                return 2;

            default:
                return localizationIndex;
        }
    }
}
