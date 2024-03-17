using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsGroup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI HighScores;
    [SerializeField]
    private TextMeshProUGUI GamesPlayed;
    [SerializeField]
    private TextMeshProUGUI GamesWon;
    [SerializeField]
    private TextMeshProUGUI WinRate;
    [SerializeField]
    private TextMeshProUGUI ShortestTime;
    [SerializeField]
    private TextMeshProUGUI currentWinning;
    [SerializeField]
    private TextMeshProUGUI currentLosingStreak;
    [SerializeField]
    private TextMeshProUGUI maxWinningStreak;



    public void SetHighScore(string highScore)
    {
        HighScores.text = highScore;
    }

    public void SetGamesPlayed(string gamesPlayed)
    {
        GamesPlayed.text = gamesPlayed;
    }
    public void SetGamesWon(string gamesWon)
    {
        GamesWon.text = gamesWon;
    }
 
    public void SetWinRate(string winRate)
    {
        WinRate.text = winRate;
    }
    public void SetShortestTime(string shortestTime)
    {
        ShortestTime.text = shortestTime;
    }
 
  
    public void SetCurrentWinning(string currentWinning)
    {
        this.currentWinning.text = currentWinning;
    }
    public void SetCurrentLosingStreak(string currentLosingStreak)
    {
        this.currentLosingStreak.text = currentLosingStreak;
    }

    public void SetMaxWinningStreak(string maxWinningStreak)
    {
        this.maxWinningStreak.text = maxWinningStreak;
    }
}
