using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS2_Bullet_Class : MonoBehaviour
{
    private int bulletDamage;
    private bool isHit = false;

    public void SetDamage(int aount)
    {
        bulletDamage = aount;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;

        if (collision .gameObject .CompareTag ("Target"))
        {
            isHit = true;
            Debug.Log("Hit Target!Damage:" + bulletDamage);

        }

        Destroy(gameObject);

    }
}
