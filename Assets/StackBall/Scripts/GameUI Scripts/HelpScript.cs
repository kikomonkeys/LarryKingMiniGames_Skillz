using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stackball_GameStake
{
    public class HelpScript : MonoBehaviour
    {
        public GameObject[] helpPages;
        public GameObject[] dotsObj;
        public GameObject nextBtn, continueBtn, closeBtn;
        int helpCount;
        void OnEnable()
        {
            if (GameUI.ShowHowToPlay == 0)
            {
                if (GameUI.instance.helpBtnClicked)
                {
                    closeBtn.SetActive(true);
                    continueBtn.SetActive(false);
                    GameUI.instance.helpBtnClicked = false;
                }
                else
                {
                    closeBtn.SetActive(false);
                    continueBtn.SetActive(true);
                }

            }
            //return;
            helpCount = 0;
            for (int i = 0; i < helpPages.Length; i++)
            {
                helpPages[i].SetActive(false);
                dotsObj[i].GetComponent<Image>().color = Color.grey;
            }
            helpPages[helpCount].SetActive(true);
            dotsObj[helpCount].GetComponent<Image>().color = Color.white;
        }
        public void NextBtnClicked()
        {
            if (GameUI.ShowHowToPlay == 1)
            {
                if (helpCount >= helpPages.Length - 1)
                {
                    helpCount = 0;
                }
                else
                {
                    helpCount++;
                }
                for (int i = 0; i < helpPages.Length; i++)
                {
                    helpPages[i].SetActive(false);
                    dotsObj[i].GetComponent<Image>().color = Color.grey;
                }
                helpPages[helpCount].SetActive(true);
                dotsObj[helpCount].GetComponent<Image>().color = Color.white;
            }
            else
            {
                helpCount++;
                if (helpCount >= helpPages.Length - 1)
                {
                    if (GameUI.instance.helpBtnClicked)
                    {
                        nextBtn.SetActive(true);
                        continueBtn.SetActive(false);
                    }
                    else
                    {
                        nextBtn.SetActive(false);
                        continueBtn.SetActive(true);
                    }
                }

                for (int i = 0; i < helpPages.Length; i++)
                {
                    helpPages[i].SetActive(false);
                    dotsObj[i].GetComponent<Image>().color = Color.grey;
                }
                helpPages[helpCount].SetActive(true);
                dotsObj[helpCount].GetComponent<Image>().color = Color.white;
            }

        }
        public void CloseBtnCLicked()
        {
            //if (GamePlayUI.isGameplay)
            //{
            //    GamePlayUI.Instance.EnableBlockShapePanel();
            //}
            gameObject.SetActive(false);
        }
        public void ContinueBtnClicked()
        {
            GameUI.ShowHowToPlay = 1;
            GameUI.instance.inGameUI.SetActive(true);

            GameUI.instance.StartGame();
            GameUI.instance.EnableCountDown();
            //SceneManager.LoadScene("MainScene");
        }
    }
}

