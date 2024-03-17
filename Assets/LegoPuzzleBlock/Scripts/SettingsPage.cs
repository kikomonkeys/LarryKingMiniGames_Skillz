using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    public Sprite musicOnSpr, musicOffSpr, soundOnSpr, soundOffSpr;
    public Image soundImg, musicImg, hapticFeedbackImg;
    int soundCount, musicCount, feedbackCount;
    public static SettingsPage instance;
    public GameObject musicParentBg;
    public Sprite musicParentBgOnSpr, musicParentBgOffSpr;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        BGMusicNew.Music = 1;

        if (BGMusicNew.Music == 0)
        {
            musicImg.sprite = musicOffSpr;
            musicParentBg.GetComponent<Image>().sprite = musicParentBgOffSpr;
            musicImg.SetNativeSize();
        }
        else
        {
            musicImg.sprite = musicOnSpr;
            musicParentBg.GetComponent<Image>().sprite = musicParentBgOnSpr;
            musicImg.SetNativeSize();

        }
    }
    public void SoundBtnCLicked()
    {
        soundCount++;
        if (soundCount % 2 == 0)
        {
            soundImg.sprite = soundOnSpr;
            soundImg.SetNativeSize();

        }
        else
        {
            soundImg.sprite = soundOffSpr;
            soundImg.SetNativeSize();

        }
    }
    public void MusicBtnClicked()
    {
        musicCount++;
        if (musicCount % 2 == 0)
        {
            musicImg.sprite = musicOnSpr;
            musicParentBg.GetComponent<Image>().sprite = musicParentBgOnSpr;

            musicImg.SetNativeSize();

            BGMusicNew.Music = 1;
            BGMusicNew.instance.SetMusicInfo();
        }
        else
        {
            musicImg.sprite = musicOffSpr;
            musicParentBg.GetComponent<Image>().sprite = musicParentBgOffSpr;
            musicImg.SetNativeSize();
            BGMusicNew.Music = 0;
            BGMusicNew.instance.SetMusicInfo();
        }
    }
    public void FeedbackBtnClicked()
    {
        feedbackCount++;
        if (feedbackCount % 2 == 0)
        {
            hapticFeedbackImg.sprite = soundOnSpr;
        }
        else
        {
            hapticFeedbackImg.sprite = soundOffSpr;
        }
    }
    public void CloseBtnClicked()
    {
        gameObject.SetActive(false);
    }
}
