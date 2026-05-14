using MoreMountains.Feedbacks;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DulePistol : MonoBehaviour
{
    [Header("Connections")]
    // 'protected' means the Child (Shotgun) can see this, but other scripts cannot.
    [SerializeField] protected RaycastHit gunRaycastInfo;
    [SerializeField] private Transform maincam;
    private FpController player;


    [Header("Stats")]
    [SerializeField] protected float gunRange = 100f;
    [SerializeField] protected float gunDamage = 10f;
    [SerializeField] protected float fireRate = 0.5f;
    [SerializeField] protected float bulletSpeed = 50f;
    [SerializeField] protected int gunAmmo = 999;
    [SerializeField] private int maxAmmo;


    [Header("Visuals")]
    [SerializeField] protected ParticleSystem muzzleEffect;
    [SerializeField] protected float muzzleEffectDuration = 0.1f;
    [SerializeField] protected Animator gunAnimator;
    [SerializeField] protected TrailRenderer bulletTrail;
    [SerializeField] protected ParticleSystem impactParticleSystem;
    [SerializeField] protected Transform trailSpawnPoint1;
    [SerializeField] protected Transform trailSpawnPoint2;
    [SerializeField] protected Transform currentTrailSpawnPoint;
    [SerializeField] protected bool isPoint1;

    [Header("hitscan")]
    [SerializeField] LayerMask hitLayers;


    [Header("TMP REF HERE")]
    [SerializeField] private TMP_Text text_Ammo;


    // Timer to track when we can shoot again
    protected float nextFireTime;


    //input button
    protected bool attackTrigger;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player reloadFeedback;
    [SerializeField] private MMF_Player ShootFeedback;
    [SerializeField] private MMF_Player ShootFeedback0;


    public void Start()
    {

        if (maincam == null)
        {
            maincam = Camera.main.transform;
        }
        player = FpController.instance;


    }

    public void Update()
    {
        HandleShooting();

        HandleAnmationSprinting();

        text_Ammo.text = gunAmmo.ToString();

    }

    private void HandleAnmationSprinting()
    {
        if (player.moveSpeed >= 7)
        {
            gunAnimator.SetBool("Moving", true);
        }
        else
        {
            gunAnimator.SetBool("Moving", false);
        }
    }
    private void HandleReload()
    {
        gunAmmo = maxAmmo;
    }

    protected virtual void HandleShooting()
    {
        if (attackTrigger && nextFireTime <= Time.time)
        {
            if (gunAmmo <= 0)
            {
            }
            else
            {

                if (isPoint1)
                {
                    gunAnimator.SetTrigger("Shooting1");
                    ShootFeedback.PlayFeedbacks();
                }
                else
                {
                    gunAnimator.SetTrigger("Shooting2");
                    ShootFeedback0.PlayFeedbacks();

                }
                if (HandleHitScan(out gunRaycastInfo))
                {
                    IDamgeable damageable = gunRaycastInfo.collider.GetComponent<IDamgeable>();

                    if (damageable != null)
                    {
                        damageable.TakeDamage(gunDamage);
                    }
                    StartCoroutine(HandleTrail(gunRaycastInfo));
                    if (isPoint1)
                    {
                        currentTrailSpawnPoint = trailSpawnPoint2;
                        isPoint1 = false;
                    }
                    else if (!isPoint1)
                    {
                        currentTrailSpawnPoint = trailSpawnPoint1;
                        isPoint1 = true;
                    }
                }
                else
                {
                    StartCoroutine(HandleLostTrail());// if we didnt hit anything in the range of the gun 
                    if (isPoint1)
                    {
                        currentTrailSpawnPoint = trailSpawnPoint2;
                        isPoint1 = false;
                    }
                    else if (!isPoint1)
                    {
                        currentTrailSpawnPoint = trailSpawnPoint1;
                        isPoint1 = true;
                    }
                }
                gunAmmo--;
                nextFireTime = Time.time + fireRate;
            }
        }
        
    }
   


    // protected virtual IEnumerator Flashmuzzle()
    // {
    //   muzzleEffect.SetActive(true);
    //   yield return new WaitForSeconds(muzzleEffectDuration);
    //   muzzleEffect.SetActive(false);

    //  }


    protected virtual void Recoil()
    {
        gunAnimator.Play("Shoting");

    }


    protected virtual bool HandleHitScan(out RaycastHit hitInfo)
    {

        Debug.DrawRay(maincam.position, maincam.forward * gunRange, Color.red, 2f);// just so we can see the line
        if (Physics.Raycast(maincam.position, maincam.forward, out hitInfo, gunRange, hitLayers))
        {
            return true;
        }
        return false;
    }


    protected virtual IEnumerator HandleTrail(RaycastHit gunRaycasthitInfo)
    {
        TrailRenderer instance = Instantiate(bulletTrail, currentTrailSpawnPoint.position, Quaternion.identity);
        while (Vector3.Distance(instance.transform.position, gunRaycasthitInfo.point) > 0.1f)
        {
            instance.transform.position = Vector3.MoveTowards(
                instance.transform.position,
                gunRaycasthitInfo.point,
                bulletSpeed * Time.deltaTime

                );
            yield return null;// wait for the next frame and redo the while again 

        }
        ParticleSystem instanceofParticleSystem = Instantiate(impactParticleSystem, gunRaycasthitInfo.point, Quaternion.LookRotation(gunRaycastInfo.normal));
        Destroy(instanceofParticleSystem.gameObject, 2f);
        Destroy(instance.gameObject, instance.time);
    }


    protected virtual IEnumerator HandleLostTrail()
    {
        Vector3 longetPointYouCanGetToInRange = maincam.transform.position + (maincam.forward * gunRange);
        TrailRenderer instance = Instantiate(bulletTrail, currentTrailSpawnPoint.position, Quaternion.identity);
        while (Vector3.Distance(instance.transform.position, longetPointYouCanGetToInRange) > 0.1f)
        {
            instance.transform.position = Vector3.MoveTowards(
                instance.transform.position,
                longetPointYouCanGetToInRange,
                bulletSpeed * Time.deltaTime
                );
            yield return null;
        }

        Destroy(instance.gameObject, instance.time);
    }

    public virtual void OnAttack(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            attackTrigger = true;

        }
        else if (context.canceled)
        {
            attackTrigger = false;
        }
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started && gunAmmo < maxAmmo)
        {
            gunAnimator.SetTrigger("Reloading");
            reloadFeedback.PlayFeedbacks();

        }
    }
}
