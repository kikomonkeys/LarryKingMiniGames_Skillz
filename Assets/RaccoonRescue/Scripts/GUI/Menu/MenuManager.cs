using UnityEngine;
using System.Collections;
using InitScriptName;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance;
	public GraphicRaycaster raycaster;
	public GameObject MenuPlay;
	public GameObject MenuPause;
	public GameObject MenuWin;
	public GameObject MenuFailed;
	public GameObject MenuCurrencyShop;
	public GameObject PrePlayBanner;
	public GameObject PreFailedBanner;
	public GameObject PreWinBanner;
	public GameObject RewardPopup;
	public GameObject MenuLifeShop;
	public GameObject MenuBoostShop;
	public GameObject MenuSettings;
	public GameObject MenuPurchased;
	public GameObject MenuTutorial;
	public GameObject Loading;

	public GameObject CongratulationsMenu;

	public delegate void MenuDelegate();

	public static MenuDelegate OnMenuLeadboard;


	

	void Awake()
	{
		Instance = this;
		Loading = GameObject.Find("CanvasLoading").transform.GetChild(0).gameObject;
		//raycaster.enabled = false;
	}

	void OnEnable()
	{
		GameEvent.OnStatus += OnStatusChanged;
	}

	void OnDisable()
	{
		GameEvent.OnStatus -= OnStatusChanged;
	}

	void OnStatusChanged(GameState status)
	{
		//raycaster.enabled = true;
		if (status == GameState.PlayMenu) {
			MenuPlay.SetActive(true);
			if (OnMenuLeadboard != null)
				OnMenuLeadboard();
		}

		if (status == GameState.WinMenu) {
			MenuWin.SetActive(true);
			if (OnMenuLeadboard != null)
				OnMenuLeadboard();
		}
		if (status == GameState.GameOver) {
			MenuFailed.SetActive(true);
			InitScript.Instance.SpendLife(1);
			if (OnMenuLeadboard != null)
				OnMenuLeadboard();
		}

		if (status == GameState.Pause) {
			MenuPause.SetActive(true);
		}

		if (status == GameState.PreFailed) {
			PreFailedBanner.SetActive(true);
		}

		if (status == GameState.PrePlayBanner) {
			Invoke(nameof(ShowPrePlayBanner), 0.5f);
		}

		if (status == GameState.WinBanner) {
			// PreWinBanner.SetActive (true);
		}

	}
	void ShowPrePlayBanner()
    {
		PrePlayBanner.SetActive(true);
    }
	public void OnCloseMenuEvent()
	{
		//raycaster.enabled = false;

	}

	public void ShowCurrencyShop()
	{
		MenuCurrencyShop.SetActive(true);
	}

	public void ShowLifeShop()
	{
		MenuLifeShop.SetActive(true);
	}

	public void ShowPurchased(BoostType bType)
	{
		MenuPurchased.GetComponent<PurchasedMenu>().SetIconSprite(bType);
		MenuPurchased.SetActive(true);
	}

	public void ShowTutorial()
	{
		MenuTutorial.SetActive(true);
	}

	public GameObject WatchVideoPopup, watchVideoBtn;
	public GameObject livesImg, timerImg;
	public Text descriptionText;
	public GameObject watchVideoSuccessBanner;
	public Text bannerText;
	public void ShowRewardedAdPopup(bool isTimeUp)
	{
		WatchVideoPopup.SetActive(true);
		if (isTimeUp)
		{
			descriptionText.text = "GET 2 MINUTES EXTRA TIME";
			timerImg.SetActive(true);
			livesImg.SetActive(false);
			watchVideoBtn.GetComponent<BubbleBlitz_GameStake.RewardedAds>().rewardType = BubbleBlitz_GameStake.RewardedAds.RewardType.Timer;

		}
		else//life description
		{
			descriptionText.text = "GET 1 EXTRA LIFE";
			timerImg.SetActive(false);
			livesImg.SetActive(true);
			watchVideoBtn.GetComponent<BubbleBlitz_GameStake.RewardedAds>().rewardType = BubbleBlitz_GameStake.RewardedAds.RewardType.Lifes;
		}
	}
	bool showRewardedAdPopup;
	public bool isTimeUp;
	public bool isLostLives;
	public Sprite redHeart;
	public GameObject addlivesAnimObj, addtimerAnimObj;
	public GameObject heartlivesPivot, timerObj;
	public void ShowLCOption()
	{
		GameEvent.Instance.GameStatus = GameState.WinMenu;

		//Debug.LogError("showrewardedadPopup::" + showRewardedAdPopup);
		//if (!showRewardedAdPopup)
		//{
		//	if (isTimeUp)
		//		ShowRewardedAdPopup(true);
		//	else
		//		ShowRewardedAdPopup(false);

			
		//}
		//else
		//{
		//	GameEvent.Instance.GameStatus = GameState.WinMenu;
		//}
	}
	public void VideoSuccessEvent(bool isLives)
	{
		WatchVideoPopup.SetActive(false);
		showRewardedAdPopup = true;
		if (isLives)
		{
			isLostLives = false;
			mainscript.Instance.Hearts = 1;

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
		iTween.MoveTo(addlivesAnimObj, iTween.Hash("x", heartlivesPivot.transform.position.x, "y", heartlivesPivot.transform.position.y, "time", 1,
			"easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
		iTween.ScaleTo(addlivesAnimObj, iTween.Hash("x", 0, "y", 0, "time", 1, "easetype", iTween.EaseType.spring, "delay", 1.2f));

		Invoke(nameof(AddLifeImg), 2.0f);

	}
	void AddLifeImg()
	{
		for (int i = 0; i < mainscript.Instance.Hearts; i++)
		{
			mainscript.Instance.G_heart_visual[i].GetComponent<Image>().sprite = redHeart;
		}
	}
	void FlyTimerAnim()
	{
		addtimerAnimObj.SetActive(true);

		iTween.ScaleFrom(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 0.5, "easetype", iTween.EaseType.spring));
		iTween.MoveTo(addtimerAnimObj, iTween.Hash("x", timerObj.transform.GetChild(0).transform.position.x, "y", timerObj.transform.GetChild(0).transform.position.y, "time", 1,
			"easetype", iTween.EaseType.easeInOutBack, "delay", 1f));
		iTween.ScaleTo(addtimerAnimObj, iTween.Hash("x", 0, "y", 0, "time", 1.5, "easetype", iTween.EaseType.spring, "delay", 1.2f));
		Invoke(nameof(TimerUpdate), 2.2f);
	}
	void TimerUpdate()
    {
		mainscript.Instance.maxTime = 120;
		mainscript.Instance.startTimer = true;
		mainscript.Instance.timerText.color = Color.white;
		mainscript.Instance.timerText.GetComponent<iTween>().enabled = false;
		mainscript.Instance.timerText.transform.localScale = Vector3.one;
	}
	public void SubmitBtnClicked()
    {
		WatchVideoPopup.SetActive(false);
		showRewardedAdPopup = true;
		GameEvent.Instance.GameStatus = GameState.WinMenu;
	}
}
