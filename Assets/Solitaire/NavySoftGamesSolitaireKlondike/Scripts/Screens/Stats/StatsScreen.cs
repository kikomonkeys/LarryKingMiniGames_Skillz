using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Convert;
using UserWindow;
using TMPro;
public class StatsScreen : MonoBehaviour
{
    private const int COUNT_GAME_TYPE = 6;
    #region Public Variable

    [SerializeField]
    private StatsGroup[] statsGroups;
    [SerializeField]
    private TextMeshProUGUI textCrowns;
    [SerializeField]
    private TextMeshProUGUI textTropy;
    private int crowns;

    private int tropy;
    #endregion
    private int positionStats = 0;
    private void OnEnable()
    {
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Shift();
       
      

    
        crowns = 0;

        Init();
    }
    private void Init()
    {
        positionStats = 0;

        CalculateStats();
        CalculateDaily();
        ShowStats();
    }
    #region Utilits
    private void CalculateStats()
    {
        for (int index = 0; index < COUNT_GAME_TYPE; index++)
        {
            int playedGames = StatsSettings.Instance.gamesPlayed[index];
            int wonGames = StatsSettings.Instance.gamesWon[index];

            if (playedGames > 0 && playedGames >= wonGames)
            {
                StatsSettings.Instance.UpdateStats(index,StatsType.winRate,(int)(100f / (float)playedGames * (float)wonGames));
            }
            
        }
    }

    private void CalculateDaily()
    {
        int year = DateTime.Now.Year - GameSettings.Instance.startYear;
        int[] medal = new int[31];
        int monthInYear = 12;
        crowns = 0;
        tropy = 0;
        for (int yearIndex = GameSettings.Instance.startYear; yearIndex < year; yearIndex++)
        {

            CaculatorMonthTropyDaily(1, monthInYear, yearIndex);
        }

        CaculatorMonthTropyDaily(1, DateTime.Now.Month, DateTime.Now.Year);


        textCrowns.text = crowns.ToString();
        textTropy.text = tropy.ToString();





    }

    private void CaculatorMonthTropyDaily(int startMonth, int endMonth, int year)
    {

        for (int monthIndex = startMonth; monthIndex <= endMonth; monthIndex++)
        {
            int crownInMonth = 0;
            int dayInMonth = DateTime.DaysInMonth(year, monthIndex);
            for (int dayIndex = 0; dayIndex < dayInMonth; dayIndex++)
            {
                string fileName = string.Format("{0}{1}{2}", dayIndex < 9 ? "0" + dayIndex : dayIndex.ToString(), monthIndex < 9 ? "0" + monthIndex : monthIndex.ToString(), year);
                if (PlayerPrefs.GetInt(fileName) == 1)
                {
                    crowns++;
                    crownInMonth++;
                }
            }

            if (crownInMonth >= 10)
            {
                tropy += 1;
            }
            else if (crownInMonth >= 20)
            {
                tropy += 2;
            }
            else if (crownInMonth >= dayInMonth)
            {
                tropy += 3;
            }


        }
    }

    private void ShowStats()
    {

        for (int i = 0; i < COUNT_GAME_TYPE; i++)
        {
            statsGroups[i].SetHighScore(StatsSettings.Instance.topScore[i].ToString());
            statsGroups[i].SetGamesPlayed(StatsSettings.Instance.gamesPlayed[i].ToString());
            statsGroups[i].SetGamesWon(StatsSettings.Instance.gamesWon[i].ToString());
            statsGroups[i].SetWinRate(StringsConvert.ConcatPersent(StatsSettings.Instance.winRate[i]));
            statsGroups[i].SetShortestTime(StringsConvert.ConvertToMinutesSeconds(StatsSettings.Instance.shortestTime[i]));

            statsGroups[i].SetCurrentWinning(StatsSettings.Instance.currentWinningStreak[i].ToString());
            statsGroups[i].SetCurrentLosingStreak(StatsSettings.Instance.currentLosingStreak[i].ToString());
            statsGroups[i].SetGamesWon(StatsSettings.Instance.gamesWon[i].ToString());

            statsGroups[i].SetMaxWinningStreak(StatsSettings.Instance.maxWinningStreak[i].ToString());
 
        }
    }
    private void CleadStats()
    {
        StatsSettings.Instance.ResetAllStats();
    }
    #endregion;
    #region Public
    public void OnResetStats()
    {
        PopUpResetStats();
    }
    public void OnBack()
    {
        PopUpManager.Instance.Close();
    }
    #endregion;
    #region PopUpDialogWindow
    private void PopUpResetStats()
    {
        string titleLineData = "RESET STATS";
        string listLinesData = "Delete all stats data.\nAre you sure?";

        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("Ok", 2, OnOK));
        buttonData.Add(new ResultButtonData("Cancel", 0, OnCancel));

        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }
    private void OnOK()
    {
        CleadStats();
        Init();
        PopUpManager.Instance.Close();
    }




    private void OnCancel()
    {
        PopUpManager.Instance.Close();
    }
    #endregion
}
