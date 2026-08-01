using System;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float startingTimeSeconds = 600f;
    [SerializeField] private float warningTimeSeconds = 60f;

    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject timeUpPanel;

    [Header("Timer Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;

    private float remainingTime;
    private bool isRunning;
    private bool hasExpired;

    public event Action TimerExpired;

    public float RemainingTime => remainingTime;
    public bool HasExpired => hasExpired;

    private void Awake()
    {
        remainingTime = startingTimeSeconds;

        if (timeUpPanel != null)
            timeUpPanel.SetActive(false);

        UpdateTimerDisplay();
    }

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError(
                "CountdownTimer: Timer Text belum dimasukkan di Inspector.",
                this
            );

            enabled = false;
            return;
        }

        remainingTime = startingTimeSeconds;
        hasExpired = false;
        isRunning = true;

        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (!isRunning || hasExpired)
            return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateTimerDisplay();

        if (remainingTime <= 0f)
            ExpireTimer();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null)
            return;

        int totalSeconds = Mathf.CeilToInt(remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";

        timerText.color = remainingTime <= warningTimeSeconds
            ? warningColor
            : normalColor;
    }

    private void ExpireTimer()
    {
        if (hasExpired)
            return;

        remainingTime = 0f;
        isRunning = false;
        hasExpired = true;

        UpdateTimerDisplay();

        TimerExpired?.Invoke();

        if (timeUpPanel != null)
        {
            timeUpPanel.SetActive(true);
            timeUpPanel.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogWarning(
                "CountdownTimer: Time Up Panel belum dimasukkan di Inspector.",
                this
            );
        }
    }

    public void PauseTimer()
    {
        if (!hasExpired)
            isRunning = false;
    }

    public void ResumeTimer()
    {
        if (!hasExpired)
            isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void RestartTimer()
    {
        remainingTime = startingTimeSeconds;
        hasExpired = false;
        isRunning = true;

        if (timeUpPanel != null)
            timeUpPanel.SetActive(false);

        UpdateTimerDisplay();
    }
}