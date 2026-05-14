using UnityEngine;

public class FlyingEnemy : MonoBehaviour , IDamgeable
{
    [SerializeField] private FpController player;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance = 2f;

    [Header("Score Amount")]
    [SerializeField] private int hitAmount = 10;
    [SerializeField] private int killAmount = 100;

    private Stats staInstance;
    private FpController playerInstance;
    private ScoreManager scoreManager;
    private SpawnEffectManager spawnEffectManager;

    public void TakeDamage(float amount)
    {
        spawnEffectManager.AddHitMarkerEffect();
        hp -= amount;
        if (hp <= 0)
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

    private void Start()
    {
        player = FpController.instance;
        staInstance = Stats.instance;
        scoreManager = ScoreManager.instance;
        spawnEffectManager = SpawnEffectManager.Instance;
    }

    private void Update()
    {
        float currentDistance = Vector3.Distance(transform.position, player.transform.position);

        if (currentDistance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        transform.LookAt(player.transform.position);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            staInstance.TakeDamage(20 * Time.deltaTime);

        }
    }
}
