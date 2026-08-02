using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BriefingController : MonoBehaviour
{
    [Header("Briefing Pages")]
    [SerializeField] private GameObject[] pages;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private TMP_Text progressText;

    private int currentPage;

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
        ShowPage(0);
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged -=
                HandleLanguageChanged;
        }
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            ShowPage(currentPage + 1);
        }
        else
        {
            SceneManager.LoadScene("Investigation");
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            ShowPage(currentPage - 1);
        }
    }

    private void ShowPage(int pageIndex)
    {
        currentPage = pageIndex;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        backButton.gameObject.SetActive(currentPage > 0);

        progressText.text =
            $"{currentPage + 1} / {pages.Length}";

        UpdateNextButtonText();
    }

    private void HandleLanguageChanged(GameLanguage language)
    {
        UpdateNextButtonText();
    }

    private void UpdateNextButtonText()
    {
        bool isIndonesian =
            LanguageManager.Instance != null &&
            LanguageManager.Instance.CurrentLanguage ==
            GameLanguage.Indonesian;

        bool isLastPage =
            currentPage == pages.Length - 1;

        if (isLastPage)
        {
            nextButtonText.text = isIndonesian
                ? "MULAI INVESTIGASI"
                : "START INVESTIGATION";
        }
        else
        {
            nextButtonText.text = isIndonesian
                ? "LANJUT"
                : "NEXT";
        }
    }
}