using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Timeline;

public class SpawnEffectManager : MonoBehaviour
{
    public static SpawnEffectManager Instance {  get; private set; }

    private FpController player;
    

    [SerializeField] private GameObject bloodBlast;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private AudioSource hitMarkerSound;


    [Header("TransformPos")]
    [SerializeField] private Transform map0;
    [SerializeField] private Transform map1;
    [SerializeField] private Transform map2;
    private int currentMap = 0;
    [SerializeField] private MMF_Player telportFeedback;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FpController.instance;
    }

    public void SpawnBloodBlastEffect(Transform bloodSpawn)
    {
        Instantiate(bloodBlast , bloodSpawn.position, bloodSpawn.rotation);
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


    public void TelportPlayer()
    {
        // i need to stop the CC for lil bit so i can telport the player 
        player.GetComponent<CharacterController>().enabled = false;
        switch(currentMap)
        {
            case 0:
                player.transform.position = map1.position;
                currentMap = 1;
                break;

            case 1:
                player.transform.position = map2.position;
                currentMap = 2;
                break;

            case 2:
                player.transform.position = map0.position;
                currentMap = 0;
                break;

        }
        telportFeedback.PlayFeedbacks();
        // active again
        player.GetComponent<CharacterController>().enabled = true;
    }
}
