using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;
    public PlateController plate;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ActivatePowerUp(string type)
    {
        switch (type)
        {
            case "SlowMotion":
                StartCoroutine(SlowMotion());
                break;
            case "DoublePlate":
                plate.speed *= 2;
                // Enlarge plate visually
                break;
        }
    }

    System.Collections.IEnumerator SlowMotion()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1f;
    }
}