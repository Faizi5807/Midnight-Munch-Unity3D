// IngredientData.cs
using UnityEngine;

/// <summary>
/// ScriptableObject representing the data for a single ingredient in the game.
/// </summary>
[CreateAssetMenu(fileName = "NewIngredient", menuName = "Game/Ingredient", order = 1)]
public class IngredientData : ScriptableObject
{
    [Header("Basic Info")]
    [Tooltip("Display name of the ingredient.")]
    public string ingredientName;

    [Header("Visuals")]
    [Tooltip("Sprite used to render this ingredient.")]
    public Sprite sprite;

    [Header("Gameplay")]
    [Tooltip("Points awarded when this ingredient is caught.")]
    public int scoreValue = 10;

    [Tooltip("If true, catching this ingredient will penalize the player.")]
    public bool isBadIngredient = false;

    [Header("Optional Effects")]
    [Tooltip("Optional particle effect prefab to spawn on catch.")]
    public GameObject catchEffect;
}