using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : Singleton<GameController> 
{
	public static GameMode gameMode = GameMode.CLASSIC;
	public Canvas UICanvas;
	public GameObject pausePage, gameOverPage, howToPlayPage;

	public static GameController instance;

    private void Awake()
    {
		instance = this;
    }

    private void Start()
    {
		Invoke(nameof(FindObj), 3);
#if UNITY_IOS
         NativeAPI.createBannerAd("BOTTOM","c6229ae5dff5ca8a");
#endif
	}

	void FindObj()
    {
		addTimerObj = GamePlay.Instance.addTimerObj;
		timerPivotObj = GameObject.Find("TimerImg");
	}
    // Checks if interner is available or not.
    public bool isInternetAvailable()
	{
		if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Quits the game.
	/// </summary>
	public void QuitGame()
	{
		Invoke ("QuitGameWithDelay", 0.5F);
	}

	/// <summary>
	/// Quits the game with delay.
	/// </summary>
	void QuitGameWithDelay ()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		//Application.Quit ();
		#endif
	}


	#region Watch Video Popup

	public GameObject WatchVideoPopup, watchVideoBtn;
	public GameObject livesImg, timerImg;
	public Text descriptionText;
	public Text currentScoreText;
	public GameObject timeAddedObj;
	public void ShowRewardedAdPopup(bool isTimeUp)
	{

		WatchVideoPopup.SetActive(true);

		currentScoreText.text = "" + FindObjectOfType<ScoreManager>().GetScore();
		if (isTimeUp)
		{
			descriptionText.text = "GET 2 MINUTES EXTRA TIME";
			timerImg.SetActive(true);
			livesImg.SetActive(false);
			watchVideoBtn.GetComponent<BlockPuzzle_GameStake.RewardedAds>().rewardType = BlockPuzzle_GameStake.RewardedAds.RewardType.Timer;
		}
		else//life description
		{
			descriptionText.text = "GET 50% of Board Wiped";
			timerImg.SetActive(false);
			livesImg.SetActive(true);
			watchVideoBtn.GetComponent<BlockPuzzle_GameStake.RewardedAds>().rewardType = BlockPuzzle_GameStake.RewardedAds.RewardType.Lifes;
		}
	}
	bool showRewardedAdPopup;
	public bool isTimeUp;
	public bool isLostLives;
	public GameObject addTimerObj;
	GameObject timerPivotObj;
	public IEnumerator ShowLCOption(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		Debug.LogError("showrewardedadPopup::" + showRewardedAdPopup);
		if (!showRewardedAdPopup)
		{
			if (isTimeUp)
				ShowRewardedAdPopup(true);
			else
				ShowRewardedAdPopup(false);


		}
		else
		{
			//load lc
			GamePlay.Instance.ShowLC();
		}
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
			GamePlay.Instance.StartTimer();
			GamePlay.Instance.ClearSomePercentOfTilesNow();
		}
		else
		{
			isTimeUp = false;
			FlyTimerAnim();
		}

	}
	void FlyTimerAnim()
	{
		addTimerObj.SetActive(true);
		timerPivotObj = GameObject.Find("TimerImg");
		iTween.ScaleFrom(addTimerObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
		iTween.MoveTo(addTimerObj, iTween.Hash("x", timerPivotObj.transform.position.x, "y", timerPivotObj.transform.position.y, "time", 1,
			"easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
		iTween.ScaleTo(addTimerObj, iTween.Hash("x", 0, "y", 0, "time", 1.5, "easetype", iTween.EaseType.spring, "delay", 1.2f));
		Invoke(nameof(TimerSuccesDelay), 2.2f);
	}
	void TimerSuccesDelay()
    {
		GamePlay.Instance.timer = 120;
		GamePlay.Instance.StartTimer();
	}
	public void SubmitBtnClicked()
	{
		WatchVideoPopup.SetActive(false);
		showRewardedAdPopup = true;
		//load lc
		GamePlay.Instance.ShowLC();
	}

	#endregion
}
