using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowtoPlay : MonoBehaviour
{

    public GameObject tut1, tut2;
    public GameObject dot1, dot2;
    public GameObject nextBtn, continueBtn, closebtn;

    void Start()
    {
        tut1.SetActive(true);
        tut2.SetActive(false);
        dot1.GetComponent<Image>().color = Color.white;
        dot2.GetComponent<Image>().color = Color.grey;
        nextBtn.SetActive(true);
        continueBtn.SetActive(false);
        closebtn.SetActive(false);
    }
    int num;
    public void NextBtnClicked()
    {
        if (GameManager.Knife_HowToPlay == 0)
        {
            dot1.GetComponent<Image>().color = Color.grey;
            dot2.GetComponent<Image>().color = Color.white;
            tut1.SetActive(false);
            tut2.SetActive(true);
            nextBtn.SetActive(false);
            continueBtn.SetActive(true);
            //if (MainMenu.intance.helpBtnClicked)
            //{
            //    closebtn.SetActive(true);
            //    MainMenu.intance.helpBtnClicked = false;
            //}
            //else
            //{
            //    continueBtn.SetActive(true);
            //}
        }
        else
        {
            dot1.GetComponent<Image>().color = Color.grey;
            dot2.GetComponent<Image>().color = Color.white;
            tut1.SetActive(false);
            tut2.SetActive(true);
            nextBtn.SetActive(false);
            closebtn.SetActive(true);
           
        }
    }
    public void ContinueBtnClicked()
    {
        //FindObjectOfType<MainMenu>().gameObject.SetActive(false);
        GameManager.Knife_HowToPlay = 1;
        tut1.SetActive(false);
        tut2.SetActive(false);
        gameObject.SetActive(false);
        GamePlayManager.instance.StartGameNow();
        //GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
    }
    public void CloseBtnCLicked()
    {
        gameObject.SetActive(false);
    }
}
