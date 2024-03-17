using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackball_Timer : MonoBehaviour
{
    [SerializeField]
    private float time=180f;
    public static Stackball_Timer instance;
    private void Awake()
    {if(instance != null)
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }
        else
        {
            instance = this;
        }
       
        
            DontDestroyOnLoad(this.gameObject);
        
        
    }

    public  float GetTime()
    {
        return time;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    float min, sec;
    void Update()
    {
        if (GameUI.instance.starttimer)
        {
            if (time > 0)
            {
                time -= Time.deltaTime * .7f;

            }
        }
    }

    public void restartTime()
    {
        time = 180;
    }
    public void AddTimeOnWatchVideo()
    {
        time = 120;
    }
}
