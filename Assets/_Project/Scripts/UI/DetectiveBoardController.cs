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

    [Header("Timer")]
    [SerializeField] private CountdownTimer countdownTimer;

    [Header("Buttons")]
    [SerializeField] private Button closeBoardButton;

    private DetectiveOptionCard selectedSuspect;
    private DetectiveOptionCard selectedLocation;

    private bool submissionLocked;

    private void Awake()
    {
        SubscribeToCards(suspectCards);
        SubscribeToCards(locationCards);

        if (closeBoardButton != null)
            closeBoardButton.onClick.AddListener(CloseBoard);

        if (reviewSubmissionButton != null)
            reviewSubmissionButton.onClick.AddListener(ReviewSubmission);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(CloseConfirmation);

        if (confirmSubmissionButton != null)
            confirmSubmissionButton.onClick.AddListener(ConfirmSubmission);

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

        if (countdownTimer != null)
            countdownTimer.TimerExpired += HandleTimerExpired;

        if (submissionConfirmationPanel != null)
            submissionConfirmationPanel.SetActive(false);

        if (successResultPanel != null)
            successResultPanel.SetActive(false);

        if (incorrectResultPanel != null)
            incorrectResultPanel.SetActive(false);

        submissionLocked = false;

        RefreshSelection();
    }

    private void OnDestroy()
    {
        UnsubscribeFromCards(suspectCards);
        UnsubscribeFromCards(locationCards);

        if (closeBoardButton != null)
            closeBoardButton.onClick.RemoveListener(CloseBoard);

        if (reviewSubmissionButton != null)
        {
            reviewSubmissionButton.onClick.RemoveListener(
                ReviewSubmission
            );
        }

        if (cancelButton != null)
            cancelButton.onClick.RemoveListener(CloseConfirmation);

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

        if (countdownTimer != null)
            countdownTimer.TimerExpired -= HandleTimerExpired;
    }

    private void SubscribeToCards(DetectiveOptionCard[] cards)
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

    private void UnsubscribeFromCards(DetectiveOptionCard[] cards)
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

    private void HandleSelectionChanged(bool value)
    {
        RefreshSelection();
    }

    private void RefreshSelection()
    {
        selectedSuspect = FindSelectedCard(suspectCards);
        selectedLocation = FindSelectedCard(locationCards);

        bool selectionComplete =
            selectedSuspect != null &&
            selectedLocation != null &&
            !submissionLocked;

        if (reviewSubmissionButton != null)
            reviewSubmissionButton.interactable = selectionComplete;

        UpdateSummary(
            selectedSuspect != null &&
            selectedLocation != null
        );
    }

    private DetectiveOptionCard FindSelectedCard(
        DetectiveOptionCard[] cards)
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

        string suspectName = selectedSuspect != null
            ? selectedSuspect.DisplayName
            : "Not selected";

        string locationName = selectedLocation != null
            ? selectedLocation.DisplayName
            : "Not selected";

        if (selectionComplete)
        {
            selectionSummary.text =
                "<b>SELECTED THEORY</b>\n" +
                suspectName + " — " + locationName;
        }
        else
        {
            selectionSummary.text =
                "<b>SELECTED THEORY</b>\n" +
                "Suspect: " + suspectName +
                "    |    Location: " + locationName;
        }
    }

    public void OpenBoard()
    {
        if (submissionLocked)
            return;

        if (countdownTimer != null &&
            countdownTimer.HasExpired)
        {
            return;
        }

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
        if (submissionLocked)
            return;

        RefreshSelection();

        if (selectedSuspect == null ||
            selectedLocation == null)
        {
            return;
        }

        if (suspectValue != null)
            suspectValue.text = selectedSuspect.DisplayName;

        if (locationValue != null)
            locationValue.text = selectedLocation.DisplayName;

        if (submissionConfirmationPanel != null)
        {
            submissionConfirmationPanel.SetActive(true);
            submissionConfirmationPanel.transform.SetAsLastSibling();
        }
    }

    private void CloseConfirmation()
    {
        if (submissionConfirmationPanel != null)
            submissionConfirmationPanel.SetActive(false);
    }

    private void ConfirmSubmission()
    {
        if (submissionLocked)
            return;

        if (countdownTimer != null &&
            countdownTimer.HasExpired)
        {
            return;
        }

        RefreshSelection();

        if (selectedSuspect == null ||
            selectedLocation == null)
        {
            return;
        }

        submissionLocked = true;

        if (confirmSubmissionButton != null)
            confirmSubmissionButton.interactable = false;

        if (reviewSubmissionButton != null)
            reviewSubmissionButton.interactable = false;

        if (countdownTimer != null)
            countdownTimer.StopTimer();

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

        bool answerCorrect =
            suspectCorrect && locationCorrect;

        Debug.Log(
            "FINAL SUBMISSION: " +
            submittedSuspectID +
            " / " +
            submittedLocationID +
            " | Correct: " +
            answerCorrect
        );

        CloseConfirmation();

        if (detectiveBoardPanel != null)
            detectiveBoardPanel.SetActive(false);

        if (answerCorrect)
            ShowSuccessResult();
        else
            ShowIncorrectResult();
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

        if (submittedSuspectText != null)
        {
            submittedSuspectText.text =
                "SUSPECT     " +
                selectedSuspect.DisplayName.ToUpper();
        }

        if (submittedLocationText != null)
        {
            submittedLocationText.text =
                "LOCATION     " +
                selectedLocation.DisplayName.ToUpper();
        }

        if (incorrectResultPanel != null)
        {
            incorrectResultPanel.SetActive(true);
            incorrectResultPanel.transform.SetAsLastSibling();
        }
    }

    private void HandleTimerExpired()
    {
        submissionLocked = true;

        if (confirmSubmissionButton != null)
            confirmSubmissionButton.interactable = false;

        if (reviewSubmissionButton != null)
            reviewSubmissionButton.interactable = false;

        CloseConfirmation();

        if (detectiveBoardPanel != null)
            detectiveBoardPanel.SetActive(false);

        if (hamburgerDropdown != null)
            hamburgerDropdown.SetActive(false);

        if (successResultPanel != null)
            successResultPanel.SetActive(false);

        if (incorrectResultPanel != null)
            incorrectResultPanel.SetActive(false);
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