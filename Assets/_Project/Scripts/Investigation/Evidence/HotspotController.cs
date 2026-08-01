using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EvidenceDocumentData
{
    [Header("Document Identity")]
    public string evidenceID;

    [Header("English Content")]
    public string tabName;
    public string evidenceTitle;
    public string evidenceType;

    [TextArea(10, 30)]
    public string evidenceBody;

    [Header("Indonesian Content")]
    public string tabNameIndonesian;
    public string evidenceTitleIndonesian;
    public string evidenceTypeIndonesian;

    [TextArea(10, 30)]
    public string evidenceBodyIndonesian;

    public string GetTabName(GameLanguage language)
    {
        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(tabNameIndonesian))
        {
            return tabNameIndonesian;
        }

        return tabName;
    }

    public string GetTitle(GameLanguage language)
    {
        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(evidenceTitleIndonesian))
        {
            return evidenceTitleIndonesian;
        }

        return evidenceTitle;
    }

    public string GetType(GameLanguage language)
    {
        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(evidenceTypeIndonesian))
        {
            return evidenceTypeIndonesian;
        }

        return evidenceType;
    }

    public string GetBody(GameLanguage language)
    {
        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(evidenceBodyIndonesian))
        {
            return evidenceBodyIndonesian;
        }

        return evidenceBody;
    }
}

[RequireComponent(typeof(Button))]
public class HotspotController : MonoBehaviour
{
    [Header("Evidence Location")]
    [SerializeField] private string evidenceLocation;

    [SerializeField]
    private string evidenceLocationIndonesian;

    [Header("Evidence Documents")]
    [SerializeField]
    private EvidenceDocumentData document1 =
        new EvidenceDocumentData();

    [SerializeField]
    private EvidenceDocumentData document2 =
        new EvidenceDocumentData();

    [Header("Hotspot Label - English")]
    [SerializeField] private string hotspotName;
    [SerializeField] private string hotspotContentSummary;

    [Header("Hotspot Label - Indonesian")]
    [SerializeField] private string hotspotNameIndonesian;

    [SerializeField]
    private string hotspotContentSummaryIndonesian;

    [Header("Hotspot Label References")]
    [SerializeField] private TMP_Text hotspotNameText;
    [SerializeField] private TMP_Text hotspotCountText;

    [Header("System Reference")]
    [SerializeField]
    private EvidencePopupController popupController;

    private Button hotspotButton;

    private void Awake()
    {
        hotspotButton = GetComponent<Button>();

        hotspotButton.onClick.AddListener(
            HandleHotspotClicked
        );

        UpdateHotspotLabel();
    }

    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged +=
                HandleLanguageChanged;
        }

        UpdateHotspotLabel();
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
        if (hotspotButton != null)
        {
            hotspotButton.onClick.RemoveListener(
                HandleHotspotClicked
            );
        }
    }

    private void OnValidate()
    {
        UpdateHotspotLabel();
    }

    private void HandleLanguageChanged(GameLanguage language)
    {
        UpdateHotspotLabel();
    }

    private void HandleHotspotClicked()
    {
        if (!ValidateHotspot())
        {
            return;
        }

        popupController.OpenEvidence(
            evidenceLocation,
            evidenceLocationIndonesian,
            document1,
            document2
        );
    }

    private void UpdateHotspotLabel()
    {
        GameLanguage language = GetCurrentLanguage();

        if (hotspotNameText != null)
        {
            hotspotNameText.text =
                GetLocalizedText(
                    hotspotName,
                    hotspotNameIndonesian,
                    language
                );
        }

        if (hotspotCountText != null)
        {
            string summary = GetLocalizedText(
                hotspotContentSummary,
                hotspotContentSummaryIndonesian,
                language
            );

            hotspotCountText.text =
                string.IsNullOrWhiteSpace(summary)
                    ? string.Empty
                    : summary.ToUpperInvariant();
        }
    }

    private GameLanguage GetCurrentLanguage()
    {
        if (LanguageManager.Instance == null)
        {
            return GameLanguage.English;
        }

        return LanguageManager.Instance.CurrentLanguage;
    }

    private string GetLocalizedText(
        string englishText,
        string indonesianText,
        GameLanguage language
    )
    {
        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(indonesianText))
        {
            return indonesianText;
        }

        return englishText ?? string.Empty;
    }

    private bool ValidateHotspot()
    {
        if (popupController == null)
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                "Popup Controller belum dihubungkan.",
                this
            );

            return false;
        }

        if (string.IsNullOrWhiteSpace(evidenceLocation))
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                "Evidence Location masih kosong.",
                this
            );

            return false;
        }

        if (!ValidateDocument(document1, "Document 1"))
        {
            return false;
        }

        if (!ValidateDocument(document2, "Document 2"))
        {
            return false;
        }

        if (document1.evidenceID == document2.evidenceID)
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                "Document 1 dan Document 2 tidak boleh " +
                "memiliki Evidence ID yang sama.",
                this
            );

            return false;
        }

        return true;
    }

    private bool ValidateDocument(
        EvidenceDocumentData document,
        string documentName
    )
    {
        if (document == null)
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                $"{documentName} belum dibuat.",
                this
            );

            return false;
        }

        if (string.IsNullOrWhiteSpace(document.evidenceID))
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                $"{documentName} Evidence ID masih kosong.",
                this
            );

            return false;
        }

        if (string.IsNullOrWhiteSpace(document.evidenceTitle))
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                $"{documentName} Evidence Title masih kosong.",
                this
            );

            return false;
        }

        if (string.IsNullOrWhiteSpace(document.evidenceBody))
        {
            Debug.LogError(
                $"Hotspot '{gameObject.name}': " +
                $"{documentName} Evidence Body masih kosong.",
                this
            );

            return false;
        }

        return true;
    }
}