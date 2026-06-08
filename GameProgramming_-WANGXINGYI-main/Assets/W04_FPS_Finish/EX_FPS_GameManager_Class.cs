using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EX_FPS_GameManager_Class : MonoBehaviour
{
    public GameObject GameOverPanel;
    public TMP_Text TimeText;

    bool isPlaying = true;
    float currentTime;
    public float maxTime = 60f;

    public W03_Class_Weapon PlayerWeapon;
    public Ex_TargetSpawner Spawner;



    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false);
        TimeText.text = maxTime.ToString("F1");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        currentTime = maxTime - Time.time;

        if (currentTime<=10f )
        {
            TimeText.color = Color.red;
        }

        if (currentTime <=0f)
        {
            isPlaying = false;
            currentTime = 0f;

            GameOverPanel.SetActive(true);

            PlayerWeapon.enabled = false;

            Spawner.enabled = false;

        }
        TimeText.text = currentTime.ToString("F1");

    }
}
