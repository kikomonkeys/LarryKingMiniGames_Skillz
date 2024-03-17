using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SolitaireEngine;
using SolitaireEngine.Model;
using ManagerUtils;
using Calendar;
using System.Collections;
using UnityEngine.Events;
//using PiperAptoidePlugin;
using UnityEngine.UI;
using System;
using TMPro;
///using VLxSecurity;
public partial class StageManager : MonoBehaviour, ICardActions, IManagerBaseCommands, IHUDActions
{


    [SerializeField]
    public Dictionary<string, string> userResData = new Dictionary<string, string>();


    [Header("UI Finish")]
    public TextMeshProUGUI flblName;
    public TextMeshProUGUI flbloppoName;
    public TextMeshProUGUI fslblName;
    public TextMeshProUGUI fslbloppoName;
    public GameObject gameOverMulti;
    public TextMeshProUGUI timerLabel;
    public GameObject gameOverSingle;

    public TextMeshProUGUI myScore, myScoreSingle, playerNameText, myFinalScore, myBoardClearedBonus;
    public TextMeshProUGUI myTimeBonusText;
    public TextMeshProUGUI winAmount;

    public TextMeshProUGUI playerScore;

    public TextMeshProUGUI player1Score, player1NameText;
    public TextMeshProUGUI player2Score, player2NameText;
    public TextMeshProUGUI player3Score, player3NameText;

    [Header("UI Name")]
    public TextMeshProUGUI lblName;
    public TextMeshProUGUI lbloppoName;

    [Header("UI oppoObject")]
    public TextMeshProUGUI oppolblScore;


    [Header("UI Prize")]
    public TextMeshProUGUI mypize;
    public TextMeshProUGUI mypize2;
    public TextMeshProUGUI mypize3;
    public TextMeshProUGUI mypize4;
    public TextMeshProUGUI prizePpool, placeText;



    public GameObject countDownObj;
    public Text countDownText;
    public GameObject scoresubmitted;

    private HUDController hud;

    private static StageManager _instance;

    public static StageManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject stageManager = GameObject.Find("StageManager");
                // GameObject obj = Instantiate(PopUpPref);

                _instance = stageManager.GetComponent<StageManager>();
            }
            return _instance;

        }
    }


    private IManagerBaseCommands manager;
    private IViewBaseCommands viewer;

    private Solitaire solitaire;
    public Solitaire GetSolitaire { get { return solitaire; } } // Debuger Del it in release

    private CommandExecutor executor = new CommandExecutor();
    [HideInInspector]
    public CommandListUpdater commandUpdater;

    private CommandBuilder commandBuilder;
    private ManagerLogic managerLogic;
    public ManagerLogic GetManagerLogic { get { return managerLogic; } }
    private bool isBestTime = false;

    // TODO in future: move out into asset
    string[,] dealContract = new string[,]
    {
        { "1", "0", "0" ,"0", "0", "0", "0" },
        { " ", "1", "0", "0", "0", "0", "0" },
        { " ", " ", "1", "0", "0" ,"0", "0" },
        { " ", " ", " ", "1", "0" ,"0", "0" },
        { " ", " ", " ", " ", "1" ,"0", "0" },
        { " ", " ", " ", " ", " " ,"1", "0" },
        { " ", " ", " ", " ", " " ," ", "1" }
    };
    //testing, open all cards so use this, other wise comment the below 2d string aray
    //string[,] dealContract = new string[,]
    //{
    //    { "1", "1", "1" ,"1", "1", "1", "1" },
    //    { " ", "1", "1" ,"1", "1", "1", "1" },
    //    { " ", " ", "1" ,"1", "1", "1", "1" },
    //    { " ", " ", " ", "1", "1" ,"1", "1" },
    //    { " ", " ", " ", " ", "1" ,"1", "1" },
    //    { " ", " ", " ", " ", " " ,"1", "1" },
    //    { " ", " ", " ", " ", " " ," ", "1" }
    //};
    private StatusBar statusBar;

    public int randomCardBackImage;
    public Sprite[] deckCardSpr;
    public GameObject DeckCardImg;
    #region EnterGame
    public void OnNewGame()
    {
        randomCardBackImage = UnityEngine.Random.Range(0, 5);

        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Shift();

        // unblink cards to prevent bug when manager add some cards to blink when settings will opened
        // this line could be removed when pause game will work properly
        if (viewer == null) viewer = StageView.instance;
        viewer.UnblinkAll();




        // TODO: re-use old cards
        SolitaireStageViewHelperClass.instance.RemoveCards();
        //  if(ContinueModeGame.instance.LoadSuccess)
        // CardItemsDeck.instance.SetDeckImage(true);

        // wait one update to prevent some bugs
        StartCoroutine(InitGenegal());
        Invoke(nameof(GetDeckCardImg), 2f);
    }
    void GetDeckCardImg()
    {
        DeckCardImg = CardItemsDeck.instance.deckHiddenContainer.gameObject;
    }
    public void OnRestartGame()
    {
        managerLogic.InitScoring();
        InitHUD();
        solitaire.Restart();
        SolitaireStageViewHelperClass.instance.ResetView();
        viewer.Restart(solitaire.GetStartingCardsId(), OnGameRestarted);

        if (DebugGame.instance != null) {
            DebugGame.instance.StopTesting();
            DebugGame.instance.StartTesting(SolitaireStageViewHelperClass.instance, solitaire);
        }
    }
    void OnGameRestarted() {

    }
    /// <summary>
    /// this method is to change the sprite of the deck image when all the cards are seen,
    /// user will be given 3 chances and after 3, he/she will be charged woth 20points for each cards rotation
    /// </summary>
    public void ChangeDeckImageSpr(int num)
    {

        if (num == 1)
        {
            DeckCardImg.GetComponent<Image>().sprite = deckCardSpr[0];
        }
        else if (num == 2)
        {
            DeckCardImg.GetComponent<Image>().sprite = deckCardSpr[1];
        }
        else if (num == 3)
        {
            DeckCardImg.GetComponent<Image>().sprite = deckCardSpr[2];
        }
        else
        {
            DeckCardImg.GetComponent<Image>().sprite = deckCardSpr[3];
        }
    }
    public bool GetBestTime()
    {
        return isBestTime;
    }

    public void CreateCommand(int id, int near_id, int parent_id, int commandCase, bool isOpen)
    {
        ICommand command = null;
        if (commandBuilder == null)
        {
            commandBuilder = new CommandBuilder(solitaire, manager, viewer);
        }
        switch (commandCase)
        {
            case 1:
                //Shift Deck
                command = commandBuilder.CreateShiftDeckCommand(parent_id);
                break;
            case 2:
                //Turn Deck
                command = commandBuilder.CreateTurnDeckCommand();
                break;
            default:
                ICommand turnCardCommand = new TurnCardCommand(viewer, parent_id, isOpen);
                ICommand moveCardCommand = new MoveCardCommand(viewer, id, near_id, parent_id, true);
                command = new CommandsPair(moveCardCommand, turnCardCommand);

                break;
        }



        executor.Execute(command);
    }

    public void OnPause()
    {
    }
    #endregion;	
    //AptiodeManager aptiodeManager;

    private void Start()
    {
        GameSettings.Instance.isGameStarted = false;
        // Debug.Log("sound::" + PlayerPrefs.GetInt("Sound") + " Music::" + PlayerPrefs.GetInt("Music"));
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        // Debug.Log("sound::" + PlayerPrefs.GetInt("Sound") + " Music::" + PlayerPrefs.GetInt("Music"));

        if (PlayerPrefs.GetInt("Music") == 1)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
        //piper
        GameProgress gp = new GameProgress();
        gp.InitPlayerPref();
        //piper

        viewer = StageView.instance;
        manager = this;
        commandUpdater = CommandListUpdater.instanse;

        managerLogic = new ManagerLogic();
        statusBar = new StatusBar();
        //aptiodeManager = FindObjectOfType<AptiodeManager>();

        //if (aptiodeManager != null)
        //aptiodeManager.ScoreUpdated += CheckTheGameStatus;


        // if (aptiodeManager)
        //   userResData = aptiodeManager.userResData;


        //Invoke(nameof(StartTimer), 3f);//mo

        //mypize.text = "$0";
        //mypize2.text = "$0";
        //mypize3.text = "$0";
        //mypize4.text = "$0";
        // prizePpool.text = "$" + aptiodeManager.prizeMoney.ToString();
        //placeText.text = "1st Place";


        //Initialise Gamestake plugin
        Invoke("initalizeSDK", 2);

    }

    void initalizeSDK()
    {
        //  GameStakeSDK.instance.InitializeSDK(); // call this when game has begun;


    }
    void StartTimer()
    {
        startTimer = true;
    }
    private void CompletedPurchase(Dictionary<string, string> data)
    {
        userResData.Clear();
        userResData = data;
    }
    IEnumerator StartCountDown(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        countDownObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        countDownText.gameObject.SetActive(true);
        countDownText.text = "3";
        yield return new WaitForSeconds(0.7f);
        countDownText.text = "2";
        yield return new WaitForSeconds(0.7f);
        countDownText.text = "1";
        yield return new WaitForSeconds(0.7f);
        countDownText.text = "GO";
        yield return new WaitForSeconds(0.7f);
        countDownObj.SetActive(false);


        GameSettings.Instance.isGameStarted = true;
        InvokeRepeating(nameof(StopTheGame), 1, 1);
    }
    public void EnableCountDown(float time)
    {
        StartCoroutine(StartCountDown(time));
    }
    public float singlePlayerTimer;
    public bool startTimer;
    public GameObject timeUpObj;
    float min, sec;
    public void StopTheGame()
    {
        //timer = GetManagerLogic.timer;
        // Debug.Log("timer::" + singlePlayerTimer);

        //if (GetManagerLogic.timer <= 0)
        //{

        //}

        if (GetManagerLogic.timer >= 5 * 60)
        //if (GetManagerLogic.timer<= 0)
        {
            CancelInvoke(nameof(StopTheGame));
            //if (aptiodeManager != null)
            //	aptiodeManager.SendSCoreToPlayer(GetManagerLogic.score, "COMPLETED");
            // Debug.LogError("stop the game here");
            //	gameOverMulti.gameObject.SetActive(true);

            //GameSettings.Instance.isGameStarted = false;
            //flblName.text = lblName.text;
            //flbloppoName.text = lbloppoName.text;
            myScoreSingle.text = GetManagerLogic.score.ToString();
            myFinalScore.text = GetManagerLogic.score.ToString();

            //Invoke(nameof(BackToHome), 3.0f);
            //fslbloppoName.text = GameManager.oppoScore.ToString(); ;
        }
        else
        {
            // timerLabel.text = GetManagerLogic.timer.ToString();
            // timerLabel.text = singlePlayerTimer.ToString();
#if UNITY_EDITOR
           // GetManagerLogic.opposcore += 23;
           // HUDController.instance.SetOppsScore(GetManagerLogic.opposcore); // dependency
            //player1Score.transform.parent.parent.gameObject.SetActive(true);
            //player1Score.text = GetManagerLogic.opposcore + "";
            //player1NameText.text = "Hanna";


            //playerScore.text = GetManagerLogic.score + "";
            //playerNameText.text = "Sudhakar";
            //myScoreSingle.text = GetManagerLogic.score + "";
#endif
            //yield return new WaitForSeconds(1);
            //Debug.Log("Sending..GameManager.score == > " + GetManagerLogic.score);
            //if (aptiodeManager != null)
            //aptiodeManager.SendSCoreToPlayer(GetManagerLogic.score, "PLAYING");
            //yield return new WaitForSeconds(90);
            //Debug.Log("Finished..");

        }
    }
    bool isGameover = true;
    public void LoadGameover()
    {
        //Debug.LogError("Loading gameover");
        if (isGameover)
        {
            StartCoroutine(StopTheGameNow(0f));
            isGameover = false;
        }
    }
    void ShowScoreSubmitted()
    {
        scoresubmitted.SetActive(true);
        iTween.MoveFrom(scoresubmitted.gameObject, iTween.Hash("y", 2000, "time", 0.5, "delay", 0.5f));
        iTween.MoveTo(scoresubmitted.gameObject, iTween.Hash("y", 2000, "time", 0.5, "delay", 3));
    }
    public GameObject nextBtn;

    void EnableNextButn()
    {
        nextBtn.GetComponent<Button>().interactable = true;
        nextBtn.GetComponentInChildren<Text>().color = Color.white;
    }
    public int finalScore;
    public IEnumerator StopTheGameNow(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        //Debug.LogError("isgamewin::" + managerLogic.isGameWin + " isquitbtnclicked::" + SettingsPage.isQuitBtnClicked);
        if (!managerLogic.isGameWin && !Solitaire_GameStake.SettingsPage.isQuitBtnClicked)// 
        {
            // Debug.LogError("Enable timer obj");
            timeUpObj.SetActive(true);
            yield return new WaitForSeconds(2f);
            timeUpObj.SetActive(false);
            Solitaire_GameStake.SettingsPage.isQuitBtnClicked = false;
        }
        //else
        //{
        //    myBoardClearedBonus.transform.parent.gameObject.SetActive(true);
        //}

        gameOverSingle.gameObject.SetActive(true);

        GameSettings.Instance.isGameStarted = false;
        GetGameScore();
        //flblName.text = lblName.text;
        //flbloppoName.text = lbloppoName.text;
       

    }
    void GetGameScore()
    {
        myScoreSingle.text = GetManagerLogic.score.ToString();

        int totalCardsInDeck;
        totalCardsInDeck = GetTotalCardsInDeck();
        int cardsDrawnFromDeck;
        cardsDrawnFromDeck = 24 - totalCardsInDeck;
        // Debug.LogError("total cards drawn from the deck ::" + cardsDrawnFromDeck);//piper

        int timeBonus;

        if (GetManagerLogic.score > 100)
        {
            if (totalCardsInDeck == 24)
            {
                timeBonus = 0;
            }
            else
            {
                //timeBonus = (StatusBar.instance.myTimer / 5) + (totalCardsInDeck * 26);
                Debug.Log("timer value::" + StatusBar.instance.myTimer);
                if (StatusBar.instance.myTimer <= 0)
                    timeBonus = 0;
                else
                    timeBonus = (StatusBar.instance.myTimer / 5) + (cardsDrawnFromDeck * 5);
            }
        }
        else
        {
            timeBonus = 0;
        }

        myTimeBonusText.text = timeBonus.ToString();

        //board clear bonus
        int clearBonus = 0;
        if (managerLogic.isGameWin)
        {
            myBoardClearedBonus.transform.parent.gameObject.SetActive(true);
            clearBonus = 500;
            myBoardClearedBonus.text = clearBonus.ToString();
        }


        finalScore = GetManagerLogic.score + timeBonus + clearBonus;
        myFinalScore.text = finalScore.ToString();
        

        Invoke(nameof(EnableNextButn), 2f);
        //PlayerPrefs.SetString("$#@_123", StringCipher.Encrypt(finalScore.ToString(), "Piper#181929"));
        //GameStakeSDK.instance.OnGameComplete(ShowScoreSubmitted);

        //Debug.LogError("score::" + GetManagerLogic.score + " timebonus:" + timeBonus + " finalscore::" + finalScore);
    }
    //private void ScoreUpdated(ResponseData data)
    //{
    //    CheckTheGameStatus(data);
    //}
    //void CheckTheGameStatus(ResponseData data)
    //{
    //    Debug.Log("data.statusstatus == " + data.status);
    //    switch (data.status)
    //    {
    //        case nameof(STATUS.PLAYING):
    //            {
    //                ScoreUpdatedData(data);
    //            }
    //            break;
    //        case nameof(STATUS.PENDING):
    //            {

    //            }
    //            break;
    //        case nameof(STATUS.COMPLETED):
    //            {
    //                if (aptiodeManager != null)
    //                    aptiodeManager.SendSCoreToPlayer(GetManagerLogic.score, "COMPLETED");

    //                if (!hasSingle)
    //                    gameOverMulti.gameObject.SetActive(true);
    //                else
    //                {
    //                    gameOverSingle.gameObject.SetActive(true);
    //                }

    //            }
    //            break;
    //        case nameof(STATUS.COMPLETING):
    //            {

    //            }
    //            break;
    //        default:
    //            break;

    //    }
    //}
    public void StartAgain()
    {
        SceneManager.LoadScene("Solitaire_Temp");
    }
    bool hasSingle = true;
    //private void ScoreUpdatedData(ResponseData root)
    //{
    //    Debug.Log("ResponseData >>>>>>> " + root.users.Count);

    //    Debug.Log("SESSIONe >>>>>>> " + userResData["SESSION"]);
    //    Debug.Log("USER_IDe >>>>>>> " + userResData["USER_ID"]);
    //    Debug.Log("ROOM_IDe >>>>>>> " + userResData["ROOM_ID"]);
    //    Debug.Log("WALLET_ADDRESSe >>>>>>> " + userResData["WALLET_ADDRESS"]);
    //    int i = 0;
    //    if (root.users != null && root.users.Count >= 2 && userResData.Count > 0)
    //    {
    //        foreach (User user in root.users)
    //        {
    //            if (root.status == STATUS.COMPLETED.ToString())
    //            {
    //                placeText.text = "Lost";

    //                if (root.status == STATUS.COMPLETED.ToString())
    //                {
    //                    if (root.room_result.winner.wallet_address == userResData["WALLET_ADDRESS"])
    //                    {
    //                        winAmount.text = root.room_result.winner_amount.ToString();
    //                        mypize.text =  "$"+root.room_result.winner_amount.ToString();
    //                        placeText.text = "1st Place";

    //                    }

    //                }
    //            }
    //            Debug.Log("SendScore == " + userResData["WALLET_ADDRESS"]);
    //            hasSingle = false;
    //            if (user.wallet_address.ToLower() != userResData["WALLET_ADDRESS"].ToLower())
    //            {
    //                if (root.users.Count == 2)
    //                {
    //                    GetManagerLogic.opposcore = user.score;
    //                    HUDController.instance.SetOppsScore(user.score); // dependency

    //                    //UpdateLable();
    //                }



    //                Debug.Log("Oppoonent score == " + user.score);
    //                if (i == 0)
    //                {
    //                    player1Score.transform.parent.parent.gameObject.SetActive(true);
    //                    player1Score.text = user.score + "";
    //                    player1NameText.text = user.user_name;
    //                    GetManagerLogic.opposcore = user.score;
    //                    oppolblScore.gameObject.SetActive(true);
    //                    oppolblScore.text = user.score + "";
    //                    if (lbloppoName != null)
    //                        lbloppoName.text = user.user_name;
    //                    lbloppoName.gameObject.SetActive(true);// = user.user_name;
    //                    //HUDController.instance.statusBars
                        

    //                }
    //                else if (i == 1)
    //                {
    //                    player2Score.transform.parent.parent.gameObject.SetActive(true);

    //                    player2Score.text = user.score + "";

    //                    player2NameText.text = user.user_name;

    //                }
    //                else if (i == 2)
    //                {
    //                    player3Score.transform.parent.parent.gameObject.SetActive(true);

    //                    player3Score.text = user.score + "";

    //                    player3NameText.text = user.user_name;

    //                };
    //                i++;

    //            }
    //            else
    //            {
    //                lblName.text = user.user_name;
    //                playerNameText.text = user.user_name;
    //                myScore.text = user.score.ToString();
    //                myScoreSingle.text = user.score.ToString();
    //                playerScore.text = user.score.ToString();



    //            }

    //        }

    //    }
    //}

    IEnumerator InitGenegal()
    {
        yield return new WaitForEndOfFrame();
        InitSolitaire();
        InitHUD();
        managerLogic.InitScoring();
        //InitViewer();piper
        commandBuilder = new CommandBuilder(solitaire, manager, viewer);


        bool isTurnImage = (GameSettings.Instance.isStandardSet || (!GameSettings.Instance.isStandardSet && managerLogic.HasDeckTurn));

        CardItemsDeck.instance.SetDeckImage(isTurnImage, deckTurnCount);



        if (DebugGame.instance != null)
        {
            DebugGame.instance.StopTesting();
            DebugGame.instance.StartTesting(SolitaireStageViewHelperClass.instance, solitaire);
        }
    }
	private void InitSolitaire()
	{
        string[] solitaireData = new string[2];

        solitaireData = GameSettings.Instance.calendarData;
         
        if (solitaireData[0] == null || solitaireData[0] == string.Empty)
        {
          
            SolutionAsset solutionAsset = new SolutionAsset(GameSettings.Instance.isOneCardSet);
            solitaireData = solutionAsset.Get();
            GameSettings.Instance.calendarData = solitaireData;
        }
       
      
        PlayerPrefAPI.Set();
		CalendarParser parser = new CalendarParser ();
		List<Card> deck = parser.DeckParse (solitaireData [0]);
		List<ContractCommand> solutionCommands = parser.SolutionCommandsParse (solitaireData [1]);
		solitaire = new Solitaire (deck, solutionCommands);
	}
	public void InitViewer()
	{	
		ConstantContainer c = solitaire.GetStackHolderId();
        
		viewer.Init (c.NULL_CARD, c.DECK_STACK_HOLDER, c.FOUNDATION_STACK_HOLDER, c.TABLEAU_STACK_HOLDER, solitaire.GetStartingCards (), dealContract);
	}
	private void InitHUD ()
	{
		hud = HUDController.instance;
		hud.ResetStatusBars ();
	}
#region Game Complete Logic
	private void OnGameCompleted ()
	{
        managerLogic.isGameWin = true;
        LoadGameover();//piper
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Win();

        return;
		managerLogic.isGameWin = true;
        isBestTime = ((int)managerLogic.timer < StatsSettings.Instance.shortestTime[0] || StatsSettings.Instance.shortestTime[0] == 0);
        AddScoreTimeBonus ();
		managerLogic.SetStopStatsAndSetting ();
        managerLogic.SetStatsStreak();
        managerLogic.SetStatsScoreAndTime (managerLogic.score, (int) managerLogic.timer, managerLogic.moves);

		if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Win ();
        HUDController.instance.VisibleLayout(false);
        HUDController.instance.SetSolutionLayout(false);
        PopUpManager.Instance.ShowWin(OnCardDance);


    }
	private void AddScoreTimeBonus()
	{
		const float scaler = 10000;
		int bonus = (int) (1f / managerLogic.timer * scaler);
		managerLogic.score += bonus;
		HUDController.instance.SetScore(managerLogic.score); // dependency
	}
	private void OnCardDance ()
	{
		// check setting before animate card
		if (GameSettings.Instance.isCongratsScreenSet) // show card dancing effect
		{ 	
			if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Claps ();
			SolitaireStageViewHelperClass.instance.ShowDance (OnGameCompletedAction);
		}
		else // show win window without card dancing
			OnGameCompletedAction();
	}
	private void OnGameCompletedAction()
	{
		if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Shift ();

		if (GameSettings.Instance.isCalendarGame)
			PopUpReturnToCalendar ();
		else
			PopUpReturnToMenu();
	}
#endregion
	// helper
	public bool HasHint
	{
		get
		{
			return (GetHints ().Count > 0);
		}
	}

	int GetRandomHint()
	{			
		SolitaireEngine.Utility.Utils utils = new SolitaireEngine.Utility.Utils ();
		return utils.RandomElement (GetHints ()).IdFrom;		
	}
	private List<ContractCommand> GetHints ()
	{
        if (solitaire.GetHints().Count > 0)
            return solitaire.GetHints();

        // if no hinds found 
        // creaTE shake deck one cvommand list
        List<ContractCommand> list_c = new List<ContractCommand>();

        bool isDeckEmpty = solitaire.IsDeckEmpty();
        bool hasHintInDeck = solitaire.HasHintInDeck(GameSettings.Instance.isOneCardSet);


        bool hasDeckBlinkStandartSet = (!isDeckEmpty && hasHintInDeck);
        bool hasDeckBlinkVegasSet = (!isDeckEmpty && hasHintInDeck && managerLogic.HasDeckTurn);

        if (hasDeckBlinkStandartSet || hasDeckBlinkVegasSet)
        {
            int deck_id = solitaire.GetLastClosedCardInDeck();
            ContractCommand c = new ContractCommand(ContractCommand.State.ShiftDeckOnece, deck_id);
            list_c.Add(c);
            return list_c;
        }
        return list_c;
    }
		
	private IEnumerator ShowSolution ()
	{
		GeneralRestartGame ();
        HUDController.instance.SetSolutionLayout(true);
        // TODO remove it and use callback
        // wait card dealt
        yield return new WaitForSeconds (1.6f);

		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Solution1x);

		List<ICommand> solution_commands = commandBuilder.ConvertContractCommandToICommand (solitaire.GetSolution(), GameSettings.Instance.isOneCardSet);
       
		// block touches
		HUDController.instance.setTrigger (false, null);

	 	commandUpdater.ExecuteList (solution_commands, null, PopUpExitSolution);

		managerLogic.isAllowAutoComplete = false; // do not allow autocomplete game in Show Solution Mode.
		managerLogic.isAllowFinish = false; // do not allow finish game in Show Solution Mode.
	}
#region Update
	private void Update()
	{
        //Debug.LogError("isgamewin::" + managerLogic.isGameWin + " isgamestarted" + GameSettings.Instance.isGameStarted + " timer::" + managerLogic.timer);
		if (GameSettings.Instance.isGameStarted && !managerLogic.isGameWin) // beter bool like isFinished
		{
            IncreaseTimers();
	        CheckFinihUpdate ();
        }
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
     
      
        //if (GUILayout.Button("Win Game"))
        //{
        //    managerLogic.isAllowFinish = false;
        //    // wait when the last card arrived 
        //    StartCoroutine(runAter(() => OnGameCompleted(), .5f));
        //}
#endif
    }
    private void IncreaseTimers()
	{

		float time = Time.deltaTime;
        managerLogic.timer += time;

        hud.SetTime((int)managerLogic.timer);
		if (GameSettings.Instance.isStandardSet && managerLogic.scoreIncreaseTimer.Tick (time))
			//managerLogic.AddScore (managerLogic.DecreaseTimeScore);//piper
		
		if (managerLogic.hintTimer.Tick(time))
		{
			// check setting before blink card
			if (!GameSettings.Instance.isAutoHintSet)
				return;
			if (managerLogic.isAllowBlinkHint && HasHint)
			{
				viewer.BlinkCard (GetRandomHint());
				managerLogic.isAllowBlinkHint = false;
			}
		}
    }


    public IEnumerator VisibleButtonComplete()
    {
        yield return new WaitForSeconds(.7f);
        if (!IsRuleAprooveAutoComplete())
        {
            HUDController.instance.VisibleButtonComplete(false);
            managerLogic.isAllowAutoComplete = true;
        }
    }


    public void ResetAllowAutoComplete(bool allow)
    {
        managerLogic.isAllowAutoComplete = allow;
    }
    private void CheckFinihUpdate ()
	{


		if (managerLogic.isAllowAutoComplete &&  IsRuleAprooveAutoComplete())
		{
            managerLogic.isAllowAutoComplete = false;
            // wait when the card finished moving
            StartCoroutine(runAter( ()=>PopUpAutoComplete(), .5f));
		}

      
       
		if (managerLogic.isAllowFinish && solitaire.IsComplete ())
		{
			managerLogic.isAllowFinish = false;
			// wait when the last card arrived 
			StartCoroutine(runAter( ()=>OnGameCompleted(), .5f));
		}
	}
	private IEnumerator runAter(UnityAction runnable, float seconds){
		yield return new WaitForSeconds (seconds);
		if(runnable != null)
			runnable ();
	}

    
	private bool IsRuleAprooveAutoComplete()
	{
        bool isCommunityHasAllOpenCard = solitaire.IsCommunityHasAllOpenCard();


        return isCommunityHasAllOpenCard;
    }
#endregion

#region Exit Game
	public void GeneralRestartGame()
	{
		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToRestartGame ();
	}
	private void GeneralNewGame()
	{
	 
		ResetActivity ();
        HUDController.instance.VisibleButtonComplete(false);

		GameFlowDispatcher.Instance.FromManagerToNewGame ();
	}
	private void GeneralBackToMenu()
	{
		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToMenu ();
	}
	private void GeneralBackToCalendar()
	{
		ResetActivity ();
		GameSettings.Instance.isGameStarted = false;
		GameFlowDispatcher.Instance.FromManagerToCalendar ();
	}
	private void GeneralBackToSettings()
	{
//		ResetActivity ();
		GameFlowDispatcher.Instance.FromManagerToSettings ();
	}
	private void ResetActivity()
	{
		executor.Reset ();
		commandUpdater.Reset ();
		viewer.UnblinkAll ();
 
		SolitaireStageViewHelperClass.instance.ResetView ();
	}
#endregion



    public void AddStatusGame(int score, int timer, int move)
    {
        managerLogic.SetBeginMove(move);
        managerLogic.SetBeginTimer(timer);
        managerLogic.AddScore(score);
    }
    public void Gameover_HomeBtnClicked()
    {
        isHomeClickedInLC = true;
        SceneManager.LoadScene("Stage");
    }
    public static bool isPlayClickedInLC, isHomeClickedInLC;
    public void Gameover_PlayBtnClicked()
    {
        //  StartAgain();
        isPlayClickedInLC = true;
        SceneManager.LoadScene("Stage");
        UnloadApp();
    }

    void UnloadApp()
    {
        Debug.LogError("unload the app");
        // Application.Quit();
        Application.Unload();
    }
    [SerializeField]
    private List<GameObject> cardsCountInDeck;
    int totalCardsInDeck;
    public int GetTotalCardsInDeck()
    {
        GameObject go = SolitaireStageViewHelperClass.instance.FindCardItem(-100).gameObject;
        CardItem[] myChildCards = go.GetComponentsInChildren<CardItem>();
        for (int i = 0; i < myChildCards.Length; i++)
        {
            CardItem ci = myChildCards[i];
            cardsCountInDeck.Add(ci.gameObject);
        }

        //in -300 gameobject
        GameObject go1 = SolitaireStageViewHelperClass.instance.FindCardItem(-300).gameObject;
        CardItem[] myChildCards_1 = go1.GetComponentsInChildren<CardItem>();
        for (int i = 0; i < myChildCards_1.Length; i++)
        {
            CardItem ci = myChildCards_1[i];
            cardsCountInDeck.Add(ci.gameObject);
        }
        totalCardsInDeck = cardsCountInDeck.Count - 2;//-2 is to exclude cards with names -300.-100
        return totalCardsInDeck;
    }

    public GameObject pausePage;
    public GameObject howToplayPage;
    public GameObject canvasCards;
    public GameObject pauseBtn, undoBtn;
    public void PauseBtnClicked()
    {
        canvasCards.SetActive(false);
        pausePage.SetActive(true);
        pauseBtn.SetActive(false);
        undoBtn.SetActive(false);
    }

    public bool rulesBtnClicked;

    public void Pause_RulesBtnClicked()
    {
        rulesBtnClicked = true;
        howToplayPage.SetActive(true);
    }
    

#region Watch Video Popup

    public GameObject WatchVideoPopup, watchVideoBtn;
    public GameObject livesImg, timerImg;
    public Text descriptionText;
    public Text currentScoreText;
    public void ShowRewardedAdPopup(bool isTimeUp)
    {
        WatchVideoPopup.SetActive(true);
        GetGameScore();
        currentScoreText.text = "" + finalScore;
        if (isTimeUp)
        {
            descriptionText.text = "GET 2 MINUTES EXTRA TIME";
            timerImg.SetActive(true);
            livesImg.SetActive(false);
            watchVideoBtn.GetComponent<Solitaire_GameStake.RewardedAds>().rewardType = Solitaire_GameStake.RewardedAds.RewardType.Timer;
        }
        else//life description
        {
            descriptionText.text = "GET 1 EXTRA LIFE";
            timerImg.SetActive(false);
            livesImg.SetActive(true);
            watchVideoBtn.GetComponent<Solitaire_GameStake.RewardedAds>().rewardType = Solitaire_GameStake.RewardedAds.RewardType.Lifes;
        }
    }
    public bool showRewardedAdPopup;
    public bool isTimeUp;
    public bool isLostLives;
    public GameObject watchVideoSuccessBanner;
    public GameObject addtimerAnimObj;
    public GameObject timerIcon;
    public IEnumerator ShowLCOption(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        //Debug.LogError("showrewardedadPopup::" + showRewardedAdPopup);
        LoadGameover();

        //if (!showRewardedAdPopup)
        //{
        //    timeUpObj.SetActive(true);
        //    yield return new WaitForSeconds(2f);
        //    timeUpObj.SetActive(false);

        //    if (isTimeUp)
        //        ShowRewardedAdPopup(true);
        //    else
        //        ShowRewardedAdPopup(false);
        //}
        //else
        //{
        //    //load lc
        //    LoadGameover();
        //}
    }
    public void ShowLCOptionNow()
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
        }
        else
        {
            isTimeUp = false;
            timeUpObj.SetActive(false);
            
            FlyTimerAnim();
        }

    }
    void FlyTimerAnim()
    {
        addtimerAnimObj.SetActive(true);

        iTween.ScaleFrom(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
        iTween.MoveTo(addtimerAnimObj, iTween.Hash("x", timerIcon.transform.position.x, "y", timerIcon.transform.position.y, "time", 1,
            "easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
        iTween.ScaleTo(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 1.5, "easetype", iTween.EaseType.spring, "delay", 1.2f));
        Invoke(nameof(TimerSuccessWithDelay), 2.2f);
    }
    void TimerSuccessWithDelay()
    {
        statusBar.timeVal = 30;
        managerLogic.timer = 0;
        
        
        hud.SetTime((int)managerLogic.timer);
        GameSettings.Instance.isGameStarted = true;
    }
   
    public void SubmitBtnClicked()
    {
        WatchVideoPopup.SetActive(false);
        showRewardedAdPopup = true;
        //load lc
    }

#endregion
}