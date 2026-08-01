using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EvidenceDocumentData
{
    [Header("Document Identity")]
    public string evidenceID;

    [Header("Document Content")]
    public string tabName;
    public string evidenceTitle;
    public string evidenceType;

    [TextArea(10, 30)]
    public string evidenceBody;
}

[RequireComponent(typeof(Button))]
public class HotspotController : MonoBehaviour
{
    [Header("Hotspot Evidence")]
    [SerializeField] private string evidenceLocation;

    [SerializeField] private EvidenceDocumentData document1 =
        new EvidenceDocumentData();

    [SerializeField] private EvidenceDocumentData document2 =
        new EvidenceDocumentData();

    [Header("Hotspot Label")]
    [SerializeField] private string hotspotName;
    [SerializeField] private string hotspotContentSummary;

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

    private void HandleHotspotClicked()
    {
        if (!ValidateHotspot())
        {
            return;
        }

        popupController.OpenEvidence(
            evidenceLocation,
            document1,
            document2
        );
    }

    private void UpdateHotspotLabel()
    {
        if (hotspotNameText != null)
        {
            hotspotNameText.text = hotspotName;
        }

        if (hotspotCountText != null)
        {
            hotspotCountText.text =
                hotspotContentSummary.ToUpper();
        }
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
