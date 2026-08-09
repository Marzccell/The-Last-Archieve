using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapRoomHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Room Hover")]
    [SerializeField] private Button roomButton;
    [SerializeField] private GameObject hoverBorder;

    private void Awake()
    {
        if (roomButton == null)
        {
            roomButton = GetComponent<Button>();
        }

        HideHoverBorder();
    }

    private void OnEnable()
    {
        HideHoverBorder();
    }

    private void OnDisable()
    {
        HideHoverBorder();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (roomButton == null || hoverBorder == null)
        {
            return;
        }

        // Current room memiliki Interactable OFF.
        // Karena itu hover hanya tampil pada room lain.
        if (roomButton.interactable)
        {
            hoverBorder.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHoverBorder();
    }

    private void HideHoverBorder()
    {
        if (hoverBorder != null)
        {
            hoverBorder.SetActive(false);
        }
    }
}