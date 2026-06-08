using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class W07_HomeWork_Cube : MonoBehaviour
{
    public string SceneName;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneName);
    }
}
