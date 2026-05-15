using TMPro;
using UnityEngine;

public class GetTotheportalText : MonoBehaviour
{
    private TMP_Text GetToTheportal;


    private void Awake()
    {
        GetToTheportal = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        InvokeRepeating("TurnOffAndOn", 0.1f, 1);
    }

    private void TurnOffAndOn()
    {
        GetToTheportal.enabled = !GetToTheportal.enabled;
    }
}
