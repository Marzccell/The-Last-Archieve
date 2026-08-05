using System.Collections;
using UnityEngine;

public class PopupOpenTransition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform content;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float duration = 0.22f;
    [SerializeField] private float startScale = 0.94f;

    private Coroutine transitionCoroutine;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(PlayOpenTransition());
    }

    private IEnumerator PlayOpenTransition()
    {
        if (canvasGroup == null || content == null)
            yield break;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        content.localScale = new Vector3(startScale, startScale, 1f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);

            canvasGroup.alpha = easedProgress;

            float scale = Mathf.Lerp(startScale, 1f, easedProgress);
            content.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        content.localScale = Vector3.one;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        transitionCoroutine = null;
    }
}