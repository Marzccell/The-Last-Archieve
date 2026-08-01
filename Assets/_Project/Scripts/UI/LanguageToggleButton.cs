using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LanguageToggleButton : MonoBehaviour
{
    [SerializeField] private TMP_Text languageLabel;

    private Button languageButton;

    private void Awake()
    {
        languageButton = GetComponent<Button>();
        languageButton.onClick.AddListener(ToggleLanguage);
    }

    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged +=
                HandleLanguageChanged;
        }
    }

    private void Start()
    {
        UpdateLabel();
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged -=
                HandleLanguageChanged;
        }
    }

    private void OnDestroy()
    {
        if (languageButton != null)
        {
            languageButton.onClick.RemoveListener(ToggleLanguage);
        }
    }

    private void ToggleLanguage()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.ToggleLanguage();
        }
    }

    private void HandleLanguageChanged(GameLanguage language)
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (languageLabel == null ||
            LanguageManager.Instance == null)
        {
            return;
        }

        languageLabel.text =
            LanguageManager.Instance.CurrentLanguage ==
            GameLanguage.English
                ? "EN"
                : "ID";
    }
}