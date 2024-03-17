using UnityEngine;

[System.Serializable]
public class PrefMainData
{
	[System.Serializable]
	public class RecordPref
	{
		// restore this from PlayerPref
		public bool isSoundSet;
		public bool isStandardSet;
        public bool isVegasSet;
        public bool isComulativeVegasSet;
		public bool isOneCardSet;
		public bool isAutoTapMoveSet;
		public bool isAutoHintSet;
		public bool isHandSet;
		public bool isScoreSet;
		public bool isMoveTimeSet;
		public bool isEffectSet;
        public bool isAutoCompleteSet;
        public bool isCongratsScreenSet;
		public int visualPlayBackgroundSet;
		public int visualCardBacksSet;
        public int visualCardFacesSet;
        public int orientationType;
        public string  data1;
        public string data2;
        public int scoreComulativeVegasOne;
		public int scoreComulativeVegasThree;
        public int countDeckTurn;
        public string moves;
        public string topScore;
        public string gamesPlayed;
        public string gamesWon;
        public string winRate;
        public string shortestTime;
        public string currentWinningStreak;
        public string currentLosingStreak;
        public string maxWinningStreak;
        //		public string playerName;
        public int danceCurrentCounter;
	}
	public RecordPref settings;
}

public static class PlayerPrefAPI 
{
	public static void Set()
	{
		PrefMainData parsedData = new PrefMainData();
		parsedData.settings = new PrefMainData.RecordPref ();
		parsedData.settings.isSoundSet = GameSettings.Instance.isSoundSet;
		parsedData.settings.isStandardSet = GameSettings.Instance.isStandardSet;
		parsedData.settings.isComulativeVegasSet = GameSettings.Instance.isCumulativeVegasSet;
        parsedData.settings.isVegasSet = GameSettings.Instance.isVegasSet;
        parsedData.settings.isOneCardSet = GameSettings.Instance.isOneCardSet;
		parsedData.settings.isAutoTapMoveSet = GameSettings.Instance.isAutoTapMoveSet;
		parsedData.settings.isAutoHintSet = GameSettings.Instance.isAutoHintSet;
		parsedData.settings.isHandSet = GameSettings.Instance.isHandSet;
		parsedData.settings.isScoreSet = GameSettings.Instance.isScoreSet;
		parsedData.settings.isMoveTimeSet = GameSettings.Instance.isMoveTimeSet;
        parsedData.settings.countDeckTurn = GameSettings.Instance.countDeckTurn;
        parsedData.settings.isEffectSet = GameSettings.Instance.isEffectSet;
		parsedData.settings.isCongratsScreenSet = GameSettings.Instance.isCongratsScreenSet;
		parsedData.settings.visualPlayBackgroundSet = GameSettings.Instance.visualPlayBackgroundSet;
		parsedData.settings.visualCardBacksSet = GameSettings.Instance.visualCardBacksSet;
        parsedData.settings.visualCardFacesSet = GameSettings.Instance.visualCardFacesSet;
        parsedData.settings.orientationType = (int)GameSettings.Instance.orientationType;
        parsedData.settings.scoreComulativeVegasOne = GameSettings.Instance.scoreComulativeVegasOne;
        parsedData.settings.isAutoCompleteSet = GameSettings.Instance.isAutoCompleteSet;
        parsedData.settings.scoreComulativeVegasThree = GameSettings.Instance.scoreComulativeVegasThree;
        parsedData.settings.data1 = GameSettings.Instance.calendarData[0];
        parsedData.settings.data2 = GameSettings.Instance.calendarData[1];
        //		parsedData.settings.playerName = GameSettings.Instance.playerName;
        parsedData.settings.danceCurrentCounter = GameSettings.Instance.danceCurrentCounter;
 

       


        string rawSetData = JsonUtility.ToJson (parsedData);

		PlayerPrefs.SetString("Setting",rawSetData);
		PlayerPrefs.Save ();
	}

	public static void Get()
	{
		if (PlayerPrefs.HasKey ("Setting"))
		{
			string rawGetData = PlayerPrefs.GetString ("Setting");
			PrefMainData parsedData = ParseSetting(rawGetData);
            GameSettings.Instance.isCumulativeVegasSet = parsedData.settings.isComulativeVegasSet;
            GameSettings.Instance.isSoundSet = parsedData.settings.isSoundSet;
			GameSettings.Instance.isStandardSet = parsedData.settings.isStandardSet;
			GameSettings.Instance.isVegasSet = parsedData.settings.isVegasSet;
			GameSettings.Instance.isOneCardSet = parsedData.settings.isOneCardSet;
			GameSettings.Instance.isAutoTapMoveSet = parsedData.settings.isAutoTapMoveSet;
			GameSettings.Instance.isAutoHintSet = parsedData.settings.isAutoHintSet;
			GameSettings.Instance.isHandSet = parsedData.settings.isHandSet;
			GameSettings.Instance.isScoreSet = parsedData.settings.isScoreSet;
			GameSettings.Instance.isMoveTimeSet = parsedData.settings.isMoveTimeSet;
			GameSettings.Instance.isEffectSet = parsedData.settings.isEffectSet;
			GameSettings.Instance.isCongratsScreenSet = parsedData.settings.isCongratsScreenSet;
			GameSettings.Instance.visualPlayBackgroundSet = parsedData.settings.visualPlayBackgroundSet;
			GameSettings.Instance.visualCardBacksSet = parsedData.settings.visualCardBacksSet;
            GameSettings.Instance.calendarData = new string[2];
            GameSettings.Instance.calendarData[0] = parsedData.settings.data1 ;
              GameSettings.Instance.calendarData[1] = parsedData.settings.data2;
            GameSettings.Instance.visualCardFacesSet = parsedData.settings.visualCardFacesSet;
            switch (parsedData.settings.orientationType)
            {
                case 0: GameSettings.Instance.orientationType = GameSettings.OrientationType.Auto; break;
                case 1: GameSettings.Instance.orientationType = GameSettings.OrientationType.Portrait; break;
                case 2: GameSettings.Instance.orientationType = GameSettings.OrientationType.LandSpace; break;
            }
            GameSettings.Instance.countDeckTurn = parsedData.settings.countDeckTurn;
 

            GameSettings.Instance.isAutoCompleteSet = parsedData.settings.isAutoCompleteSet ;
            GameSettings.Instance.scoreComulativeVegasOne = parsedData.settings.scoreComulativeVegasOne;
			GameSettings.Instance.scoreComulativeVegasThree = parsedData.settings.scoreComulativeVegasThree;
//			GameSettings.Instance.playerName = parsedData.settings.playerName;
			GameSettings.Instance.danceCurrentCounter = parsedData.settings.danceCurrentCounter;
		}
		else {
			ApplyDefaultGameSettings ();
//			throw new UnityException ("There are no Setting in PlayerPref");
		}

	}

	static void ApplyDefaultGameSettings(){
  
		GameSettings.Instance.isSoundSet 	= true;
		GameSettings.Instance.isStandardSet = true;
		GameSettings.Instance.isVegasSet 	= false;
        GameSettings.Instance.isCumulativeVegasSet = false;
        GameSettings.Instance.isOneCardSet 			= false; // NOTE: default Draw is 3 Cards for Setting
		GameSettings.Instance.isAutoTapMoveSet 	= true;
		GameSettings.Instance.isAutoHintSet 	= true;
		GameSettings.Instance.isHandSet 	= false;
		GameSettings.Instance.isScoreSet	= true;
		GameSettings.Instance.isMoveTimeSet = true;
		GameSettings.Instance.isEffectSet 	= true;
		GameSettings.Instance.isCongratsScreenSet 	= true;
		GameSettings.Instance.visualPlayBackgroundSet 	= 0;
        GameSettings.Instance.visualCardFacesSet = 1;
        GameSettings.Instance.visualCardBacksSet 		= 0;
        GameSettings.Instance.calendarData = new string[2];
        for (int i = 0; i < GameSettings.Instance.calendarData.Length; i++)
        {
            GameSettings.Instance.calendarData[i] = string.Empty;
        }
		GameSettings.Instance.scoreComulativeVegasOne 	= 0;
		GameSettings.Instance.scoreComulativeVegasThree = 0;
//			GameSettings.Instance.playerName = ;
		GameSettings.Instance.danceCurrentCounter 		= 0;


  

    }


	private static PrefMainData ParseSetting (string rawGetData)
	{
		PrefMainData parsedData;
		try
		{
			parsedData = JsonUtility.FromJson<PrefMainData>(rawGetData);
		}
		catch
		{
			parsedData = null;
		}
		return parsedData;
	}

    public static void SaveDataUserClickCard(string data)
    {
        PlayerPrefs.SetString("ClickCard", data);
    }

    public static void SaveDataInMatch(string data)
    {
        PlayerPrefs.SetString("DataInMatch", data);
    }
    public static string LoadDataUserClickCard()
    {
        return PlayerPrefs.GetString("ClickCard");
    }
    public static string LoadDataInMatch( )
    {
       return PlayerPrefs.GetString("DataInMatch");
    }
}
