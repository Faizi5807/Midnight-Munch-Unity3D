using UnityEngine;
using System.Collections;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Ingredient Data")]
    public Sprite[] ingredientSprites;
    public int[] scoreValues;
    public bool[] isBadIngredient;
    public GameObject[] catchEffects;
    public float baseSpawnInterval = 1.5f;
    [Header("Spawning")]
    public Transform spawnPoint;
    public GameObject ingredientPrefab;
    public float spawnInterval = 1.5f;
    public float minX = -8f, maxX = 8f;

    private bool isSpawning = false;
    private Coroutine spawnCoroutine;
    private object spawnIntervalLock = new object();
    private void Start()
    {
        StartSpawning();
    }
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
        return;
    }

    public float GetCurrentSpawnInterval()
    {
        lock (spawnIntervalLock)
        {
            return spawnInterval;
        }
    }
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void Spawn()
    {
        if (ingredientSprites.Length == 0 || ingredientSprites.Length != scoreValues.Length ||
            ingredientSprites.Length != isBadIngredient.Length)
        {
            Debug.LogError("IngredientSpawner: Arrays are not set up correctly or lengths mismatch.");
            return;
        }

        int i = Random.Range(0, ingredientSprites.Length);
        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, spawnPoint.position.y, 0f);

        GameObject go = Instantiate(ingredientPrefab, pos, Quaternion.identity);

        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = ingredientSprites[i];

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
