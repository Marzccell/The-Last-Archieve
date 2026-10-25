using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitInvestigationController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject quitConfirmationPanel;

    [Header("Menu")]
    [SerializeField] private GameObject hamburgerDropdown;

    [Header("Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isReturningToMenu;
    private DetectiveBoardController detectiveBoardController;

    private void Awake()
    {
        // Both controllers are on GameSystems in the Investigation scene.
        detectiveBoardController = GetComponent<DetectiveBoardController>();

        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    public void OpenQuitConfirmation()
    {
        if (isReturningToMenu)
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
        if (isReturningToMenu)
            return;

        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    public void ConfirmQuit()
    {
        if (isReturningToMenu ||
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

        isReturningToMenu = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
