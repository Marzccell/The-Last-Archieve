using UnityEngine;

public class QuitInvestigationController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject quitConfirmationPanel;

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
        if (isQuitting)
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

        if (!detectiveBoardController.RecordQuit())
            return;

        isQuitting = true;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}