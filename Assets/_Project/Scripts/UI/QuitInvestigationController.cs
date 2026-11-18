using UnityEngine;
using UnityEngine.EventSystems;

public class QuitInvestigationController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject quitConfirmationPanel;
    [SerializeField] private GameObject quitResultPanel;

    [Header("Menu")]
    [SerializeField] private GameObject hamburgerDropdown;

    private bool isQuitting;
    private DetectiveBoardController detectiveBoardController;

    private void Awake()
    {
        detectiveBoardController =
            GetComponent<DetectiveBoardController>();

        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    public void OpenQuitConfirmation()
    {
        if (isQuitting || (detectiveBoardController != null && detectiveBoardController.HasFinalScore))
            return;

        if (quitConfirmationPanel == null)
        {
            Debug.LogError(
                "Quit Confirmation Panel belum dihubungkan.",
                this
            );
            return;
        }

        if (hamburgerDropdown != null)
        {
            hamburgerDropdown.SetActive(false);
        }

        quitConfirmationPanel.transform.SetAsLastSibling();
        quitConfirmationPanel.SetActive(true);
    }

    public void CancelQuit()
    {
        if (isQuitting)
            return;

        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    public void ConfirmQuit()
    {
        if (isQuitting ||
            quitConfirmationPanel == null ||
            !quitConfirmationPanel.activeSelf)
        {
            return;
        }

        if (detectiveBoardController == null)
        {
            Debug.LogError(
                "QuitInvestigationController: DetectiveBoardController " +
                "harus berada pada GameObject yang sama agar poin Quit tercatat.",
                this
            );
            return;
        }

        if (quitResultPanel == null)
        {
            Debug.LogError("Quit Result Panel belum dihubungkan.", this);
            return;
        }

        if (!detectiveBoardController.RecordQuit())
            return;

        isQuitting = true;
        quitConfirmationPanel.SetActive(false);
        quitResultPanel.transform.SetAsLastSibling();
        quitResultPanel.SetActive(true);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }
}
