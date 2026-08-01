using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidencePopupController : MonoBehaviour
{
    [Header("Popup Root")]
    [SerializeField] private GameObject evidencePopup;

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
    private Color activeTabColor = new Color32(25, 102, 125, 255);

    [SerializeField]
    private Color inactiveTabColor = new Color32(18, 31, 40, 255);

    [SerializeField]
    private Color activeTextColor = Color.white;

    [SerializeField]
    private Color inactiveTextColor = new Color32(160, 178, 185, 255);

    [Header("Popup Controls")]
    [SerializeField] private ScrollRect bodyScrollRect;
    [SerializeField] private Button closeButton;

    [Header("Evidence System")]
    [SerializeField] private EvidenceManager evidenceManager;

    private EvidenceDocumentData document1;
    private EvidenceDocumentData document2;

    private string currentLocation = string.Empty;
    private int activeDocumentIndex;

    private void Awake()
    {
        AddButtonListeners();
    }

    private void Start()
    {
        CloseEvidence();
    }

    private void OnDestroy()
    {
        RemoveButtonListeners();
    }

    public void OpenEvidence(
        string location,
        EvidenceDocumentData firstDocument,
        EvidenceDocumentData secondDocument
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

        currentLocation = location ?? string.Empty;
        document1 = firstDocument;
        document2 = secondDocument;

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

    private void ShowDocument(int documentIndex)
    {
        EvidenceDocumentData selectedDocument =
            GetDocument(documentIndex);

        if (selectedDocument == null)
        {
            Debug.LogError(
                $"EvidencePopupController: Document {documentIndex + 1} " +
                "tidak tersedia.",
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
        if (evidenceTitleText != null)
        {
            evidenceTitleText.text =
                ToUpperSafe(selectedDocument.evidenceTitle);
        }

        if (evidenceMetaText != null)
        {
            string location = ToUpperSafe(currentLocation);
            string evidenceType =
                ToUpperSafe(selectedDocument.evidenceType);

            evidenceMetaText.text =
                $"{location}  •  {evidenceType}";
        }

        if (evidenceBodyText != null)
        {
            evidenceBodyText.text =
                selectedDocument.evidenceBody ?? string.Empty;
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

        if (string.IsNullOrWhiteSpace(selectedDocument.evidenceID))
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
            activeDocumentIndex == 0
        );

        UpdateSingleTab(
            tab2Button,
            tab2Text,
            document2,
            "DOCUMENT 2",
            activeDocumentIndex == 1
        );
    }

    private void UpdateSingleTab(
        Button tabButton,
        TMP_Text tabText,
        EvidenceDocumentData document,
        string fallbackName,
        bool isActive
    )
    {
        if (tabText != null)
        {
            string tabName = GetTabName(
                document,
                fallbackName
            );

            bool hasBeenRead = HasDocumentBeenRead(document);

            tabText.text = hasBeenRead
                ? tabName
                : $"{tabName}\nUNREAD";
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
        string fallbackName
    )
    {
        if (document == null ||
            string.IsNullOrWhiteSpace(document.tabName))
        {
            return fallbackName;
        }

        return document.tabName.Trim().ToUpperInvariant();
    }

    private void ApplyTabStyle(
        Button tabButton,
        TMP_Text tabText,
        bool isActive
    )
    {
        if (tabButton != null && tabButton.image != null)
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