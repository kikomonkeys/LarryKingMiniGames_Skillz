using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Solitaire_GameStake
{
    public class SettingsPage : MonoBehaviour
    {
        public GameObject soundBtn, musicBtn;
        int soundCount, musicCount;
        public Sprite soundOnSpr, soundOffSpr;
        public TextMeshProUGUI soundText;
        public TextMeshProUGUI musicText;
        public static SettingsPage instance;


        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            GameSettings.Instance.isGameStarted = false;
            // Debug.LogError("time remaining::" + StatusBar.instance.myTimer);
        }
        void Start()
        {
            // Debug.Log("sound::" + PlayerPrefs.GetInt("Sound") + " Music::" + PlayerPrefs.GetInt("Music"));
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                soundBtn.GetComponent<Image>().sprite = soundOnSpr;
                soundText.text = "ON";
                GameSettings.Instance.isSoundSet = true;
            }
            else
            {
                soundBtn.GetComponent<Image>().sprite = soundOffSpr;
                soundText.text = "OFF";
                GameSettings.Instance.isSoundSet = false;
            }
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                musicBtn.GetComponent<Image>().sprite = soundOnSpr;
                musicText.text = "ON";
                FindObjectOfType<StageManager>().GetComponent<AudioSource>().enabled = true;
            }
            else
            {
                musicBtn.GetComponent<Image>().sprite = soundOffSpr;
                musicText.text = "OFF";
                FindObjectOfType<StageManager>().GetComponent<AudioSource>().enabled = false;
            }
        }


        public void SoundBtnClicked()
        {
            soundCount++;
            if (soundCount % 2 == 0)
            {
                //GameSettings.Instance.isSoundSet = true;
                PlayerPrefs.SetInt("Sound", 1);
                GameSettings.Instance.isSoundSet = true;
                soundBtn.GetComponent<Image>().sprite = soundOnSpr;
                soundText.text = "ON";
            }
            else
            {
                //GameSettings.Instance.isSoundSet = false;
                PlayerPrefs.SetInt("Sound", 0);
                GameSettings.Instance.isSoundSet = false;
                soundBtn.GetComponent<Image>().sprite = soundOffSpr;
                soundText.text = "OFF";
            }
        }
        public void MusicBtnClicked()
        {
            musicCount++;
            if (musicCount % 2 == 0)
            {
                //GameSettings.Instance.isSoundSet = true;
                FindObjectOfType<StageManager>().GetComponent<AudioSource>().enabled = true;
                PlayerPrefs.SetInt("Music", 1);
                musicBtn.GetComponent<Image>().sprite = soundOnSpr;
                musicText.text = "ON";
            }
            else
            {
                //GameSettings.Instance.isSoundSet = false;
                FindObjectOfType<StageManager>().GetComponent<AudioSource>().enabled = false;

                PlayerPrefs.SetInt("Music", 0);
                musicBtn.GetComponent<Image>().sprite = soundOffSpr;
                musicText.text = "OFF";
            }
        }
        public void Pause_ResumeBtnClicked()
        {
            GameSettings.Instance.isGameStarted = true;
            StageManager.instance.pauseBtn.SetActive(true);
            StageManager.instance.undoBtn.SetActive(true);
            StageManager.instance.canvasCards.SetActive(true);
            gameObject.SetActive(false);
        }
        public static bool isQuitBtnClicked;
        public void Pause_QuitBtn()
        {
            isQuitBtnClicked = true;

            StageManager.instance.LoadGameover();
        }
    }
}

