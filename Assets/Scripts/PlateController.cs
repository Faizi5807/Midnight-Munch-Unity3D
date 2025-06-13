using UnityEngine;

public class PlateController : MonoBehaviour
{
    public float speed = 10f;
    public float boundary = 7.5f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x + move, -boundary, boundary);
        transform.position = pos;
    }
}