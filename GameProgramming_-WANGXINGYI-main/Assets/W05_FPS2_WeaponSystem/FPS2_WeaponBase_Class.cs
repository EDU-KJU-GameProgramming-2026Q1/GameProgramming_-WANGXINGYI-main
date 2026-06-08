using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS2_WeaponBase_Class : MonoBehaviour
{
    public string weaponName;
    public int damage = 10;
    public float fireRate = 0.2f;
    public float bulletSpeed = 30f;
    public GameObject BulletPrefab;
    public Transform FirePoint;
    protected float nextFireTime = 0f;

   protected virtual void Update()
    {
        if (Input .GetButton("Fire1")&& Time .time >=nextFireTime )
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }
    protected virtual void  Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

        FPS2_Bullet_Class bulletScript = bullet.GetComponent<FPS2_Bullet_Class>();

        if  (bulletScript !=null)
        {
            bulletScript.SetDamage(damage);

        }
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if(rb!=null)
        {
            rb.velocity = FirePoint.forward * bulletSpeed;

        }
        Destroy(bullet, 3f);
    }
}
