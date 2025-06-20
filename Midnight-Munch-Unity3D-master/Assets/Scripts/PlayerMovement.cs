using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float boundary = 8f; // Screen boundary for movement restriction

    void Update()
    {
        // Get horizontal input (arrow keys or A/D keys)
        float h = Input.GetAxis("Horizontal");

        // Calculate new position
        Vector3 newPosition = transform.position + new Vector3(h, 0, 0) * speed * Time.deltaTime;

        // Clamp position to stay within the boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, -boundary, boundary);

        // Apply position to the sprite
        transform.position = newPosition;
    }
}
