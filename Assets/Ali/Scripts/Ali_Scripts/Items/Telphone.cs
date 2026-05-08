using UnityEngine;

public class Telphone : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject normalUI;
    [SerializeField] private GameObject telphoneUI;

    public void Interact()
    {
       normalUI.SetActive(false);
        telphoneUI.SetActive(true);
    }
}
