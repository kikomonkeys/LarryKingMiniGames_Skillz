using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicNew : MonoBehaviour
{
    public static int Music
    {
        get
        {
            return PlayerPrefs.GetInt("myMusic", 1);
        }
        set
        {
            PlayerPrefs.SetInt("myMusic", value);
        }
    }
    public static BGMusicNew instance;
    public AudioSource bgSource;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetMusicInfo();
    }
    public void SetMusicInfo()
    {
        if (Music == 0)
        {
            bgSource.enabled = false;
        }
        else
        {
            bgSource.enabled = true;
        }
    }

    public void MusicBtnClicked()
    {

    }
    
}
