using MoreMountains.Feedbacks;
using UnityEngine;

public class TelportPlayerToSavePlace : MonoBehaviour
{
    private FpController player;



    [SerializeField] private Transform TelportPoint;
    private Vector3 lastPlayerPosition;
    private Quaternion lastPlayerRotation;
    [SerializeField] private AudioSource song3;

    [SerializeField] private GameObject endVid;
    private void Start()
    {
        player = FpController.instance;
    }


    public void TelportNow()
    {
        song3.Stop();
        lastPlayerPosition = player.transform.position;
        lastPlayerRotation = player.transform.rotation;
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = TelportPoint.position;
        PlaytheEnding();
        player.GetComponent<CharacterController>().enabled = true;


    }


    private void PlaytheEnding()
    {
        endVid.SetActive(true);

        Invoke("GetThePlayerBack", 55);

    }

    // Here Get the player back to the point he was in 
    private void GetThePlayerBack()
    {
        endVid.SetActive(false);

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = lastPlayerPosition;
        player.transform.rotation = lastPlayerRotation;
        player.GetComponent<CharacterController>().enabled = true;


    }
}
