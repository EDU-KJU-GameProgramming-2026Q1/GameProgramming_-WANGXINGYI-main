using UnityEngine;

public class Actor_Shotgun : InterfaceBase_IItem
{
    [Header("Shoot Options")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public float BulletDamage = 1f;

    [Header("Shotgun Spread")]
    public int pelletCount = 8;      // 一次几发弹丸
    public float spreadAngle = 8f;   // 散射角度

    [Header("Reload Options")]
    public int maxAmmo = 8;
    public float reloadTime = 2f;
    private int currentAmmo;
    private bool isReloading = false;

    [Header("UI")]
    public GameObject reloadUI;

    void Start()
    {
        currentAmmo = maxAmmo;
        if (reloadUI != null)
            reloadUI.SetActive(false);
    }

    public override void OnUse()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        Fire();
    }

    void Fire()
    {
        Debug.Log("펑! (샷건 발사)");
        currentAmmo--;

        Vector3 pos = FirePoint.position;

        for (int i = 0; i < pelletCount; i++)
        {
            // 随机散射
            Quaternion spreadRot = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0
            );
            Quaternion finalRot = FirePoint.rotation * spreadRot;

            GameObject bulletClone = Instantiate(Bullet, pos, finalRot);
            bulletClone.GetComponent<Actor_Bullet>().SetDamage(BulletDamage);

            Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(finalRot * Vector3.forward * BulletSpeed, ForceMode.VelocityChange);
            }

            Destroy(bulletClone, 2f);
        }
    }

    public void StartReload()
    {
        if (isReloading) return;
        isReloading = true;

        Debug.Log("샷건 재장전 시작");

        if (reloadUI != null)
            reloadUI.SetActive(true);

        Invoke(nameof(FinishReload), reloadTime);
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;

        Debug.Log("샷건 재장전 완료");

        if (reloadUI != null)
            reloadUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartReload();
        }
    }
}