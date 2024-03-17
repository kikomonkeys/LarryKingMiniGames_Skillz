using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour {

	public static bool isGameOver=false;
	public static Knife selectedKnifePrefab=null;


	public static Dictionary<string, string> _userResData = new Dictionary<string, string>();


	public static Dictionary<string, string> UserResData
	{
		get
		{
			return _userResData;
		}
		set
		{
			_userResData = value;
			
		}
	}
	public static float ScreenHeight{
		get
		{ 
			if(Camera.main!=null)
				return Camera.main.orthographicSize * 2f;
			return 0f;
		}
	}
	public static float ScreenWidth{
		get
		{ 
			if(Camera.main!=null)
				return  ScreenHeight / Screen.height * Screen.width;
			return 0f;
		}
	}
	public static int score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			if(GamePlayManager.instance != null) {
				GamePlayManager.instance.UpdateLable();

			}

		}
	}
	public static int oppoScore
	{
		get
		{
			return _opposcore;
		}
		set
		{
			_opposcore = value;
			
		}
	}
	static int _score;
	static int _opposcore;

	public static int Stage
	{
		get
		{
			return _stage;
		}
		set
		{
			_stage = value;
			if(GamePlayManager.instance != null)
				GamePlayManager.instance.UpdateLable ();
		}
	}
	static int _stage;
	public static int HighScore
	{
		get
		{
			return PlayerPrefs.GetInt ("Player's HighScore", 0);
		}
		set
		{
			PlayerPrefs.SetInt ("Player's HighScore", value);
		}
	}
	public static int Apple
	{
		get
		{
			return PlayerPrefs.GetInt ("Player's Apple", 0);
		}
		set
		{
			PlayerPrefs.SetInt ("Player's Apple", value);
			if (GeneralFunction.intance != null)
				GeneralFunction.intance.appleLbl.text = GameManager.Apple + "";
			
		}
	}
	public static int SelectedKnifeIndex
	{
		get
		{
			return PlayerPrefs.GetInt ("SelectedKnifeIndex", 0);
		}
		set
		{
			PlayerPrefs.SetInt ("SelectedKnifeIndex", value);
		}
	}
	public static bool Sound
	{
		get
		{
			return PlayerPrefs.GetInt ("GameSound", 1)==1;
		}
		set
		{
			PlayerPrefs.SetInt ("GameSound", value?1:0);
		}
	}
	public static bool Music
	{
		get
		{
			return PlayerPrefs.GetInt("GameMusic", 1) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("GameMusic", value ? 1 : 0);
		}
	}
	public static bool Vibration
	{
		get
		{
			return PlayerPrefs.GetInt ("GameVibration", 1)==1;
		}
		set
		{
			PlayerPrefs.SetInt ("GameVibration", value?1:0);
		}
	}
	public static bool GiftAvalible
	{
		get
		{
			return	RemendingTimeSpanForGift.TotalSeconds<= 0;
		}
	}
	public static TimeSpan RemendingTimeSpanForGift
	{
		get {
			return (NextGiftTime - DateTime.Now);
		}
	}
	public static DateTime NextGiftTime
	{
		get
		{
			return DateTime.FromFileTime(long.Parse(PlayerPrefs.GetString("LastBonusTime",DateTime.Now.ToFileTime()+"")));
		}
		set
		{
			PlayerPrefs.SetString ("LastBonusTime",value.ToFileTime()+"");
		}
	}
	public static int Knife_HowToPlay
	{
		get
		{
			return PlayerPrefs.GetInt("knifeHowtoPlay", 0);
		}
		set
		{
			PlayerPrefs.SetInt("knifeHowtoPlay", value);
		}
	}
}

[Serializable]
public class RoundData
{
	public List<Round> rounds;

}

[Serializable]
public class Round
{
	public List<Circle> circles;
}