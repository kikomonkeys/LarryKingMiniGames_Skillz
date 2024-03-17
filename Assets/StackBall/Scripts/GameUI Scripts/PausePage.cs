using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stackball_GameStake
{
    public class PausePage : MonoBehaviour
    {
        public GameObject soundBtn;
        public GameObject soundOnSpr, soundOffSpr;

        public GameObject musicBtn;
        public GameObject musicOnSpr, musicOffSpr;
        void Start()
        {

        }
        private void OnEnable()
        {
            DisplaySound();
            DisplayMusic();
        }

        void DisplaySound()
        {
            if (Stackball_GameStake.SoundManager.instance.sound)
            {
                soundOnSpr.SetActive(true);
                soundOffSpr.SetActive(false);
            }
            else if (!Stackball_GameStake.SoundManager.instance.sound)
            {
                soundOnSpr.SetActive(false);
                soundOffSpr.SetActive(true);
            }
        }

        void DisplayMusic()
        {
            //if (GameUI.Music==0)
            //{
            //    musicOffSpr.SetActive(true);
            //    musicOnSpr.SetActive(false);
            //}
            //else 
            //{
            //    musicOffSpr.SetActive(false);
            //    musicOnSpr.SetActive(true);
            //}
        }
        int sCount;
        public void SoundBtnClicked()
        {
            sCount++;
            if (sCount % 2 != 0)
            {
                Stackball_GameStake.SoundManager.instance.sound = false;
                soundOnSpr.SetActive(false);
                soundOffSpr.SetActive(true);
            }
            else
            {
                Stackball_GameStake.SoundManager.instance.sound = true;
                soundOnSpr.SetActive(true);
                soundOffSpr.SetActive(false);
            }
        }

        int mCount;
        public void MusicBtnClicked()
        {
            //mCount++;
            //if (mCount % 2 != 0)
            //{
            //    GameUI.Music = 0;
            //    musicOffSpr.SetActive(true);
            //    musicOnSpr.SetActive(false);
            //}
            //else
            //{
            //    GameUI.Music = 1;
            //    musicOffSpr.SetActive(false);
            //    musicOnSpr.SetActive(true);
            //}
        }
    }
}

