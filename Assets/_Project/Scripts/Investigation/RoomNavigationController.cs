using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNavigationController : MonoBehaviour
{
    [Header("Room Panels")]
    [SerializeField] private GameObject mainExhibitionHall;
    [SerializeField] private GameObject securityOffice;
    [SerializeField] private GameObject staffOffice;

    [Header("Room Title")]
    [SerializeField] private TMP_Text roomTitleText;

    [Header("Map")]
    [SerializeField] private GameObject mapOverlay;

    [Header("Map Room Buttons")]
    [SerializeField] private Button mainExhibitionHallButton;
    [SerializeField] private Button securityOfficeButton;
    [SerializeField] private Button staffOfficeButton;

    [Header("Map Current Location Indicators")]
    [SerializeField] private GameObject mainExhibitionHallCurrentTag;
    [SerializeField] private GameObject securityOfficeCurrentTag;
    [SerializeField] private GameObject staffOfficeCurrentTag;

    [Header("Map Active Borders")]
    [SerializeField] private GameObject mainExhibitionHallActiveBorder;
    [SerializeField] private GameObject securityOfficeActiveBorder;
    [SerializeField] private GameObject staffOfficeActiveBorder;

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
            mainExhibitionHall,
            mainExhibitionHallButton,
            "MAIN EXHIBITION HALL"
        );
    }

    public void ShowSecurityOffice()
    {
        ShowRoom(
            securityOffice,
            securityOfficeButton,
            "SECURITY OFFICE"
        );
    }

    public void ShowStaffOffice()
    {
        ShowRoom(
            staffOffice,
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
        // Menampilkan hanya panel ruangan yang dipilih.
        mainExhibitionHall.SetActive(
            selectedRoom == mainExhibitionHall
        );

        securityOffice.SetActive(
            selectedRoom == securityOffice
        );

        staffOffice.SetActive(
            selectedRoom == staffOffice
        );

        // Current room tidak dapat dipilih kembali.
        mainExhibitionHallButton.interactable =
            selectedButton != mainExhibitionHallButton;

        securityOfficeButton.interactable =
            selectedButton != securityOfficeButton;

        staffOfficeButton.interactable =
            selectedButton != staffOfficeButton;

        // Menampilkan CURRENT LOCATION pada ruangan aktif.
        mainExhibitionHallCurrentTag.SetActive(
            selectedButton == mainExhibitionHallButton
        );

        securityOfficeCurrentTag.SetActive(
            selectedButton == securityOfficeButton
        );

        staffOfficeCurrentTag.SetActive(
            selectedButton == staffOfficeButton
        );

        // Menampilkan border gold pada ruangan aktif.
        mainExhibitionHallActiveBorder.SetActive(
            selectedButton == mainExhibitionHallButton
        );

        securityOfficeActiveBorder.SetActive(
            selectedButton == securityOfficeButton
        );

        staffOfficeActiveBorder.SetActive(
            selectedButton == staffOfficeButton
        );

        // Memperbarui nama ruangan pada HUD.
        roomTitleText.text = roomTitle;

        // Menutup map setelah berpindah ruangan.
        CloseMap();
    }
}