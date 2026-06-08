using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EX_Actor_Scene : MonoBehaviour
{
    public string SceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneName = sceneName;
        SceneManager.LoadScene(SceneName);
    }
}
