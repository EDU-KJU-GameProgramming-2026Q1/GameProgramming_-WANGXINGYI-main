using UnityEngine;
using UnityEngine.UI;

public class Actor_Rifle : InterfaceBase_IItem
{
    [Header("射击设置")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public float BulletDamage = 5f;

    [Header("换弹设置")]
    [Tooltip("单弹夹最大子弹数")]
    public int MaxMagazineAmmo = 30;
    [Tooltip("换弹耗时(秒)")]
    public float ReloadTime = 1.5f;

    [Header("UI 引用")]
    [Tooltip("换弹提示文本/物体")]
    public GameObject ReloadTipUI;

    private bool isFiring = false;
    private bool isReloading = false;
    private float lastFireTime;
    private int currentAmmo;

    private void Start()
    {
        currentAmmo = MaxMagazineAmmo;
        // 初始隐藏换弹提示
        if (ReloadTipUI != null)
            ReloadTipUI.SetActive(false);
    }

    public override void OnUse()
    {
        isFiring = true;
    }

    public override void OnStopUse()
    {
        isFiring = false;
    }

    private void Update()
    {
        if (isReloading) return;

        // R键换弹
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // 开火判断
        if (isFiring && currentAmmo > 0 && Time.time >= lastFireTime + itemData.FireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    void Reload()
    {
        if (isReloading || currentAmmo >= MaxMagazineAmmo) return;

        isReloading = true;
        isFiring = false;

        // 显示换弹UI
        if (ReloadTipUI != null)
            ReloadTipUI.SetActive(true);

        Invoke(nameof(FinishReload), ReloadTime);
    }

    void FinishReload()
    {
        currentAmmo = MaxMagazineAmmo;
        isReloading = false;

        // 隐藏换弹UI
        if (ReloadTipUI != null)
            ReloadTipUI.SetActive(false);
    }

    void Fire()
    {
        currentAmmo--;

        Vector3 pos = FirePoint.position;
        Quaternion dir = FirePoint.rotation;
        GameObject bulletClone = Instantiate(Bullet, pos, dir);

        Actor_Bullet bullet = bulletClone.GetComponent<Actor_Bullet>();
        if (bullet != null)
            bullet.SetDamage(BulletDamage);

        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(FirePoint.forward * BulletSpeed, ForceMode.VelocityChange);

        Destroy(bulletClone, 2f);
    }

    #region 子弹数量接口（供UI调用）
    public int GetCurrentAmmo() => currentAmmo;
    public int GetMaxAmmo() => MaxMagazineAmmo;
    public bool IsReloading() => isReloading;
    #endregion
}