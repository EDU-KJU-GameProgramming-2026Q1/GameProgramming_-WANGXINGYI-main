using UnityEngine;

public class FPS2_Weapon_Shotgun_homework : FPS2_WeaponBase_Class
{
    [Header("霰弹枪专属设置")]
    public Transform firePoint;
    public int pellets = 8;
    public float spreadAngle = 8f;
    public GameObject bulletPrefab;


    protected override void Update()
    {
        // 和手枪保持完全一致的逻辑
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < pellets; i++)
        {           
            Vector3 dir = firePoint.forward;
            dir = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0
            ) * dir;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }
            Destroy(bullet, 3f);
        }
    }
}
