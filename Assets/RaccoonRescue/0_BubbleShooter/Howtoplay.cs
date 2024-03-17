using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Howtoplay : MonoBehaviour
{
    public GameObject[] tut,dots;
    public GameObject nextbtn, continueBtn;
    void Start()
    {
        for (int i = 0; i < tut.Length; i++)
        {
            tut[i].SetActive(false);
        }
        tut[0].SetActive(true);

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].GetComponent<Image>().color = Color.grey;
        }
        dots[0].GetComponent<Image>().color = Color.white;
    }
    int count;
    public void NextBtnClicked()
    {
        count++;
        if (count >= tut.Length - 1)
        {
            nextbtn.SetActive(false);
            continueBtn.SetActive(true);
        }
        else
        {
            tut[count - 1].SetActive(false);
            tut[count].SetActive(true);
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].GetComponent<Image>().color = Color.grey;
            }
            dots[count].GetComponent<Image>().color = Color.white;
        }
    }
    public void ContinueBtnClicked()
    {
        PlayerPrefs.SetInt("howtoplay", 1);
        mainscript.Instance.StartCreatingLevelNow();
        gameObject.SetActive(false);
    }
}
