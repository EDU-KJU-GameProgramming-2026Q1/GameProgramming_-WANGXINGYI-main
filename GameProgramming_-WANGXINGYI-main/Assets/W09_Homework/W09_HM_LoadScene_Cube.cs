using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W09_HM_LoadScene_Cube : MonoBehaviour
{
    public string SceneName;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneName);
    }
}