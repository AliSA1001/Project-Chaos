using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour, IDamgeable
{
    public static Stats instance { get; private set; }

    [SerializeField] private float hp = 100;
    private float maxHp; 

    [Header("UI References")]
    [SerializeField] private TMP_Text text_HP;
    [SerializeField] private Slider hpSlider; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        maxHp = hp;
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }

        UpdateUI(); 
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;

        UpdateUI(); 

        if (hp <= 0)
        {
            Debug.Log("you are dead");
            SceneManager.LoadScene(2);
        }
    }

   
    private void UpdateUI()
    {
        if (text_HP != null)
        {
            text_HP.text = hp.ToString();
        }

        if (hpSlider != null)
        {
            hpSlider.value = hp;
        }
    }
}