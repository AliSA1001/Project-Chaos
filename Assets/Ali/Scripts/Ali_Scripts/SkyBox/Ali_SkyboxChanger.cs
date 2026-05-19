using UnityEngine;

public class Ali_SkyboxChanger : MonoBehaviour
{
    [SerializeField] private Material zoneSkybox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.skybox = zoneSkybox;
        }
    }
}
