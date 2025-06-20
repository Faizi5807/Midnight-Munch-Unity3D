using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    public int maxLevels = 8;
    public int scorePerLevel = 20;
    public float spawnIntervalStep = 0.1f;

    [Header("Runtime State")]
    public int currentLevel { get; private set; }
    public int score { get; private set; }
    public int lives { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public GameObject nextLevelPanel;
    public GameObject gameOverPanel;
    public GameObject gameEndPanel;
    public TMP_InputField NameField;
    public string playerName = "";
    public GameObject LeaderboardPanel;


    [Header("Spawner Reference")]
    public IngredientSpawner spawner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitGame();
    }

    void InitGame()
    {
        currentLevel = 1;
        score = 0;
        lives = 3;
        UpdateUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (nextLevelPanel != null)
            nextLevelPanel.SetActive(false);

        if (spawner != null)
        {
            spawner.spawnInterval = spawner.baseSpawnInterval;
            spawner.StopSpawning(); 
            spawner.StartSpawning();
            spawner.InvokeRepeating(nameof(spawner.Spawn), 0.5f, spawner.spawnInterval);
        }
        else
        {
            Debug.LogError("Spawner is not assigned!");
        }
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {lives}";
        levelText.text = $"Level: {currentLevel}";
    }

    public void AddScore(int pts)
    {
        if (nextLevelPanel.activeSelf || gameOverPanel.activeSelf) return;

        score += pts;
        UpdateUI();

        Debug.Log($"Score updated: {score}");

        if (score >= scorePerLevel && lives > 0)
        {
            Debug.Log("Triggering Next Level");
            nextLevelPanel.SetActive(true);
            //TriggerNextLevel();
        }
    }

    public void AdjustLives(int delta)
    {
        if (nextLevelPanel.activeSelf || gameOverPanel.activeSelf) return;

        lives += delta;
        UpdateUI();

        Debug.Log($"Lives updated: {lives}");

        if (lives <= 0)
        {
            Debug.Log("Triggering Game Over!");
            gameOverPanel.SetActive(true);
            //TriggerGameOver();
        }
    }

    void TriggerNextLevel()
    {
        if (nextLevelPanel == null)
        {
            Debug.LogError("Next Level Panel is not assigned!");
            return;
        }

        Debug.Log("Triggering Next Level...");
        //spawner.StopSpawning(); ;
        nextLevelPanel.SetActive(true);
    }

    public void NextLevel()
    {
        if (currentLevel >= maxLevels)
        {
            TriggerGameOver();
            return;
        }

        currentLevel++;
        score = 0;
        UpdateUI();

        Debug.Log($"Advancing to Level: {currentLevel}");
        //float newSpawnInterval = Mathf.Max(0.2f, spawner.GetCurrentSpawnInterval() - spawnIntervalStep);
        //spawner.AdjustSpawnInterval(Mathf.Max(0.2f, spawner.spawnInterval - spawnIntervalStep));
        nextLevelPanel.SetActive(false);
        spawner.StartSpawning();
        spawner.InvokeRepeating(nameof(spawner.Spawn), 0.5f, spawner.spawnInterval);
    }

    public void ExitToGameOver()
    {
        nextLevelPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        //TriggerGameOver();
    }

    void TriggerGameOver()
    {
        Debug.Log("Game Over triggered");

        if (spawner != null)
        {
            spawner.StopSpawning();
            CancelInvoke(nameof(spawner.Spawn));
        }

        if (nextLevelPanel != null)
            nextLevelPanel.SetActive(false);

        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        if (livesText != null)
            livesText.gameObject.SetActive(false);

        if (levelText != null)
            levelText.gameObject.SetActive(false);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned!");
        }

        if (NameField != null)
        {
            playerName = NameField.text;
            LeaderboardManager.Instance.AddEntry(playerName, score);
        }
        else
        {
            Debug.LogWarning("NameField not assigned!");
        }
    }



    public void RestartFromLeaderboard()
    {
        LeaderboardPanel.SetActive(false);  

        Debug.Log("✅ Restarting game from leaderboard");

        InitGame();  
    }


    public void QuitToMenu() => SceneManager.LoadScene("MainMenu");

    public void OnSubmitPlayerName()
    {
        Debug.Log($"[DEBUG] NameField: {NameField}");
        Debug.Log($"[DEBUG] LeaderboardManager.Instance: {LeaderboardManager.Instance}");
        Debug.Log($"[DEBUG] leaderboardPanel: {LeaderboardPanel}");

        if (NameField != null && LeaderboardManager.Instance != null)
        {
            string playerName = NameField.text;
            LeaderboardManager.Instance.AddEntry(playerName, score);

            gameOverPanel.SetActive(false);
            LeaderboardPanel.SetActive(true);

            var leaderboardUI = LeaderboardPanel.GetComponent<LeaderboardUI>();
            if (leaderboardUI != null)
                leaderboardUI.ShowLeaderboard();
        }
        else
        {
            Debug.LogWarning("Something is not assigned.");
        }
    }



}
