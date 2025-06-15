using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // or UnityEngine.UI if you use UI.Text

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    [Tooltip("Number of levels in the game")]
    public int maxLevels = 8;
    [Tooltip("Score required to advance per level")]
    public int scorePerLevel = 20;
    [Tooltip("Multiplier to decrease spawn interval each level")]
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

    [Header("Spawner Reference")]
    public IngredientSpawner spawner;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
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
        lives = 3;                // or pull from your existing startingLives
        UpdateUI();
        gameOverPanel.SetActive(false);
        nextLevelPanel.SetActive(false);

        // configure spawner
        spawner.spawnInterval = spawner.baseSpawnInterval;
        spawner.CancelInvoke();
        spawner.InvokeRepeating(nameof(spawner.Spawn), 0.5f, spawner.spawnInterval);
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {lives}";
        levelText.text = $"Level: {currentLevel}";
    }

    public void AddScore(int pts)
    {
        Debug.Log($"current score {score}");
        Debug.Log($"current lives {lives}");
       //if (nextLevelPanel.activeSelf || gameOverPanel.activeSelf) return;

        score += pts;
        Debug.Log($"current score {score}");
        Debug.Log($"current lives {lives}");
       UpdateUI();

        //Check for level up
        if (score >= scorePerLevel && lives > 0)
                TriggerNextLevel();
    }

    public void AdjustLives(int delta)

    {
        Debug.Log($"current score {score}");
        Debug.Log($"current lives {lives}");
        //if (nextLevelPanel.activeSelf || gameOverPanel.activeSelf) return;

        lives += delta;
        Debug.Log($"current score {score}");
        Debug.Log($"current lives {lives}");
        UpdateUI();

        if (lives <= 0)
           TriggerGameOver();
    }

    void TriggerNextLevel()
    {
        // Stop spawning
        spawner.CancelInvoke();
        // Show the panel (expects two buttons wired to NextLevel() and ExitToGameOver())
        nextLevelPanel.SetActive(true);
    }

    public void NextLevel()
    {
        if (currentLevel >= maxLevels)
        {
            // You could treat this as win state
            TriggerGameOver();
            return;
        }

        currentLevel++;
        score = 0;  // reset score for new level
        UpdateUI();

        // Speed up spawning
        spawner.spawnInterval = Mathf.Max(0.2f, spawner.spawnInterval - spawnIntervalStep);

        // Hide panel and resume spawning
        nextLevelPanel.SetActive(false);
        spawner.InvokeRepeating(nameof(spawner.Spawn), 0.5f, spawner.spawnInterval);
    }

    public void ExitToGameOver()
    {
        nextLevelPanel.SetActive(false);
        TriggerGameOver();
    }

    void TriggerGameOver()
    {
        // Stop spawning once and for all
        spawner.CancelInvoke();
        gameOverPanel.SetActive(true);
    }

    // Optional UI button hooks
    public void RestartGame() => InitGame();
    public void QuitToMenu() => SceneManager.LoadScene("MainMenu");
}
