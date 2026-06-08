using UnityEngine;

public class Actor_Pistol : InterfaceBase_IItem
{
    [Header("Shoot Options")]
    public Transform FirePoint;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public float BulletDamage = 1f;

    [Header("Reload Options")]
    public int maxAmmo = 12;          // 最大子弹数
    public float reloadTime = 1.5f;    // 换弹时间（秒）
    private int currentAmmo;          // 当前子弹
    private bool isReloading = false; // 是否正在换弹

    [Header("UI")]
    public GameObject reloadUI; // 拖入“换弹提示UI”


    void Start()
    {
        // 一开始子弹填满
        currentAmmo = maxAmmo;

        // 确保一开始UI是关闭的
        if (reloadUI != null)
            reloadUI.SetActive(false);
    }


    public override void OnUse()
    {
        // 正在换弹 → 不能射击
        if (isReloading)
            return;

        // 没子弹了 → 自动换弹
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        // 正常射击
        Fire();
    }


    void Fire()
    {
        Debug.Log("탕! (피스톨 단사)");

        // 子弹减少
        currentAmmo--;

        Vector3 pos = FirePoint.position;
        Quaternion dir = FirePoint.rotation;

        GameObject bulletClone = Instantiate(Bullet, pos, dir);
        bulletClone.GetComponent<Actor_Bullet>().SetDamage(BulletDamage);

        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(FirePoint.forward * BulletSpeed, ForceMode.VelocityChange);
        }

        Destroy(bulletClone, 2f);
    }


    // 开始换弹
    public void StartReload()
    {
        if (isReloading) return;
        isReloading = true;

        Debug.Log("피스톨 재장전 시작");

        // 显示换弹UI
        if (reloadUI != null)
            reloadUI.SetActive(true);

        // 等待换弹时间后完成换弹
        Invoke(nameof(FinishReload), reloadTime);
    }


    // 结束换弹
    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;

        Debug.Log("피스톨 재장전 완료");

        // 关闭UI
        if (reloadUI != null)
            reloadUI.SetActive(false);
    }


    // 手动按R键换弹（可选）
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartReload();
        }
    }
}