using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOrMusicBtn : MonoBehaviour
{
    public GameObject buttonBg;
    public Sprite buttonBgOnSpr, buttonBgOffSpr;
    public GameObject onObj, offObj;
    public static int Music
    {
        get
        {
            return PlayerPrefs.GetInt("musicc", 0);
        }
        set
        {
            PlayerPrefs.SetInt("musicc", value);
        }
    }
    public static int Sound
    {
        get
        {
            return PlayerPrefs.GetInt("sound", 0);
        }
        set
        {
            PlayerPrefs.SetInt("sound", value);
        }
    }
    void Start()
    {
        ShowSoundUI();
        ShowMusicUI();
    }
    int sCount;
    public void OnSoundButtonClick()
    {
        Debug.LogError("sound buton clicked" + Sound);
        sCount++;
        if (sCount % 2 == 0)
        {
            Sound = 0;
        }
        else
        {
            Sound = 1;
        }
        ShowSoundUI();
    }
    void ShowSoundUI()
    {
        if (Sound == 1)
        {
            onObj.SetActive(false);
            offObj.SetActive(true);
            Stackball_GameStake.SoundManager.instance.sound = false;
            if (buttonBg)
            {
                buttonBg.GetComponent<Image>().sprite = buttonBgOffSpr;
            }
        }
        else
        {
            onObj.SetActive(true);
            offObj.SetActive(false);
            Stackball_GameStake.SoundManager.instance.sound = true;
            if (buttonBg)
            {
                buttonBg.GetComponent<Image>().sprite = buttonBgOnSpr;
            }
        }
    }
    int mCount;
    public void OnMusicBtnClicked()
    {
        Debug.LogError("music buton clicked" + Music);
        mCount++;
        if (mCount % 2 == 0)
        {
            Music = 0;
        }
        else
        {
            Music = 1;
        }
        ShowMusicUI();
    }
    void ShowMusicUI()
    {
        if (Music == 1)
        {
            onObj.SetActive(false);
            offObj.SetActive(true);
            Stackball_GameStake.SoundManager.instance.musicManager.GetComponent<AudioSource>().enabled = false;
            if (buttonBg)
            {
                buttonBg.GetComponent<Image>().sprite = buttonBgOffSpr;
            }
        }
        else
        {
            onObj.SetActive(true);
            offObj.SetActive(false);
            Stackball_GameStake.SoundManager.instance.musicManager.GetComponent<AudioSource>().enabled = true;

            if (buttonBg)
            {
                buttonBg.GetComponent<Image>().sprite = buttonBgOnSpr;
            }
        }
    }
}
