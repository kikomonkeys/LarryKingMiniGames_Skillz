using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneManagerScript : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Panelhide;

    public void GoToExitScreen()
    {
        Panel.SetActive(true);
        Panelhide.SetActive(false);
        SceneManager.LoadScene("Solitaire_Temp");
    }

}