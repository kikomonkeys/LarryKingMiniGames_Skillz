using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
//using VLxSecurity;

public class GameOver : MonoBehaviour {

	[SerializeField] Text txtScore;
	[SerializeField] private Text txtBestScore;
	[SerializeField] private Text txtCoinReward;
	[SerializeField] private Text txtBonusScore;
	[SerializeField] private Text totalScoreText;
	[SerializeField] private float timer;
	bool isStartTimer;

    public int finalscr;

	public static GameOver instance;
	int totalscore;


	private void Awake()
    {
		instance = this;
    }
    public void SetLevelScore(int score, int coinReward,int bonusScore)
	{
		int bestScore = PlayerPrefs.GetInt ("BestScore_" + GameController.gameMode.ToString (), score);

		if (score >= bestScore) {
			PlayerPrefs.SetInt ("BestScore_" + GameController.gameMode.ToString (), score);
		}

		txtScore.text = string.Format("{0:#,#.}", score.ToString("0"));
		//txtBestScore.text = string.Format("{0:#,#.}", (PlayerPrefs.GetInt("BestScore_" +
		//GameController.gameMode.ToString()).ToString("0")));
		Debug.LogError("bonus score is::" + bonusScore);
		txtBonusScore.text = bonusScore.ToString();

		txtCoinReward.text = string.Format("{0:#,#.}", coinReward.ToString("0"));
		totalScoreText.text = "" + (score + bonusScore);
		CurrencyManager.Instance.AddCoinBalance (coinReward);
		isStartTimer = true;

		totalscore = score + bonusScore;
		finalscr = totalscore;

		//Invoke(nameof(ShowScoreSubmissionObj), 0f);
		Invoke(nameof(EnableNextBtn), 2f);
		TryToSubmitScoreToSkillz();
		//GameStakeSDK.instance.OnGameComplete(finalscr);
		//PlayerPrefs.SetString("&^&#@_2021", StringCipher.Encrypt(finalscr.ToString(), "Piper#2021202"));
		//GameStakeSDK.instance.OnGameComplete(ShowScoreSubmissionObj);
	}
	public GameObject scoreSubmmissionObj;
	public GameObject playAgaianBtn;

	void EnableNextBtn()
    {
		playAgaianBtn.GetComponent<Button>().interactable = true;
		playAgaianBtn.GetComponentInChildren<Text>().color = Color.white;
    }
	void ShowScoreSubmissionObj()
    {
		scoreSubmmissionObj.SetActive(true);
		iTween.MoveFrom(scoreSubmmissionObj.gameObject, iTween.Hash("y", 2000, "time", 0.5, "delay", 0.5f));
		Invoke(nameof(DisableToast), 2);
	}
	void DisableToast()
    {
		scoreSubmmissionObj.SetActive(false);
	}
	private void Update()
    {
        if (isStartTimer)
        {
			timer += Time.deltaTime;
            if (timer >= 15)
            {
				isStartTimer = false;
				//SceneManager.LoadScene("Main Menu");
            }
        }
    }

    public void OnHomeButtonPressed()
	{
		if (InputManager.Instance.canInput ()) {
			AudioManager.Instance.PlayButtonClickSound ();
			StackManager.Instance.OnCloseButtonPressed ();
		}
	}

	public void OnReplayButtonPressed()
	{
		AudioManager.Instance.PlayButtonClickSound();
		//
		//GameStakeSDK.instance.OnGameComplete(finalscr);
	//	Invoke("loadScene",2);
		SceneManager.LoadScene("blockpuzzle_mainmenu");
		UnloadApp();
	}
	void UnloadApp()
	{
		Debug.LogError("unload the app");
		// Application.Quit();
		Application.Unload();
	}

	void loadScene()
    {
		SceneManager.LoadScene("blockpuzzle_mainmenu");
	}

    public void Rate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=YOUR_PACKAGE_NAME_HERE");
    }

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
		string score = totalscore.ToString();
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
		SkillzCrossPlatform.DisplayTournamentResultsWithScore(totalscore.ToString());
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
