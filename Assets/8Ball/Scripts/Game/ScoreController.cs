using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;
using Random = UnityEngine.Random;
//using VLxSecurity;

public class ScoreController : MonoBehaviour
{
    #region singleton
    public static ScoreController instance;
    #endregion singleton

    [SerializeField] List<Transform> pots, playerLifeBallUI, lostLifeUI, missedPopup_PlayerlifeBallUI, missedPopup_lostLifeUI;
    [SerializeField]
    List<Vector3> inititalPositions;

    public LineRenderer aimLineRenderer;
    public Texture[] aimColortex;
    public Gradient[] aimColortexC;
    public Text CountdownText;
    public GameObject multiplierObj;

    public GameObject trickVfxPrefab, normalVfxPrefab;

    public GameObject blackBallBonusMessage;

    public GameObject pausedMenu;

    public bool allowDisplayTouch = false;

    public GameObject targetLine, spinBall;

    public GameObject Fader, ResultMenu;

    public TextMeshPro[] colorBallsUI;

    public GameObject[] firstMultiplier, potMultiplier, blackBallBonus;

    public GameObject gameOverUI, successUI, timeUpUI;

    public bool wallTrickshot, ballTrickShot;
    public int wallTSvalue, ballTSvalue; //wall trickshot and ball trickshot value...

    public int totalBalls;
    public int potedBallValue;

    public GameObject countdownUI, cue;
    public float timerToStartGame;

    public GameObject currentScoredPointPrefab, wallBonusPrefab, comboBonusPrefab,
        trickShotTextPrefab, streakBonusTextPrefab;
    public GameObject scoreUIPosition;
    public Animator mainScoreAnim;

    public float uiMoveSpeed;

    public int gamescore;
    public int ballscorevalue = 100;

    //public TextMeshProUGUI scoretext;
    public Text scoretext;

    public int playerLife;
    public bool canReduceLife = false;

    public bool canCheckForPotedBalls = false;
    public bool ballPotted = false;

    public bool canAddScore;

    int potCheckTimer = 0;

    public float gameTimer;
    //public TextMeshProUGUI gameTimerUI;
    public Text gameTimerUI;

    public bool stopGameTimer = true;

    public GameObject noBallPocketedMessage;

    public int deathCounter;

    public int ballPlayedCount = 0;

    public int multiplierValue = 0;

    public int FinalShotsSCore = 0;
    public int FinalTSScore = 0;
    public int FinalTimeLeft = 0;
    public int TotalScore = 0;
    public int BestScore = 0;

    public TextMeshPro shotScoreUI, trickshotScoreUI, timeLeftUI, totalScoreUI, streakBonusUI, bestScoreUI;
    public Image streakBonusImg;
    public Sprite[] streakImgs;

    public bool endgame = false;

    public bool doubleBallBonus = false;

    public bool blackBallPotted = false;

    public int pocketedBallCounter;

    public AudioSource musicController;

    public GameObject streakBonusActivatedUI;
    public GameObject bottomGraphics;
    public CueController cueController;
    public ShotPowerScript shotPowerscript;
    public GameObject playagainBtn;
    public GameObject howtoplayObj;
    public GameObject countDownObj;
    public GameObject WhiteBall;
    private void Awake()
    {
        instance = this;
        SoundMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        MusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
    }

    private void Start()
    {

        inititalPositions = new List<Vector3>();

        foreach (var item in pots)
        {
            inititalPositions.Add(item.position);
        }

        cue.GetComponent<SpriteRenderer>().enabled = false;

        //targetLine.SetActive(false);
        //spinBall.SetActive(false);

        PlayerPrefs.SetInt("Score", TotalScore);
        Invoke(nameof(EnableAudioListener), 1f);

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            howtoplayObj.SetActive(true);
            AudioListener.volume = 0;
            DisablePots();
        }
        else
        {
            StartCoroutine(StartCountDown(0f));
        }


    }
    void EnableAudioListener()
    {
        GameObject.Find("MainCamera").GetComponent<AudioListener>().enabled = true;
    }
    bool canLoadNextScene = true;
    [SerializeField]
    GameObject toastMsg;
    void SendScoreToGS()
    {
        //PlayerPrefs.SetString("@$%2021", StringCipher.Encrypt(TotalScore.ToString(), "Piper@2021$#"));
        //GameStakeSDK.instance.OnGameComplete(ShowToastAction);
        TryToSubmitScoreToSkillz();
    }

    public GameObject loadingPage;

    public void SubmitScoreSkillzbtnClicked()
    {
        if(loadingPage!=null)
        loadingPage.SetActive(true);

        if (scoreSubmitSuccess)
        {
            Debug.Log("Score submit success");
            StartCoroutine(MatchComplete());
        }
        else
        {
            StartCoroutine(RetrySubmitScoreToSkillz());
            StartCoroutine(MatchComplete());
            scoreSubmitSuccess = false;
        }
    }
    #region Score Submitting to Skillz

    bool scoreSubmitSuccess;
    void TryToSubmitScoreToSkillz()
    {
        string score = TotalScore.ToString();
        SkillzCrossPlatform.SubmitScore(score, OnSuccess, OnFailure);

        //firebase log event
        //if (FirebaseInit.instance)
        //    FirebaseInit.instance.FirebaseGameOverLogEvent(int.Parse(score));

        //tenjin log event
        // TenjinInit.instance.SendGameOverEvent(score);
    }

    void OnSuccess()
    {
        scoreSubmitSuccess = true;
    }

    void OnFailure(string reason)
    {
        //Debug.LogWarning("Fail: " + reason);
        StartCoroutine(RetrySubmitScoreToSkillz());
        SkillzCrossPlatform.DisplayTournamentResultsWithScore(TotalScore.ToString());
    }

    IEnumerator RetrySubmitScoreToSkillz()
    {
        yield return new WaitForSeconds(1);
        TryToSubmitScoreToSkillz();
    }
    IEnumerator MatchComplete()
    {
        yield return new WaitForSeconds(1);
        SkillzCrossPlatform.ReturnToSkillz();
    }
    #endregion

    public void HowToPlayBtnClicked()
    {
        howtoplayObj.SetActive(true);
    }
    public void StartCountDownObj()
    {
        StartCoroutine(StartCountDown(0));
    }
    IEnumerator StartCountDown(float waittime)
    {
        countDownObj.SetActive(true);
        yield return new WaitForSeconds(waittime);
        CountdownText.text = "3";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "2";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "1";
        yield return new WaitForSeconds(1f);
        CountdownText.text = "GO";
        yield return new WaitForSeconds(1f);
        countdownUI.SetActive(false);

        Invoke("StartTimer", timerToStartGame);

    }
    void ShowToastAction()
    {
        StartCoroutine(ShowToastMsg(0f));
    }
    IEnumerator ShowToastMsg(float waittime)
    {
        Debug.LogError("showing toast msg");
        yield return new WaitForSeconds(waittime);
        toastMsg.SetActive(true);
        yield return new WaitForSeconds(2f);
        toastMsg.SetActive(false);
    }
    void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        Invoke(nameof(ShowPlayAgainBtn), 3f);
        isShowingSomeUI = true;
        DisablePots();
    }
    void ShowPlayAgainBtn()
    {
        playagainBtn.SetActive(true);
    }
    public void PauseBtnClicked()
    {
        Time.timeScale = 0;
        pausedMenu.SetActive(true);
    }

    public void GameOver_PlayAgainBtnClicked()
    {
        SceneManager.LoadScene("PoolGame_GameScene");
        UnloadApp();
    }
    void UnloadApp()
    {
        Debug.LogError("unload the app");
        // Application.Quit();
        Application.Unload();
    }
    int timeBonus;
    public bool isQuitBtnClicked;
    private void Update()
    {
        //reduce player life if white ball is poted....
        //callDecreasePlayerLife();

        if ((playerLife == 0 && !endgame))
        {
            //end game...
            endgame = true;
            stopGameTimer = true;
            print("Player life is 0 so game has ended");
            FinalTimeLeft = (int)gameTimer;
            //FinalTimeLeft = 0;
            if (potedBallValue == 0)
                FinalTimeLeft = 0;

            timeBonus = 0;// FinalTimeLeft * 3;
            Debug.Log("timeleftbonus::" + timeBonus);
            timeLeftUI.text = timeBonus.ToString();

            TotalScore = FinalShotsSCore + FinalTSScore + timeBonus + totalStreakBonus;
            totalScoreUI.text = TotalScore.ToString();

            streakBonusUI.text = totalStreakBonus.ToString();

            //updating Best Score...
            if (PlayerPrefs.GetInt("Score") <= TotalScore)
            {
                PlayerPrefs.SetInt("Score", TotalScore);
            }

            bestScoreUI.text = PlayerPrefs.GetInt("Score").ToString();

            //GameManager.Instance.audioSources[3].Play();
            //gameOverUI.SetActive(true);
            Invoke(nameof(ShowGameOverUI), 2f);
            allowDisplayTouch = false;
            //GameManager.Instance.iWon = true;
            SendScoreToGS();
        }

        if (potedBallValue == totalBalls && !endgame)
        {
            StartCoroutine("AllBallsPoted");
        }

        if ((gameTimer <= 0 && !endgame))
        {
            //end game
            endgame = true;
            stopGameTimer = true;
            print("Game time is 0 so game has ended");
            FinalTimeLeft = (int)gameTimer;

            //FinalTimeLeft = 0;
            if (potedBallValue == 0)
                FinalTimeLeft = 0;


            timeBonus = 0;// FinalTimeLeft * 3;
            timeLeftUI.text = timeBonus.ToString();
            Debug.Log("timeleftbonus::" + timeBonus);

            TotalScore = FinalShotsSCore + FinalTSScore + timeBonus + totalStreakBonus;
            totalScoreUI.text = TotalScore.ToString();

            streakBonusUI.text = totalStreakBonus.ToString();

            //updating Best Score...
            if (PlayerPrefs.GetInt("Score") <= TotalScore)
            {
                PlayerPrefs.SetInt("Score", TotalScore);
            }

            bestScoreUI.text = PlayerPrefs.GetInt("Score").ToString();

            //GameManager.Instance.audioSources[3].Play();
            timeUpUI.SetActive(true);
            allowDisplayTouch = false;
            //GameManager.Instance.iWon = true;
            SendScoreToGS();
        }
        if (isQuitBtnClicked && !endgame)
        {
            FinalTimeLeft = (int)gameTimer;

            //FinalTimeLeft = 0;
            if (potedBallValue == 0)
                FinalTimeLeft = 0;

            timeBonus = 0;// FinalTimeLeft * 3;
            timeLeftUI.text = timeBonus.ToString();
            Debug.Log("timeleftbonus::" + timeBonus);

            TotalScore = FinalShotsSCore + FinalTSScore + timeBonus + totalStreakBonus;
            totalScoreUI.text = TotalScore.ToString();

            streakBonusUI.text = totalStreakBonus.ToString();
            isQuitBtnClicked = false;
            SendScoreToGS();
        }
        if (gameOverUI.gameObject.activeSelf && canLoadNextScene ||
            timeUpUI.gameObject.activeSelf && canLoadNextScene ||
            successUI.gameObject.activeSelf && canLoadNextScene)
        {
            //GameManager.Instance.audioSources[3].Play();
            if (showRewardedAdPopup)//mo
            {
                canLoadNextScene = false;
                PoolGame_GameManager.Instance.resetAllData();
                bottomGraphics.SetActive(false);
                StartCoroutine("ShowResultMenu");
            }
            else
            {
                if (timeUpUI.gameObject.activeSelf)
                {
                    isTimeUp = true;
                    isLostLives = false;
                }
                else if (gameOverUI.gameObject.activeSelf)
                {
                    isTimeUp = false;
                    isLostLives = true;
                }
                StartCoroutine(ShowLCOption(3f));

            }
        }

        //game timer...
        if (stopGameTimer == false)
        {
            UpdateGameTimer();
        }

        if (canReduceLife)
        {
            canReduceLife = false;
            deathCounter++;
            //if (deathCounter == 1)
            {
                //deathCounter = 10;
                DecreasePlayerLife();
                Invoke("ResetDeathCounter", 2f);
            }


        }


    }


    IEnumerator AllBallsPoted()
    {
        endgame = true;
        //end game...
        stopGameTimer = true;
        print("All Balls Potted, gameover");

        FinalTimeLeft = (int)gameTimer;
        //FinalTimeLeft = FinalTimeLeft;
        timeBonus = FinalTimeLeft * 3;
        timeLeftUI.text = timeBonus.ToString();
        Debug.Log("timeleftbonus::" + timeBonus);

        TotalScore = FinalShotsSCore + FinalTSScore + timeBonus + totalStreakBonus;
        totalScoreUI.text = TotalScore.ToString();

        streakBonusUI.text = totalStreakBonus.ToString();

        //updating Best Score...
        if (PlayerPrefs.GetInt("Score") <= TotalScore)
        {
            PlayerPrefs.SetInt("Score", TotalScore);
        }

        bestScoreUI.text = PlayerPrefs.GetInt("Score").ToString();

        //GameManager.Instance.audioSources[3].Play();
        allowDisplayTouch = false;

        yield return new WaitForSeconds(7f);
        successUI.SetActive(true);
        DisablePots();
        //GameManager.Instance.iWon = true;
    }

    IEnumerator ShowResultMenu()
    {

        //noBallPocketedMessage.SetActive(false);

        yield return new WaitForSeconds(4f);

        // Fader.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        gameOverUI.SetActive(false);
        successUI.SetActive(false);
        timeUpUI.SetActive(false);

        cue.GetComponent<SpriteRenderer>().enabled = false;
        shotPowerscript.HidePowerBar();//mohith

        ResultMenu.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Invoke(nameof(ShowPlayAgainBtn), 3f);

        // Fader.SetActive(false);
    }

    public void LoadNextScene()
    {
        StartCoroutine("LoadingScene");
    }

    public void RestartScene()
    {
        StartCoroutine("RestartingScene");
    }

    IEnumerator LoadingScene()
    {
        Fader.SetActive(true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MenuPortrait");
    }

    IEnumerator RestartingScene()
    {
        Fader.SetActive(true);

        yield return new WaitForSeconds(1f);
        //InitMenuScript.isRestart = true;
        SceneManager.LoadScene("MenuPortrait");
    }

    void ResetDeathCounter()
    {
        deathCounter = 0;
    }

    public void UpdateGameTimer()
    {
        gameTimer -= Time.deltaTime / 2;
        TimeSpan t = TimeSpan.FromSeconds(gameTimer);

        string answer = string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);

        gameTimerUI.text = answer;


    }

    void StartTimer()
    {
        cue.GetComponent<SpriteRenderer>().enabled = true;
        shotPowerscript.ShowPowerBar();//mohith

        //targetLine.SetActive(true);
        cueController.targetLine.SetActive(true);
        //spinBall.SetActive(true);
        allowDisplayTouch = true;
        cueController.EnableWhiteBallCollider();
        shotPowerscript.EnableShotPowerScriptCollider();
        //countdownUI.SetActive(false);
        stopGameTimer = false;
    }

    //public void callDecreasePlayerLife()
    //{

    //    if (canReduceLife)
    //    {
    //        canReduceLife = false;
    //        DecreasePlayerLife();

    //    }
    //}

    public void DecreasePlayerLife()
    {
        ballPlayedCount++;


        canReduceLife = false;
        playerLife = playerLife - 1;

        StartCoroutine("DecreasePlayerLifeCor");

    }

    IEnumerator DecreasePlayerLifeCor()
    {
        if (playerLife == 4)
        {
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[4].transform.DOScale(0, 1);
            missedPopup_PlayerlifeBallUI[4].transform.DOScale(0, 1);
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[4].transform.DOScale(1, 1);
            missedPopup_PlayerlifeBallUI[4].transform.DOScale(1, 1);
            // playerLifeBallUI[4].GetComponent<SpriteRenderer>().enabled = false;
            lostLifeUI[4].GetComponent<Image>().enabled = true;

            // missedPopup_PlayerlifeBallUI[4].GetComponent<SpriteRenderer>().enabled = false;
            missedPopup_lostLifeUI[4].GetComponent<Image>().enabled = true;
        }
        if (playerLife == 3)
        {
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[3].transform.DOScale(0, 1);
            missedPopup_PlayerlifeBallUI[3].transform.DOScale(0, 1);
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[3].transform.DOScale(1, 1);
            missedPopup_PlayerlifeBallUI[3].transform.DOScale(1, 1);
            //playerLifeBallUI[3].GetComponent<SpriteRenderer>().enabled = false;
            lostLifeUI[3].GetComponent<Image>().enabled = true;

            //  missedPopup_PlayerlifeBallUI[3].GetComponent<SpriteRenderer>().enabled = false;
            missedPopup_lostLifeUI[3].GetComponent<Image>().enabled = true;
        }
        if (playerLife == 2)
        {
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[2].transform.DOScale(0, 1);
            missedPopup_PlayerlifeBallUI[2].transform.DOScale(0, 1);
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[2].transform.DOScale(1, 1);
            missedPopup_PlayerlifeBallUI[2].transform.DOScale(1, 1);
            // playerLifeBallUI[2].GetComponent<SpriteRenderer>().enabled = false;
            lostLifeUI[2].GetComponent<Image>().enabled = true;

            // missedPopup_PlayerlifeBallUI[2].GetComponent<SpriteRenderer>().enabled = false;
            missedPopup_lostLifeUI[2].GetComponent<Image>().enabled = true;


        }
        if (playerLife == 1)
        {
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[1].transform.DOScale(0, 1);
            missedPopup_PlayerlifeBallUI[1].transform.DOScale(0, 1);
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[1].transform.DOScale(1, 1);
            missedPopup_PlayerlifeBallUI[1].transform.DOScale(1, 1);
            //  playerLifeBallUI[1].GetComponent<SpriteRenderer>().enabled = false;
            lostLifeUI[1].GetComponent<Image>().enabled = true;

            // missedPopup_PlayerlifeBallUI[1].GetComponent<SpriteRenderer>().enabled = false;
            missedPopup_lostLifeUI[1].GetComponent<Image>().enabled = true;
        }
        if (playerLife == 0)
        {
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[0].transform.DOScale(0, 1);
            missedPopup_PlayerlifeBallUI[0].transform.DOScale(0, 1);
            yield return new WaitForSeconds(1f);
            playerLifeBallUI[0].transform.DOScale(1, 1);
            missedPopup_PlayerlifeBallUI[0].transform.DOScale(1, 1);
            //playerLifeBallUI[0].GetComponent<SpriteRenderer>().enabled = false;
            lostLifeUI[0].GetComponent<Image>().enabled = true;

            //  missedPopup_PlayerlifeBallUI[0].GetComponent<SpriteRenderer>().enabled = false;
            missedPopup_lostLifeUI[0].GetComponent<Image>().enabled = true;
        }

    }


    public void AddScore(int score, int ballNumber, GameObject colliderPosition, int ballCount)
    {
        StartCoroutine(AddScoreCor(score, ballNumber, colliderPosition, ballCount));

    }

    public int commonTrickScore = 0;
    [SerializeField]
    GameObject bonusStreakUi;
    IEnumerator AddScoreCor(int score, int ballNumber, GameObject colliderPosition, int ballCount)

    {
        //if(canAddScore)
        {
            canAddScore = false;
            potedBallValue++;


            //Score logic...
            int currentscore = 0;
            int wallscore = 0;
            int comboscore = 0;
            int doubleComboScore = 0;
            bool isballBonus = false;



            if (wallTrickshot)
            {
                //currentscore = score * ballNumber;
                wallscore = wallscore + wallTSvalue;
            }
            if (ballCount > 1)
            {
                if (ScoreController.instance.firstBallPotedCounter >= 1)
                {
                    ballTrickShot = true;
                    comboscore += ballTSvalue;
                }
            }

            if (doubleBallBonus)
            {
                Debug.Log("doubleBallBonus");
                isballBonus = true;
                doubleBallBonus = false;

                doubleComboScore += ballTSvalue;
            }

            currentscore = score * ballNumber;

            if (wallTrickshot || ballTrickShot || isballBonus)
            {
                Debug.Log("is ball trick shottttt");
                GameObject trickVfx = Instantiate(trickVfxPrefab, colliderPosition.transform.position, Quaternion.identity);
                trickVfx.transform.position = new Vector3(colliderPosition.transform.position.x, colliderPosition.transform.position.y, -1f);
            }
            else
            {
                GameObject normalVfx = Instantiate(normalVfxPrefab, colliderPosition.transform.position, Quaternion.identity);
                normalVfx.transform.position = new Vector3(colliderPosition.transform.position.x, colliderPosition.transform.position.y, -1f);
                if (score == 10)
                    normalVfx.GetComponent<SpriteRenderer>().color = Color.yellow;
                else if (score == 15)
                    normalVfx.GetComponent<SpriteRenderer>().color = Color.green;

            }

            //Normal score
            GameObject scoredUI = Instantiate(currentScoredPointPrefab, colliderPosition.transform.position, Quaternion.identity);
            TextMeshPro currentScoreUI = scoredUI.transform.Find("ScoredTextUI").GetComponent<TextMeshPro>();
            currentScoreUI.text = currentscore.ToString();

            gamescore += currentscore;
            //gamescore += wallscore + comboscore + currentscore;
            //print("Game Score = " + score + "PotValue " + gamescore + "Gamescore");
            //piper for ball streak bonus

            if (wallTrickshot || ballTrickShot)
            {
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                yield return new WaitForSeconds(3.5f);
            }
            //update score ui.. // add a time delay
            scoretext.text = gamescore.ToString();

            StartCoroutine(ShowBonusStreakBonus(currentscore, colliderPosition));


            //store data for Result Screen...
            //yield return new WaitForSeconds(1f);

            if (wallTrickshot)
            {
                yield return new WaitForSeconds(2.5f);

                GameObject wallshotobj = Instantiate(wallBonusPrefab, colliderPosition.transform.position, Quaternion.identity);
                GameObject trickShotText = Instantiate(trickShotTextPrefab, colliderPosition.transform.position, Quaternion.identity);

                TextMeshPro wallshotUI = wallshotobj.transform.Find("ScoredTextUI").GetComponent<TextMeshPro>();
                wallshotUI.text = wallscore.ToString();

                yield return new WaitForSeconds(1.5f);

                gamescore += wallscore;
                scoretext.text = gamescore.ToString();
            }

            if (ballTrickShot && commonTrickScore == 0)
            {
                Debug.Log("ballTrickShot");
                yield return new WaitForSeconds(2.5f);

                GameObject comboBonusobj = Instantiate(comboBonusPrefab, colliderPosition.transform.position, Quaternion.identity);
                GameObject trickShotText = Instantiate(trickShotTextPrefab, colliderPosition.transform.position, Quaternion.identity);

                TextMeshPro comboBonusUI = comboBonusobj.transform.Find("ScoredTextUI").GetComponent<TextMeshPro>();
                comboBonusUI.text = ballTSvalue.ToString();

                gamescore += comboscore;

                commonTrickScore = comboscore;

                yield return new WaitForSeconds(1.5f);


                scoretext.text = gamescore.ToString();
            }
            else if (isballBonus && commonTrickScore == 0)
            {
                Debug.Log("isballBonus");
                isballBonus = false;
                GameObject doubleBonusobj = Instantiate(comboBonusPrefab, colliderPosition.transform.position, Quaternion.identity);
                GameObject trickShotText = Instantiate(trickShotTextPrefab, colliderPosition.transform.position, Quaternion.identity);

                TextMeshPro doublComboBonusUI = doubleBonusobj.transform.Find("ScoredTextUI").GetComponent<TextMeshPro>();
                doublComboBonusUI.text = ballTSvalue.ToString();
                gamescore += doubleComboScore;

                commonTrickScore = doubleComboScore;

                yield return new WaitForSeconds(1.5f);


                scoretext.text = gamescore.ToString();
            }





            //Updating result ball UI
            BallResultMenu(ballNumber, score);//mohith uncomment later

            //Updating Shots total...
            FinalShotsSCore += currentscore;
            shotScoreUI.text = FinalShotsSCore.ToString();

            //updating Tricks total...
            FinalTSScore += wallscore + commonTrickScore;
            trickshotScoreUI.text = FinalTSScore.ToString();

            //updating Time total....
            //FinalTimeLeft = FinalTimeLeft * 10;
            FinalTimeLeft = (int)gameTimer;

            // timeBonus = 0;// FinalTimeLeft * 3;
            // timeLeftUI.text = timeBonus.ToString();

            TotalScore = FinalShotsSCore + FinalTSScore + timeBonus + totalStreakBonus;
            totalScoreUI.text = TotalScore.ToString();

            streakBonusUI.text = totalStreakBonus.ToString();

            //updating Best Score...
            if (PlayerPrefs.GetInt("Score") <= TotalScore)
            {
                PlayerPrefs.SetInt("Score", TotalScore);
            }

            bestScoreUI.text = PlayerPrefs.GetInt("Score").ToString();

        }
    }
    public int totalStreakBonus;
    //Piper for bonus streak UI
    public IEnumerator ShowBonusStreakBonus(int currentscore, GameObject colliderPosition)
    {
        if (ballStreakBonus != 0)
            StartCoroutine(ActivateStreakBonusUI(ballStreakBonus + 1));

        if (ballStreakBonus >= 2)
        {
            //bonusStreakUi.SetActive(true);
            //bonusStreakUi.GetComponentInChildren<Text>().text = "x" + ballStreakBonus;
            // yield return new WaitForSeconds(1f);
            // bonusStreakUi.SetActive(false);
            //Vector3 pos;
            // pos = new Vector3(bonusStreakUi.transform.position.x, bonusStreakUi.transform.position.y, -1);
            yield return new WaitForSeconds(0.5f);

            GameObject streakShot = Instantiate(streakBonusTextPrefab, colliderPosition.transform.position, Quaternion.identity);
            GameObject scoredUI = Instantiate(currentScoredPointPrefab, colliderPosition.transform.position, Quaternion.identity);
            scoredUI.transform.localScale = new Vector3(1, 1, 1);
            TextMeshPro currentScoreUI = scoredUI.transform.Find("ScoredTextUI").GetComponent<TextMeshPro>();
            int streakBonus;
            streakBonus = ballStreakBonus * currentscore;
            currentScoreUI.text = streakBonus.ToString();
            gamescore += streakBonus;
            yield return new WaitForSeconds(3.5f);
            scoretext.text = gamescore.ToString();
            totalStreakBonus = totalStreakBonus + streakBonus;
            Debug.Log("totalstreakBonus::" + totalStreakBonus);
        }
    }
    IEnumerator ActivateStreakBonusUI(int streakCount)
    {
        yield return new WaitForSeconds(2f);
        streakBonusActivatedUI.SetActive(true);
        streakBonusImg.sprite = streakImgs[streakCount - 2];
        streakBonusImg.SetNativeSize();
        if (streakCount > 2)
        {
            iTween.MoveTo(streakBonusActivatedUI.gameObject, iTween.Hash("y", 100, "time", 1,
            "easetype", iTween.EaseType.linear, "islocal", true));
            yield return new WaitForSeconds(2f);
        }
        iTween.MoveTo(streakBonusActivatedUI.gameObject, iTween.Hash("y", 100, "time", 1,
            "easetype", iTween.EaseType.linear, "islocal", true));

        //streakBonusActivatedUI.transform.GetChild(2).GetComponent<Text>().text = streakCount + "x";
        //yield return new WaitForSeconds(12f);
        // streakBonusActivatedUI.SetActive(false);
    }
    public void DeactivateStreakBonusUI()
    {
        iTween.MoveTo(streakBonusActivatedUI.gameObject, iTween.Hash("y", 2000, "time", 1,
            "easetype", iTween.EaseType.linear, "islocal", true));
    }
    public int firstBallPotedCounter, noBallCounter;



    public void RandomizePots()
    {



        if (ballPotted == true)
        {
            multiplierValue++;
            StartCoroutine("SwitchScoreMultiplier");
        }
        Debug.Log("firstballpotedcounter " + firstBallPotedCounter);
        if (!ballPotted)
        {

            //firstBallPotedCounter++;
            if (firstBallPotedCounter >= 2)
            {

                DecreasePlayerLife();
                Debug.Log("Endgame::" + endgame);
                if (!endgame)
                    StartCoroutine("ShowNoBallPocketedMessage");
            }

        }
        ballPotted = false;
    }


    public void TurnOffFirstMultiplier()
    {
        StartCoroutine("SwitchScoreMultiplier");

        firstMultiplier[0].SetActive(false);
        firstMultiplier[1].SetActive(false);
        firstMultiplier[2].SetActive(false);
        firstMultiplier[3].SetActive(false);
        firstMultiplier[4].SetActive(false);
        firstMultiplier[5].SetActive(false);

        potMultiplier[0].SetActive(true);
        potMultiplier[1].SetActive(true);
        potMultiplier[2].SetActive(true);
        potMultiplier[3].SetActive(true);
        potMultiplier[4].SetActive(true);
        potMultiplier[5].SetActive(true);
    }

    public void BlackBallBonusOn()
    {
        StartCoroutine("SwitchScoreMultiplier");

        // StartCoroutine("ShowBlackBallBonusMessage");

        if (blackBallBonus[0].activeSelf)
        {
            potMultiplier[0].SetActive(true);
            potMultiplier[1].SetActive(true);
            potMultiplier[2].SetActive(true);
            potMultiplier[3].SetActive(true);
            potMultiplier[4].SetActive(true);
            potMultiplier[5].SetActive(true);

            blackBallBonus[0].SetActive(false);
            blackBallBonus[1].SetActive(false);
            blackBallBonus[2].SetActive(false);
            blackBallBonus[3].SetActive(false);
            blackBallBonus[4].SetActive(false);
            blackBallBonus[5].SetActive(false);
        }
        else
        {
            potMultiplier[0].SetActive(false);
            potMultiplier[1].SetActive(false);
            potMultiplier[2].SetActive(false);
            potMultiplier[3].SetActive(false);
            potMultiplier[4].SetActive(false);
            potMultiplier[5].SetActive(false);

            blackBallBonus[0].SetActive(true);
            blackBallBonus[1].SetActive(true);
            blackBallBonus[2].SetActive(true);
            blackBallBonus[3].SetActive(true);
            blackBallBonus[4].SetActive(true);
            blackBallBonus[5].SetActive(true);
        }
    }

    public void BlackBallBonusMessage()
    {
        StartCoroutine("ShowBlackBallBonusMessage");
    }

    IEnumerator ShowBlackBallBonusMessage()
    {
        blackBallBonusMessage.gameObject.SetActive(true);
        DisablePots();
        isShowingSomeUI = true;
        yield return new WaitForSeconds(3f);
        blackBallBonusMessage.gameObject.SetActive(false);
        EnablePots();
        isShowingSomeUI = false;
    }

    public void ShowNoBallPocked()
    {
        StartCoroutine(ShowNoBallPocketedMessage());
    }
    public int ballStreakBonus;//piper
    public bool isShowingSomeUI;//mohith
    IEnumerator ShowNoBallPocketedMessage()
    {
        Debug.LogError("show no ball pocketed");
        PoolGame_GameManager.Instance.whiteBall.transform.DOScale(0, 1);

        GameObject shadow = PoolGame_GameManager.Instance.whiteBall.GetComponent<FakeShadowController>().shadow;

        shadow.transform.DOScale(0, 1);
        if (playerLife != 0)
        {
            noBallPocketedMessage.gameObject.SetActive(true);
            isShowingSomeUI = true;
            DisablePots();//mohith
            cue.GetComponent<SpriteRenderer>().enabled = false;//piper
            shotPowerscript.HidePowerBar();//mohith

            if (ballStreakBonus > 0)
            {
                ballStreakBonus = 0;
                DeactivateStreakBonusUI();
                Debug.Log("ballstreak bonus cancelled");
            }
            yield return new WaitForSeconds(4f);
            noBallPocketedMessage.gameObject.SetActive(false);
            isShowingSomeUI = false;
            EnablePots();//mohith
            cue.GetComponent<SpriteRenderer>().enabled = true;//piper
            shotPowerscript.ShowPowerBar();//mohith
            PoolGame_GameManager.Instance.whiteBall.transform.DOScale(0.33f, 1);
            shadow.transform.DOScale(1.5f, 1);
        }

    }

    private bool isCoroutineExecuting = false;

    IEnumerator SwitchScoreMultiplier()
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        for (int i = 0; i < pots.Count; i++)
        {
            pots[i].transform.DOScale(0, 1);
        }

        //multiplierObj.transform.DOScale(3, 1);

        yield return new WaitForSeconds(1f);
        inititalPositions.Rotate();
        for (int i = 0; i < pots.Count; i++)
        {
            pots[i].position = inititalPositions[i];
        }
        yield return new WaitForSeconds(0.5f);

        //multiplierObj.transform.DOScale(1, 1);

        for (int i = 0; i < pots.Count; i++)
        {
            pots[i].transform.DOScale(0.3f, 1);
            pots[i].GetComponent<Renderer>().enabled = false;
        }

        isCoroutineExecuting = false;

    }
    public void DisablePots()
    {
        for (int i = 0; i < pots.Count; i++)
        {
            pots[i].gameObject.SetActive(false);
        }
        cue.SetActive(false);
    }
    public void EnablePots()
    {
        for (int i = 0; i < pots.Count; i++)
        {
            pots[i].gameObject.SetActive(true);
        }
        cue.SetActive(true);
    }
    public void BallResultMenu(int ballNumber, int score)
    {
        if (ballNumber == 2)
        {
            colorBallsUI[0].text = "x" + score;
        }
        else if (ballNumber == 3)
        {
            colorBallsUI[1].text = "x" + score;
        }
        else if (ballNumber == 4)
        {
            colorBallsUI[2].text = "x" + score;
        }
        else if (ballNumber == 5)
        {
            colorBallsUI[3].text = "x" + score;
        }
        else if (ballNumber == 6)
        {
            colorBallsUI[4].text = "x" + score;
        }
        else if (ballNumber == 7)
        {
            colorBallsUI[5].text = "x" + score;
        }
        else if (ballNumber == 8)
        {
            colorBallsUI[6].text = "x" + score;
        }
        else if (ballNumber == 9)
        {
            colorBallsUI[7].text = "x" + score;
        }
        else if (ballNumber == 10)
        {
            colorBallsUI[8].text = "x" + score;
        }
        else if (ballNumber == 12)
        {
            colorBallsUI[9].text = "x" + score;
        }
    }

    [SerializeField] SpriteRenderer soundIcon;
    [SerializeField] Image soundOnOffIcon;
    [SerializeField] Sprite soundOnSprite, soundOffSprite, sOn, sOff;

    bool soundMuted;
    public bool SoundMuted
    {
        get
        {
            return soundMuted;
        }
        set
        {
            soundMuted = value;
            soundIcon.sprite = soundMuted ? soundOffSprite : soundOnSprite;
            soundOnOffIcon.sprite = soundMuted ? sOff : sOn;
            PlayerPrefs.SetInt("Muted", soundMuted ? 1 : 0);
            AudioListener.volume = soundMuted ? 0 : 1;
        }
    }

    public void SoundButton()
    {
        SoundMuted = !SoundMuted;
    }

    [SerializeField] Image musicOnOffIcon;
    [SerializeField] SpriteRenderer musicIcon;
    [SerializeField] Sprite musicOnSprite, musicOffSprite, mOn, mOff;


    bool musicMuted;
    public bool MusicMuted
    {
        get
        {
            return musicMuted;
        }
        set
        {
            musicMuted = value;
            musicIcon.sprite = musicMuted ? musicOffSprite : musicOnSprite;
            musicOnOffIcon.sprite = musicMuted ? mOff : mOn;
            PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0);

        }
    }

    public void MusicButton()
    {
        MusicMuted = !MusicMuted;
    }

    public void MultiplierMeshRendererOn()
    {
        for (int i = 0; i < pots.Capacity; i++)
        {
            foreach (Renderer r in pots[i].GetComponentsInChildren<Renderer>())
            {
                // Debug.LogError("r.name::" + r.name);
                r.enabled = true;
            }
        }
    }
    public void Pause_ResumeButtonCLicked()
    {
        Time.timeScale = 2;
        pausedMenu.SetActive(false);
    }
    public void Pause_QuitBtnCLicked()
    {
        Time.timeScale = 2;
        pausedMenu.SetActive(false);
        isQuitBtnClicked = true;
        gameOverUI.SetActive(false);
        successUI.SetActive(false);
        timeUpUI.SetActive(false);

        cue.GetComponent<SpriteRenderer>().enabled = false;
        shotPowerscript.HidePowerBar();//mohith

        ResultMenu.SetActive(true);
        Invoke(nameof(ShowPlayAgainBtn), 3f);

    }

    #region Watch Video Popup

    public GameObject WatchVideoPopup, watchVideoBtn;
    public GameObject livesImg, timerImg;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI currentScoreText;
    public GameObject watchVideoSuccessBanner;
    public Text BannerText;
    public void ShowRewardedAdPopup(bool isTimeUp)
    {

        WatchVideoPopup.SetActive(true);
        timeUpUI.SetActive(false);
        gameOverUI.SetActive(false);
        currentScoreText.text = "" + TotalScore;
        if (isTimeUp)
        {
            descriptionText.text = "GET 2 MINUTES EXTRA TIME";
            timerImg.SetActive(true);
            livesImg.SetActive(false);
            watchVideoBtn.GetComponent<PoolGame_GameStake.RewardedAds>().rewardType = PoolGame_GameStake.RewardedAds.RewardType.Timer;
        }
        else//life description
        {
            descriptionText.text = "GET 1 EXTRA LIFE";
            timerImg.SetActive(false);
            livesImg.SetActive(true);
            watchVideoBtn.GetComponent<PoolGame_GameStake.RewardedAds>().rewardType = PoolGame_GameStake.RewardedAds.RewardType.Lifes;
        }
    }
    bool showRewardedAdPopup;
    public bool isTimeUp;
    public bool isLostLives;
    public Sprite redHeart;
    public GameObject addTimerObj, timerPivotObj;
    public GameObject addlivesAnimObj, heartlivesObj;
    public IEnumerator ShowLCOption(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        StartCoroutine("ShowResultMenu");

        //Debug.LogError("showrewardedadPopup::" + showRewardedAdPopup);
        //if (!showRewardedAdPopup)
        //{
        //    if (isTimeUp)
        //        ShowRewardedAdPopup(true);
        //    else
        //        ShowRewardedAdPopup(false);


        //}
        //else
        //{
        //    StartCoroutine("ShowResultMenu");
        //}
    }
    public void VideoSuccessEvent(bool isLives)
    {
        WatchVideoPopup.SetActive(false);
        showRewardedAdPopup = true;
        if (isLives)
        {
            isLostLives = false;

            FlyLivesAnim();
        }
        else
        {
            isTimeUp = false;

            FlyTimerAnim();
        }

    }
    void FlyLivesAnim()
    {
        addlivesAnimObj.SetActive(true);

        iTween.ScaleFrom(addlivesAnimObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
        iTween.MoveTo(addlivesAnimObj, iTween.Hash("x", heartlivesObj.transform.position.x, "y", heartlivesObj.transform.position.y, "time", 1,
            "easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
        iTween.ScaleTo(addlivesAnimObj, iTween.Hash("x", 0, "y", 0, "time", 1, "easetype", iTween.EaseType.spring, "delay", 1.2f));

        Invoke(nameof(LivesSuccessDelay), 2.2f);
            

    }

    void LivesSuccessDelay()
    {
        playerLife = 1;
        endgame = false;
        stopGameTimer = false;
        lostLifeUI[0].GetComponent<Image>().enabled = false;

        WhiteBall.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
        EnablePots();
    }
    void FlyTimerAnim()
    {
        addTimerObj.SetActive(true);
        iTween.ScaleFrom(addTimerObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
        iTween.MoveTo(addTimerObj, iTween.Hash("x", timerPivotObj.transform.position.x, "y", timerPivotObj.transform.position.y, "time", 1,
            "easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
        iTween.ScaleTo(addTimerObj, iTween.Hash("x", 0, "y", 0, "time", 1.5, "easetype", iTween.EaseType.spring, "delay", 1.2f));

        Invoke(nameof(TimerSuccesDelay), 2.2f);
    }

    void TimerSuccesDelay()
    {
        gameTimer = 120;
        endgame = false;
        stopGameTimer = false;
        cue.SetActive(true);
    }
    void DisableBanner()
    {
        watchVideoSuccessBanner.SetActive(false);
        
    }
    public void SubmitBtnClicked()
    {
        WatchVideoPopup.SetActive(false);
        showRewardedAdPopup = true;
        gameOverUI.SetActive(false);
        successUI.SetActive(false);
        timeUpUI.SetActive(false);

        cue.GetComponent<SpriteRenderer>().enabled = false;
        shotPowerscript.HidePowerBar();//mohith

        ResultMenu.SetActive(true);
        Invoke(nameof(ShowPlayAgainBtn), 3f);
    }

#endregion
}

public static class ExtensionMethods
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Rotate<T>(this IList<T> list)
    {
        T first = list[0];
        list.RemoveAt(0);
        list.Add(first);
    }
}