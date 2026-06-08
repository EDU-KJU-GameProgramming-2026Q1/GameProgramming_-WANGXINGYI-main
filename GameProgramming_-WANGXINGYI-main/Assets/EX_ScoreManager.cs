using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EX_ScoreManager : MonoBehaviour
{
    public TMP_Text text;
    int currentScore = 0;
    public int hitPoint = 1;
    // Start is called before the first frame update
    public void AddScore()
    {
        currentScore += hitPoint;
        text.text = currentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input .GetKeyDown ( KeyCode.Alpha1))
        {
            AddScore();
        }
    }
}
