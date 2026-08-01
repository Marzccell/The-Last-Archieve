using System;
using UnityEngine;

public enum GameLanguage
{
    English,
    Indonesian
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    public GameLanguage CurrentLanguage { get; private set; } =
        GameLanguage.English;

    public event Action<GameLanguage> LanguageChanged;

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.BeforeSceneLoad
    )]
    private static void CreateLanguageManager()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject managerObject =
            new GameObject("LanguageManager");

        managerObject.AddComponent<LanguageManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleLanguage()
    {
        GameLanguage nextLanguage =
            CurrentLanguage == GameLanguage.English
                ? GameLanguage.Indonesian
                : GameLanguage.English;

        SetLanguage(nextLanguage);
    }

    public void SetLanguage(GameLanguage language)
    {
        if (CurrentLanguage == language)
        {
            return;
        }

        CurrentLanguage = language;
        LanguageChanged?.Invoke(CurrentLanguage);

        Debug.Log($"Language changed to: {CurrentLanguage}");
    }
}