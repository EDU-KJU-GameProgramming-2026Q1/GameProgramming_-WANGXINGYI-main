using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Bullet_Enhanced : MonoBehaviour
{
    public GameObject ShootParticle, HitParticle, MissParticle;
    EX_ScoreManager ScoreManager;
    bool isHit = false;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
        GameObject shootParticle = Instantiate(ShootParticle, transform.position - transform.forward*1.8f, transform.rotation);
        Destroy(shootParticle, 2f);
        ScoreManager = Object.FindAnyObjectByType<EX_ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;
        if (collision.gameObject.tag == "Target")
        {
            isHit = true;
            GameObject Particle = Instantiate(HitParticle, transform.position, transform.rotation);
            Destroy(Particle, 2f);
            print("Hit");
            ScoreManager.AddScore();
            Destroy(collision.gameObject);
         
        }
        else
        {
            isHit = true;
            GameObject Particle = Instantiate(MissParticle, transform.position, transform.rotation);
            Destroy(Particle, 2f);
            print("Miss");
        }
        Destroy(gameObject);
    }
}
