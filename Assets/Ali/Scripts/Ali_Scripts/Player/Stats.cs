using TMPro;
using UnityEngine;
using UnityEngine.UI; // 1. Required to access the Slider component
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour, IDamgeable
{
    public static Stats instance { get; private set; }

    [SerializeField] private float hp = 100;
    private float maxHp; // 2. Keep track of the starting health for the slider's maximum

    [Header("UI References")]
    [SerializeField] private TMP_Text text_HP;
    [SerializeField] private Slider hpSlider; // 3. The reference to your UI Slider

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
        maxHp = hp; // Store the initial health

        // Set up the slider's initial values
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }

        UpdateUI(); // Make sure the UI is correct right when the game starts
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;

        UpdateUI(); // 4. Update the slider and text ONLY when health changes

        if (hp <= 0)
        {
            Debug.Log("you are dead");
            SceneManager.LoadScene(2);
        }
    }

    // A dedicated method to handle all UI updates
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