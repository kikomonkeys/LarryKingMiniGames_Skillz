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

		int totalscore;
		totalscore = score + bonusScore;
		finalscr = totalscore;

		//Invoke(nameof(ShowScoreSubmissionObj), 0f);
		Invoke(nameof(EnableNextBtn), 2f);
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
}
