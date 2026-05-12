using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : MonoBehaviour, IDamgeable
{
    [SerializeField] private float enemyHp;
    [SerializeField] private Transform playerPOS;


    [Header("Effect")]
    [SerializeField] private AudioSource hitmarkerSound;
    [SerializeField] private GameObject hitmarker;

    [Header("Score Amount")]
    [SerializeField] private int hitAmount = 10;
    [SerializeField] private int killAmount = 100;

    private NavMeshAgent agent;
    private Stats staInstance;
    private FpController playerInstance;
    private ScoreManager scoreManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        staInstance = Stats.instance;
        playerInstance = FpController.instance;
        scoreManager = ScoreManager.instance;
    }


    private void Update()
    {
        float distance = Vector3. Distance(transform.position, transform.forward);
        //  if(!agent.pathPending && distance   )
        agent.SetDestination(playerPOS.position);
    }


    public void TakeDamage(float amount)
    {
        hitmarkerSound.Play();
        hitmarker.SetActive(true);
        Invoke("RemoveHitmarker", 0.5f);
        enemyHp -= amount;
        if (enemyHp <= 0)
        {
            scoreManager.AddKillScore(killAmount);
            Destroy(gameObject);
        }
        else
        {
            scoreManager.AddHitScore(hitAmount);
        }

        
    }

    private void RemoveHitmarker()
    {
        hitmarker.SetActive(false);
    }
   
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == playerInstance.gameObject)
        {
            staInstance.TakeDamage(20 * Time.deltaTime);

        }
    }
}
