using UnityEngine;
using UnityEngine.Timeline;

public class SpawnEffectManager : MonoBehaviour
{
    public static SpawnEffectManager Instance;


    [SerializeField] private GameObject bloodBlast;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private AudioSource hitMarkerSound;
   // [SerializeField] private AudioSource enemyDeathSound;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBloodBlastEffect(Transform bloodSpawn)
    {
        Instantiate(bloodBlast , bloodSpawn.position, bloodSpawn.rotation);
     //   enemyDeathSound.Play();
    }

    public void AddHitMarkerEffect()
    {
        hitMarkerSound.Play();
        hitMarker.SetActive(true);
        Invoke("RemoveHitmarker", 0.5f);
    }

    private void RemoveHitmarker()
    {
        hitMarker.SetActive(false);
    }
}
