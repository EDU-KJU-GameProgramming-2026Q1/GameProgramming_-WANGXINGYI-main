using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W03_Class_Bullet : MonoBehaviour
{
    public GameObject ShootParticle, HitParticle, MissParticle;
    // Start is called before the first frame update
    void Start()
    {
        if (ShootParticle != null)
        {
            GameObject shootParticle = Instantiate(ShootParticle, transform.position, transform.rotation);
            Destroy(shootParticle, 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (HitParticle != null)
            {
                GameObject hitParticle = Instantiate(HitParticle, transform.position, transform.rotation);
                Destroy(hitParticle, 2f);
            }
            Destroy(collision.gameObject);
        }
        else
        {
            if (MissParticle != null)
            {
                GameObject missParticle = Instantiate(MissParticle, transform.position, transform.rotation);
                Destroy(missParticle, 2f);
            }
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }
}