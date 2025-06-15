using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
public class IngredientBehavior : MonoBehaviour
{
    // These values will be set by the spawner:
    private int scoreValue;
    private bool isBadIngredient;
    private GameObject catchEffect;

    /// <summary>
    /// Call this immediately after Instantiate(...) to set up scoring and VFX.
    /// </summary>
    /// <param name="value">Points to add (or subtract if isBad)</param>
    /// <param name="isBad">True if catching this should deduct a life</param>
    /// <param name="effect">Optional particle prefab to spawn on catch</param>
    public void Initialize(int value, bool isBad, GameObject effect = null)
    {
        scoreValue = value;
        isBadIngredient = isBad;
        catchEffect = effect;
    }

    // Trigger-based catching
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only respond to collisions with the player
        if (!other.CompareTag("Player"))
            return;

        // 1) Play catch VFX if assigned
        if (catchEffect != null)
            Instantiate(catchEffect, transform.position, Quaternion.identity);
       
        // 2) Update score or lives via GameManager
        if (isBadIngredient)
        {
            GameManager.Instance.AdjustLives(-1);
        }
        else
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        Destroy(gameObject);
        // 3) Destroy this ingredient

    }
}
