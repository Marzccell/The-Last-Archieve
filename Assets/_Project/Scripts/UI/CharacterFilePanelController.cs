using TMPro;
using UnityEngine;

public class CharacterFilePanelController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject characterFilePanel;

    [Header("Profile Text")]
    [SerializeField] private TMP_Text profileInitialText;
    [SerializeField] private TMP_Text characterCodeText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text characterRoleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text areaAccessText;
    [SerializeField] private TMP_Text operationalAccessText;

    [Header("Selection Bars")]
    [SerializeField] private GameObject danielSelectionBar;
    [SerializeField] private GameObject oliviaSelectionBar;
    [SerializeField] private GameObject ethanSelectionBar;
    [SerializeField] private GameObject lucasSelectionBar;
    [SerializeField] private GameObject graceSelectionBar;

    private void Start()
    {
        if (characterFilePanel != null)
        {
            characterFilePanel.SetActive(false);
        }

        ShowDaniel();
    }

    public void OpenCharacterFile()
    {
        characterFilePanel.SetActive(true);
        characterFilePanel.transform.SetAsLastSibling();

        ShowDaniel();
    }

    public void CloseCharacterFile()
    {
        characterFilePanel.SetActive(false);
    }

    private void SetSelection(GameObject selectedBar)
    {
        danielSelectionBar.SetActive(selectedBar == danielSelectionBar);
        oliviaSelectionBar.SetActive(selectedBar == oliviaSelectionBar);
        ethanSelectionBar.SetActive(selectedBar == ethanSelectionBar);
        lucasSelectionBar.SetActive(selectedBar == lucasSelectionBar);
        graceSelectionBar.SetActive(selectedBar == graceSelectionBar);
    }

   public void ShowDaniel()
{
    profileInitialText.text = "D";
    characterCodeText.text = "PERSONNEL ID / SEC-01";
    characterNameText.text = "DANIEL";
    characterRoleText.text = "Security Guard";

    descriptionText.text =
        "Responsible for CCTV, patrols, checkpoints, and the East Service Passage.";

    areaAccessText.text =
        "• Security Office\n" +
        "• Public corridors\n" +
        "• Museum entrance checkpoints";

    operationalAccessText.text =
        "• SEC-01 credential\n" +
        "• Door control\n" +
        "• Hold-Open Mode";

    SetSelection(danielSelectionBar);
}

public void ShowOlivia()
{
    profileInitialText.text = "O";
    characterCodeText.text = "PERSONNEL ID / CUR-02";
    characterNameText.text = "OLIVIA";
    characterRoleText.text = "Museum Curator";

    descriptionText.text =
        "Responsible for the Royal Sapphire, replica checks, and restoration document preparation.";

    areaAccessText.text =
        "• Main Exhibition Hall\n" +
        "• Staff Office\n" +
        "• Replica storage\n" +
        "• Dispatch area";

    operationalAccessText.text =
        "• CUR-02 credential\n" +
        "• Inner holder access\n" +
        "• Collection Database";

    SetSelection(oliviaSelectionBar);
}

public void ShowEthan()
{
    profileInitialText.text = "E";
    characterCodeText.text = "PERSONNEL ID / ADM-03";
    characterNameText.text = "ETHAN";
    characterRoleText.text = "Administrative Staff";

    descriptionText.text =
        "Responsible for museum archives, inventory records, and monthly reports.";

    areaAccessText.text =
        "• Staff Office\n" +
        "• Archive Room\n" +
        "• Administrative archive";

    operationalAccessText.text =
        "• ADM-03 credential\n" +
        "• Workstation scheduler\n" +
        "• Archive search";

    SetSelection(ethanSelectionBar);
}

public void ShowLucas()
{
    profileInitialText.text = "L";
    characterCodeText.text = "PERSONNEL ID / MNT-04";
    characterNameText.text = "LUCAS";
    characterRoleText.text = "Maintenance Technician";

    descriptionText.text =
        "Responsible for sensor calibration and alarm inspection.";

    areaAccessText.text =
        "• Maintenance Room\n" +
        "• Security Office\n" +
        "• Main Exhibition Hall during work orders";

    operationalAccessText.text =
        "• MNT-04 credential\n" +
        "• Maintenance bypass\n" +
        "• Camera diagnostic";

    SetSelection(lucasSelectionBar);
}

public void ShowGrace()
{
    profileInitialText.text = "G";
    characterCodeText.text = "PERSONNEL ID / GRA-05";
    characterNameText.text = "GRACE";
    characterRoleText.text = "Museum Director";

    descriptionText.text =
        "Responsible for approving maintenance, dispatch schedules, and operational decisions.";

    areaAccessText.text =
        "• Staff Records Bay\n" +
        "• Main Exhibition Hall\n" +
        "• Security Office\n" +
        "• Staff Office";

    operationalAccessText.text =
        "• GRA-05 credential\n" +
        "• Approval authority\n" +
        "• Schedule authority\n" +
        "• No inner holder access";

    SetSelection(graceSelectionBar);
}
}