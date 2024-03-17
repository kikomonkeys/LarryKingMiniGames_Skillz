using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Solitaire_GameStake
{
    public class ScoreManager : MonoBehaviour
    {

        public TextMeshProUGUI text;
        [HideInInspector]
        public float score;
        float tempScore;
        Coroutine scoreCo;
        public static ScoreManager instance

        {
            get; set;
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            score = PlayerPrefs.GetFloat("_score_");
            if (text)
                text.text = score.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// pass the number it will add the main score
        /// </summary>
        public void UpdateScore(int scr)
        {
            tempScore = scr + score;
            PlayerPrefs.SetFloat("_score_", (int)tempScore);
            scoreCo = StartCoroutine(UpdateScr());

        }

        IEnumerator UpdateScr()
        {
            CancelInvoke("stopCoroutines");
            Invoke("stopCoroutines", 1);
            while ((int)score != (int)tempScore)
            {
                yield return null;
                Debug.Log("updating (" + score + "," + tempScore + ")");

                score = Mathf.Lerp(score, tempScore, .1f);
                text.text = score.ToString("0");

            }
            score = tempScore;
            scoreCo = null;
        }

        void stopCoroutines()
        {
            score = tempScore;
            text.text = score.ToString("0");
            if (scoreCo != null)
            {
                StopCoroutine(scoreCo);
            }
        }
    }
}

