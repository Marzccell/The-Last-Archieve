using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectiveBoardController : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private GameObject detectiveBoardPanel;
    [SerializeField] private GameObject hamburgerDropdown;

    [Header("Suspect Options")]
    [SerializeField] private DetectiveOptionCard[] suspectCards;

    [Header("Location Options")]
    [SerializeField] private DetectiveOptionCard[] locationCards;

    [Header("Footer")]
    [SerializeField] private TMP_Text selectionSummary;
    [SerializeField] private Button reviewSubmissionButton;

    [Header("Confirmation Popup")]
    [SerializeField] private GameObject submissionConfirmationPanel;
    [SerializeField] private TMP_Text suspectValue;
    [SerializeField] private TMP_Text locationValue;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmSubmissionButton;

    [Header("Attempts")]
    [SerializeField] private TMP_Text attemptsRemainingText;

    [Header("Retry Feedback")]
    [SerializeField] private GameObject retryFeedbackPanel;
    [SerializeField] private Button continueInvestigationButton;

    [Header("Correct Answer")]
    [SerializeField] private string correctSuspectID = "olivia";
    [SerializeField] private string correctLocationID = "c07";

    [Header("Result Panels")]
    [SerializeField] private GameObject successResultPanel;
    [SerializeField] private GameObject incorrectResultPanel;

    [Header("Incorrect Result Values")]
    [SerializeField] private TMP_Text submittedSuspectText;
    [SerializeField] private TMP_Text submittedLocationText;

    [Header("Result Buttons")]
    [SerializeField] private Button successReturnToMenuButton;
    [SerializeField] private Button incorrectReturnToMenuButton;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Buttons")]
    [SerializeField] private Button closeBoardButton;

    private DetectiveOptionCard selectedSuspect;
    private DetectiveOptionCard selectedLocation;

    private const int MaximumAttempts = 2;

    private int attemptsUsed;
    private bool awaitingRetry;
    private bool submissionInProgress;
    private bool submissionLocked;

    public int AttemptsUsed => attemptsUsed;
    public int AttemptsRemaining =>
        Mathf.Max(0, MaximumAttempts - attemptsUsed);

    private bool CanSubmit =>
        !submissionLocked &&
        !awaitingRetry &&
        !submissionInProgress &&
        AttemptsRemaining > 0;

    private bool IsIndonesian =>
        LanguageManager.Instance != null &&
        LanguageManager.Instance.CurrentLanguage ==
        GameLanguage.Indonesian;

    private void Awake()
    {
        attemptsUsed = 0;
        awaitingRetry = false;
        submissionInProgress = false;
        submissionLocked = false;

        if (continueInvestigationButton != null)
        {
            continueInvestigationButton.onClick.AddListener(
                ContinueInvestigation
            );
        }

        if (retryFeedbackPanel != null)
            retryFeedbackPanel.SetActive(false);

        SubscribeToCards(suspectCards);
        SubscribeToCards(locationCards);

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged +=
                HandleLanguageChanged;
        }

        if (closeBoardButton != null)
            closeBoardButton.onClick.AddListener(CloseBoard);

        if (reviewSubmissionButton != null)
        {
            reviewSubmissionButton.onClick.AddListener(
                ReviewSubmission
            );
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(
                CloseConfirmation
            );
        }

        if (confirmSubmissionButton != null)
        {
            confirmSubmissionButton.onClick.AddListener(
                ConfirmSubmission
            );
        }

        if (successReturnToMenuButton != null)
        {
            successReturnToMenuButton.onClick.AddListener(
                ReturnToMainMenu
            );
        }

        if (incorrectReturnToMenuButton != null)
        {
            incorrectReturnToMenuButton.onClick.AddListener(
                ReturnToMainMenu
            );
        }

        if (submissionConfirmationPanel != null)
            submissionConfirmationPanel.SetActive(false);

        if (successResultPanel != null)
            successResultPanel.SetActive(false);

        if (incorrectResultPanel != null)
            incorrectResultPanel.SetActive(false);

        RefreshSelection();
    }

    private void OnDestroy()
    {
        if (continueInvestigationButton != null)
        {
            continueInvestigationButton.onClick.RemoveListener(
                ContinueInvestigation
            );
        }

        UnsubscribeFromCards(suspectCards);
        UnsubscribeFromCards(locationCards);

        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged -=
                HandleLanguageChanged;
        }

        if (closeBoardButton != null)
        {
            closeBoardButton.onClick.RemoveListener(
                CloseBoard
            );
        }

        if (reviewSubmissionButton != null)
        {
            reviewSubmissionButton.onClick.RemoveListener(
                ReviewSubmission
            );
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveListener(
                CloseConfirmation
            );
        }

        if (confirmSubmissionButton != null)
        {
            confirmSubmissionButton.onClick.RemoveListener(
                ConfirmSubmission
            );
        }

        if (successReturnToMenuButton != null)
        {
            successReturnToMenuButton.onClick.RemoveListener(
                ReturnToMainMenu
            );
        }

        if (incorrectReturnToMenuButton != null)
        {
            incorrectReturnToMenuButton.onClick.RemoveListener(
                ReturnToMainMenu
            );
        }
    }

    private void SubscribeToCards(
        DetectiveOptionCard[] cards
    )
    {
        if (cards == null)
            return;

        foreach (DetectiveOptionCard card in cards)
        {
            if (card != null && card.Toggle != null)
            {
                card.Toggle.onValueChanged.AddListener(
                    HandleSelectionChanged
                );
            }
        }
    }

    private void UnsubscribeFromCards(
        DetectiveOptionCard[] cards
    )
    {
        if (cards == null)
            return;

        foreach (DetectiveOptionCard card in cards)
        {
            if (card != null && card.Toggle != null)
            {
                card.Toggle.onValueChanged.RemoveListener(
                    HandleSelectionChanged
                );
            }
        }
    }

    private void HandleLanguageChanged(
        GameLanguage language
    )
    {
        RefreshSelection();

        if (submissionConfirmationPanel != null &&
            submissionConfirmationPanel.activeSelf)
        {
            if (suspectValue != null &&
                selectedSuspect != null)
            {
                suspectValue.text =
                    selectedSuspect.DisplayName;
            }

            if (locationValue != null &&
                selectedLocation != null)
            {
                locationValue.text =
                    selectedLocation.DisplayName;
            }
        }

        if (incorrectResultPanel != null &&
            incorrectResultPanel.activeSelf)
        {
            UpdateIncorrectResultValues();
        }
    }

    private void HandleSelectionChanged(bool value)
    {
        RefreshSelection();
    }

    private void RefreshSelection()
    {
        selectedSuspect =
            FindSelectedCard(suspectCards);

        selectedLocation =
            FindSelectedCard(locationCards);

        bool selectionComplete =
            selectedSuspect != null &&
            selectedLocation != null &&
            CanSubmit;

        if (reviewSubmissionButton != null)
        {
            reviewSubmissionButton.interactable =
                selectionComplete;
        }

        UpdateSummary(
            selectedSuspect != null &&
            selectedLocation != null
        );

        UpdateAttemptsDisplay();

        if (confirmSubmissionButton != null)
        {
            confirmSubmissionButton.interactable =
                selectionComplete &&
                submissionConfirmationPanel != null &&
                submissionConfirmationPanel.activeInHierarchy;
        }
    }

    private DetectiveOptionCard FindSelectedCard(
        DetectiveOptionCard[] cards
    )
    {
        if (cards == null)
            return null;

        foreach (DetectiveOptionCard card in cards)
        {
            if (card != null &&
                card.Toggle != null &&
                card.Toggle.isOn)
            {
                return card;
            }
        }

        return null;
    }

    private void UpdateSummary(bool selectionComplete)
    {
        if (selectionSummary == null)
            return;

        string notSelected = IsIndonesian
            ? "Belum dipilih"
            : "Not selected";

        string suspectName = selectedSuspect != null
            ? selectedSuspect.DisplayName
            : notSelected;

        string locationName = selectedLocation != null
            ? selectedLocation.DisplayName
            : notSelected;

        string heading = IsIndonesian
            ? "TEORI TERPILIH"
            : "SELECTED THEORY";

        string suspectLabel = IsIndonesian
            ? "TERSANGKA: "
            : "SUSPECT: ";

        string locationLabel = IsIndonesian
            ? "LOKASI: "
            : "LOCATION: ";

        selectionSummary.text =
            "<color=#D2A94F><b>" + heading + "</b></color>\n" +
            "<color=#91A7B4>" + suspectLabel + "</color>" +
            "<color=#E7EDF1><b>" + suspectName + "</b></color>" +
            "    <color=#4C6675>|</color>    " +
            "<color=#91A7B4>" + locationLabel + "</color>" +
            "<color=#E7EDF1><b>" + locationName + "</b></color>";
    }

    private void UpdateAttemptsDisplay()
    {
        if (attemptsRemainingText == null)
            return;

        attemptsRemainingText.text = IsIndonesian
            ? $"SISA KESEMPATAN: {AttemptsRemaining}/{MaximumAttempts}"
            : $"ATTEMPTS REMAINING: {AttemptsRemaining}/{MaximumAttempts}";
    }

    public void OpenBoard()
    {
        if (!CanSubmit)
            return;

        if (hamburgerDropdown != null)
            hamburgerDropdown.SetActive(false);

        if (detectiveBoardPanel != null)
            detectiveBoardPanel.SetActive(true);

        RefreshSelection();
    }

    public void CloseBoard()
    {
        CloseConfirmation();

        if (detectiveBoardPanel != null)
            detectiveBoardPanel.SetActive(false);
    }

    private void ReviewSubmission()
    {
        if (!CanSubmit)
            return;

        RefreshSelection();

        if (selectedSuspect == null ||
            selectedLocation == null)
        {
            return;
        }

        if (suspectValue != null)
        {
            suspectValue.text =
                selectedSuspect.DisplayName;
        }

        if (locationValue != null)
        {
            locationValue.text =
                selectedLocation.DisplayName;
        }

        if (submissionConfirmationPanel != null)
        {
            submissionConfirmationPanel.SetActive(true);

            submissionConfirmationPanel.transform
                .SetAsLastSibling();
        }

        RefreshSelection();
    }

    private void CloseConfirmation()
    {
        if (confirmSubmissionButton != null)
            confirmSubmissionButton.interactable = false;

        if (submissionConfirmationPanel != null)
        {
            submissionConfirmationPanel.SetActive(
                false
            );
        }
    }

    private void ConfirmSubmission()
    {
        if (!CanSubmit ||
            submissionConfirmationPanel == null ||
            !submissionConfirmationPanel.activeInHierarchy)
        {
            return;
        }

        RefreshSelection();

        if (selectedSuspect == null || selectedLocation == null)
            return;

        if (retryFeedbackPanel == null ||
            continueInvestigationButton == null)
        {
            Debug.LogError(
                "DetectiveBoardController: Retry Feedback Panel dan " +
                "Continue Investigation Button harus dihubungkan.",
                this
            );
            return;
        }

        submissionInProgress = true;
        RefreshSelection();

        string submittedSuspectID =
            selectedSuspect.OptionID.Trim();

        string submittedLocationID =
            selectedLocation.OptionID.Trim();

        bool suspectCorrect = string.Equals(
            submittedSuspectID,
            correctSuspectID.Trim(),
            StringComparison.OrdinalIgnoreCase
        );

        bool locationCorrect = string.Equals(
            submittedLocationID,
            correctLocationID.Trim(),
            StringComparison.OrdinalIgnoreCase
        );

        bool answerCorrect = suspectCorrect && locationCorrect;

        attemptsUsed++;

        Debug.Log(
            $"SUBMISSION {attemptsUsed}/{MaximumAttempts}: " +
            submittedSuspectID + " / " + submittedLocationID +
            " | Correct: " + answerCorrect
        );

        CloseConfirmation();

        if (detectiveBoardPanel != null)
            detectiveBoardPanel.SetActive(false);

        if (answerCorrect)
        {
            submissionLocked = true;
            ShowSuccessResult();
        }
        else if (AttemptsRemaining > 0)
        {
            awaitingRetry = true;
            ShowRetryFeedback();
        }
        else
        {
            submissionLocked = true;
            ShowIncorrectResult();
        }

        submissionInProgress = false;
        RefreshSelection();
    }

    private void ShowRetryFeedback()
    {
        if (successResultPanel != null)
            successResultPanel.SetActive(false);

        if (incorrectResultPanel != null)
            incorrectResultPanel.SetActive(false);

        retryFeedbackPanel.transform.SetAsLastSibling();
        retryFeedbackPanel.SetActive(true);
    }

    private void ContinueInvestigation()
    {
        if (!awaitingRetry ||
            submissionLocked ||
            submissionInProgress)
        {
            return;
        }

        awaitingRetry = false;

        if (retryFeedbackPanel != null)
            retryFeedbackPanel.SetActive(false);

        RefreshSelection();
    }

    private void ShowSuccessResult()
    {
        if (incorrectResultPanel != null)
            incorrectResultPanel.SetActive(false);

        if (successResultPanel != null)
        {
            successResultPanel.SetActive(true);
            successResultPanel.transform.SetAsLastSibling();
        }
    }

    private void ShowIncorrectResult()
    {
        if (successResultPanel != null)
            successResultPanel.SetActive(false);

        UpdateIncorrectResultValues();

        if (incorrectResultPanel != null)
        {
            incorrectResultPanel.SetActive(true);

            incorrectResultPanel.transform
                .SetAsLastSibling();
        }
    }

    private void UpdateIncorrectResultValues()
    {
        if (submittedSuspectText != null &&
            selectedSuspect != null)
        {
            string label = IsIndonesian
                ? "TERSANGKA     "
                : "SUSPECT     ";

            submittedSuspectText.text =
                label +
                selectedSuspect.DisplayName
                    .ToUpperInvariant();
        }

        if (submittedLocationText != null &&
            selectedLocation != null)
        {
            string label = IsIndonesian
                ? "LOKASI     "
                : "LOCATION     ";

            submittedLocationText.text =
                label +
                selectedLocation.DisplayName
                    .ToUpperInvariant();
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public string GetSelectedSuspectID()
    {
        return selectedSuspect != null
            ? selectedSuspect.OptionID
            : string.Empty;
    }

    public string GetSelectedLocationID()
    {
        return selectedLocation != null
            ? selectedLocation.OptionID
            : string.Empty;
    }
}
