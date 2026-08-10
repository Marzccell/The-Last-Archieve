using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidencePopupController : MonoBehaviour
{
    [Header("Popup Root")]
    [SerializeField] private GameObject evidencePopup;

    [Header("Presentation Skins")]
    [SerializeField] private GameObject museumDisplaySkin;

    [Header("Popup Text")]
    [SerializeField] private TMP_Text evidenceTitleText;
    [SerializeField] private TMP_Text evidenceMetaText;
    [SerializeField] private TMP_Text evidenceBodyText;

    [Header("Document Tabs")]
    [SerializeField] private Button tab1Button;
    [SerializeField] private Button tab2Button;
    [SerializeField] private TMP_Text tab1Text;
    [SerializeField] private TMP_Text tab2Text;

    [Header("Tab Appearance")]
    [SerializeField]
    private Color activeTabColor =
        new Color32(25, 102, 125, 255);

    [SerializeField]
    private Color inactiveTabColor =
        new Color32(18, 31, 40, 255);

    [SerializeField]
    private Color activeTextColor = Color.white;

    [SerializeField]
    private Color inactiveTextColor =
        new Color32(160, 178, 185, 255);

    [Header("Popup Controls")]
    [SerializeField] private ScrollRect bodyScrollRect;
    [SerializeField] private Button closeButton;

    [Header("Evidence System")]
    [SerializeField] private EvidenceManager evidenceManager;

    private EvidenceDocumentData document1;
    private EvidenceDocumentData document2;

    private string currentLocationEnglish = string.Empty;
    private string currentLocationIndonesian = string.Empty;

    private int activeDocumentIndex;

    private void Awake()
    {
        AddButtonListeners();
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
        CloseEvidence();
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
        RemoveButtonListeners();
    }

    public void OpenEvidence(
        string locationEnglish,
        string locationIndonesian,
        EvidenceDocumentData firstDocument,
        EvidenceDocumentData secondDocument,
        EvidencePresentationStyle presentationStyle
    )
    {
        if (evidencePopup == null)
        {
            Debug.LogError(
                "EvidencePopupController: Evidence Popup belum dihubungkan.",
                this
            );

            return;
        }

        if (firstDocument == null || secondDocument == null)
        {
            Debug.LogError(
                "EvidencePopupController: Kedua document harus dihubungkan.",
                this
            );

            return;
        }

        currentLocationEnglish =
            locationEnglish ?? string.Empty;

        currentLocationIndonesian =
            locationIndonesian ?? string.Empty;

        document1 = firstDocument;
        document2 = secondDocument;

        ApplyPresentationStyle(presentationStyle);

        evidencePopup.SetActive(true);
        ShowDocument(0);
    }

    public void CloseEvidence()
    {
        if (evidencePopup != null)
        {
            evidencePopup.SetActive(false);
        }
    }

    private void ApplyPresentationStyle(
        EvidencePresentationStyle presentationStyle
    )
    {
        if (museumDisplaySkin != null)
        {
            bool shouldShowMuseumDisplay =
                presentationStyle ==
                EvidencePresentationStyle.MuseumDisplay;

            museumDisplaySkin.SetActive(
                shouldShowMuseumDisplay
            );
        }
        else if (
            presentationStyle ==
            EvidencePresentationStyle.MuseumDisplay
        )
        {
            Debug.LogWarning(
                "EvidencePopupController: Museum Display Skin " +
                "belum dihubungkan di Inspector.",
                this
            );
        }
    }

    private void AddButtonListeners()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseEvidence);
        }

        if (tab1Button != null)
        {
            tab1Button.onClick.AddListener(OpenFirstDocument);
        }

        if (tab2Button != null)
        {
            tab2Button.onClick.AddListener(OpenSecondDocument);
        }
    }

    private void RemoveButtonListeners()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CloseEvidence);
        }

        if (tab1Button != null)
        {
            tab1Button.onClick.RemoveListener(OpenFirstDocument);
        }

        if (tab2Button != null)
        {
            tab2Button.onClick.RemoveListener(OpenSecondDocument);
        }
    }

    private void OpenFirstDocument()
    {
        ShowDocument(0);
    }

    private void OpenSecondDocument()
    {
        ShowDocument(1);
    }

    private void HandleLanguageChanged(GameLanguage language)
    {
        if (document1 == null || document2 == null)
        {
            return;
        }

        EvidenceDocumentData selectedDocument =
            GetDocument(activeDocumentIndex);

        if (selectedDocument == null)
        {
            return;
        }

        UpdateDocumentContent(selectedDocument);
        UpdateTabAppearance();
        ResetScrollToTop();
    }

    private void ShowDocument(int documentIndex)
    {
        EvidenceDocumentData selectedDocument =
            GetDocument(documentIndex);

        if (selectedDocument == null)
        {
            Debug.LogError(
                $"EvidencePopupController: Document " +
                $"{documentIndex + 1} tidak tersedia.",
                this
            );

            return;
        }

        activeDocumentIndex = documentIndex;

        UpdateDocumentContent(selectedDocument);
        RegisterDocument(selectedDocument);
        UpdateTabAppearance();
        ResetScrollToTop();
    }

    private EvidenceDocumentData GetDocument(int documentIndex)
    {
        return documentIndex == 0 ? document1 : document2;
    }

    private void UpdateDocumentContent(
        EvidenceDocumentData selectedDocument
    )
    {
        GameLanguage language = GetCurrentLanguage();

        if (evidenceTitleText != null)
        {
            evidenceTitleText.text =
                ToUpperSafe(
                    selectedDocument.GetTitle(language)
                );
        }

        if (evidenceMetaText != null)
        {
            string location = ToUpperSafe(
                GetCurrentLocation(language)
            );

            string evidenceType = ToUpperSafe(
                selectedDocument.GetType(language)
            );

            evidenceMetaText.text =
                $"{location}  •  {evidenceType}";
        }

        if (evidenceBodyText != null)
        {
            evidenceBodyText.text =
                selectedDocument.GetBody(language)
                ?? string.Empty;
        }
    }

    private void RegisterDocument(
        EvidenceDocumentData selectedDocument
    )
    {
        if (evidenceManager == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(
            selectedDocument.evidenceID))
        {
            Debug.LogWarning(
                "EvidencePopupController: Evidence ID masih kosong.",
                this
            );

            return;
        }

        evidenceManager.RegisterEvidence(
            selectedDocument.evidenceID
        );
    }

    private void UpdateTabAppearance()
    {
        UpdateSingleTab(
            tab1Button,
            tab1Text,
            document1,
            "DOCUMENT 1",
            "DOKUMEN 1",
            activeDocumentIndex == 0
        );

        UpdateSingleTab(
            tab2Button,
            tab2Text,
            document2,
            "DOCUMENT 2",
            "DOKUMEN 2",
            activeDocumentIndex == 1
        );
    }

    private void UpdateSingleTab(
        Button tabButton,
        TMP_Text tabText,
        EvidenceDocumentData document,
        string englishFallback,
        string indonesianFallback,
        bool isActive
    )
    {
        if (tabText != null)
        {
            GameLanguage language = GetCurrentLanguage();

            string fallbackName =
                language == GameLanguage.Indonesian
                    ? indonesianFallback
                    : englishFallback;

            string tabName = GetTabName(
                document,
                fallbackName,
                language
            );

            bool hasBeenRead =
                HasDocumentBeenRead(document);

            string unreadText =
                language == GameLanguage.Indonesian
                    ? "BELUM DIBACA"
                    : "UNREAD";

            tabText.text = hasBeenRead
                ? tabName
                : $"{tabName}\n{unreadText}";
        }

        ApplyTabStyle(tabButton, tabText, isActive);
    }

    private bool HasDocumentBeenRead(
        EvidenceDocumentData document
    )
    {
        if (evidenceManager == null ||
            document == null ||
            string.IsNullOrWhiteSpace(document.evidenceID))
        {
            return false;
        }

        return evidenceManager.HasDiscoveredEvidence(
            document.evidenceID
        );
    }

    private string GetTabName(
        EvidenceDocumentData document,
        string fallbackName,
        GameLanguage language
    )
    {
        if (document == null)
        {
            return fallbackName;
        }

        string tabName = document.GetTabName(language);

        return string.IsNullOrWhiteSpace(tabName)
            ? fallbackName
            : tabName.Trim().ToUpperInvariant();
    }

    private string GetCurrentLocation(
        GameLanguage language
    )
    {
        if (language == GameLanguage.Indonesian &&
            !string.IsNullOrWhiteSpace(
                currentLocationIndonesian))
        {
            return currentLocationIndonesian;
        }

        return currentLocationEnglish;
    }

    private GameLanguage GetCurrentLanguage()
    {
        if (LanguageManager.Instance == null)
        {
            return GameLanguage.English;
        }

        return LanguageManager.Instance.CurrentLanguage;
    }

    private void ApplyTabStyle(
        Button tabButton,
        TMP_Text tabText,
        bool isActive
    )
    {
        if (tabButton != null &&
            tabButton.image != null)
        {
            tabButton.image.color = isActive
                ? activeTabColor
                : inactiveTabColor;
        }

        if (tabText != null)
        {
            tabText.color = isActive
                ? activeTextColor
                : inactiveTextColor;
        }
    }

    private void ResetScrollToTop()
    {
        if (bodyScrollRect == null)
        {
            return;
        }

        Canvas.ForceUpdateCanvases();
        bodyScrollRect.verticalNormalizedPosition = 1f;
    }

    private string ToUpperSafe(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().ToUpperInvariant();
    }
}