using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stackball_GameStake
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        public GameObject musicManager;

        private AudioSource audioSource;

        public bool sound = true;

        void Awake()
        {
            MakeSingleton();
            audioSource = GetComponent<AudioSource>();
        }
        void MakeSingleton()
        {
            if (instance != null)
                Destroy(gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void SoundOnOff()
        {
            sound = !sound;
        }

        public void PlaySoundFX(AudioClip clip, float voulme)
        {
            if (sound)
                audioSource.PlayOneShot(clip, voulme);
        }
    }
}

