using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
public class IngredientBehavior : MonoBehaviour
{
    private int scoreValue;
    private bool isBadIngredient;
    private GameObject catchEffect;

    public void Initialize(int value, bool isBad, GameObject effect = null)
    {
        scoreValue = value;
        isBadIngredient = isBad;
        catchEffect = effect;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Ingredient collided with: {other.name}");

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Collision ignored: Not a player");
            return;
        }

        if (catchEffect != null)
        {
            Instantiate(catchEffect, transform.position, Quaternion.identity);
            Debug.Log("Catch effect instantiated");
        }

        if (isBadIngredient)
        {
            Debug.Log("Bad ingredient caught! Deducting a life.");
            GameManager.Instance.AdjustLives(-1);
            GameManager.Instance.AddScore(-20);
        }
        else
        {
            Debug.Log("Good ingredient caught! Adding score.");
            GameManager.Instance.AddScore(scoreValue);
        }

        Debug.Log("Destroying ingredient...");
        Destroy(gameObject);
    }
}
