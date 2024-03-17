using System.Collections.Generic;

using HUD;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UserWindow;
using SmartOrientation;
using TMPro;

public class HUDController : MonoHandler {


	IHUDActions managerHUD;

	ICardActions managerCardActions;


	[SerializeField]
	private GameObject topPanelPortrait;

 
    [SerializeField]
    private GameObject[] noHintsDetected;
    [SerializeField]
    private Text []  textsHint;
 
	[SerializeField]
	private List<StatusBar> statusBars;
	[SerializeField]
	private PopUpWindow popUpWindow;
	[SerializeField]
	private HideBottomPanelAnim bottomPanel;
	[SerializeField]
	private HideBottomPanelAnim bottomPanelLandscape;
//	[SerializeField]
	public SmartOrientationHUD smartOrientationHUD;


    private bool playClicked = false;

	[SerializeField]
	private RectTransform adsOffsetPort;
	[SerializeField]
	private RectTransform adsOffsetLand;
    [SerializeField]
    private TextMeshProUGUI [] playModes;
    [SerializeField]
    private GameObject [] buttonAutoComplete;

    [SerializeField]
    private GameObject portraitLayout;
    [SerializeField]
    private GameObject landSpaceLayout;
    [SerializeField]
    private LayoutElement spaceStatus1;
    [SerializeField]
    private LayoutElement spaceStatus2;

    [SerializeField]
    private List<Button> buttonsAction = new List<Button>();

    private int countAdsNewGame;

	public GameObject triggerLess;
	public GameObject triggerFull;

    private int countAdsHitHint = 0;
    private int countAdsHitUndo = 0;
    private int countAdsSolution = 0;


    public bool WinGame { get; private set; }
    private static HUDController _instance = null;
	public static HUDController instance{
		get{ 
			if (_instance == null) {
                GameObject go = GameObject.Find("HUDData");

                //GameObject go = (GameObject)Resources.Load("HUD");
				//GameObject go2 = Instantiate (go);
				_instance = go.GetComponentInChildren<HUDController> ();
			}
			return _instance;
		}
	}
//	private void Awake() {instance = this;}

	// Use this for initialization
	void Start () {
		managerHUD = (IHUDActions)StageManager.instance;
		managerCardActions = (ICardActions)StageManager.instance;
        transform.SetAsFirstSibling();
		 

	}


    public void UpdateGameMode()
    {

        return;

        if (GameSettings.Instance.isCalendarGame)
        {
            for (int i = 0; i < playModes.Length; i++)
            {

                playModes[i].text = "DAILY";
            }

        }
        else
        {
            for (int i = 0; i < playModes.Length; i++)
            {

                playModes[i].text = (GameSettings.Instance.randomDeal) ? "RANDOM" : "WINNING";


            }

        }
    }
    
	public void SetBarsVisibility (bool isMoveTime, bool isMoveTime2, bool isScore)
	{
		foreach (var b in statusBars) {
			b.SetVisibility (isMoveTime, isMoveTime2, isScore);
		}
	}

 
 

	public void ResetStatusBars ()
	{
		foreach (var b in statusBars) {
			b.reset ();
		}
	}

	public void SetMove(int move){
		// syncronize landscape status bar
		foreach (var b in statusBars) {
			b.move = move;
		}
	}

	public void SetTime(int time){
		// syncronize landscape status bar
		foreach (var b in statusBars) {
			b.time = time;
		}
	}

    public void SetOppsScore(int score)
    {
        // syncronize landscape status bar
        foreach (var b in statusBars)
        {
            b.opposcore = score;
        }
    }
    public void SetScore(int score){
		// syncronize landscape status bar
		foreach (var b in statusBars) {
			b.score = score;
		}
	}
    public void VisibleButtonsAction(bool visible)
    {
        //for (int i = 0; i < buttonsAction.Count; i++)
        //{
        //    buttonsAction[i].interactable = visible;
        //}
    }
    private UnityAction actionForTrigger;
	public void triggerClick(){
		if (actionForTrigger != null) {
			actionForTrigger();
		}
 
	}
	public void setTrigger(bool isFull, UnityAction action){
		actionForTrigger = action;
		if (isFull) {
			triggerFull.SetActive (true);
		} else {
			triggerLess.SetActive (true);
		}
	}
 
    public void VisibleLayout(bool visible)
    {
        WinGame = !visible;
        if(DeviceOrientationHandler.instance.isVertical)
        portraitLayout.SetActive(visible);
        else
        landSpaceLayout.SetActive(visible);
    }

    public bool isBottomPanelShown = true;
	public void ToggleBottomPanel(){

        popUpWindow.hide();
        playClicked = false;
        isBottomPanelShown = !isBottomPanelShown;
		//bottomPanel.ShowPanel (isBottomPanelShown);
		bottomPanelLandscape.ShowPanel (isBottomPanelShown);
	}


    public void SetSolutionLayout(bool isSet)
    {
        // top panel
        topPanelPortrait.SetActive(!isSet);


        // bottom panels
        if(bottomPanel)
        bottomPanel.ShowSolutionControlls(isSet);
        bottomPanelLandscape.ShowSolutionControlls(isSet);
    }

    public void VisibleButtonComplete(bool visible)
    {

        if (!GameSettings.Instance.isAutoCompleteSet) visible = false;
       // Debug.Log("buttonAutoComplete  " + visible);
        for (int i = 0; i < buttonAutoComplete.Length; i++)
        {
            buttonAutoComplete[i].SetActive(visible);
        }
    }

    public void OnPressAutoComplete()
    {
      
        VisibleButtonComplete(false);
        StageManager.instance.OnAuto();
      
    }

    #region HUD buttons pressed 

    /// <summary>
    /// Raises the back to menu button pressed event.
    /// </summary>
    public void OnBackToMenuPressed()
	{
		if(managerCardActions != null)	
			managerCardActions.OnClickAnywhere ();

		managerHUD.OnBackToMenuPressed ();
	}

	/// <summary>
	/// Raises the shop click event.
	/// </summary>
	public void OnStorePressed()
	{
		if(managerCardActions != null)	
			managerCardActions.OnClickAnywhere ();
		
		managerHUD.OnStorePressed ();
	}

	/// <summary>
	/// Show the manual(tutorial) screen.
	/// </summary>
	public void OnManualPressed()
	{
		if(managerCardActions != null)	
			managerCardActions.OnClickAnywhere ();



		managerHUD.OnShowManualPressed ();
	}




	/// <summary>
	/// Shows the option menu.
	/// </summary>
	public void OnOptionsPressed()
	{
        playClicked = false;
        popUpWindow.hide();
        if (managerCardActions != null)	
			managerCardActions.OnClickAnywhere ();
		
		managerHUD.OnOptionsPressed ();
	}

    /// <summary>
	/// Shows the solution menu.
	/// </summary>
	public void OnSolutionPressed()
    {
        playClicked = false;
        popUpWindow.hide();
        countAdsSolution++;

        if (countAdsSolution >= 3)
        {
            countAdsSolution = 0;
            //GoogleMobileAdsScript.instance.ShowRewardBasedVideo();
        }

        if (managerCardActions != null)
            managerCardActions.OnClickAnywhere();

        playClicked = false;
        GameSettings.Instance.isSolutionMode = true;
        popUpWindow.hide();
        managerHUD.OnSolutionPressed();
    }
    public void OnStatsPressed()
    {

        playClicked = false;
        PopUpManager.Instance.ShowStats();
        popUpWindow.hide();

    }
    

	/// <summary>
	/// Shows the new menu.
	/// </summary>
	public void OnNewPressed(){

        if (playClicked)
        {
            popUpWindow.hide();
        }
        else {

            if (managerCardActions != null)
                managerCardActions.OnClickAnywhere();

            List<ButtonModel> buts = new List<ButtonModel>();



            buts.Add(button
                .setTitle("Daily Challenge")
                .setAction(() => {
                    playClicked = false;
                    managerHUD.OnDailyChallengePressed();
                    popUpWindow.hide();
                })
            );
            buts.Add(button
            .setTitle("Random Shuffle")
            .setAction(() => {
                ShowAds();
                playClicked = false;
                managerHUD.OnNewGamePressed();
                ContinueModeGame.instance.ClearAllDataCard();
                GameSettings.Instance.randomDeal = true;
                GameSettings.Instance.isCalendarGame = false;
                popUpWindow.hide();
            })
        );
            buts.Add(button
            .setTitle("Winning Deal")
            .setAction(() => {
                ShowAds();
                playClicked = false;
                ContinueModeGame.instance.ClearAllDataCard();
                GameSettings.Instance.randomDeal = false;
                GameSettings.Instance.isCalendarGame = false;
                managerHUD.OnNewGamePressed();
                popUpWindow.hide();
            })
        );
            buts.Add(button
                .setTitle("Restart This Game")
                .setAction(() => {
                    ShowAds();
                    playClicked = false;
                  
                    managerHUD.OnRestartPressed();
                    popUpWindow.hide();
                })
            );
            buts.Add(button
              .setTitle("Stats")
              .setAction(() => {
                  playClicked = false;
                  PopUpManager.Instance.ShowStats();
                  popUpWindow.hide();
              })
          );


         
            popUpWindow.show(buts);
        }
        playClicked = !playClicked;
	}

	/// <summary>
	/// Shows the hint menu.
	/// </summary>
	public void OnHintsPressed(){
        playClicked = false;
        popUpWindow.hide();


        countAdsHitHint++;

        if (countAdsHitHint >= 6)
        {
            countAdsHitHint = 0;


            //GoogleMobileAdsScript.instance.ShowRewardBasedVideo();


        }


       
        if (managerCardActions != null)	
			managerCardActions.OnClickAnywhere ();

		if (StageManager.instance.HasHint) {
			managerHUD.OnHintsPressed ();
			return;
		}
      

        for (int i = 0; i < noHintsDetected.Length; i++)
        {
            noHintsDetected[i].SetActive(true);
        }

	}


	/// <summary>
	/// Shows the undo menu.
	/// </summary>
	public void OnUndoPressed()
    {
        playClicked = false;
        popUpWindow.hide();
        countAdsHitUndo++;
        if (countAdsHitUndo >= 6)
        {
            countAdsHitUndo = 0;
            //GoogleMobileAdsScript.instance.ShowRewardBasedVideo();
        }
        if (managerCardActions != null)	
			managerCardActions.OnClickAnywhere ();
		managerHUD.OnUndoPressed ();
        StartCoroutine(StageManager.instance.VisibleButtonComplete());
	}
	#endregion


	#region Solution buttons
	public void OnExitSolutionPressed(){
       
		managerHUD.OnExitSolution ();
	}
	public void OnPauseSolutionPressed(){
		StageManager.instance.commandUpdater.Pause ();
	}
	public void OnPlaySolutionPressed(){
		StageManager.instance.commandUpdater.Continue ();
		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed (SmoothMovementManager.Speed.Solution1x);//(.5f);
	}
	public void OnX2SolutionPressed(){
		StageManager.instance.commandUpdater.Continue ();
		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed (SmoothMovementManager.Speed.Solution2x);//(1f);
	}
	public void OnX3SolutionPressed(){
		StageManager.instance.commandUpdater.Continue ();
		SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed (SmoothMovementManager.Speed.Solution3x);//(1.5f);

	}
	#endregion

	// it makes life a bit easier :)
	ButtonModel button{
		get{ 
			return new ButtonModel ();
		}
	}


    private void ShowAds()
    {
        countAdsNewGame++;
        if (countAdsNewGame > 3)
        {
            //GoogleMobileAdsScript.instance.ShowRewardBasedVideo();
            countAdsNewGame = 0;
        }
        else
        {
            //GoogleMobileAdsScript.instance.ShowInterstitial();
        }
    }
   
}