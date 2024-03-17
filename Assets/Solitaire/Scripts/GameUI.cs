using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solitaire_GameStake
{
    public class GameUI : MonoBehaviour
    {

        public GameObject menuPage, howtoplayscreen;
        public GameObject dummybg;
        public static GameUI instance;
        void ShowPage()
        {
            if (PlayerPrefs.GetInt("firstTime") == 0)
            {
                howtoplayscreen.SetActive(true);
            }
            else
            {
                if (StageManager.isPlayClickedInLC)
                {
                    dummybg.SetActive(true);
                    Invoke(nameof(InitViewer), 3f);
                    StageManager.instance.EnableCountDown(0f);
                    StageManager.isPlayClickedInLC = false;
                }
                else if (StageManager.isHomeClickedInLC)
                {
                    menuPage.SetActive(true);

                    StageManager.isHomeClickedInLC = false;
                }
                else
                {
                    dummybg.SetActive(true);
                    Invoke(nameof(InitViewer), 3f);
                    StageManager.instance.EnableCountDown(0f);
                }
            }
        }

        void InitViewer()
        {
            dummybg.SetActive(false);
            StageManager.instance.InitViewer();
        }

    }
}

