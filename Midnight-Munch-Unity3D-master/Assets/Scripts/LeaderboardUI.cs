using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentPanel;

    void Start()
    {
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        Debug.Log("✅ ShowLeaderboard called");
        LeaderboardManager.Instance.ReloadLeaderboard();

        // Clear old entries
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        List<LeaderboardEntry> entries = LeaderboardManager.Instance.GetLeaderboard();
        Debug.Log($"Entries Count: {entries.Count}");

        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            GameObject entryGO = Instantiate(entryPrefab, contentPanel);
            TextMeshProUGUI[] texts = entryGO.GetComponentsInChildren<TextMeshProUGUI>();

            Debug.Log($"Entry {i + 1}: {entry.playerName} - {entry.score} | TextFields Found: {texts.Length}");

            if (texts.Length >= 2)
            {
                texts[0].text = $"{i + 1}. {entry.playerName}";
                texts[1].text = $"{entry.score}";
            }
            else if (texts.Length == 1)
            {
                texts[0].text = $"{i + 1}. {entry.playerName} - {entry.score}";
            }
            else
            {
                Debug.LogWarning("No TextMeshProUGUI components found in entry prefab!");
            }
        }
    }


}
