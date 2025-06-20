using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
    private const int MaxEntries = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEntry(string name, int score)
    {
        leaderboard.Add(new LeaderboardEntry(name, score));
        leaderboard = leaderboard.OrderByDescending(e => e.score).Take(MaxEntries).ToList();
        SaveLeaderboard();
    }

    void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboard.Count; i++)
        {
            PlayerPrefs.SetString($"LB_Name_{i}", leaderboard[i].playerName);
            PlayerPrefs.SetInt($"LB_Score_{i}", leaderboard[i].score);
        }
        PlayerPrefs.SetInt("LB_Count", leaderboard.Count);
        PlayerPrefs.Save();
    }

    void LoadLeaderboard()
    {
        leaderboard.Clear();
        int count = PlayerPrefs.GetInt("LB_Count", 0);
        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString($"LB_Name_{i}", "Unknown");
            int score = PlayerPrefs.GetInt($"LB_Score_{i}", 0);
            leaderboard.Add(new LeaderboardEntry(name, score));
        }
    }

    public List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboard;
    }

    public void ReloadLeaderboard()
    {
        LoadLeaderboard();
    }

    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey("LB_Count");

        for (int i = 0; i < leaderboard.Count; i++)
        {
            PlayerPrefs.DeleteKey($"LB_Name_{i}");
            PlayerPrefs.DeleteKey($"LB_Score_{i}");
        }

        PlayerPrefs.Save();
        leaderboard.Clear();
    }

    public void OnClearLeaderboardButtonClicked()
    {
        ClearLeaderboard();
        Debug.Log("✅ Leaderboard cleared.");

        // Optional: Refresh the UI immediately
        var ui = FindObjectOfType<LeaderboardUI>();
        if (ui != null)
            ui.ShowLeaderboard();
    }

}
