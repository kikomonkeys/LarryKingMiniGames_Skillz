using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpScript : MonoBehaviour
{
    public GameObject[] helpPages;
    public GameObject[] dotsObj;
    public GameObject nextBtn, continueBtn, closeBtn;
    int helpCount;
    void OnEnable()
    {
        if (BlockPuzzle_GameStake.MainMenu.ShowHowToPlay == 0)
        {
            if (BlockPuzzle_GameStake.MainMenu.instance.helpBtnClicked)
            {
                closeBtn.SetActive(true);
                continueBtn.SetActive(false);
                BlockPuzzle_GameStake.MainMenu.instance.helpBtnClicked = false;
            }
            else
            {
                closeBtn.SetActive(false);
                continueBtn.SetActive(true);
            }
           
        }
        return;
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
        if (BlockPuzzle_GameStake.MainMenu.ShowHowToPlay == 1)
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
            if (helpCount > helpPages.Length - 1)
            {
                nextBtn.SetActive(false);
                continueBtn.SetActive(true);
                return;
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
        if (GamePlayUI.isGameplay)
        {
            GamePlayUI.Instance.EnableBlockShapePanel();
        }
        gameObject.SetActive(false);
    }
    public void ContinueBtnClicked()
    {
        BlockPuzzle_GameStake.MainMenu.ShowHowToPlay = 1;
        SceneManager.LoadScene("blockpuzzle_ingame");
    }
}
