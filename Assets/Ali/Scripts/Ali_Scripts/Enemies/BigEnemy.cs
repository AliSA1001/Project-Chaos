using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    [SerializeField] private float enemyHp;
    [SerializeField] private Transform playerPOS;


    [Header("Score Amount")]
    [SerializeField] private int hitAmount = 10;
    [SerializeField] private int killAmount = 100;


    [Header("AI")]
    [SerializeField] private float distanceToAttack;

    private UnityEngine.AI.NavMeshAgent agent;
    private Stats staInstance;
    private FpController playerInstance;
    private ScoreManager scoreManager;
    private SpawnEffectManager spawnEffectManager;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        staInstance = Stats.instance;
        playerInstance = FpController.instance;
        scoreManager = ScoreManager.instance;
        spawnEffectManager = SpawnEffectManager.Instance;

        playerPOS = playerInstance.gameObject.transform;
    }


    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerPOS.position);
        if (!agent.pathPending && distance > distanceToAttack)
        {
            agent.SetDestination(playerPOS.position);
        }
    }


    public void TakeDamage(float amount)
    {
        spawnEffectManager.AddHitMarkerEffect();
        enemyHp -= amount;
        if (enemyHp <= 0)
        {
            scoreManager.AddKillScore(killAmount);
            spawnEffectManager.SpawnBloodBlastEffect(gameObject.transform);
            Destroy(gameObject);
        }
        else
        {
            scoreManager.AddHitScore(hitAmount);
        }

    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerInstance.gameObject)
        {
            staInstance.TakeDamage(20 * Time.deltaTime);

        }
    }
}
