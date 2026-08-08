using UnityEngine;

public class HamburgerMenuController : MonoBehaviour
{
    [Header("Dropdown Reference")]
    [SerializeField] private GameObject hamburgerDropdown;

    [Header("Notification")]
    [SerializeField] private GameObject notificationDot;

    private void Start()
    {
        if (hamburgerDropdown != null)
        {
            hamburgerDropdown.SetActive(false);
        }

        if (notificationDot != null)
        {
            notificationDot.SetActive(true);
        }
    }

    public void ToggleDropdown()
    {
        if (hamburgerDropdown == null)
        {
            Debug.LogWarning(
                "Hamburger Dropdown belum dipasang di Inspector."
            );
            return;
        }

        bool willOpen = !hamburgerDropdown.activeSelf;
        hamburgerDropdown.SetActive(willOpen);

        if (willOpen && notificationDot != null)
        {
            notificationDot.SetActive(false);
        }
    }

    public void CloseDropdown()
    {
        if (hamburgerDropdown != null)
        {
            hamburgerDropdown.SetActive(false);
        }
    }
}