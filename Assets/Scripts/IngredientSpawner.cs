using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject[] ediblePrefabs;
    public GameObject[] badPrefabs;
    public GameObject[] powerupPrefabs;

    public float spawnInterval = 1.5f;
    private float timer = 0f;

    public float spawnRange = 7.5f;

    public void IncreaseDifficulty(int level)
    {
        spawnInterval = Mathf.Max(0.5f, 1.5f - level * 0.1f);
        // Optionally increase variety, hazards, etc.
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnInterval)
        {
            timer = 0;
            SpawnIngredient();
        }
    }

    void SpawnIngredient()
    {
        float x = Random.Range(-spawnRange, spawnRange);
        Vector3 pos = new Vector3(x, transform.position.y, 0);
        int ingredientType = Random.Range(0, 100);

        GameObject prefabToSpawn;
        if (ingredientType < 75) // 75% edible
            prefabToSpawn = ediblePrefabs[Random.Range(0, ediblePrefabs.Length)];
        else if (ingredientType < 90) // 15% bad
            prefabToSpawn = badPrefabs[Random.Range(0, badPrefabs.Length)];
        else // 10% powerup
            prefabToSpawn = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];

        Instantiate(prefabToSpawn, pos, Quaternion.identity);
    }
}