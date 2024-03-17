using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public bool startTimer;
    public float gameTime;
    public Text timerText;

    void Start()
    {
        startTimer = true;
    }
    float min, sec;
    string niceTime;
    void Update()
    {
        if (startTimer)
        {
            gameTime -= Time.deltaTime;
            min = Mathf.FloorToInt(gameTime / 60F);
            sec = Mathf.FloorToInt(gameTime - min * 60);
            niceTime = string.Format("{0:0}:{1:00}", min, sec);
            timerText.text = "" + niceTime;
            if (gameTime <= 0)
            {
                startTimer = false;
                gameTime = 0;
                Debug.LogError("time up");
            }
        }
    }
}
