using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNavigationController : MonoBehaviour
{
    [Header("Room Panels")]
    [SerializeField] private GameObject mainExhibitionHallPanel;
    [SerializeField] private GameObject securityOfficePanel;
    [SerializeField] private GameObject staffOfficePanel;

    [Header("Map")]
    [SerializeField] private GameObject mapOverlay;

    [Header("Map Room Buttons")]
    [SerializeField] private Button mainExhibitionHallButton;
    [SerializeField] private Button securityOfficeButton;
    [SerializeField] private Button staffOfficeButton;

    [Header("HUD")]
    [SerializeField] private TMP_Text roomTitleText;

    private void Start()
    {
        ShowMainExhibitionHall();
    }

    public void OpenMap()
    {
        mapOverlay.SetActive(true);
    }

    public void CloseMap()
    {
        mapOverlay.SetActive(false);
    }

    public void ShowMainExhibitionHall()
    {
        ShowRoom(
            mainExhibitionHallPanel,
            mainExhibitionHallButton,
            "MAIN EXHIBITION HALL"
        );
    }

    public void ShowSecurityOffice()
    {
        ShowRoom(
            securityOfficePanel,
            securityOfficeButton,
            "SECURITY OFFICE"
        );
    }

    public void ShowStaffOffice()
    {
        ShowRoom(
            staffOfficePanel,
            staffOfficeButton,
            "STAFF OFFICE"
        );
    }

    private void ShowRoom(
        GameObject selectedRoom,
        Button selectedButton,
        string roomTitle
    )
    {
        // Menampilkan room yang dipilih
        mainExhibitionHallPanel.SetActive(
            selectedRoom == mainExhibitionHallPanel
        );

        securityOfficePanel.SetActive(
            selectedRoom == securityOfficePanel
        );

        staffOfficePanel.SetActive(
            selectedRoom == staffOfficePanel
        );

        // Mengatur penanda room aktif
        mainExhibitionHallButton.interactable =
            selectedButton != mainExhibitionHallButton;

        securityOfficeButton.interactable =
            selectedButton != securityOfficeButton;

        staffOfficeButton.interactable =
            selectedButton != staffOfficeButton;

        roomTitleText.text = roomTitle;

        CloseMap();
    }
}