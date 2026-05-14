using UnityEngine;

public class Portal : MonoBehaviour
{
   private SpawnEffectManager spawnEffectManager;
    private FpController player;


    private void Start()
    {
        spawnEffectManager = SpawnEffectManager.Instance;
        player = FpController.instance;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnEffectManager.TelportPlayer();
        }
    }
}
