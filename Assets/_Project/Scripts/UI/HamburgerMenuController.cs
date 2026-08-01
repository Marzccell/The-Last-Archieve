using UnityEngine;

public class HamburgerMenuController : MonoBehaviour
{
    [Header("Dropdown Reference")]
    [SerializeField] private GameObject hamburgerDropdown;

    private void Start()
    {
        if (hamburgerDropdown != null)
        {
            hamburgerDropdown.SetActive(false);
        }
    }

    public void ToggleDropdown()
    {
        if (hamburgerDropdown == null)
        {
            Debug.LogWarning("Hamburger Dropdown belum dipasang di Inspector.");
            return;
        }

        hamburgerDropdown.SetActive(!hamburgerDropdown.activeSelf);
    }

    public void CloseDropdown()
    {
        if (hamburgerDropdown != null)
        {
            hamburgerDropdown.SetActive(false);
        }
    }
}