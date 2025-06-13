using UnityEngine;

public enum IngredientType { Edible, Bad, PowerUp }

public class Ingredient : MonoBehaviour
{
    public IngredientType ingredientType;
    public int scoreValue = 1;
    public AudioClip catchSound, badSound, powerupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plate"))
        {
            switch (ingredientType)
            {
                case IngredientType.Edible:
                    GameManager.Instance.AddScore(scoreValue);
                    break;
                case IngredientType.Bad:
                    GameManager.Instance.LoseLife();
                    break;
                case IngredientType.PowerUp:
                    // PowerUpManager.Instance.ActivatePowerUp(...);
                    break;
            }
            Destroy(gameObject);
        }
    }
}