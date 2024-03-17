using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BlockPuzzle_GameStake
{
    public class MainMenu : MonoBehaviour
    {

        // Use this for initialization

        public GameObject settingsPanel, helpPanel, menuPanel;

        public GameObject titleName, playBtn;
        public bool isMenupage;
        public static MainMenu instance;
        public bool helpBtnClicked;
        public bool gameScene;
        private void Awake()
        {
            instance = this;
        }
        private void OnEnable()
        {
            if (gameScene)
            {
                StartCoroutine(ShowCOuntDownObj(0f));
            }
        }
        IEnumerator TweenBtns(float waittime)
        {
            yield return new WaitForSeconds(waittime);
            iTween.ScaleFrom(titleName.gameObject, iTween.Hash("x", 0, "y", 0, "time", 0.35, "easetype", iTween.EaseType.spring));
            iTween.ScaleFrom(playBtn.gameObject, iTween.Hash("x", 0, "y", 0, "time", 0.35, "easetype", iTween.EaseType.spring, "delay", 0.5f));
            yield return new WaitForSeconds(0.75f);
            playBtn.GetComponent<PingPongAnim>().enabled = true;
        }
        public static int ShowHowToPlay
        {
            get
            {
                return PlayerPrefs.GetInt("help", 0);
            }
            set
            {
                PlayerPrefs.SetInt("help", value);
            }
        }
        private void Start()
        {
            GamePlayUI.isGameplay = false;
            if (!gameScene)
            {
                if (ShowHowToPlay == 0)
                {
                    helpPanel.SetActive(true);
                    menuPanel.SetActive(false);
                    //AudioManager.Instance.PlayButtonClickSound();
                    //StackManager.Instance.SpawnUIScreen("HelpPanel");
                }
                else
                {
                    SceneManager.LoadScene("blockpuzzle_ingame");
                }
            }

            //initialize the gamestake plugin

            //Invoke("ShowResults", 10);

        }



        public void PlayGame()
        {

            if (ShowHowToPlay == 0)
            {
                helpPanel.SetActive(true);
                //AudioManager.Instance.PlayButtonClickSound();
                //StackManager.Instance.SpawnUIScreen("HelpPanel");
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }


        public void Home()
        {

            SceneManager.LoadScene("Main Menu");
        }
        public void ExitThisGame()
        {

            Application.Quit();
        }

        public void HelpBtnCLicked()
        {
            helpBtnClicked = true;
            helpPanel.SetActive(true);
        }
        public void SettingsBtnClicked()
        {
            settingsPanel.SetActive(true);
        }
        public GameObject countDownObj;
        public Text countdownText;
        IEnumerator ShowCOuntDownObj(float waittime)
        {
            yield return new WaitForSeconds(waittime);
            countDownObj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            countdownText.text = "3";
            yield return new WaitForSeconds(0.75f);
            countdownText.text = "2";
            yield return new WaitForSeconds(0.75f);
            countdownText.text = "1";
            yield return new WaitForSeconds(0.75f);
            countdownText.text = "GO";
            yield return new WaitForSeconds(0.75f);
            countDownObj.SetActive(false);
        }
    }
}

