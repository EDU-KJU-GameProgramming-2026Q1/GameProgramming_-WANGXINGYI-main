using UnityEngine;

public class EX_Weapon : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public float bulletSpeed = 20f;

    [Header("Recoil")]
    public float recoilAmount = 2f;       // Amount of recoil
    public float recoilRecoverSpeed = 5f; // Speed at which recoil recovers
    private Vector3 CurrentRecoil;        // Current recoil value
    private Vector3 TargetRecoil;         // Target recoil value
    public Transform CameraTransform;     // FPS camera transform

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            ApplyRecoil();
        }

        // Recoil recovery
        CurrentRecoil = Vector3.Lerp(CurrentRecoil, Vector3.zero, recoilRecoverSpeed * Time.deltaTime);
        CameraTransform.localEulerAngles = new Vector3(-CurrentRecoil.x, -CurrentRecoil.y, 0);
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

    void ApplyRecoil()
    {
        TargetRecoil = new Vector3(Random.Range(-recoilAmount, recoilAmount), Random.Range(-recoilAmount, recoilAmount), 0);
        CurrentRecoil += TargetRecoil;
    }
}