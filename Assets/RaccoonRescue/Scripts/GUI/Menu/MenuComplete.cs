using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using VLxSecurity;

public class MenuComplete : MonoBehaviour {
	public GameObject[] stars;
	public Text score;
	public Text bubblePointsText, starBonusText, multiplierBonusText, heartsLeftText, boardClearText;
	public Text bubblePointsText_2, starBonusText_2, multiplierBonusText_2,scoreText_2;
	public int bubblePoints, starBonus, multiplierBonus, heartsLeftBonus, boardClearedBonus;
	public GameObject heartsLeftObj, boardClearedObj, bubblePointsObj, starBonusObj, multiplerBonusObj, totalScoreObj;
	public int totalScore;
	public GameObject nextBtn, nextBtn_2;
	bool showFullScorePopup;
	public GameObject fullScorePopup, scorePopup;
	public void OnEnable () {

		mainscript.Instance.startTimer = false;
		for (int i = 0; i < 3; i++) {
			stars [i].SetActive (false);
		}

		fullScorePopup.SetActive(false);
		scorePopup.SetActive(false);


		if (mainscript.isBoardCleared)
		{
			showFullScorePopup = true;
			boardClearedObj.SetActive(true);
			boardClearedBonus = 2000;
			mainscript.isBoardCleared = false;
		}

		if (mainscript.Instance.Hearts > 0)
		{
            if (AnimationManager.isPauseQuitBtnClicked)
            {
				AnimationManager.isPauseQuitBtnClicked = false;
            }
            else
            {
				showFullScorePopup = true;
				heartsLeftObj.SetActive(true);
				heartsLeftBonus = mainscript.Instance.Hearts * 500;
			}
		}
        if (showFullScorePopup)
        {
			fullScorePopup.SetActive(true);
			scorePopup.SetActive(false);
        }
        else
        {
			fullScorePopup.SetActive(false);
			scorePopup.SetActive(true);
		}
		multiplierBonus = mainscript.Instance.multiplierBonus;
		starBonus = mainscript.Instance.starBonus;
		bubblePoints = mainscript.Score - multiplierBonus - starBonus;
		totalScore = bubblePoints + starBonus + multiplierBonus + heartsLeftBonus + boardClearedBonus;

		Debug.LogError("LEVEL COMPLETE:::" + "Bubble Points::" + bubblePoints + " StarBonus::" + starBonus + " MultiplierBonus::" +
			multiplierBonus + " HeartsLeft::" + heartsLeftBonus + "TOTAL SCORE::" + totalScore);


		//Invoke(nameof(EnableNextBtn), 3f);

		//bubblePointsText.text = "" + bubblePoints;
		//starBonusText.text = "" + starBonus;
		//multiplierBonusText.text = "" + multiplierBonus;
		//heartsLeftText.text = "" + heartsLeftBonus;
		//boardClearText.text = "" + boardClearedBonus;
	}

	public void OnAnimationFinished () {
		StartCoroutine (MenuCompleteCor ());

		StartCoroutine(AllScoringAnimations());//piper

	}

	IEnumerator MenuCompleteCor () {
		for (int i = 0; i < mainscript.Instance.stars; i++) {
			//  SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoringStar );
			stars [i].SetActive (true);
			yield return new WaitForSeconds (0.5f);
			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.hit);
		}
	}

	IEnumerator MenuCompleteScoring () {
		totalScoreObj.SetActive(true);
		for (int i = 0; i <= mainscript.Score; i += 500) {
			if (showFullScorePopup)
				score.text = "" + i;
			else
				scoreText_2.text = "" + i;
			// SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoring );
			yield return new WaitForSeconds (0.00001f);
		}
		//score.text = "" + mainscript.Score;

		if (totalScore.ToString().Length >= 5)
        {
			if (showFullScorePopup)
				score.text = totalScore.ToString("n0");
			else
				scoreText_2.text = totalScore.ToString("n0");
		}
		else
        {
			if (showFullScorePopup)
				score.text = totalScore.ToString();
			else
				scoreText_2.text = totalScore.ToString();
		}

		yield return new WaitForSeconds(0.2f);

		if (showFullScorePopup)
			nextBtn.SetActive(false);
		else
			nextBtn_2.SetActive(false);

		yield return new WaitForSeconds(0.2f);
		showFullScorePopup = false;
		EnableNextBtn();
		//PlayerPrefs.SetString("&^&#@_2021", StringCipher.Encrypt(totalScore.ToString(), "Piper#2021202"));
		//GameStakeSDK.instance.OnGameComplete(ShowScoreSubmissionObj);
		UnityiOSHandler.instance.SendScore(totalScore, true);
	}

	float waittime = 0.25f;
	IEnumerator AllScoringAnimations()
	{
		//bubble scoring 
		bubblePointsObj.SetActive(true);
		for (int i = 0; i <= bubblePoints; i+=1000)
		{
			if(showFullScorePopup)
				bubblePointsText.text = "" + i;
			else
				bubblePointsText_2.text = "" + i;

			yield return new WaitForSeconds(0.00001f);

		}
		if(showFullScorePopup)
			bubblePointsText.text = "" + bubblePoints;
		else
			bubblePointsText_2.text = "" + bubblePoints;

		if (starBonus == 0)
			yield return new WaitForSeconds(0);
		else
			yield return new WaitForSeconds(waittime);

		//StarBonus Scoring
		starBonusObj.SetActive(true);
		for (int i = 0; i <= starBonus; i += 500)
		{
			if(showFullScorePopup)
				starBonusText.text = "" + i;
			else
				starBonusText_2.text = "" + i;

			yield return new WaitForSeconds(0.00001f);

		}
		if(showFullScorePopup)
			starBonusText.text = "" + starBonus;
		else
			starBonusText_2.text = "" + starBonus;

		if (multiplierBonus == 0)
			yield return new WaitForSeconds(0);
		else
			yield return new WaitForSeconds(waittime);

		//Multiplier Bonus Scoring
		multiplerBonusObj.SetActive(true);
		for (int i = 0; i <= multiplierBonus; i += 10)
		{
			if(showFullScorePopup)
				multiplierBonusText.text = "" + i;
			else
				multiplierBonusText_2.text = "" + i;

			yield return new WaitForSeconds(0.00001f);

		}
		if(showFullScorePopup)
			multiplierBonusText.text = "" + multiplierBonus;
		else
			multiplierBonusText_2.text = "" + multiplierBonus;

		yield return new WaitForSeconds(waittime);

		//Hearst Bonus Scoring
		for (int i = 0; i <= heartsLeftBonus; i += 100)
		{
			heartsLeftText.text = "" + i;
			yield return new WaitForSeconds(0.00001f);

		}
		heartsLeftText.text = "" + heartsLeftBonus;
		yield return new WaitForSeconds(waittime);

		
		//Board Clwared Bonus Scoring
		for (int i = 0; i <= boardClearedBonus; i += 100)
		{
			boardClearText.text = "" + i;
			yield return new WaitForSeconds(0.00001f);

		}
		boardClearText.text = "" + boardClearedBonus;
		yield return new WaitForSeconds(waittime);
		
		StartCoroutine(MenuCompleteScoring());

	}

	public GameObject scoreSubmmissionObj;
	void EnableNextBtn()
	{
		//nextBtn.GetComponent<Button>().interactable = true;
		//nextBtn.GetComponentInChildren<Text>().color = Color.white;

		//nextBtn_2.GetComponent<Button>().interactable = true;
		//nextBtn_2.GetComponentInChildren<Text>().color = Color.white;
		nextBtn.SetActive(true);
		nextBtn_2.SetActive(true);
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
}
