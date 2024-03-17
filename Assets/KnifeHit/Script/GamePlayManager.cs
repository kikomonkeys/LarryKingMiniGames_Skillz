//using EasyMobile;
//using GoogleMobileAds.Api;
//using PiperAptoidePlugin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//using VLxSecurity;

public class GamePlayManager : MonoBehaviour
{


    public static GamePlayManager instance;
    [Header("Circle Setting")]
    public Circle[] circlePrefabs;
    public Bosses[] BossPrefabs;
    public GameObject _circlePrefabSingle;
    public Transform circleSpawnPoint;

    [Header("Knife Setting")]
    public Knife knifePrefab;
    public List<Knife> knifeList;
    public Transform KnifeSpawnPoint;
    [Range(0f, 1f)] public float knifeHeightByScreen = .1f;

    public GameObject ApplePrefab;
    [Header("UI Object")]
    public Text lblScore;
    public Text lblStage;


    [Header("UI Name")]
    public Text lblName;
    public Text lbloppoName;

    [Header("UI oppoObject")]
    public Text oppolblScore;

    public List<Image> stageIcons;
    public Color stageIconActiveColor;
    public Color stageIconNormalColor;

    [Header("UI Boss")]
    public GameObject bossFightStart;
    public GameObject bossFightEnd;
    public AudioClip[] bossFightStartSounds;
    public AudioClip[] bossFightEndSounds;
    [Header("Ads Show")]
    public GameObject adsShowView;
    public Image adTimerImage;
    public Text adSocreLbl;

    [Space]
    [Header("Lives")]
    public List<GameObject> liveObj = new List<GameObject>();
    public Sprite lostLifeSpr, lifeSpr;

    [Header("GameOver Popup")]
    public GameObject gameOverView;
    public Text gameOverSocreLbl, gameOverStageLbl;
    public GameObject newBestScore;
    public Text finalScoreText;
    [Space(50)]

    public int cLevel = 0;
    public bool isDebug = false;
    string currentBossName = "";
    public Circle currentCircle;
    Knife currentKnife;
    bool usedAdContinue;
    public int totalSpawnKnife;
    public RectTransform safeAreaTransform, rootCanvasTranform;
    [HideInInspector]
    public float knifeScale;

    //AptiodeManager aptiodeManager;
    [SerializeField]
    public Dictionary<string, string> userResData = new Dictionary<string, string>();


    [Header("UI Finish")]
    public Text flblName;
    public Text flbloppoName;
    public Text fslblName;
    public Text fslbloppoName;
    public GameObject gameOverMulti;
    public Text timerLabel;
    public GameObject gameOverSingle;

    public Text myScore, myScoreSingle, playerNameText;
    public Text myScoreDouble,justSingleScore,lifesScoretxt,livesLeftText;
    public Text winAmount;

    public Text playerScore;

    public Text player1Score, player1NameText;
    public Text player2Score, player2NameText;
    public Text player3Score, player3NameText;

    public GameObject pausePage;
    public GameObject howToPlayObj;


    [Header("Round Data")]

    public RoundData RoundData;

    public Image bg, header;
    public List<Sprite> bgSpr, headerSpr;

    [Header("Time Left")]
    public GameObject timeLeftObj;
    public Text timeLeftText;
    public GameObject timeUpObj,noLivesObj;
    public Text pointsTextPrefab;
    public GameObject heartlivesObj;
    public GameObject countDownObj;
    public Text countDownText;
    public GameObject toastMsg;
    public GameObject touchArea;
    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (instance == null)
        {
            instance = this;
        }
        if (!PlayerPrefs.HasKey("sessionCount"))
        {
            PlayerPrefs.SetInt("sessionCount", 0);
        }
        
    }
    void SetRandomBg()
    {
        int rand;
        rand = Random.Range(0, bgSpr.Count-1);
        bg.sprite = bgSpr[rand];
        header.sprite = headerSpr[rand];
        if (bg.sprite == null)
        {
            bg.sprite = bgSpr[0];
        }
    }
    static int sessionCount;
    void Start()
    {
        sessionCount++;
        PlayerPrefs.SetInt("sessionCount", sessionCount);
        if (GameManager.Knife_HowToPlay == 0)
        {
            if(PlayerPrefs.GetInt("sessionCount") == 1)
            howToPlayObj.SetActive(true);
        }
        else
        {
            howToPlayObj.SetActive(false);
            howToPlayObj.transform.position = new Vector3(10000, 0, 0);
            Destroy(howToPlayObj);
            StartGameNow();
        }
            


#if UNITY_IOS
        float bottom = safeAreaTransform.rect.height * -5 / rootCanvasTranform.rect.height;
        KnifeSpawnPoint.position = new Vector3(0, bottom + 1 + 1.7f, 0);
#endif
        // if (aptiodeManager != null)
        //    userResData = aptiodeManager.userResData;

        //  Debug.Log("StartGame Count." + userResData.Count);

        //time = 10;
        //StartCoroutine(StopTheGame());

         //isPlayingGame = true;
    }
    
   
    public void StartGameNow()//piper
    {
        howToPlayObj.SetActive(false);
        
        StartGame();
        SetRandomBg();
        InvokeRepeating(nameof(StopTheGame), 4.5f, 1);
        knifePrefab = knifeList[Random.Range(0, knifeList.Count)];
        lifes = 3;
        Invoke(nameof(EnableCountDownObj), 0.2f);
        //Invoke(nameof(DisableHowtoPlay), 1f);
    }
    void DisableHowtoPlay()
    {
        howToPlayObj.SetActive(false);
    }
    void EnableCountDownObj()
    {
        //countDownObj.SetActive(true);
        StartCoroutine(StartCountDown(0f));
    }
   
    public int time = 120;
    int lifes = 0;
    int appleScr;
    public void storeAppleScore(int val)
    {
        appleScr += val;
    }

    int finalScore;
    public Text knifeScore_2, multiplierBonusText_2, finalScoreText_2;
    public GameObject smallPopup, bigPopup;
    public void SetScoreForTheGame()
    {
        var a = (appleScr * 2) - GameManager.score;
        if (a < 0)
        {
            a *= -1;
        }
        justSingleScore.text = a.ToString();

      
        if (lifes <= 0)
        {
            //smallpopup scores
            knifeScore_2.text = a.ToString();
            multiplierBonusText_2.text = (appleScr * 2).ToString();
        }
        else
        {
            //bigpopup scorre
            myScoreSingle.text = GameManager.score.ToString();
            myScoreDouble.text = (appleScr * 2).ToString();
        }

        if (SettingUI.isGameQuitByUser)
        {
            lifesScoretxt.text = (lifes *  0).ToString();
            livesLeftText.text = "";
            livesLeftText.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
            finalScore = (GameManager.score + (lifes * 0));
            //SettingUI.isGameQuitByUser = false;
        }
        
        else
        {
            if (isLostLives)
            {
                lifesScoretxt.text = "0";
                livesLeftText.text = "";
                livesLeftText.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                lifesScoretxt.text = (lifes * 150).ToString();
                livesLeftText.text = "x" + lifes;
            }
            finalScore = (GameManager.score + (lifes * 150));
        }
        //Debug.LogError("score::" + GameManager.score + "life score::" + (lifes * 50) + "total::" + (GameManager.score + (lifes * 50)));
        finalScoreText.text = finalScore.ToString();
        if (lifes <= 0)
            finalScore = GameManager.score;
        finalScoreText_2.text = finalScore.ToString();

    }
    IEnumerator ShowTimeLeftObj(string str)
    {
        yield return new WaitForSeconds(0f);
        timeLeftObj.SetActive(true);
        timeLeftText.text = str;
        if(str == "30s Left")
        {
            timerLabel.color = Color.red;
            timerLabel.gameObject.GetComponent<PingPongAnim>().enabled = true;
        }
        yield return new WaitForSeconds(2f);
        timeLeftObj.SetActive(false);
    }
    public GameObject WatchVideoPopup, watchVideoBtn;
    public GameObject livesImg, timerImg;
    public Text descriptionText;
    public GameObject addlivesAnimObj, addtimerAnimObj;
    public void ShowRewardedAdPopup(bool isTimeUp)
    {
        WatchVideoPopup.SetActive(true);
        if (isTimeUp)
        {
            descriptionText.text = "GET 2 MINUTES EXTRA TIME";
            timerImg.SetActive(true);
            livesImg.SetActive(false);
            watchVideoBtn.GetComponent<KnifeGame_Gamestake.RewardedAds>().rewardType = KnifeGame_Gamestake.RewardedAds.RewardType.Timer;
        }
        else//life description
        {
            descriptionText.text = "GET 1 EXTRA LIFE";
            timerImg.SetActive(false);
            livesImg.SetActive(true);
            watchVideoBtn.GetComponent<KnifeGame_Gamestake.RewardedAds>().rewardType = KnifeGame_Gamestake.RewardedAds.RewardType.Lifes;
        }
    }
    public void VideoSuccessEvent(bool isLives)
    {
        WatchVideoPopup.SetActive(false);

        if (isLives)
        {
            isLostLives = false;
            lifes = 1;
            
            
            noLivesObj.SetActive(false);
            FlyLivesAnim();
        }
        else
        {
            isTimeUp = false;
            
            time = 120;
            timeUpObj.SetActive(false);
            timerLabel.color = Color.white;
            timerLabel.gameObject.GetComponent<PingPongAnim>().enabled = false;
            timerLabel.gameObject.transform.localScale = Vector3.one;
            
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

        Invoke(nameof(EnableTouchArea), 2.5f);
        Invoke(nameof(AddLifeImg), 2.0f);
        InvokeRepeating(nameof(StopTheGame), 2.5f, 1);

    }
    void AddLifeImg()
    {
        for (int i = 0; i < lifes; i++)
        {
            liveObj[i].GetComponent<Image>().sprite = lifeSpr;
        }
    }
    void FlyTimerAnim()
    {
        addtimerAnimObj.SetActive(true);

        iTween.ScaleFrom(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
        iTween.MoveTo(addtimerAnimObj, iTween.Hash("x", timerLabel.transform.GetChild(0).transform.position.x, "y", timerLabel.transform.GetChild(0).transform.position.y, "time", 1,
            "easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
        iTween.ScaleTo(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 1.5, "easetype", iTween.EaseType.spring, "delay", 1.2f));
        Invoke(nameof(EnableTouchArea), 2.5f);
        InvokeRepeating(nameof(StopTheGame), 2.5f, 1);
    }
    void EnableTouchArea()
    {
        touchArea.SetActive(true);
    }

    float min, sec;
    bool isTimeUp;
    bool isLostLives;
    bool showRewardedAdPopup;
    public void StopTheGame()
    {
        //Debug.Log("stop the game");
        countDownObj.SetActive(false);
        touchArea.SetActive(true);
        time--;
        if(time <= 120 && time >= 118)
        {
            StartCoroutine(ShowTimeLeftObj("2 Min Left"));
        }
        else if (time <= 60 && time >= 58)
        {
            StartCoroutine(ShowTimeLeftObj("1 Min Left"));
        }
        else if (time <= 30 && time >= 28)
        {
            StartCoroutine(ShowTimeLeftObj("30s Left"));
        }
        else if (time <= 10 && time >= 8)
        {
            StartCoroutine(ShowTimeLeftObj("10s Left"));
        }
        //Debug.LogError("lifes::" + lifes);
        if (lifes <= 0)
        {
            min = Mathf.CeilToInt(time) / 60;
            sec = Mathf.CeilToInt(time) % 60;
            timerLabel.text = "" + min.ToString("0") + ":" + sec.ToString("00");
            touchArea.SetActive(false);

            CancelInvoke(nameof(StopTheGame));
            isLostLives = true;
            lifes = 0;

            SetScoreForTheGame();
            noLivesObj.SetActive(true);
            playerScore.text = GameManager.score.ToString();
            Invoke(nameof(ShowLCOption), 2f);
        }
        if (time <= 0)
        {
            min = Mathf.CeilToInt(time) / 60;
            sec = Mathf.CeilToInt(time) % 60;
            timerLabel.text = "" + min.ToString("0") + ":" + sec.ToString("00");

            CancelInvoke(nameof(StopTheGame));

            isTimeUp = true;
            SetScoreForTheGame();
            
            playerScore.text = GameManager.score.ToString();
            timeUpObj.SetActive(true);
            touchArea.SetActive(false);
            Invoke(nameof(ShowLCOption), 2f);
            //if (!hasSingle)
            //    gameOverMulti.gameObject.SetActive(true);
            //else
            //{
            //    gameOverSingle.gameObject.SetActive(true);
            //}
            //flblName.text = lblName.text;

            //fslblName.text = GameManager.score.ToString();
            //Invoke(nameof(BackToHome), 3.0f);
            //Time.timeScale = 0;
        }
        else
        {
            min = Mathf.CeilToInt(time) / 60;
            sec = Mathf.CeilToInt(time) % 60;
            //timerLabel.text = time.ToString();
            timerLabel.text = "" + min.ToString("0") + ":" + sec.ToString("00");

            //yield return new WaitForSeconds(1);
           // Debug.Log("Sending..GameManager.score == > " + GameManager.score);
          //  if (aptiodeManager != null)
          //      aptiodeManager.SendSCoreToPlayer(GameManager.score, "PLAYING");
            //yield return new WaitForSeconds(90);
           // Debug.Log("Finished..");

        }
        
    }
    public void ShowLCOption()
    {
        ShowLC();
        //if (!showRewardedAdPopup)
        //{
        //    if (isTimeUp)
        //        ShowRewardedAdPopup(true);
        //    else
        //        ShowRewardedAdPopup(false);

        //    showRewardedAdPopup = true;
        //}
        //else
        //{
        //    ShowLC();
        //}
    }
    public void ShowLC()
    {
        if (isTimeUp)
        {
            timeUpObj.SetActive(false);
            isTimeUp = false;
        }
        if (isLostLives)
        {
            noLivesObj.SetActive(false);
            isLostLives = false;
        }
        if (!hasSingle)
            gameOverMulti.gameObject.SetActive(true);
        else
        {
            gameOverSingle.gameObject.SetActive(true);
            if (lifes <= 0)
            {
                smallPopup.SetActive(true);
                bigPopup.SetActive(false);
            }
            else
            {
                if (SettingUI.isGameQuitByUser)
                {
                    smallPopup.SetActive(true);
                    bigPopup.SetActive(false);
                    var a = (appleScr * 2) - GameManager.score;
                    if (a < 0)
                    {
                        a *= -1;
                    }
                    knifeScore_2.text = a.ToString();
                    multiplierBonusText_2.text = (appleScr * 2).ToString();
                    if (lifes <= 0)
                        finalScore = GameManager.score;
                    finalScoreText_2.text = finalScore.ToString();
                    SettingUI.isGameQuitByUser = false;
                }
                else
                {
                    bigPopup.SetActive(true);
                    smallPopup.SetActive(false);
                }
               
            }
        }
        flblName.text = lblName.text;

        fslblName.text = GameManager.score.ToString();
        isPlayingGame = false;

        Invoke(nameof(EnableNextBtn), 2f);
        //Invoke(nameof(BackToHome), 3.0f);i
        //sTime.timeScale = 0;
       /// Invoke(nameof(ShowToast), 2f);
       // PlayerPrefs.SetString("%^&@789", StringCipher.Encrypt(finalScore.ToString(), "Piper@102030#"));
        //GameStakeSDK.instance.OnGameComplete(ShowToast);
    }
    
    public GameObject nextBtn, nextBtn_2;
    void EnableNextBtn()
    {
        nextBtn.GetComponent<Button>().interactable = true;
        nextBtn.GetComponentInChildren<Text>().color = Color.white;
        nextBtn.GetComponentInChildren<Outline>().enabled = true;

        nextBtn_2.GetComponent<Button>().interactable = true;
        nextBtn_2.GetComponentInChildren<Text>().color = Color.white;
        nextBtn_2.GetComponentInChildren<Outline>().enabled = true;
    }
    void ShowToast()
    {
        toastMsg.SetActive(true);
        iTween.MoveFrom(toastMsg.gameObject, iTween.Hash("y", 2000, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeOutBack, "islocal", true));
        iTween.MoveTo(toastMsg.gameObject, iTween.Hash("y", 2000, "time", 0.5f, "delay", 2.5f, "easetype", iTween.EaseType.easeInBack, "islocal", true));
        //toastMsg.SetActive(false);
    }
    public void StartAgain()
    {
        GameManager.score = 0;
        GameManager.oppoScore = 0;
        SceneManager.LoadScene("HomeScene");

    }
    public static bool isKnifeHitToBubble;
    public void SpawnPointsText(int score,int fontSize)
    {
        GameObject go;
        go = Instantiate(pointsTextPrefab.gameObject);
        go.transform.SetParent(heartlivesObj.transform);
        go.transform.localPosition = new Vector3(-150, 0, 0);
        //Debug.LogError("score in points prefab::" + score + "fontsize::" + fontSize);
        go.GetComponent<Text>().text = "+" + score;
        go.GetComponent<Text>().resizeTextMaxSize = fontSize;
    }
    private void OnEnable()
    {
        Timer.Schedule(this, 0.1f, AddEvents);
       // if (aptiodeManager != null)
          //  aptiodeManager.ScoreUpdated += CheckTheGameStatus;
    }

  /* private void ScoreUpdated(ResponseData data)
    {
        CheckTheGameStatus(data);
    }
    void CheckTheGameStatus(ResponseData data)
    {
        Debug.Log("data.statusstatus == " + data.status);
        switch (data.status)
        {
            case nameof(STATUS.PLAYING):
                {
                    ScoreUpdatedData(data);
                }
                break;
            case nameof(STATUS.PENDING):
                {

                }
                break;
            case nameof(STATUS.COMPLETED):
                {
                    if (aptiodeManager != null)
                        aptiodeManager.SendSCoreToPlayer(GameManager.score, "COMPLETED");

                    if (!hasSingle)
                        gameOverMulti.gameObject.SetActive(true);
                    else
                    {
                        gameOverSingle.gameObject.SetActive(true);
                    }

                }
                break;
            case nameof(STATUS.COMPLETING):
                {

                }
                break;
            default:
                break;

        }
    }*/
    bool hasSingle = true;
    private void ScoreUpdatedData(/*ResponseData root = null*/)
    {
       // Debug.Log("ResponseData >>>>>>> " + root.users.Count);

        Debug.Log("SESSIONe >>>>>>> " + userResData["SESSION"]);
        Debug.Log("USER_IDe >>>>>>> " + userResData["USER_ID"]);
        Debug.Log("ROOM_IDe >>>>>>> " + userResData["ROOM_ID"]);
        Debug.Log("WALLET_ADDRESSe >>>>>>> " + userResData["WALLET_ADDRESS"]);
        int i = 0;
        /*if (root.users != null && root.users.Count >= 2 && userResData.Count > 0)
        {
            foreach (User user in root.users)
            {
                if (root.status == STATUS.COMPLETED.ToString())
                {
                    if (root.status == STATUS.COMPLETED.ToString())
                    {
                        if (root.room_result.winner.wallet_address == userResData["WALLET_ADDRESS"])
                        {
                            winAmount.text = root.room_result.winner_amount.ToString();

                        }

                    }
                }
                Debug.Log("SendScore == " + userResData["WALLET_ADDRESS"]);
                hasSingle = false;
                if (user.wallet_address.ToLower() != userResData["WALLET_ADDRESS"].ToLower())
                {
                    if (root.users.Count == 2) {
                        GameManager.oppoScore = user.score;
                        UpdateLable();
                    }



                    Debug.Log("Oppoonent score == " + user.score);
                    if (i == 0)
                    {
                        player1Score.transform.parent.gameObject.SetActive(true);
                        player1Score.text = user.score + "";
                        player1NameText.text = user.user_name;

                        oppolblScore.gameObject.SetActive(true);
                        oppolblScore.text = user.score + "";
                        if (lbloppoName != null)
                            lbloppoName.text = user.user_name;
                        lbloppoName.gameObject.SetActive(true);// = user.user_name;


                    }
                    else if (i == 1) {
                        player2Score.transform.parent.gameObject.SetActive(true);

                        player2Score.text = user.score + "";

                        player2NameText.text = user.user_name;

                    }
                    else if (i == 2) {
                        player3Score.transform.parent.gameObject.SetActive(true);

                        player3Score.text = user.score + "";

                        player3NameText.text = user.user_name;

                    };
                    i++;

                }
                else
                {
                    lblName.text = user.user_name;
                    playerNameText.text = user.user_name;
                    myScore.text = user.score.ToString();
                    myScoreSingle.text = user.score.ToString();
                    playerScore.text = user.score.ToString();

                }

            }

        }*/
    }


    private void AddEvents()
    {
#if UNITY_ANDROID || UNITY_IOS
        //if (AdmobController.instance.rewardBasedVideo != null)
        //{
        //    AdmobController.instance.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        //    AdmobController.instance.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        //}
#endif
    }

    //bool doneWatchingAd = false;
    //public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    //{
    //    if (usedAdContinue)
    //    {
    //        doneWatchingAd = true;
    //        AdShowSucessfully();
    //    }
    //}

    //public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
    //{
    //    if (usedAdContinue)
    //    {
    //        if (doneWatchingAd == false)
    //        {
    //            adsShowView.SetActive(false);
    //            usedAdContinue = false;
    //            ShowGameOverPopup();
    //        }
    //    }
    //}

//    private bool IsAdAvailable()
//    {
//        if (AdmobController.instance.rewardBasedVideo == null) return false;
//        bool isLoaded = AdmobController.instance.rewardBasedVideo.IsLoaded();
//        return isLoaded;
//    }

//    private void OnDisable()
//    {
//#if UNITY_ANDROID || UNITY_IOS
//        if (AdmobController.instance.rewardBasedVideo != null)
//        {
//            AdmobController.instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
//            AdmobController.instance.rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;
//        }
//#endif
//    }

    public void StartGame()
    {
        GameManager.Stage = 0;
        GameManager.score = 0;
        GameManager.oppoScore = 0;
        //GameManager.Stage = 1;
        GameManager.isGameOver = false;
        usedAdContinue = false;
        if (isDebug)
        {
            GameManager.Stage = cLevel;
        }
        SetupGame();
    }

    public void UpdateLable()
    {
        lblScore.text = GameManager.score + "";
        if (GameManager.Stage % 5 == 0)
        {
            for (int i = 0; i < stageIcons.Count - 1; i++)
            {
                stageIcons[i].gameObject.SetActive(false);
            }
            stageIcons[stageIcons.Count - 1].color = stageIconActiveColor;
            lblStage.color = stageIconActiveColor;
            lblStage.text = currentBossName;
        }
        else
        {
            lblStage.text = "STAGE " + GameManager.Stage;
            for (int i = 0; i < stageIcons.Count; i++)
            {
                stageIcons[i].gameObject.SetActive(true);
                stageIcons[i].color = GameManager.Stage % stageIcons.Count <= i ? stageIconNormalColor : stageIconActiveColor;
            }
            lblStage.color = stageIconNormalColor;
        }


    }

    public void SetupGame()
    {
        SpawnCircle();
        KnifeCounter.intance.setUpCounter(currentCircle.totalKnife);

        totalSpawnKnife = 0;
        StartCoroutine(GenerateKnife(0f));
    }
    public void ThrowKnife()
    {
        if (currentKnife == null || gameOverMulti.activeInHierarchy)
            return;
        if (!currentKnife.isFire)
        {
            KnifeCounter.intance.setHitedKnife(totalSpawnKnife);
            currentKnife.ThrowKnife();
            StartCoroutine(GenerateKnife(0f));
        }
    }

    bool isPlayingGame;
    void Update()
    {
        //if (currentKnife == null || gameOverMulti.activeInHierarchy)
        //    return;
        //if (Input.GetMouseButtonDown(0) && !currentKnife.isFire)
        //{
        //    KnifeCounter.intance.setHitedKnife(totalSpawnKnife);
        //    currentKnife.ThrowKnife();
        //    StartCoroutine(GenerateKnife());
        //}
       
    }

    public void SpawnCircle()
    {
        GameObject tempCircle;
        if (GameManager.Stage % 5 == 0 &&GameManager.Stage != 0)
        {
            Bosses b = BossPrefabs[Random.Range(0, BossPrefabs.Length)];
            tempCircle = Instantiate(b.BossPrefab, circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
            currentBossName = "Boss : " + b.Bossname;
            UpdateLable();
            OnBossFightStart();
        }
        else
        {
            /* Debug.LogError("Stage No " + GameManager.Stage);
             var index = GameManager.Stage > 50 ? Random.Range(11, circlePrefabs.Length - 1) : GameManager.Stage ;
            */
            if (!_circlePrefabSingle)
            {
                int index = 0;
                if (GameManager.Stage <= 4)
                {
                    //Debug.LogError("Probablity range EASY:HARD -> {100:0}");
                    index = Random.Range(0, 43);
                }
                else if (GameManager.Stage > 4 && GameManager.Stage <= 9)
                {
                    //Debug.LogError("Probablity range EASY:HARD -> {70:30}");
                    int probability = Random.Range(0, 10);
                    if (probability < 6)
                    {
                        index = Random.Range(0, 43);
                    }
                    else
                    {
                        index = Random.Range(44, circlePrefabs.Length - 1);
                    }

                }
                else if (GameManager.Stage > 10 && GameManager.Stage <= 14)
                {
                    //Debug.LogError("Probablity range EASY:HARD -> {50:50}");
                    int probability = Random.Range(1, 10);
                    if (probability < 6)
                    {
                        index = Random.Range(0, 43);
                    }
                    else
                    {
                        index = Random.Range(44, circlePrefabs.Length - 1);
                    }
                }
                else if (GameManager.Stage > 15 && GameManager.Stage <= 19)
                {
                   // Debug.LogError("Probablity range EASY:HARD -> {40:60}");
                    int probability = Random.Range(1, 10);
                    if (probability < 5)
                    {
                        index = Random.Range(0, 43);
                    }
                    else
                    {
                        index = Random.Range(44, circlePrefabs.Length - 1);
                    }
                }
                else
                {
                   // Debug.LogError("Probablity range EASY:HARD -> {30:70}");
                    int probability = Random.Range(1, 10);
                    if (probability < 4)
                    {
                        index = Random.Range(0, 43);
                    }
                    else
                    {
                        index = Random.Range(44, circlePrefabs.Length - 1);
                    }
                }
                tempCircle = Instantiate(circlePrefabs[index], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
                //Debug.LogError("Stage No " + GameManager.Stage + ", Circle No " + index);
            }
            else
            {
                tempCircle = Instantiate(_circlePrefabSingle, circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
            }
           
            
        }

        float circleScale = (GameManager.ScreenWidth * 0.5f) / tempCircle.GetComponent<SpriteRenderer>().bounds.size.x;
        circleScale = Mathf.Min(circleScale, 1);

        tempCircle.transform.localScale = Vector3.one * .2f;
        LeanTween.scale(tempCircle, new Vector3(circleScale, circleScale, circleScale), .3f).setEaseOutBounce();
        currentCircle = tempCircle.GetComponent<Circle>();
        currentCircle.circleScale = circleScale;
    }

    public IEnumerator OnBossFightStart()
    {
        bossFightStart.SetActive(false);
        //SoundManager.instance.PlaySingle(bossFightStartSounds[Random.Range(0, bossFightEndSounds.Length - 1)], 1f);
        yield return new WaitForSeconds(0f);
        bossFightStart.SetActive(false);
        SetupGame();
        //currentKnife.isFire = false;
    }

    public IEnumerator OnBossFightEnd()
    {
        bossFightEnd.SetActive(false);
        //SoundManager.instance.PlaySingle(bossFightEndSounds[Random.Range(0, bossFightEndSounds.Length - 1)], 1f);
        yield return new WaitForSeconds(0f);
        bossFightEnd.SetActive(false);
        SetupGame();

    }

    public IEnumerator GenerateKnife(float waittime)
    {
        //yield return new WaitForSeconds(waittime);
        yield return new WaitUntil(() =>
        {
            return KnifeSpawnPoint.childCount == 0;
        });
        if (currentCircle.totalKnife > totalSpawnKnife && !GameManager.isGameOver)
        {
            totalSpawnKnife++;
            //var prefab = GameManager.selectedKnifePrefab ?? knifePrefab;
            var prefab = knifePrefab;
            GameObject tempKnife = Instantiate(prefab, KnifeSpawnPoint.position + Vector3.down * 2f, Quaternion.identity, KnifeSpawnPoint).gameObject;
            
            knifeScale = (GameManager.ScreenHeight * knifeHeightByScreen) / tempKnife.GetComponent<SpriteRenderer>().bounds.size.y;
            tempKnife.transform.localScale = Vector3.one * knifeScale;
            LeanTween.moveLocalY(tempKnife, 0, 0.1f);
            tempKnife.name = "Knife" + totalSpawnKnife;
            currentKnife = tempKnife.GetComponent<Knife>();
        }

    }

    public void NextLevel()
    {
        Debug.Log("Next Level");
        if (currentCircle != null)
        {
            currentCircle.DestroyMeAndAllKnives();
        }
        if (GameManager.Stage % 5 == 0)
        {
            GameManager.Stage++;
            StartCoroutine(OnBossFightEnd());

        }
        else
        {
            GameManager.Stage++;
            if (GameManager.Stage % 5 == 0)
            {
                StartCoroutine(OnBossFightStart());
            }
            else
            {
                Invoke("SetupGame", .3f);
            }
        }
    }

    
    IEnumerator currentShowingAdsPopup;
    public void GameOver()
    {
        GameManager.isGameOver = true;

        //NextLevel();

        AdShowSucessfully();
        lifes--;
        if (lifes >= 0)
        {
            //Debug.LogError("lifes::" + lifes);
            //liveObj[lifes].SetActive(false);
            liveObj[lifes].GetComponent<Image>().sprite = lostLifeSpr;
        }
        //RestartGame();

        //RestartGame();

        //RestartGame();

        //if (usedAdContinue || !IsAdAvailable())
        //{
        //    ShowGameOverPopup();
        //}
        //else
        //{
        //    currentShowingAdsPopup = ShowAdPopup();
        //    StartCoroutine(currentShowingAdsPopup);
        //}
    }

    public IEnumerator ShowAdPopup()
    {
        adsShowView.SetActive(true);
        adSocreLbl.text = GameManager.score + "";
        SoundManager.instance.PlayTimerSound();
        for (float i = 1f; i > 0; i -= 0.01f)
        {
            adTimerImage.fillAmount = i;
            yield return new WaitForSeconds(0.1f);
        }
        CancleAdsShow();
        SoundManager.instance.StopTimerSound();
    }

//    public void OnShowAds()
//    {
//        doneWatchingAd = false;

//        SoundManager.instance.StopTimerSound();
//        SoundManager.instance.PlaybtnSfx();
//        usedAdContinue = true;
//        StopCoroutine(currentShowingAdsPopup);

//#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
//        AdmobController.instance.ShowRewardBasedVideo();
//#else
//        HandleRewardBasedVideoRewarded(null, null);
//#endif
//    }

    public void AdShowSucessfully()
    {
        adsShowView.SetActive(false);
        totalSpawnKnife--;
        GameManager.isGameOver = false;
        KnifeCounter.intance.setHitedKnife(totalSpawnKnife);
        if (KnifeSpawnPoint.childCount == 0)
        {
            StartCoroutine(GenerateKnife(0f));
        }
    }

    public void CancleAdsShow()
    {
        SoundManager.instance.StopTimerSound();
        SoundManager.instance.PlaybtnSfx();
        StopCoroutine(currentShowingAdsPopup);
        adsShowView.SetActive(false);
        ShowGameOverPopup();
    }

    public void ShowGameOverPopup()
    {
        gameOverView.SetActive(true);
        gameOverSocreLbl.text = GameManager.score + "";
        gameOverStageLbl.text = "Stage " + GameManager.Stage;
        finalScoreText.text = "" + GameManager.score;
        if (GameManager.score >= GameManager.HighScore)
        {
            GameManager.HighScore = GameManager.score;
            newBestScore.SetActive(true);
        }
        else
        {
            newBestScore.SetActive(false);
        }

        CUtils.ShowInterstitialAd();
    }

    public void OpenShop()
    {
        SoundManager.instance.PlaybtnSfx();
        KnifeShop.intance.ShowShop();
    }
    public void RestartGame()
    {
        //Debug.LogError("load the game freshly");
        SoundManager.instance.PlaybtnSfx();
        Time.timeScale = 1;

        GameManager.score = 0;
        GameManager.oppoScore = 0;
        GameManager.Stage = 0;
        GeneralFunction.intance.LoadSceneByName("GameScene");
        UnloadApp();
       
    }
    void UnloadApp()
    {
        Debug.LogError("unload the app");
       // Application.Quit();
        Application.Unload();
    }
    public static bool homeBtnClicked;
    public void BackToHome()
    {
        GameManager.score = 0;
        GameManager.oppoScore = 0;
        //PlayerPrefs.DeleteAll();
        homeBtnClicked = true;
        GameManager.Stage = 0;
        GeneralFunction.intance.LoadSceneByName("GameScene");
        Time.timeScale = 1;
    }

    public void FBClick()
    {
        SoundManager.instance.PlaybtnSfx();
        StartCoroutine(CROneStepSharing());
    }

    public void ShareClick()
    {
        SoundManager.instance.PlaybtnSfx();
        StartCoroutine(CROneStepSharing());
    }

    public void SettingClick()
    {
        SoundManager.instance.PlaybtnSfx();
        SettingUI.intance.ShowUI();
    }

    IEnumerator CROneStepSharing()
    {
        yield return new WaitForEndOfFrame();
      //  Sharing.ShareScreenshot("screenshot", "");
    }

    public void PauseBtnClicked()
    {
        pausePage.SetActive(true);
        Time.timeScale = 0;
    }
    IEnumerator StartCountDown(float waitime)
    {
        yield return new WaitForSeconds(waitime);
        countDownObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        countDownText.text = "3";
        yield return new WaitForSeconds(0.75f);
        countDownText.text = "2";
        yield return new WaitForSeconds(0.75f);
        countDownText.text = "1";
        yield return new WaitForSeconds(0.75f);
        countDownText.text = "GO";
        yield return new WaitForSeconds(1f);
        countDownObj.SetActive(false);
    }
}

[System.Serializable]
public class Bosses
{
    public string Bossname;
    public Circle BossPrefab;
}
