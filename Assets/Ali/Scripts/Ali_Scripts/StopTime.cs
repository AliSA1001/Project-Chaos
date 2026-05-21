using UnityEngine;

public class StopTime : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;


    }

    private void Start()
    {
        Invoke("ReactiveTime", 3);
    }

    private void ReactiveTime()
    {
        Time.timeScale = 1f;
    }
}
