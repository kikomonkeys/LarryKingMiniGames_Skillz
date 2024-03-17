/// <summary>
/// Game settings.
/// Singleton game data holder
/// </summary>
using UnityEngine;
public class GameSettings : ScriptableObject
{
	private static GameSettings _instance = null;
	public static GameSettings Instance
	{
		get 
		{
			if (_instance == null)
			{
				_instance = (GameSettings)Resources.Load("GameSettings");
				if (_instance == null)
				{
					throw new UnityException ("Asset can't found");
				}
			}
			return _instance;
		}
	}
	#region Settings
	public enum MedalType {None=0,Bronze,Silver,Gold}
    [System.Serializable]
    public enum OrientationType { Auto = 0, Portrait, LandSpace }
    public OrientationType orientationType;
    // restore this from PlayerPref
    public bool isSoundSet;
	public bool isStandardSet;
	public bool isVegasSet;
    public bool isCumulativeVegasSet;
    public bool isOneCardSet;
	public bool isAutoTapMoveSet;
	public bool isAutoHintSet;
	public bool isHandSet;
	public bool isScoreSet;
	public bool isMoveTimeSet;
	public bool isEffectSet;
    public bool randomDeal;
    public bool isSolutionMode = false;
    public bool isAutoCompleteSet;
    public bool isCongratsScreenSet;
	public int visualPlayBackgroundSet;
	public int visualCardBacksSet;
    public int visualCardFacesSet;
    public int scoreComulativeVegasOne;
	public int scoreComulativeVegasThree;
	public int danceCurrentCounter;
    public int countDeckTurn;
	// keep it from default product settings
	public int[] calendarRankLevelUp;
	public int danceTotalCounter;
	public int overflowRecordLimit;
	public TextAsset textAssetOneCard;
	public TextAsset textAssetThreeCards;
	public string url;

	// detect and save it in flash scene
	public string deviceUniqueIdentifier;
	public string playerName;
	public int startDay;
	public int startMonth;
	public int startYear;

    // clear in flash scene
    public int cellCurrentDay;
    public bool isCalendarGame;
	public bool isGameWon;
	public string[] calendarData;
	public int calendarGameDay;
	public int calendarGameMonth;
	public int calendarGameYear;
	public int calendarTryEarnPoints;
	public bool calendarIsOneCardSet;
	public bool isGameStarted;
	public bool isMenu;
	public bool isCriticalChanges;
	public bool isSocial;
	#endregion
}