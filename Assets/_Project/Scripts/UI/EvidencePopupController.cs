using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidencePopupController : MonoBehaviour
{
    // FINAL BUILD: Four presentation themes, 11 August 2026.
    // If this exact text is missing in your editor, you opened the old file.
    public const string ControllerVersion =
        "2026.08.11-four-presentation-themes-final";

    private const float OfficeBodyFontSize = 28f;

    [Header("Popup Root")]
    [SerializeField] private GameObject evidencePopup;

    [Header("Presentation Skins")]
    [SerializeField] private GameObject museumDisplaySkin;
    [SerializeField] private GameObject securityConsoleSkin;
    [SerializeField] private GameObject officeWorkstationSkin;
    [SerializeField] private GameObject physicalDocumentSkin;

    [Header("Theme References")]
    [SerializeField] private Image evidenceCardImage;
    [SerializeField] private Image evidenceHeaderImage;
    [SerializeField] private Image evidenceTabBarImage;
    [SerializeField] private Image evidenceScrollViewImage;
    [SerializeField] private Image scrollbarTrackImage;
    [SerializeField] private Image scrollbarHandleImage;

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

    private Color defaultCardColor;
    private Color defaultHeaderColor;
    private Color defaultTabBarColor;
    private Color defaultScrollViewColor;
    private Color defaultScrollbarTrackColor;
    private Color defaultScrollbarHandleColor;

    private Color defaultTitleColor;
    private Color defaultMetaColor;
    private Color defaultBodyColor;
    private Color defaultCloseButtonColor;

    private float defaultBodyFontSize;
    private FontStyles defaultBodyFontStyle;

    private Color defaultActiveTabColor;
    private Color defaultInactiveTabColor;
    private Color defaultActiveTextColor;
    private Color defaultInactiveTextColor;

    private void Awake()
    {
        CacheDefaultTheme();
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
        bool showMuseumDisplay =
            presentationStyle ==
            EvidencePresentationStyle.MuseumDisplay;

        bool showSecurityConsole =
            presentationStyle ==
            EvidencePresentationStyle.SecurityConsole;

        bool showOfficeWorkstation =
            presentationStyle ==
            EvidencePresentationStyle.OfficeWorkstation;

        bool showPhysicalDocument =
            presentationStyle ==
            EvidencePresentationStyle.PhysicalDocument;

        SetSkinState(
            museumDisplaySkin,
            showMuseumDisplay,
            "Museum Display"
        );

        SetSkinState(
            securityConsoleSkin,
            showSecurityConsole,
            "Security Console"
        );

        SetSkinState(
            officeWorkstationSkin,
            showOfficeWorkstation,
            "Office Workstation"
        );

        SetSkinState(
            physicalDocumentSkin,
            showPhysicalDocument,
            "Physical Document"
        );

        switch (presentationStyle)
        {
            case EvidencePresentationStyle.SecurityConsole:
                ApplySecurityConsoleTheme();
                break;

            case EvidencePresentationStyle.OfficeWorkstation:
                ApplyOfficeWorkstationTheme();
                break;

            case EvidencePresentationStyle.PhysicalDocument:
                ApplyPhysicalDocumentTheme();
                break;

            default:
                RestoreDefaultTheme();
                break;
        }
    }

    private void SetSkinState(
        GameObject skin,
        bool shouldShow,
        string skinName
    )
    {
        if (skin != null)
        {
            skin.SetActive(shouldShow);
            return;
        }

        if (shouldShow)
        {
            Debug.LogWarning(
                $"EvidencePopupController: {skinName} Skin " +
                "belum dihubungkan di Inspector.",
                this
            );
        }
    }

    private void CacheDefaultTheme()
    {
        if (evidenceCardImage != null)
        {
            defaultCardColor = evidenceCardImage.color;
        }

        if (evidenceHeaderImage != null)
        {
            defaultHeaderColor = evidenceHeaderImage.color;
        }

        if (evidenceTabBarImage != null)
        {
            defaultTabBarColor = evidenceTabBarImage.color;
        }

        if (evidenceScrollViewImage != null)
        {
            defaultScrollViewColor =
                evidenceScrollViewImage.color;
        }

        if (scrollbarTrackImage != null)
        {
            defaultScrollbarTrackColor =
                scrollbarTrackImage.color;
        }

        if (scrollbarHandleImage != null)
        {
            defaultScrollbarHandleColor =
                scrollbarHandleImage.color;
        }

        if (evidenceTitleText != null)
        {
            defaultTitleColor = evidenceTitleText.color;
        }

        if (evidenceMetaText != null)
        {
            defaultMetaColor = evidenceMetaText.color;
        }

        if (evidenceBodyText != null)
        {
            defaultBodyColor = evidenceBodyText.color;
            defaultBodyFontSize = evidenceBodyText.fontSize;
            defaultBodyFontStyle = evidenceBodyText.fontStyle;
        }

        if (closeButton != null &&
            closeButton.image != null)
        {
            defaultCloseButtonColor =
                closeButton.image.color;
        }

        defaultActiveTabColor = activeTabColor;
        defaultInactiveTabColor = inactiveTabColor;
        defaultActiveTextColor = activeTextColor;
        defaultInactiveTextColor = inactiveTextColor;
    }

    private void ApplySecurityConsoleTheme()
    {
        SetImageColor(
            evidenceCardImage,
            new Color32(7, 20, 15, 255)
        );

        SetImageColor(
            evidenceHeaderImage,
            new Color32(11, 29, 23, 255)
        );

        SetImageColor(
            evidenceTabBarImage,
            new Color32(19, 39, 31, 230)
        );

        SetImageColor(
            evidenceScrollViewImage,
            new Color32(7, 17, 14, 235)
        );

        SetImageColor(
            scrollbarTrackImage,
            new Color32(19, 35, 29, 220)
        );

        SetImageColor(
            scrollbarHandleImage,
            new Color32(121, 205, 176, 255)
        );

        if (evidenceTitleText != null)
        {
            evidenceTitleText.color =
                new Color32(143, 216, 190, 255);
        }

        if (evidenceMetaText != null)
        {
            evidenceMetaText.color =
                new Color32(144, 175, 164, 255);
        }

        if (evidenceBodyText != null)
        {
            evidenceBodyText.color =
                new Color32(212, 227, 221, 255);

            evidenceBodyText.fontSize =
                defaultBodyFontSize;

            evidenceBodyText.fontStyle =
                defaultBodyFontStyle;
        }

        if (closeButton != null &&
            closeButton.image != null)
        {
            closeButton.image.color =
                new Color32(77, 13, 13, 255);
        }

        activeTabColor =
            new Color32(36, 95, 80, 255);

        inactiveTabColor =
            new Color32(15, 26, 22, 255);

        activeTextColor =
            new Color32(224, 246, 238, 255);

        inactiveTextColor =
            new Color32(146, 175, 165, 255);
    }

    private void ApplyOfficeWorkstationTheme()
    {
        SetImageColor(
            evidenceCardImage,
            new Color32(5, 24, 40, 255)
        );

        SetImageColor(
            evidenceHeaderImage,
            new Color32(8, 43, 70, 255)
        );

        SetImageColor(
            evidenceTabBarImage,
            new Color32(8, 30, 49, 255)
        );

        SetImageColor(
            evidenceScrollViewImage,
            new Color32(242, 244, 245, 255)
        );

        SetImageColor(
            scrollbarTrackImage,
            new Color32(210, 216, 220, 255)
        );

        SetImageColor(
            scrollbarHandleImage,
            new Color32(207, 165, 75, 255)
        );

        if (evidenceTitleText != null)
        {
            evidenceTitleText.color =
                new Color32(232, 244, 252, 255);
        }

        if (evidenceMetaText != null)
        {
            evidenceMetaText.color =
                new Color32(181, 207, 226, 255);
        }

        if (evidenceBodyText != null)
        {
            evidenceBodyText.color =
                new Color32(10, 15, 20, 255);

            evidenceBodyText.fontSize = OfficeBodyFontSize;
            evidenceBodyText.fontStyle = FontStyles.Bold;
        }

        if (closeButton != null &&
            closeButton.image != null)
        {
            closeButton.image.color =
                new Color32(77, 13, 13, 255);
        }

        activeTabColor =
            new Color32(11, 113, 151, 255);

        inactiveTabColor =
            new Color32(12, 28, 44, 255);

        activeTextColor = Color.white;

        inactiveTextColor =
            new Color32(198, 213, 223, 255);
    }

    private void ApplyPhysicalDocumentTheme()
    {
        SetImageColor(
            evidenceCardImage,
            new Color32(58, 34, 21, 255)
        );

        SetImageColor(
            evidenceHeaderImage,
            new Color32(73, 42, 25, 255)
        );

        SetImageColor(
            evidenceTabBarImage,
            new Color32(220, 202, 164, 255)
        );

        SetImageColor(
            evidenceScrollViewImage,
            new Color32(244, 236, 216, 255)
        );

        SetImageColor(
            scrollbarTrackImage,
            new Color32(188, 163, 122, 255)
        );

        SetImageColor(
            scrollbarHandleImage,
            new Color32(167, 117, 48, 255)
        );

        if (evidenceTitleText != null)
        {
            evidenceTitleText.color =
                new Color32(255, 244, 218, 255);
        }

        if (evidenceMetaText != null)
        {
            evidenceMetaText.color =
                new Color32(222, 193, 143, 255);
        }

        if (evidenceBodyText != null)
        {
            evidenceBodyText.color =
                new Color32(47, 32, 21, 255);

            evidenceBodyText.fontSize =
                defaultBodyFontSize;

            evidenceBodyText.fontStyle =
                defaultBodyFontStyle;
        }

        if (closeButton != null &&
            closeButton.image != null)
        {
            closeButton.image.color =
                new Color32(91, 31, 24, 255);
        }

        activeTabColor =
            new Color32(139, 93, 41, 255);

        inactiveTabColor =
            new Color32(211, 192, 154, 255);

        activeTextColor =
            new Color32(255, 247, 226, 255);

        inactiveTextColor =
            new Color32(72, 47, 29, 255);
    }

    private void RestoreDefaultTheme()
    {
        SetImageColor(
            evidenceCardImage,
            defaultCardColor
        );

        SetImageColor(
            evidenceHeaderImage,
            defaultHeaderColor
        );

        SetImageColor(
            evidenceTabBarImage,
            defaultTabBarColor
        );

        SetImageColor(
            evidenceScrollViewImage,
            defaultScrollViewColor
        );

        SetImageColor(
            scrollbarTrackImage,
            defaultScrollbarTrackColor
        );

        SetImageColor(
            scrollbarHandleImage,
            defaultScrollbarHandleColor
        );

        if (evidenceTitleText != null)
        {
            evidenceTitleText.color =
                defaultTitleColor;
        }

        if (evidenceMetaText != null)
        {
            evidenceMetaText.color =
                defaultMetaColor;
        }

        if (evidenceBodyText != null)
        {
            evidenceBodyText.color =
                defaultBodyColor;

            evidenceBodyText.fontSize =
                defaultBodyFontSize;

            evidenceBodyText.fontStyle =
                defaultBodyFontStyle;
        }

        if (closeButton != null &&
            closeButton.image != null)
        {
            closeButton.image.color =
                defaultCloseButtonColor;
        }

        activeTabColor = defaultActiveTabColor;
        inactiveTabColor = defaultInactiveTabColor;
        activeTextColor = defaultActiveTextColor;
        inactiveTextColor = defaultInactiveTextColor;
    }

    private void SetImageColor(
        Image targetImage,
        Color targetColor
    )
    {
        if (targetImage != null)
        {
            targetImage.color = targetColor;
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
