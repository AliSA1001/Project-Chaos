using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour , IDamgeable
{
    public static Stats instance { get; private set; }



    [SerializeField] private float hp = 100;
    [SerializeField] private float maxHp = 100;




    [Header("TMP REF HERE")]
    [SerializeField] private TMP_Text text_HP;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void TakeDamage(float amount)
    {
        hp -= amount;
        // damage taken effect
        bl_DamageScreen.UpdateHealth(Mathf.CeilToInt(hp), Mathf.RoundToInt(maxHp));
    }

    private void Update()
    {
        text_HP.text = hp.ToString();
        if (hp <= 0)
        {
            Debug.Log("you are dead");
            SceneManager.LoadScene(2);
        }

    }
}
