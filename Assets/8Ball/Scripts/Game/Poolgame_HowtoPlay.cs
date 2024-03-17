using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poolgame_HowtoPlay : MonoBehaviour
{

    public GameObject[] helpObj, dotsObj;
    public GameObject closeBtn_1, closeBtn_2, nextBtn, gameCountDown;
    public GameControllerScript gamecontrollerScript;
   // public ShotPowerScript shotPowerScript;
    void OnEnable()
    {
        closeBtn_1.SetActive(false);
        nextBtn.SetActive(true);
        Debug.Log("firsttime:" + PlayerPrefs.GetInt("FirstTime"));
        if (PlayerPrefs.GetInt("FirstTime") != 0)
        {
            closeBtn_2.SetActive(true);
            gamecontrollerScript.isHowtoPlayClicked = false;
            ScoreController.instance.DisablePots();
            ScoreController.instance.spinBall.SetActive(false);
        }
        else
        {
            Invoke(nameof(DisableSpinBall), 1f);
        }

        for (int i = 0; i < dotsObj.Length; i++)
        {
            dotsObj[i].GetComponent<Image>().color = Color.grey;
        }
        dotsObj[0].GetComponent<Image>().color = Color.white;

        for (int i = 0; i < helpObj.Length; i++)
        {
            helpObj[i].SetActive(false);
        }
        helpObj[0].SetActive(true);
        count = 0;
    }
    void DisableSpinBall()
    {
        ScoreController.instance.spinBall.SetActive(false);
    }
    private void OnDisable()
    {
        ScoreController.instance.spinBall.SetActive(true);
    }
    int count;
    public void NextBtnClicked()
    {
        if (count == helpObj.Length - 1)
        {
            if (PlayerPrefs.GetInt("FirstTime") == 0)
            {
                nextBtn.SetActive(false);
                closeBtn_1.SetActive(true);
            }
            else
            {
                count = 0;
            }
        }
        else
        {
            count++;
        }
        for (int i = 0; i < helpObj.Length; i++)
        {
            helpObj[i].SetActive(false);
            dotsObj[i].GetComponent<Image>().color = Color.grey;
        }
        helpObj[count].SetActive(true);
        dotsObj[count].GetComponent<Image>().color = Color.white;

    }

    public void CloseBtn_1_Clicked()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("FirstTime", 1);
        gameCountDown.SetActive(true);
        ScoreController.instance.StartCountDownObj();
        //shotPowerScript.HideCue();
        ScoreController.instance.cue.SetActive(false);
        Invoke(nameof(ShowCue), 6f);
        ScoreController.instance.EnablePots();
        AudioListener.volume = 1;
        PoolGame_GameManager.Instance.audioSources[1].Play();
        //PoolGame_GameManager.Instance.tableNumber = 0;
        //PoolGame_GameManager.Instance.offlineMode = true;
        //PoolGame_GameManager.Instance.roomOwner = true;
    }
    void ShowCue()
    {
        ScoreController.instance.cue.SetActive(true);
        // shotPowerScript.ShowCue();
    }
    public void CloseBtn_2_Clicked()
    {
        ScoreController.instance.EnablePots();
        //gameObject.SetActive(false);
    }
}
