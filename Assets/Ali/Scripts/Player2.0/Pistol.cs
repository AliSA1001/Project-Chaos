using UnityEngine;

public class Pistol : MasterGun
{
    protected override void Recoil()
    {
        gunAnimator.SetTrigger("Fire");
    }
}
