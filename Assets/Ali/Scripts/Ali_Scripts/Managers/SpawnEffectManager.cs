using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class SpawnEffectManager : MonoBehaviour
{
    public static SpawnEffectManager Instance { get; private set; }

    private FpController player;
    private ScoreManager scoreManager;

    [Header("Hurting Enemy System")]
    [SerializeField] private GameObject bloodBlast;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private AudioSource hitMarkerSound;


    [Header("TransformPos")]
    [SerializeField] private Transform map0;
    [SerializeField] private Transform map1;
    [SerializeField] private Transform map2;
    private int currentMap = 0;
    [SerializeField] private MMF_Player telportFeedback;

    [SerializeField] private GameObject zoneLighing0;
    [SerializeField] private GameObject zoneLighing1;
    [SerializeField] private GameObject zoneLighing2;


    [Header("Spawn Enemy System")]
    [SerializeField] private GameObject[] enemiesType;
    [SerializeField] private Transform[] map0Points;
    [SerializeField] private Transform[] map1Points;
    [SerializeField] private Transform[] map2Points;
    [SerializeField] private int maxEnemyNum;


    [Header("Portal Opening System")]
    [SerializeField] private Portal[] allportals;
    [SerializeField] private int newKillCount = 10;
    [SerializeField] private GameObject getToThePortalTextGameObject;
    [SerializeField] private TMP_Text newkillcountText;
    public bool canScore { get; private set; }


    private void Awake()
    {
        Instance = this;
        canScore = true;
    }

    private void Start()
    {
        player = FpController.instance;
        scoreManager = ScoreManager.instance;


        InvokeRepeating("SpawnEnemiesSystem", 1, 1);

    }
    private void Update()
    {
        newkillcountText.text = newKillCount.ToString();

        if (scoreManager.KillCount >= newKillCount)
        {
            newKillCount = 2 * newKillCount;
            for (int i = 0; i < allportals.Length; i++)
            {
                allportals[i].gameObject.SetActive(true);
                canScore = false;
                // text
                getToThePortalTextGameObject.SetActive(true);
            }
        }


    }

    public void SpawnBloodBlastEffect(Transform bloodSpawn)
    {
        Instantiate(bloodBlast, bloodSpawn.position, bloodSpawn.rotation);

        // add kill count
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
        switch (currentMap)
        {
            case 0:
                player.transform.position = map1.position;
                currentMap = 1;

                zoneLighing0.SetActive(false);
                zoneLighing1.SetActive(true);
                zoneLighing2.SetActive(false);
                break;

            case 1:
                player.transform.position = map2.position;
                currentMap = 2;
                zoneLighing0.SetActive(false);
                zoneLighing1.SetActive(false);
                zoneLighing2.SetActive(true);
                break;

            case 2:
                player.transform.position = map0.position;
                currentMap = 0;
                zoneLighing0.SetActive(true);
                zoneLighing1.SetActive(false);
                zoneLighing2.SetActive(false);
                break;

        }
        telportFeedback.PlayFeedbacks();
        // active again
        player.GetComponent<CharacterController>().enabled = true;

        for (int i = 0; i < allportals.Length; i++)
        {
            allportals[i].gameObject.SetActive(false);
            canScore = true;
            getToThePortalTextGameObject.SetActive(false);
        }
       
        maxEnemyNum = 2* maxEnemyNum;
    }

    private void SpawnEnemiesSystem()
    {
        if (GetEnemyCount() >= maxEnemyNum || enemiesType == null)
        {
            return;
        }

        Transform spawnPoint = null;
        GameObject enemyToSpawn = null;

        switch (currentMap)
        {
            case 0:
                spawnPoint = map0Points[Random.Range(0, map0Points.Length)];
                enemyToSpawn = enemiesType[Random.Range(0, enemiesType.Length)];
                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
                break;

            case 1:
                spawnPoint = map1Points[Random.Range(0, map1Points.Length)];
                enemyToSpawn = enemiesType[Random.Range(0, enemiesType.Length)];
                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
                break;

            case 2:
                spawnPoint = map2Points[Random.Range(0, map2Points.Length)];
                enemyToSpawn = enemiesType[Random.Range(0, enemiesType.Length)];
                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
                break;
        }
    }

    private int GetEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }
}
