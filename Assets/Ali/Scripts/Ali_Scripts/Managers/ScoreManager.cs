using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    private GunManager gunManager;
    private SpawnEffectManager spawnEffectManager;


    [SerializeField] private TMP_Text scoreNumber;
    [SerializeField] private TMP_Text currentDeathCountText;
    [SerializeField] private GameObject hitMarakerUI;
    public int KillCount {  get; private set; }

    private bool isHitMarkerActive;
    


    private void Awake()
    {
        instance = this;
        KillCount = 0;
    }

    private void Start()
    {
        gunManager = GunManager.instance;
        spawnEffectManager = SpawnEffectManager.Instance;
    }
    private void Update()
    {
        scoreNumber.text = score.ToString();
        currentDeathCountText.text = KillCount.ToString();
    }


    public void AddHitScore(int amount)
    {
        if (spawnEffectManager.canScore )
        {
            score += amount;
            Invoke("RemoveHitmarker", 0.5f);
        }
    }
    public void AddKillScore(int amount)
    {
        if (spawnEffectManager.canScore)
        {
            score += amount;
            KillCount += 1;
            Invoke("RemoveHitmarker", 0.5f);
        }
    }


    private void RemoveHitmarker()
    {
        hitMarakerUI.SetActive(false);
    }

    public int CheckScore()
    {
        return score;
    }
}
