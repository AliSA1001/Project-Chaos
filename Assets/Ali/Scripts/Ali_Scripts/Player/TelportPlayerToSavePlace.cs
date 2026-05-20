using UnityEngine;

public class TelportPlayerToSavePlace : MonoBehaviour
{
    private FpController player;



    [SerializeField] private Transform TelportPoint;
    private Vector3 lastPlayerPosition;
    private Quaternion lastPlayerRotation;
    private void Start()
    {
        player = FpController.instance;
    }


    public void TelportNow()
    {
        lastPlayerPosition = player.transform.position;
        lastPlayerRotation = player.transform.rotation;
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = TelportPoint.position;
        // here we will play the vid 

        player.GetComponent<CharacterController>().enabled = true;

        Invoke("GetThePlayerBack", 3);
    }


    // Here Get the player back to the point he was in 
    private void GetThePlayerBack()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = lastPlayerPosition;
        player.transform.rotation = lastPlayerRotation;
        player.GetComponent<CharacterController>().enabled = true;


    }
}
