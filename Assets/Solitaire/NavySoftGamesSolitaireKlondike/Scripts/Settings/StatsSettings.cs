 
using UnityEngine;


public enum StatsType
{
    moves,
    topScore,
    gamesPlayed,
    gamesWon,
    winRate,
    shortestTime,
    currentWinningStreak,
    currentLosingStreak,
    maxWinningStreak,
}
public class StatsSettings : MonoBehaviour
{
	private static StatsSettings _instance = null;
  
    public static StatsSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StatsSettings>();
                if (_instance == null)
                {
                    throw new UnityException("StatsSetting Not In Hierarchy ");
                }
            }
            return _instance;
        }
    }
    public int[] moves;
    public int[] topScore;
	public int[] gamesPlayed;
	public int[] gamesWon;
	public int[] winRate;
	public int[] shortestTime;
	public int[] currentWinningStreak;
	public int[] currentLosingStreak;
	public int[] maxWinningStreak;
    private StatsData statsData = new StatsData();
    public class StatsData
    {
        public string movesData;
        public string topScoreData;
        public string gamesPlayedData;
        public string gamesWonData;
        public string winRateData;
        public string shortestTimeData;
        public string currentWinningStreakData;
        public string currentLosingStreakData;
        public string maxWinningStreakData;
    }
    private void Start()
    {
        Load();
    }

    public void ResetAllStats()
    {
        int COUNT_GAME_TYPE = 6;
        moves = new int[COUNT_GAME_TYPE];
        topScore = new int[COUNT_GAME_TYPE];
        gamesPlayed = new int[COUNT_GAME_TYPE];
        gamesWon = new int[COUNT_GAME_TYPE];
        winRate = new int[COUNT_GAME_TYPE];
        shortestTime = new int[COUNT_GAME_TYPE];
        currentWinningStreak = new int[COUNT_GAME_TYPE];
        currentLosingStreak = new int[COUNT_GAME_TYPE];
        maxWinningStreak = new int[COUNT_GAME_TYPE];

       
        for (int i = 0; i < moves.Length; i++)
        {
            moves[i] = 9999;
        }
        Save();
    }



    public void Save()
    {
        statsData.movesData = JsonHelper.ToJson<int>(moves);
        statsData.topScoreData = JsonHelper.ToJson<int>(topScore);
        statsData.gamesPlayedData = JsonHelper.ToJson<int>(gamesPlayed);
        statsData.gamesWonData = JsonHelper.ToJson<int>(gamesWon);
        statsData.winRateData = JsonHelper.ToJson<int>(winRate);
        statsData.shortestTimeData = JsonHelper.ToJson<int>(shortestTime);
        statsData.currentWinningStreakData = JsonHelper.ToJson<int>(currentWinningStreak);
        statsData.currentLosingStreakData = JsonHelper.ToJson<int>(currentLosingStreak);
        statsData.maxWinningStreakData = JsonHelper.ToJson<int>(maxWinningStreak);
        string data = JsonUtility.ToJson(statsData);

        PlayerPrefs.SetString("StatsGame", data);

    }

    public void Load()
    {
        string data = PlayerPrefs.GetString("StatsGame", string.Empty);
        if (string.IsNullOrEmpty(data))
        {
            ResetAllStats();
        }
        else
        {
            statsData = JsonUtility.FromJson<StatsData>(data);
            moves = JsonHelper.FromJson<int>(statsData.movesData);
            topScore = JsonHelper.FromJson<int>(statsData.topScoreData);
            gamesPlayed = JsonHelper.FromJson<int>(statsData.gamesPlayedData);
            gamesWon = JsonHelper.FromJson<int>(statsData.gamesWonData);
            winRate = JsonHelper.FromJson<int>(statsData.winRateData);
            shortestTime = JsonHelper.FromJson<int>(statsData.shortestTimeData);

            currentWinningStreak = JsonHelper.FromJson<int>(statsData.currentWinningStreakData);
            currentLosingStreak = JsonHelper.FromJson<int>(statsData.currentLosingStreakData);
            maxWinningStreak = JsonHelper.FromJson<int>(statsData.maxWinningStreakData);
        }
    }


    public void UpdateStats(int position, StatsType statsType, int value)
    {
        switch (statsType)
        {
            case StatsType.moves:
                moves[position] = value;
                break;
            case StatsType.topScore:
                topScore[position] = value;
                break;
            case StatsType.gamesPlayed:
                gamesPlayed[position] += value;
                break;
            case StatsType.gamesWon:
                gamesWon[position] += value;
                break;
            case StatsType.winRate:
                winRate[position] = value;
                break;
            case StatsType.shortestTime:
                shortestTime[position] = value;
                break;
            case StatsType.currentWinningStreak:
                Debug.Log("Current Winning");
                currentWinningStreak[position] += value;
                break;
            case StatsType.currentLosingStreak:
                currentLosingStreak[position] += value;
                break;
            case StatsType.maxWinningStreak:
                maxWinningStreak[position] = value;
                break;

        }

        Save();
    }
}