using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using VLxSecurity;
public class GameUI : MonoBehaviour
{
    public GameObject homeUI, inGameUI, finishUI, gameOverUI, playareaUI;
    public GameObject allbtns;

    private bool btns;

    [Header("PreGame")]
    public Button soundBtn;
    public GameObject soundOnSpr, soundOffSpr;
    public Button musicBtn;
    public Sprite musicOnSpr, musicOffSpr;
    [Header("InGame")]
    public Image levelSlider;
    public Image currentLevetImg;
    public Image nextLevelImg;
    public Text currentLevelText, nextLevelText;

    [Header("Finish")]
    public Text finishLevelText;

    [Header("GameOver")]
    public Text gameOverScoreText;
    public Text gameOverBestText;

    private Material playerMat;
    private Player player;

    public Text timerText;
    public Stackball_Timer timer;

    [Header("Settings Page")]
    public GameObject settingsOptionsPanel;

    [Header("Extra")]
    public GameObject scoreText;
    public GameObject howToPlayUI;
    public GameObject pauseUI;

    public static bool isLoadNextLevel = false;
    public static GameUI instance;
    public bool helpBtnClicked;

    public GameObject countDownObj;
    public Text countDownText;
    public GameObject scoreSubmttedObj;

    public static int ShowHowToPlay
    {
        get
        {
            return PlayerPrefs.GetInt("help", 0);
        }
        set
        {
            PlayerPrefs.SetInt("help", value);
        }
    }
    //public static int Music
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt("musicc", 0);
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt("musicc", value);
    //    }
    //}
    void Awake()
    {
        instance = this;
        playerMat = FindObjectOfType<Player>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = FindObjectOfType<Player>();
        Time.timeScale = 1.5f;
        AudioListener.volume = 1;

        if(!isLoadNextLevel)
        PlayerPrefs.SetInt("Level", 1);
        //levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;
        //levelSlider.color = playerMat.color;
        //currentLevetImg.color = playerMat.color;
        //nextLevelImg.color = playerMat.color;

        //soundBtn.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
        GetSoundAndMusic();
        //if (ShowHowToPlay == 0)
        //{
        //    howToPlayUI.SetActive(true);
        //}
        //else
        //{
        //    PlayBtnCLicked();
        //}
    }

    void GetSoundAndMusic()
    {
        if (SoundOrMusicBtn.Music == 0)
        {
            Stackball_GameStake.SoundManager.instance.musicManager.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            Stackball_GameStake.SoundManager.instance.musicManager.GetComponent<AudioSource>().enabled = false;
        }

    }
    private void Start()
    {
        currentLevelText.text = FindObjectOfType<LevelSpawner>().level.ToString();
        nextLevelText.text = FindObjectOfType<LevelSpawner>().level + 1 + "";
        timer = Stackball_Timer.instance;
        if (isLoadNextLevel)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            finishUI.SetActive(false);
            gameOverUI.SetActive(false);
            starttimer = true;
            Stackball_GameStake.ScoreManager.instance.AssignScoreText();
            player.playerState = Player.PlayerState.Playing;
        }
        if (isPlayBtnClickedInLC)
        {
            PlayBtnCLicked();
            isPlayBtnClickedInLC = false;
        }
        else if (isHomeBtnCLickedinLC)
        {
            homeUI.SetActive(true);
            isHomeBtnCLickedinLC = false;
        }
        else
        {
            if (ShowHowToPlay == 0)
            {
                howToPlayUI.SetActive(true);
            }
            else
            {
                PlayBtnCLicked();
            }
        }
      
    }

    private float time;
    public Text gameoverText;
    float min, sec;
    public bool starttimer;
    void Update()
    {
        if (starttimer)
        {
            time = timer.GetTime();
            min = Mathf.CeilToInt(time) / 60;
            sec = Mathf.CeilToInt(time) % 60;
            //timerText.text = time.ToString("0");
            timerText.text = "" + min.ToString("0") + ":" + sec.ToString("00");

            if (timer.GetTime() <= 0)
            {
                homeUI.SetActive(false);
                inGameUI.SetActive(false);
                finishUI.SetActive(false);
                isTimeUp = true;
                isLostLives = false;
                //Invoke(nameof(EnableTimeUpPage), 1f);
                Debug.Log("time up here");
                
                starttimer = false;
                StartCoroutine(ShowLCOption(0f));
            }
        }
       

        //if (timer.GetTime() <= 0)
        //{
        //    homeUI.SetActive(false);
        //    inGameUI.SetActive(false);
        //    finishUI.SetActive(false);
        //    Invoke(nameof(EnableTimeUpPage), 1f);
        //}

        if (player.playerState == Player.PlayerState.Prepare)//settings page
        {
            //Sound
            //if (SoundManager.instance.sound)// && soundBtn.GetComponent<Image>().sprite != soundOnSpr
            //{
            //    //soundBtn.GetComponent<Image>().sprite = soundOnSpr;
            //    soundOnSpr.SetActive(false);
            //    soundOffSpr.SetActive(true);
            //}
                
            //else if (!SoundManager.instance.sound)// && soundBtn.GetComponent<Image>().sprite != soundOffSpr
            //{
            //    //soundBtn.GetComponent<Image>().sprite = soundOffSpr;
            //    soundOnSpr.SetActive(true);
            //    soundOffSpr.SetActive(false);
            //}

            ////Music
            //if (Music==0)
            //    musicBtn.GetComponent<Image>().sprite = musicOnSpr;
            //else
            //    musicBtn.GetComponent<Image>().sprite = musicOffSpr;

        }

        if (Input.GetMouseButtonDown(0) && !IgnoreUI() && player.playerState == Player.PlayerState.Prepare)//&&  
        {
            player.playerState = Player.PlayerState.Playing;
            //homeUI.SetActive(false);
            //inGameUI.SetActive(true);
            //finishUI.SetActive(false);
            //gameOverUI.SetActive(false);
            Stackball_GameStake.ScoreManager.instance.AssignScoreText();

        }
        
        if (player.playerState == Player.PlayerState.Finish)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            playareaUI.SetActive(false);

            //finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level " + FindObjectOfType<LevelSpawner>().level;
        }

        if(player.playerState == Player.PlayerState.Died)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);

            if (isQuitbtnclicked)
            {
                EnableGameoverPage();

                isQuitbtnclicked = false;
            }
            else
            {
                
                Invoke(nameof(EnableGameoverPage), 1f);

            }
            //if (Input.GetMouseButtonDown(0))
            //{

            //}
        }
    }
    public GameObject nextBtn;
    public void SubmitScoreAndRestartGame()
    {
        isHomeBtnCLickedinLC = true;
        //GameStakeSDK.instance.OnGameComplete(ScoreManager.instance.score);
        PlayerPrefs.SetInt("Level", 1);
        timer.restartTime();
        StartCoroutine(LoadSceneDelay());
    }
    public static bool isPlayBtnClickedInLC, isHomeBtnCLickedinLC;
    public void Gameover_PlayBtn()
    {
        isPlayBtnClickedInLC = true;
        //GameStakeSDK.instance.OnGameComplete(ScoreManager.instance.score);
        PlayerPrefs.SetInt("Level", 1);
        timer.restartTime();
        StartCoroutine(LoadSceneDelay());
    }
    IEnumerator LoadSceneDelay() //called from invoke after 2 seconds
    {
        yield return new WaitForSecondsRealtime(0f);
        Stackball_GameStake.ScoreManager.instance.ResetScore();
        SceneManager.LoadScene("MainScene");
        UnloadApp();
    }
    void UnloadApp()
    {
        Debug.LogError("unload the app");
        // Application.Quit();
        Application.Unload();
    }
    IEnumerator EnableScoreSubmissionObj(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        scoreSubmttedObj.SetActive(true);
       
        //iTween.MoveFrom(scoreSubmttedObj.gameObject, iTween.Hash("y", 2000, "time", 0.5f, "delay", 0.25f));
        yield return new WaitForSeconds(3f);
        //iTween.MoveTo(scoreSubmttedObj.gameObject, iTween.Hash("y", 2000, "time", 0.5f, "delay", 2f));
        scoreSubmttedObj.SetActive(false);
    }
    public bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }
    int count;
    public void Settings()
    {
        //btns = !btns;
        //allbtns.SetActive(btns);
        count++;
        if (count % 2 == 0)
        {
            settingsOptionsPanel.SetActive(false);
        }
        else
        {
            settingsOptionsPanel.SetActive(true);
        }
    }

    public void PlayBtnCLicked()
    {
        if (ShowHowToPlay == 0)
        {
            howToPlayUI.SetActive(true);
        }
        else
        {
            howToPlayUI.SetActive(false);
        }
        //player.playerState = Player.PlayerState.Playing;
        Stackball_GameStake.ScoreManager.instance.AssignScoreText();
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
        finishUI.SetActive(false);
        homeUI.SetActive(false);
        if(!isLoadNextLevel)
        EnableCountDown();
    }
    public void StartGame()
    {
        howToPlayUI.SetActive(false);
    }
    public void HowToPlayBtnClicked()
    {
        helpBtnClicked = true;
        howToPlayUI.SetActive(true);
    }

    public void PauseBtnCLicked()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
        player.playerState = Player.PlayerState.pause;
    }
    public void Pause_ResumeBtnClicked()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1.5f;
        player.playerState = Player.PlayerState.Playing;
    }
    bool isQuitbtnclicked;
    public void Pause_QuitBtnClicked()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        isQuitbtnclicked = true;
        EnableGameoverPage();
    }

    int mCount;
    public void MusicBtnClicked()
    {
        mCount++;
        if (mCount % 2 == 0)// && musicBtn.GetComponent<Image>().sprite==musicOnSpr
        {
            musicBtn.GetComponent<Image>().sprite = musicOffSpr;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = musicOnSpr;
        }
    }
    void ShowToast()
    {
        StartCoroutine(EnableScoreSubmissionObj(1f));
    }
    void EnableNextBtn()
    {
        nextBtn.GetComponent<Button>().interactable = true;
        nextBtn.GetComponentInChildren<Text>().color = Color.white;
    }
    bool showScoresubmissionObj;
    public void EnableTimeUpPage()
    {
        homeUI.SetActive(false);
        inGameUI.SetActive(false);
        finishUI.SetActive(false);
        gameOverUI.SetActive(true);
        isLoadNextLevel = false;
        gameoverText.text = "Time Up";
        // Time.timeScale = 0;
        FindObjectOfType<Rotator>().speed = 0;
        AudioListener.volume = 0;
        gameOverScoreText.text = Stackball_GameStake.ScoreManager.instance.score.ToString();
        gameOverBestText.text = PlayerPrefs.GetInt("HighScore").ToString();
        Invoke(nameof(EnableNextBtn), 3f);
        //StartCoroutine(EnableScoreSubmissionObj(0.75f));
        Invoke(nameof(EnableNextBtn), 3f);

        //PlayerPrefs.SetString("P_@123$₹", StringCipher.Encrypt(ScoreManager.instance.score.ToString(), "Piper#102030"));
        //GameStakeSDK.instance.OnGameComplete(ShowToast);

    }
    public void EnableGameoverPage()
    {
        homeUI.SetActive(false);
        inGameUI.SetActive(false);
        finishUI.SetActive(false);
        isQuitbtnclicked = false;
        gameOverUI.SetActive(true);
        countDownObj.SetActive(false);
        isLoadNextLevel = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        FindObjectOfType<Rotator>().enabled = false;
        //Time.timeScale = 0;
        FindObjectOfType<Rotator>().speed = 0;
        AudioListener.volume = 0;
        gameOverScoreText.text = Stackball_GameStake.ScoreManager.instance.score.ToString();
        gameOverBestText.text = PlayerPrefs.GetInt("HighScore").ToString();
        //StartCoroutine(EnableScoreSubmissionObj(0.75f));


        Invoke(nameof(EnableNextBtn), 3f);
        TryToSubmitScoreToSkillz();
        // PlayerPrefs.SetString("P_@123$₹", StringCipher.Encrypt(ScoreManager.instance.score.ToString(), "Piper#102030"));
        //GameStakeSDK.instance.OnGameComplete(ShowToast);
        //player.playerState = Player.PlayerState.none;

    }
    IEnumerator StartCountDown(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        countDownObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        countDownText.gameObject.SetActive(true);
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.text = "GO";
        yield return new WaitForSeconds(1f);
        countDownObj.SetActive(false);
        starttimer = true;
        player.playerState = Player.PlayerState.Playing;
    }
    public void EnableCountDown()
    {
        StartCoroutine(StartCountDown(0f));
    }

    #region Watch Video Popup

    public GameObject WatchVideoPopup, watchVideoBtn;
    public GameObject livesImg, timerImg;
    public Text descriptionText;
    public Text currentScoreText;
    public void ShowRewardedAdPopup(bool isTimeUp)
    {

        WatchVideoPopup.SetActive(true);
        
        currentScoreText.text = "" + Stackball_GameStake.ScoreManager.instance.score;
        if (isTimeUp)
        {
            descriptionText.text = "GET 2 MINUTES EXTRA TIME";
            timerImg.SetActive(true);
            livesImg.SetActive(false);
            watchVideoBtn.GetComponent<Stackball_RewardedAds>().rewardType = Stackball_RewardedAds.RewardType.Timer;
        }
        else//life description
        {
            descriptionText.text = "GET 1 EXTRA LIFE";
            timerImg.SetActive(false);
            livesImg.SetActive(true);
            watchVideoBtn.GetComponent<Stackball_RewardedAds>().rewardType = Stackball_RewardedAds.RewardType.Lifes;
        }
    }
    public static bool showRewardedAdPopup;
    public bool isTimeUp;
    public bool isLostLives;
    public GameObject watchVideoSuccessBanner;
    public Text bannerText;
    public GameObject addtimerAnimObj, timerPivot;
    public IEnumerator ShowLCOption(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        EnableGameoverPage();
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
        //    //load lc
        //    EnableGameoverPage();
        //    showRewardedAdPopup = false;
        //}
    }
    public void ShowLcOptionNow()
    {
        StartCoroutine(ShowLCOption(0f));
    }
    public void VideoSuccessEvent(bool isLives)
    {
        WatchVideoPopup.SetActive(false);
        showRewardedAdPopup = true;
        if (isLives)
        {
            isLostLives = false;
            isTimeUp = false;
            starttimer = true;
            inGameUI.SetActive(true);
            player.visual.gameObject.SetActive(true);
            player.GetComponent<Rigidbody>().isKinematic = false;
            watchVideoSuccessBanner.SetActive(true);
            Invoke(nameof(DisableBanner), 2f);
            bannerText.text = "PLAYER SPAWNED SUCCESSFULLY";
        }
        else
        {
            isTimeUp = false;
            isLostLives = false;
            inGameUI.SetActive(true);
            FlyTimerAnim();
        }

    }
    void FlyTimerAnim()
    {
        addtimerAnimObj.SetActive(true);

        iTween.ScaleFrom(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
        iTween.MoveTo(addtimerAnimObj, iTween.Hash("x", timerPivot.transform.position.x, "y", timerPivot.transform.position.y, "time", 1,
            "easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
        iTween.ScaleTo(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 1.5, "easetype", iTween.EaseType.spring, "delay", 1.2f));

        Invoke(nameof(TimeSuccessDelay),2.2f);
    }
    void TimeSuccessDelay()
    {
        timer.AddTimeOnWatchVideo();
        starttimer = true;
        bannerText.text = "2 MIN ADDED SUCCESSFULLY";
    }
    void DisableBanner()
    {
        watchVideoSuccessBanner.SetActive(false);
    }
    public void SubmitBtnClicked()
    {
        WatchVideoPopup.SetActive(false);
        showRewardedAdPopup = false;
        //load lc
        EnableGameoverPage();
    }

    #endregion


    #region Score Submitting to Skillz

    public GameObject loadingPage;

    public void SubmitScorebtnClicked()
    {
        if (loadingPage != null)
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

    bool scoreSubmitSuccess;
    void TryToSubmitScoreToSkillz()
    {
        string score = Stackball_GameStake.ScoreManager.instance.score.ToString();
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
        SkillzCrossPlatform.DisplayTournamentResultsWithScore(Stackball_GameStake.ScoreManager.instance.score.ToString());
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
}
