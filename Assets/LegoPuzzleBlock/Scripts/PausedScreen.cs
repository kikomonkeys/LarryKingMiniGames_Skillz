using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PausedScreen : MonoBehaviour
{
	bool hasGameExit = false;
	public Text timerText;
	public Text scoreText;
	int score;
	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		#region time mode
		if (GamePlay.Instance != null && (GameController.gameMode == GameMode.TIMED || GameController.gameMode == GameMode.CHALLENGE)) {
			GamePlay.Instance.timeSlider.PauseTimer ();
		}
		#endregion
		if (BGMusicNew.Music == 0)
		{
			musicImg.sprite = musicOffSpr;
			musicImgParent.GetComponent<Image>().sprite =musicImgParentOffSpr;
			musicImg.SetNativeSize();
		}
		else
		{
			musicImg.sprite = musicOnSpr;
			musicImgParent.GetComponent<Image>().sprite =musicImgParentOnSpr;
			musicImg.SetNativeSize();

		}
		scoreText.text = "" + ScoreManager.Instance.Score;
	}
	float timer;
	float min, sec;
    private void Update()
    {
		timer = GamePlay.Instance.timer;
		min = Mathf.CeilToInt(timer) / 60;
		sec = Mathf.CeilToInt(timer) % 60;
		timerText.text = "" + min.ToString("0") + ":" + sec.ToString("00");
    }
    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
	{
		#region time mode
		if(!hasGameExit)
		{
			if (GamePlay.Instance != null && (GameController.gameMode == GameMode.TIMED || GameController.gameMode == GameMode.CHALLENGE)) {
				GamePlay.Instance.timeSlider.ResumeTimer ();
			}
		}
		#endregion
	}

	/// <summary>
	/// Raises the home button pressed event.
	/// </summary>
	public void OnHomeButtonPressed ()
	{
		if (InputManager.Instance.canInput ()) 
		{
			hasGameExit = true;
			AudioManager.Instance.PlayButtonClickSound ();
			StackManager.Instance.OnCloseButtonPressed ();
			StackManager.Instance.CloseGameplay ();
		}
	}

	/// <summary>
	/// Raises the reset button pressed event.
	/// </summary>
	public void OnResetButtonPressed ()
	{
		AudioManager.Instance.PlayButtonClickSound();
		//StackManager.Instance.RestartGamePlay();
		gameObject.SetActive(false);
		GamePlay.Instance.startTimer = true;
		//GamePlay.Instance.OnGameOver();
		GameObject gameOverScreen = StackManager.Instance.SpawnUIScreen ("GameOver");
		
		if (InputManager.Instance.canInput())
        {
			//AudioManager.Instance.PlayButtonClickSound();
			////StackManager.Instance.RestartGamePlay();
			//GamePlay.Instance.startTimer = true;
			//GamePlay.Instance.OnGameOver();
			//gameObject.SetActive(false);
		}

        
	}
	public void QuitBtnClicked()
    {
		gameObject.SetActive(false);
		GamePlay.Instance.ShowLC();
	}
	/// <summary>
	/// Raises the close button pressed event.
	/// </summary>
	public void OnCloseButtonPressed ()
	{
		GamePlay.Instance.startTimer = true;
		AudioManager.Instance.PlayButtonClickSound();
		gameObject.SetActive(false);
		//StackManager.Instance.OnCloseButtonPressed();
		if (InputManager.Instance.canInput ()) {
			//GamePlay.Instance.startTimer = true;
			//AudioManager.Instance.PlayButtonClickSound ();
			//StackManager.Instance.OnCloseButtonPressed ();
		}
	}

	int musicCount;
	public Image musicImg;
	public GameObject musicImgParent;
	public Sprite musicOnSpr, musicOffSpr, musicImgParentOnSpr, musicImgParentOffSpr;

	public void MusicBtnClicked()
	{
		musicCount++;
		if (musicCount % 2 == 0)
		{
			musicImg.sprite = musicOnSpr;
			musicImgParent.GetComponent<Image>().sprite = musicImgParentOnSpr;
			musicImg.SetNativeSize();

			BGMusicNew.Music = 1;
			BGMusicNew.instance.SetMusicInfo();
		}
		else
		{
			musicImg.sprite = musicOffSpr;
			musicImg.SetNativeSize();
			musicImgParent.GetComponent<Image>().sprite =musicImgParentOffSpr;

			BGMusicNew.Music = 0;
			BGMusicNew.instance.SetMusicInfo();
		}
	}
}
