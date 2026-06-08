using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS2_Weapon_Pistol_Class : FPS2_WeaponBase_Class
{
    protected override void Update ()
    {
        if (Input .GetButtonDown("Fire1")&& Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }
}
