using UnityEngine;
using UnityEngine.UI;

public class CaseBriefPanelController : MonoBehaviour
{
    [Header("Case Brief References")]
    [SerializeField] private GameObject caseBriefPanel;
    [SerializeField] private ScrollRect caseBriefScrollRect;

    private void Start()
    {
        caseBriefPanel.SetActive(false);
    }

    public void OpenCaseBrief()
    {
        caseBriefPanel.SetActive(true);

        // Memastikan panel muncul di depan UI lain.
        caseBriefPanel.transform.SetAsLastSibling();

        // Memastikan layout selesai dihitung sebelum mengatur scroll.
        Canvas.ForceUpdateCanvases();

        // Mengembalikan posisi scroll ke paling atas.
        if (caseBriefScrollRect != null)
        {
            caseBriefScrollRect.verticalNormalizedPosition = 1f;
        }
    }

    public void CloseCaseBrief()
    {
        caseBriefPanel.SetActive(false);
    }
}