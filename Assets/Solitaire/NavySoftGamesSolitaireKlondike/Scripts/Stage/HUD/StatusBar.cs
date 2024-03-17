using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusBar : MonoBehaviour
{

    const int MAX_MOVES = 999;


    [SerializeField]
    private GameObject moveTextContainer;

    [SerializeField]
    private GameObject timeTextContainer;

    [SerializeField]
    private GameObject scoreTextContainer;

    [SerializeField]
    private GameObject opposcoreTextContainer;

    [SerializeField]
    private TextMeshProUGUI moveText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI timeTextAptoide;

    [SerializeField]
    private GameObject timeTextAptoideContainer;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI opposcoreText;
    public static StatusBar instance;

    // Set default values
    //	void Start () {
    //		reset ();
    //	}

    private void Awake()
    {
        instance = this;
    }
    public void SetVisibility(bool isMove, bool isTime, bool isScore)
    {
        moveTextContainer.SetActive(false);
        timeTextContainer.SetActive(isTime);
        if (timeTextAptoideContainer)
            timeTextAptoideContainer.SetActive(isTime);
        scoreTextContainer.SetActive(isScore);
        if (opposcoreTextContainer)
            opposcoreTextContainer.SetActive(true);
    }


    /// <summary>
    /// Set all values as default
    /// </summary>
    public void reset()
    {
        move = 0;
        time = 0;
        score = 0;
    }
    public void ResetTimeAfterWatchVideo()
    {
        time = 30;
        timeVal = 30;
        myTimer = 30;
    }
    // **************************
    // *******  GETTERS  ********
    // ****** / SETTERS  ********
    // **************************
    int _move = 0;
    public int move
    {
        get
        {
            return _move;
        }
        set
        {
#if UNITY_EDITOR
            if (!isMoveValueValid(value))
            {
                throw new UnityException("Invalid move value!");
            }
#endif
            _move = value;


            moveText.text = string.Format("Moves:{0}", parseScoreText(_move));
        }
    }
    /// <summary>
    /// Sets the time in a seconds
    /// </summary>
    public int _time = 0;
    public int myTimer;
    public bool startTimer = false;
    
    public int timeVal;
    public int time
    {
        get
        {
            return _time;
        }
        set
        {
#if UNITY_EDITOR
            if (!isTimeValueValid(value))
            {
                throw new UnityException("Invalid time value!");
            }
#endif
            _time = value;

            if(!StageManager.instance.showRewardedAdPopup)
                timeVal = 36;//36 for 3min and 60 for 5min

            myTimer = (5 * timeVal - _time);
           // Debug.LogError("my timer::" + myTimer + " _time::" + _time + "  timestartnow::" + timeStartNow);
            
            if (myTimer <= 0)
            {
                // Debug.LogError("Gameoveer1111111111");
                myTimer = 0;
                timeText.text = string.Format("{0}", parseTimeText(myTimer));
                GameSettings.Instance.isGameStarted = false;
                //StageManager.instance.LoadGameover();
                StageManager.instance.isTimeUp = true;
                StageManager.instance.ShowLCOptionNow();

                if (!StageManager.instance.showRewardedAdPopup)
                    timeVal = 24;///24 for 2min

            }
            else
            {
                timeText.text = string.Format("{0}", parseTimeText(myTimer));
                // Debug.LogError("time::" + myTimer);
            }

            if (timeTextAptoide)
                timeTextAptoide.text = string.Format("{0}", parseTimeText(myTimer));
        }
    }
    int _score = 0;
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
#if UNITY_EDITOR
            if (!isScoreValueValid(value))
            {
                throw new UnityException("Invalid score value!");
            }
#endif
            _score = value;
          
            scoreText.text =string.Format("{0}", parseScoreText(_score)) ;
            UnityiOSHandler.instance.SendScore(_score, false);
        }
    }

    int _opposcore = 0;
    public int opposcore
    {
        get
        {
            return _opposcore;
        }
        set
        {
#if UNITY_EDITOR
            if (!isScoreValueValid(value))
            {
                throw new UnityException("Invalid score value!");
            }
#endif
            _opposcore = value;

            if (opposcoreText)
                opposcoreText.text = string.Format("{0}", parseScoreText(_opposcore));
        }
    }

    // **************************
    // *****  VALIDATORS  *******
    // **************************

    bool isMoveValueValid(int val)
    {
        if (val < 0 || val > 99999)
            return false;

        return true;
    }

    bool isTimeValueValid(int val)
    {
        int dayInSeconds = 86400;
        if (val < 0 || val > dayInSeconds)
            return false;

        return true;
    }

    bool isScoreValueValid(int val)
    {
        if (val > 99999)
            return false;
        return true;
    }




    // **************************
    // *******  PARSERS  ********
    // **************************

    string parseMoveText(int text)
    {
        if (text > MAX_MOVES)
        {
            // make text such as 999+ format
            return String.Format("{0}+", MAX_MOVES);
        }
        return text.ToString();
    }

    string parseTimeText(int seconds)
    {
 

      
        int min = seconds / 60  ;
        int sec = seconds % 60;
        return String.Format("{0:D2}:{1:D2}", min, sec);
    }

    string parseScoreText(int text)
    {
        return text.ToString();
    }

}
