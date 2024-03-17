//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Solitaire_GameStake
{
    public class HowtoPlay : MonoBehaviour
    {
        public Button btn;
        public GameObject panel;
        public GameObject panel2;
        public GameObject panel3;
        public GameObject panel4;
        public GameObject nextBtn, continueBtn;
        public GameObject canvasCards;
        private int i = 0;
        public GameObject menupage;
        public GameObject dummybg;
        public GameObject closeBtn;
        private void Start()
        {
            btn.onClick.AddListener(delegate { backTotut1(); });

        }

        private void OnEnable()
        {
            Debug.LogError("firsttime::" + PlayerPrefs.GetInt("firstTime"));
            if (PlayerPrefs.GetInt("firstTime") == 0)
            {
                i = 0;
                panel.SetActive(true);
                panel2.SetActive(false);
                panel3.SetActive(false);
                panel4.SetActive(false);
                nextBtn.SetActive(true);
                continueBtn.SetActive(false);
            }
            else
            {
                if (StageManager.instance.rulesBtnClicked)
                {
                    i = 0;
                    panel.SetActive(true);
                    panel2.SetActive(false);
                    panel3.SetActive(false);
                    panel4.SetActive(false);
                    nextBtn.SetActive(true);
                    continueBtn.SetActive(false);
                    closeBtn.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                    dummybg.SetActive(true);
                    StageManager.instance.EnableCountDown(0f);

                    Invoke(nameof(InitViewer), 3f);
                }
                
            }
            
        }
        public void backTotut1()
        {
            i++;
            if (i >= 4)
            {
                i = 0;
                //return;

            }
            if (i == 0)
            {
                panel.SetActive(true);
                panel2.SetActive(false);
                panel3.SetActive(false);
                panel4.SetActive(false);
            }

            if (i == 1)
            {
                panel.SetActive(false);
                panel2.SetActive(true);
                panel3.SetActive(false);
                panel4.SetActive(false);

            }
            if (i == 2)
            {
                panel.SetActive(false);
                panel2.SetActive(false);
                panel3.SetActive(true);
                panel4.SetActive(false);

            }
            if (i == 3)
            {
                panel.SetActive(false);
                panel2.SetActive(false);
                panel3.SetActive(false);
                panel4.SetActive(true);
                if (!PlayButton.rulesBtnClicked)
                {
                    nextBtn.SetActive(false);
                    continueBtn.SetActive(true);
                }
            }
        }
        public void ContinueBtnClicked()
        {
            PlayerPrefs.SetInt("firstTime", 1);
            //SceneManager.LoadScene("Splash");
            gameObject.SetActive(false);
            menupage.SetActive(false);
            //GameSettings.Instance.isGameStarted = true;
            dummybg.SetActive(true);
            PlayButton.rulesBtnClicked = false;
            StageManager.instance.EnableCountDown(0f);

            Invoke(nameof(InitViewer), 3f);
        }
        void InitViewer()
        {
            dummybg.SetActive(false);
            StageManager.instance.InitViewer();
        }
        public void PlayareaRulesBtnClicked()
        {
            PlayButton.rulesBtnClicked = true;

            GameSettings.Instance.isGameStarted = false;
        }
        public void PlayareaRulesCloseBtn()
        {
            PlayButton.rulesBtnClicked = false;
            //GameSettings.Instance.isGameStarted = true;
            //canvasCards.SetActive(true);

        }
        public void CloseBtnCliked()
        {
            gameObject.SetActive(false);

        }
    }
}

