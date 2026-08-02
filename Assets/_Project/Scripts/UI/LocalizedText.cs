using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [Header("Localized Content")]
    [TextArea(1, 10)]
    [SerializeField] private string englishText;

    [TextArea(1, 10)]
    [SerializeField] private string indonesianText;

    private TMP_Text targetText;

    private void Awake()
    {
        targetText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged +=
                HandleLanguageChanged;
        }

        RefreshText();
    }

    private void Start()
    {
        RefreshText();
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged -=
                HandleLanguageChanged;
        }
    }

    private void HandleLanguageChanged(GameLanguage language)
    {
        RefreshText();
    }

    public void RefreshText()
    {
        if (targetText == null)
        {
            targetText = GetComponent<TMP_Text>();
        }

        if (targetText == null)
        {
            return;
        }

        GameLanguage language =
            LanguageManager.Instance != null
                ? LanguageManager.Instance.CurrentLanguage
                : GameLanguage.English;

        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(indonesianText))
        {
            targetText.text = indonesianText;
        }
        else
        {
            targetText.text = englishText;
        }
    }
}