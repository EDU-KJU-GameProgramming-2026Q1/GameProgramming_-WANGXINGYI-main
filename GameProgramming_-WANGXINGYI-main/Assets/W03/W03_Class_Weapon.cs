using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W03_Class_Weapon : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;

    public float bulletSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Shoot");
            Shoot();
            //ApplyRecoil();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position + FirePoint.forward, FirePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = FirePoint.forward * bulletSpeed;
        }
        Destroy(bullet, 3f);
    }
}
