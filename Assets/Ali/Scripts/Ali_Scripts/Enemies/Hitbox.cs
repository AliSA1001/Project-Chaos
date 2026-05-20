using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Stats stats;


    private void Start()
    {
        stats = Stats.instance;
    }

    private void OnEnable()
    {
        Invoke("DeactivesSelf", 0.1f);
    }

    private void DeactivesSelf()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == stats.gameObject)
        {
            stats.TakeDamage(25);
        }
        
            
        
    }
}
