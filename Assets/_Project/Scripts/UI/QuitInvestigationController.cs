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

    private void Awake()
    {
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

        isReturningToMenu = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}