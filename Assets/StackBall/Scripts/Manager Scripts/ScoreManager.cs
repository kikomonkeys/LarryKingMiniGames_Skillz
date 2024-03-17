using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Stackball_GameStake
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        public Text scoreText;

        public int score = 10;

        void Awake()
        {
            // scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

            MakeSingleton();
        }
        public void AssignScoreText()
        {
            if (!GameUI.isLoadNextLevel)
                scoreText = GameUI.instance.scoreText.GetComponent<Text>();
        }
        void Start()
        {
            AddScore(0);
        }

        private void Update()
        {
            if (scoreText == null)
            {
                scoreText = GameUI.instance.scoreText.GetComponent<Text>();
                scoreText.text = score.ToString();
            }
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

        public void AddScore(int amount)
        {
            score += amount;
            if (score > PlayerPrefs.GetInt("HighScore", 0))
                PlayerPrefs.SetInt("HighScore", score);

            if (scoreText != null)
                scoreText.text = score.ToString();
        }

        public void ResetScore()
        {
            score = 0;
        }
    }
}

