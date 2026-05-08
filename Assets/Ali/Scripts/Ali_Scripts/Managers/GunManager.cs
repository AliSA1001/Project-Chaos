using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    // this script will handle the our guns slots system and active the right gun
    public GunManager instance {  get; private set; }

    [Header("Guns")]
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject shotGun;


    [Header("Slots")]
    private GameObject[] slots = new GameObject[2];// for the guns
    private GameObject[] slots_UI = new GameObject[2]; // for the ui we will set them both at the same time


    [Header("Guns")]
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun1_ui;
    [Header("Guns UI")]
    [SerializeField] private GameObject gun2;
    [SerializeField] private GameObject gun2_ui;
    // we give id numbers to the guns
    // dulepistol will be 1 , Rifle 2 and shotgun 3 


    private void Awake()
    {
        slots[0] = gun1;
        slots[1] = gun2;

        slots_UI[0] = gun1_ui;
        slots_UI[1] = gun2_ui;
    }
    private void Start()
    {
        slots[0].SetActive(true);
        slots_UI[0].SetActive(true);
    }

    private void Update()
    {
        
    }

    private void HandleGunSwaping()
    {
       /* switch(gunNum)
        {
            case 1:
                dulePistol.gameObject.SetActive(true);
                ammoAmount_dulePistol.SetActive(true);

                rifle.gameObject.SetActive(false); 
                ammoAmount_rifle.SetActive(false);

                break;
                case 2:
                dulePistol.gameObject.SetActive(false);
                ammoAmount_dulePistol.SetActive(false);

                rifle.gameObject.SetActive(true);
                ammoAmount_rifle.SetActive(true);
                break;

       */// }
        if (slots[0].gameObject.activeInHierarchy)
        {
            slots[0].gameObject.SetActive(false);
            slots_UI[0].gameObject.SetActive(false);

            slots[1].gameObject.SetActive(true);
            slots_UI[1].gameObject.SetActive(true);

        }
        else if (slots[1].gameObject.activeInHierarchy)
        {
            slots[0].gameObject.SetActive(true);
            slots_UI[0].gameObject.SetActive(true);

            slots[1].gameObject.SetActive(false);
            slots_UI[1].gameObject.SetActive(false);
        }
      
    }

    public void OnSwap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float scrolValue = context.ReadValue<Vector2>().y;
            if (Mathf.Abs(scrolValue) > 0.1f)
            {
                HandleGunSwaping();
            }
        }
    }

    public void AddpistolToSlot()
    {
       if(slots[0].gameObject== null)
        {
            slots[0] = pistol;
        }
       else if (slots[1].gameObject== null)
        {
            slots[1] = pistol;
        }
        else
        {
            Debug.Log("you dont have space");
        }
    }


}
