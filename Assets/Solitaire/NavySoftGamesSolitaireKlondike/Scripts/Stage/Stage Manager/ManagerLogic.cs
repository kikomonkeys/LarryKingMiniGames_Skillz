using UnityEngine;

public class ManagerLogic 
{
	public const int SCORE_TIME_DECREASE = 0;//-2;
	public const int SCORE_VEGAS_DECREASE = 0;
	public const float SCORE_TIME = 10;//10
	public const float HINT_TIME = 10;
	public const int DECK_TURN_LIMIT = 10;//piperplay 2; revealing the card or flipping the card
    public const int DECK_TURN_LIMIT_VEGAS_CUMULATIVE = 10;
    public int DecreaseTimeScore {get{ return SCORE_TIME_DECREASE;}}


	public int moves;
	public float timer=300;
	public int score;
	public int opposcore;

	public Solitaire_GameStake.Timer scoreIncreaseTimer = new Solitaire_GameStake.Timer(SCORE_TIME);
	public Solitaire_GameStake.Timer hintTimer = new Solitaire_GameStake.Timer(HINT_TIME);

    

	public bool isGameWin;
	public bool isAllowBlinkHint;
	public bool isAllowAutoComplete;
	public bool isAllowFinish;

	public bool HasDeckTurn
	{
		get {
            int turnLimit = DECK_TURN_LIMIT;
          
            if (GameSettings.Instance.isCumulativeVegasSet)
            {
                turnLimit = DECK_TURN_LIMIT_VEGAS_CUMULATIVE;
            }
           
                
            return GameSettings.Instance.countDeckTurn < turnLimit;


        }
	}

	// ref it
	public void InitScoring()
	{
		moves = 0;
		timer = 0;
		score = 0;
		if (GameSettings.Instance.isCumulativeVegasSet)
			score = (GameSettings.Instance.isOneCardSet) ? GameSettings.Instance.scoreComulativeVegasOne : GameSettings.Instance.scoreComulativeVegasThree;
		if (!GameSettings.Instance.isStandardSet) AddScore (SCORE_VEGAS_DECREASE);

		scoreIncreaseTimer.Clear ();
		hintTimer.Clear ();
		isAllowBlinkHint = true;
		GameSettings.Instance.isGameStarted = false;
		isGameWin = false;
 
		isAllowAutoComplete = true;
		isAllowFinish = true;
	}

	public void AddScore (int value)
	{
		score += value;
		//Debug.Log("Mohith scored::" + score);

		if (GameSettings.Instance.isStandardSet)
		{
			if (score < 0) score = 0;
		}
		HUDController.instance.SetScore(score); // dependency

	}
	public void SetBeginTimer(int timer)
    {
        this.timer = timer;
		HUDController.instance.SetTime(timer); // dependency
	}
	public void SetBeginMove(int moves)
    {
        this.moves = moves;
        HUDController.instance.SetMove(moves); // dependency
    }
    public void AddMoves()
	{
		HUDController.instance.SetMove(++moves); // dependency
	}

	#region Stats
	public void SetStopStatsAndSetting()
	{
		GameSettings.Instance.isGameWon = isGameWin;
	 
		if (GameSettings.Instance.isCumulativeVegasSet) SetVegas (score);
	}
	public void SetStatsStreak()
	{
		int solitaireType = GetSolitaireType ();
		if (isGameWin)
		{
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.currentWinningStreak, 1);
            StatsSettings.Instance.currentLosingStreak[solitaireType] = 0;
			if (StatsSettings.Instance.currentWinningStreak[solitaireType] > StatsSettings.Instance.maxWinningStreak[solitaireType])
				StatsSettings.Instance.UpdateStats(solitaireType,StatsType.maxWinningStreak,StatsSettings.Instance.currentWinningStreak[solitaireType]);
		}
		else
		{
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.currentLosingStreak, 1);
			StatsSettings.Instance.currentWinningStreak [solitaireType] = 0;
		}
	}
	private void SetVegas (int score)
	{	
		if (GameSettings.Instance.isOneCardSet) {
			GameSettings.Instance.scoreComulativeVegasOne = score;
		} else {
			GameSettings.Instance.scoreComulativeVegasThree = score;
		}
	}
	public void SetStatsScoreAndTime(int score, int time,int moves)
	{
		int statScore = score;
		int statTime = (int) time;
        int statMoves = moves;
		int solitaireType = GetSolitaireType ();
        StatsSettings.Instance.UpdateStats(solitaireType, StatsType.gamesWon, 1);
   
		if (statScore > StatsSettings.Instance.topScore [solitaireType]) {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.topScore, statScore);
        }
		if (StatsSettings.Instance.shortestTime [solitaireType].Equals (0)) {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.shortestTime, statTime);
        }
		if (statTime < StatsSettings.Instance.shortestTime [solitaireType]) {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.shortestTime, statTime);
        }

        if (StatsSettings.Instance.moves[solitaireType].Equals(0))
        {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.moves, statMoves);
        }
        if (statMoves < StatsSettings.Instance.moves[solitaireType])
        {
            StatsSettings.Instance.UpdateStats(solitaireType, StatsType.moves, statMoves);
        }
    }

	public int GetGameTypeIndex()
	{
        
        return GetSolitaireType();
    }

	#endregion
	public void StartCountGame()
	{
		hintTimer.Clear ();
		isAllowBlinkHint = true; // ? Try del it and check
		if (GameSettings.Instance.isGameStarted) return;
		//GameSettings.Instance.isGameStarted = true;piper
		int solitaireType = GetSolitaireType ();
        StatsSettings.Instance.UpdateStats(solitaireType, StatsType.gamesPlayed, 1);
     
	}
	public int GetSolitaireType()
	{
        if (GameSettings.Instance.isOneCardSet && GameSettings.Instance.isStandardSet) return 0;
        if (!GameSettings.Instance.isOneCardSet && GameSettings.Instance.isStandardSet) return 1;
        if (GameSettings.Instance.isOneCardSet && GameSettings.Instance.isVegasSet) return 2;
        if (!GameSettings.Instance.isOneCardSet && GameSettings.Instance.isVegasSet) return 3;
        if (GameSettings.Instance.isOneCardSet &&  GameSettings.Instance.isCumulativeVegasSet) return 4;
        if (!GameSettings.Instance.isOneCardSet &&  GameSettings.Instance.isCumulativeVegasSet) return 5;
        return 0;
    }
}