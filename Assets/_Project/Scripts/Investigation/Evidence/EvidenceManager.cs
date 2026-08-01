using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    [Header("Evidence Progress")]
    [SerializeField] private int totalEvidence = 14;
    [SerializeField] private TMP_Text evidenceCounterText;

    private readonly HashSet<string> discoveredEvidence =
        new HashSet<string>();

    public int DiscoveredCount => discoveredEvidence.Count;
    public int TotalEvidence => totalEvidence;

    private void Start()
    {
        UpdateCounterText();
    }

    public bool RegisterEvidence(string evidenceID)
    {
        if (string.IsNullOrWhiteSpace(evidenceID))
        {
            Debug.LogError(
                "EvidenceManager: Evidence ID tidak boleh kosong.",
                this
            );

            return false;
        }

        bool isNewEvidence = discoveredEvidence.Add(evidenceID);

        if (isNewEvidence)
        {
            UpdateCounterText();

            Debug.Log(
                $"Evidence ditemukan: {evidenceID} " +
                $"({DiscoveredCount}/{totalEvidence})",
                this
            );
        }

        return isNewEvidence;
    }

    public bool HasDiscoveredEvidence(string evidenceID)
    {
        return !string.IsNullOrWhiteSpace(evidenceID)
            && discoveredEvidence.Contains(evidenceID);
    }

    private void UpdateCounterText()
    {
        if (evidenceCounterText == null)
        {
            return;
        }

        evidenceCounterText.text =
            $"EVIDENCE FOUND {DiscoveredCount}/{totalEvidence}";
    }
}