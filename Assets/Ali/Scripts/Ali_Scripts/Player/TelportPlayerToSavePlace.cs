using UnityEngine;

public class TelportPlayerToSavePlace : MonoBehaviour
{
    private FpController player;



    [SerializeField] private Transform TelportPoint;

    private void Start()
    {
        player = FpController.instance;
    }


    public void TelportNow()
    {

        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = TelportPoint.position;
        // here we will play the vid 

        player.GetComponent<CharacterController>().enabled = true;


    }

}
