using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(Image))]
public class DetectiveOptionCard : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Option Data")]
    [SerializeField] private string optionID;
    [SerializeField] private string optionDisplayName;

    [Header("References")]
    [SerializeField] private Toggle optionToggle;
    [SerializeField] private Image cardImage;
    [SerializeField] private Outline cardOutline;
    [SerializeField] private TMP_Text optionNameText;
    [SerializeField] private GameObject selectionCheckmark;

    [Header("Colors")]
    [SerializeField] private Color normalColor =
        new Color32(24, 44, 60, 255);

    [SerializeField] private Color hoverColor =
        new Color32(35, 70, 94, 255);

    [SerializeField] private Color selectedColor =
        new Color32(35, 91, 126, 255);

    [SerializeField] private Color normalOutlineColor =
        new Color32(63, 99, 122, 220);

    [SerializeField] private Color selectedOutlineColor =
        new Color32(104, 200, 255, 255);

    private bool isPointerInside;
    private bool listenerRegistered;

    public string OptionID => optionID;
    public string DisplayName => optionDisplayName;

    public Toggle Toggle
    {
        get
        {
            InitializeReferences();
            return optionToggle;
        }
    }

    private void Awake()
    {
        InitializeReferences();
        RegisterListener();
        RefreshVisual();
    }

    private void OnEnable()
    {
        InitializeReferences();
        RegisterListener();

        isPointerInside = false;

        if (optionToggle != null)
            optionToggle.transition = Selectable.Transition.None;

        RefreshVisual();
    }

    private void OnDisable()
    {
        isPointerInside = false;
    }

    private void OnDestroy()
    {
        UnregisterListener();
    }

    private void InitializeReferences()
    {
        if (optionToggle == null)
            optionToggle = GetComponent<Toggle>();

        if (cardImage == null)
            cardImage = GetComponent<Image>();

        if (cardOutline == null)
            cardOutline = GetComponent<Outline>();

        if (optionToggle != null)
            optionToggle.transition = Selectable.Transition.None;
    }

    private void RegisterListener()
    {
        if (listenerRegistered || optionToggle == null)
            return;

        optionToggle.onValueChanged.AddListener(
            HandleToggleChanged
        );

        listenerRegistered = true;
    }

    private void UnregisterListener()
    {
        if (!listenerRegistered || optionToggle == null)
            return;

        optionToggle.onValueChanged.RemoveListener(
            HandleToggleChanged
        );

        listenerRegistered = false;
    }

    private void HandleToggleChanged(bool isSelected)
    {
        RefreshVisual();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (optionToggle != null &&
            !optionToggle.interactable)
        {
            return;
        }

        isPointerInside = true;
        RefreshVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        RefreshVisual();
    }

    public void SetSelectedWithoutNotify(bool selected)
    {
        InitializeReferences();

        if (optionToggle == null)
            return;

        optionToggle.SetIsOnWithoutNotify(selected);
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        InitializeReferences();

        if (optionToggle == null)
            return;

        bool selected = optionToggle.isOn;

        if (selectionCheckmark != null)
            selectionCheckmark.SetActive(selected);

        if (cardImage != null)
        {
            if (selected)
            {
                cardImage.color = selectedColor;
            }
            else if (isPointerInside &&
                     optionToggle.interactable)
            {
                cardImage.color = hoverColor;
            }
            else
            {
                cardImage.color = normalColor;
            }
        }

        if (cardOutline != null)
        {
            cardOutline.effectColor = selected
                ? selectedOutlineColor
                : normalOutlineColor;
        }

        if (optionNameText != null)
        {
            optionNameText.color = selected
                ? Color.white
                : new Color32(231, 237, 241, 255);
        }
    }
}