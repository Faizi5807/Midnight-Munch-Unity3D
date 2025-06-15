using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Ingredient Data")]
    [Tooltip("Assign all your ingredient sprites here.")]
    public Sprite[] ingredientSprites;
    [Tooltip("Points corresponding to each sprite (same order).")]
    public int[] scoreValues;
    [Tooltip("Set true if the corresponding ingredient is bad.")]
    public bool[] isBadIngredient;
    [Tooltip("Optional VFX prefab to play on catch, per ingredient.")]
    public GameObject[] catchEffects;

    [Header("Spawning")]
    [Tooltip("Empty Transform positioned just above the top of the camera view.")]
    public Transform spawnPoint;
    [Tooltip("Prefab with SpriteRenderer, Rigidbody2D (Dynamic, gravity=1), Collider2D (isTrigger=true) and IngredientBehavior.")]
    public GameObject ingredientPrefab;
    [Tooltip("Time in seconds between spawns.")]
    public float spawnInterval = 1.5f;
    [Tooltip("Min and max X?positions (world units) for spawn.")]
    public float minX = -8f, maxX = 8f;
    public float baseSpawnInterval = 1.5f;
    private void Start()
    {
        // Begin repeating spawn calls
        InvokeRepeating(nameof(Spawn), 0.5f, spawnInterval);
    }

    public void Spawn()
    {
        // Safety checks
        if (ingredientSprites.Length == 0 || ingredientSprites.Length != scoreValues.Length ||
            ingredientSprites.Length != isBadIngredient.Length)
        {
            Debug.LogError("IngredientSpawner: Arrays are not set up correctly or lengths mismatch.");
            return;
        }

        // Choose a random index
        int i = Random.Range(0, ingredientSprites.Length);

        // Compute spawn position
        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, spawnPoint.position.y, 0f);

        // Instantiate the prefab
        GameObject go = Instantiate(ingredientPrefab, pos, Quaternion.identity);

        // Assign the sprite
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = ingredientSprites[i];

        // Initialize behavior
        var ib = go.GetComponent<IngredientBehavior>();
        if (ib != null)
        {
            GameObject effect = (catchEffects != null && catchEffects.Length > i)
                                ? catchEffects[i] : null;
            ib.Initialize(scoreValues[i], isBadIngredient[i], effect);
        }
        else
        {
            Debug.LogWarning("IngredientSpawner: Missing IngredientBehavior on prefab.");
        }
    }
}
