using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score = 0;
    private GunManager gunManager;


    [SerializeField] private TMP_Text scoreNumber;
    [SerializeField] private GameObject hitMarakerUI;
    private bool isHitMarkerActive;

    [SerializeField] private int scoreMultplaier;

    


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gunManager = GunManager.instance;
    }
    private void Update()
    {
        scoreNumber.text = score.ToString();

    }


    public void AddHitScore(int amount)
    {
        score += amount;
        Invoke("RemoveHitmarker", 0.5f);
    }
    public void AddKillScore(int amount)
    {
        score += amount;
        Invoke("RemoveHitmarker", 0.5f);
    }


    private void RemoveHitmarker()
    {
        hitMarakerUI.SetActive(false);
    }


}
