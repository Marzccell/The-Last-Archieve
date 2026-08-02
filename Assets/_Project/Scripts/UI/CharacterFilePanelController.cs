using TMPro;
using UnityEngine;

public class CharacterFilePanelController : MonoBehaviour
{
    private enum CharacterProfile
    {
        Daniel,
        Olivia,
        Ethan,
        Lucas,
        Grace
    }

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

    private CharacterProfile currentProfile =
        CharacterProfile.Daniel;

    private bool IsIndonesian =>
        LanguageManager.Instance != null &&
        LanguageManager.Instance.CurrentLanguage ==
        GameLanguage.Indonesian;

    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged +=
                HandleLanguageChanged;
        }

        RefreshCurrentProfile();
    }

    private void Start()
    {
        if (characterFilePanel != null)
        {
            characterFilePanel.SetActive(false);
        }

        ShowDaniel();
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.LanguageChanged -=
                HandleLanguageChanged;
        }
    }

    private void HandleLanguageChanged(GameLanguage language)
    {
        RefreshCurrentProfile();
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

    private void RefreshCurrentProfile()
    {
        switch (currentProfile)
        {
            case CharacterProfile.Daniel:
                RenderDaniel();
                break;

            case CharacterProfile.Olivia:
                RenderOlivia();
                break;

            case CharacterProfile.Ethan:
                RenderEthan();
                break;

            case CharacterProfile.Lucas:
                RenderLucas();
                break;

            case CharacterProfile.Grace:
                RenderGrace();
                break;
        }
    }

    private void SetSelection(GameObject selectedBar)
    {
        danielSelectionBar.SetActive(
            selectedBar == danielSelectionBar
        );

        oliviaSelectionBar.SetActive(
            selectedBar == oliviaSelectionBar
        );

        ethanSelectionBar.SetActive(
            selectedBar == ethanSelectionBar
        );

        lucasSelectionBar.SetActive(
            selectedBar == lucasSelectionBar
        );

        graceSelectionBar.SetActive(
            selectedBar == graceSelectionBar
        );
    }

    private void SetProfile(
        string initial,
        string code,
        string name,
        string englishRole,
        string indonesianRole,
        string englishDescription,
        string indonesianDescription,
        string englishAreaAccess,
        string indonesianAreaAccess,
        string englishOperationalAccess,
        string indonesianOperationalAccess,
        GameObject selectionBar
    )
    {
        profileInitialText.text = initial;

        characterCodeText.text = IsIndonesian
            ? "ID PERSONEL / " + code
            : "PERSONNEL ID / " + code;

        characterNameText.text = name;

        characterRoleText.text = IsIndonesian
            ? indonesianRole
            : englishRole;

        descriptionText.text = IsIndonesian
            ? indonesianDescription
            : englishDescription;

        areaAccessText.text = IsIndonesian
            ? indonesianAreaAccess
            : englishAreaAccess;

        operationalAccessText.text = IsIndonesian
            ? indonesianOperationalAccess
            : englishOperationalAccess;

        SetSelection(selectionBar);
    }

    public void ShowDaniel()
    {
        currentProfile = CharacterProfile.Daniel;
        RenderDaniel();
    }

    private void RenderDaniel()
    {
        SetProfile(
            "D",
            "SEC-01",
            "DANIEL",
            "Security Guard",
            "Petugas Keamanan",
            "Responsible for CCTV, patrols, checkpoints, and the East Service Passage.",
            "Bertanggung jawab atas CCTV, patroli, titik pemeriksaan, dan Jalur Servis Timur.",
            "• Security Office\n" +
            "• Public corridors\n" +
            "• Museum entrance checkpoints",
            "• Kantor Keamanan\n" +
            "• Koridor publik\n" +
            "• Titik pemeriksaan pintu masuk museum",
            "• SEC-01 credential\n" +
            "• Door control\n" +
            "• Hold-Open Mode",
            "• Kredensial SEC-01\n" +
            "• Kontrol pintu\n" +
            "• Mode Hold-Open",
            danielSelectionBar
        );
    }

    public void ShowOlivia()
    {
        currentProfile = CharacterProfile.Olivia;
        RenderOlivia();
    }

    private void RenderOlivia()
    {
        SetProfile(
            "O",
            "CUR-02",
            "OLIVIA",
            "Museum Curator",
            "Kurator Museum",
            "Responsible for the Royal Sapphire, replica checks, and restoration document preparation.",
            "Bertanggung jawab atas Royal Sapphire, pemeriksaan replika, dan penyiapan dokumen restorasi.",
            "• Main Exhibition Hall\n" +
            "• Staff Office\n" +
            "• Replica storage\n" +
            "• Dispatch area",
            "• Ruang Pameran Utama\n" +
            "• Kantor Staf\n" +
            "• Penyimpanan replika\n" +
            "• Area pengiriman",
            "• CUR-02 credential\n" +
            "• Inner holder access\n" +
            "• Collection Database",
            "• Kredensial CUR-02\n" +
            "• Akses dudukan bagian dalam\n" +
            "• Database Koleksi",
            oliviaSelectionBar
        );
    }

    public void ShowEthan()
    {
        currentProfile = CharacterProfile.Ethan;
        RenderEthan();
    }

    private void RenderEthan()
    {
        SetProfile(
            "E",
            "ADM-03",
            "ETHAN",
            "Administrative Staff",
            "Staf Administrasi",
            "Responsible for museum archives, inventory records, and monthly reports.",
            "Bertanggung jawab atas arsip museum, catatan inventaris, dan laporan bulanan.",
            "• Staff Office\n" +
            "• Archive Room\n" +
            "• Administrative archive",
            "• Kantor Staf\n" +
            "• Ruang Arsip\n" +
            "• Arsip administrasi",
            "• ADM-03 credential\n" +
            "• Workstation scheduler\n" +
            "• Archive search",
            "• Kredensial ADM-03\n" +
            "• Penjadwal stasiun kerja\n" +
            "• Pencarian arsip",
            ethanSelectionBar
        );
    }

    public void ShowLucas()
    {
        currentProfile = CharacterProfile.Lucas;
        RenderLucas();
    }

    private void RenderLucas()
    {
        SetProfile(
            "L",
            "MNT-04",
            "LUCAS",
            "Maintenance Technician",
            "Teknisi Pemeliharaan",
            "Responsible for sensor calibration and alarm inspection.",
            "Bertanggung jawab atas kalibrasi sensor dan pemeriksaan alarm.",
            "• Maintenance Room\n" +
            "• Security Office\n" +
            "• Main Exhibition Hall during work orders",
            "• Ruang Pemeliharaan\n" +
            "• Kantor Keamanan\n" +
            "• Ruang Pameran Utama selama perintah kerja",
            "• MNT-04 credential\n" +
            "• Maintenance bypass\n" +
            "• Camera diagnostic",
            "• Kredensial MNT-04\n" +
            "• Bypass pemeliharaan\n" +
            "• Diagnostik kamera",
            lucasSelectionBar
        );
    }

    public void ShowGrace()
    {
        currentProfile = CharacterProfile.Grace;
        RenderGrace();
    }

    private void RenderGrace()
    {
        SetProfile(
            "G",
            "GRA-05",
            "GRACE",
            "Museum Director",
            "Direktur Museum",
            "Responsible for approving maintenance, dispatch schedules, and operational decisions.",
            "Bertanggung jawab menyetujui pemeliharaan, jadwal pengiriman, dan keputusan operasional.",
            "• Staff Records Bay\n" +
            "• Main Exhibition Hall\n" +
            "• Security Office\n" +
            "• Staff Office",
            "• Area Catatan Staf\n" +
            "• Ruang Pameran Utama\n" +
            "• Kantor Keamanan\n" +
            "• Kantor Staf",
            "• GRA-05 credential\n" +
            "• Approval authority\n" +
            "• Schedule authority\n" +
            "• No inner holder access",
            "• Kredensial GRA-05\n" +
            "• Wewenang persetujuan\n" +
            "• Wewenang penjadwalan\n" +
            "• Tidak memiliki akses dudukan bagian dalam",
            graceSelectionBar
        );
    }
}