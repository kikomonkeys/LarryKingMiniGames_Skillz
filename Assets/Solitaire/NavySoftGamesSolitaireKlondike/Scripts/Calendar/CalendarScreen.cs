namespace Calendar
{
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections.Generic;
    using UserWindow;
    using TMPro;

    public class CalendarScreen : MonoBehaviour
    {
        [SerializeField]
        private AbstractCalendarView view;
        [SerializeField]
        private TextMeshProUGUI[] txtDateTime;
      
        [SerializeField]
        private Image[] Medals;
        [SerializeField]
        private Text[] inforMedals;

        private int cellBaseShift;
        private int currentMonth;
        private int currentYear;
        [SerializeField]
        private int[] medalRanking;

        private CalendarIOManager ioManager;
        private List<CalendarMonthData> tournamentData;
        private bool isMonthRankingPanel;

        private string editName;

        private bool isCalendarPanel;
        private int prevClickCellIndex = 0;
        private void OnEnable()
        {
          
            isCalendarPanel = true;
            view.ShowMainPanel(isCalendarPanel);
            ioManager = new CalendarIOManager();
            InitView();
            cellBaseShift = 0;
            DateTime dt = DateTime.Today;
            currentMonth = dt.Month;
            currentYear = dt.Year;









            if (GameSettings.Instance.isCalendarGame && GameSettings.Instance.isGameWon)
            {
                int month = GameSettings.Instance.calendarGameMonth;
                int year = GameSettings.Instance.calendarGameYear;

                CalendarMonthData prevData = ioManager.CreateTournamentData(month, year);
                GameSettings.MedalType prevMedal = prevData.Medal;
                PutWinCalendarGame();
                CalendarMonthData nextData = ioManager.CreateTournamentData(month, year);
                GameSettings.MedalType nextMedal = nextData.Medal;

                
            }
            RefreshCalendar();
           
            PlayerPrefAPI.Set();
        }

        #region View
        private void InitView()
        {
            view.InitView();
        }
        #endregion
        #region Utilits
        private void RefreshCalendar() // TODO: Ref Obsolate architecture
        {
  
            if (!ioManager.HasMonthRanking(currentMonth, currentYear))
            {
                medalRanking = new int[31];
                ioManager.SaveMonthRanking(currentMonth, currentYear, medalRanking);
            }
            else
                medalRanking = ioManager.LoadMonthRanking(currentMonth, currentYear);

            tournamentData = ioManager.InitTournament();

            HideAllCells();
            ShowMonthYear(currentMonth, currentYear);
            ShowCalendar(currentMonth, currentYear);
            SetCalendar(currentMonth, currentYear);
            ShowArrowsPrevNextMonth(currentMonth, currentYear);
            isMonthRankingPanel = true;
            ShowRankingPanel(currentMonth, currentYear, tournamentData);
        }
        private void HideAllCells()
        {
            view.HideAllCells();
        }
        private void ShowMonthYear(int month, int year)
        {
            string info = (ioManager.monthName[month - 1] + " " + year.ToString()).ToUpper();
            //infoMonthYearText.text = info;
            view.ShowMonthYear(info);
        }
        private void ShowCalendar(int month, int year)
        {
            DateTime dt = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int dayOfWeek = ((int)dt.DayOfWeek);
            cellBaseShift = (dayOfWeek.Equals(0)) ? 6 : dayOfWeek - 1;

            DateTime today = DateTime.Today;

            for (int dayIndex = 0; dayIndex < daysInMonth; dayIndex++)
            {
                int shiftIndex = dayIndex + cellBaseShift;

                view.CellObjSetActive(shiftIndex, true);


                view.CellViewHideComponents(shiftIndex);

                view.CellViewSetDate(shiftIndex, dayIndex + 1);

                if (IsEquilDate(dayIndex + 1, dt.Month, dt.Year, GameSettings.Instance.startDay, GameSettings.Instance.startMonth, GameSettings.Instance.startYear) ||

                    (IsAfterDate(dayIndex + 1, dt.Month, dt.Year, GameSettings.Instance.startDay, GameSettings.Instance.startMonth, GameSettings.Instance.startYear) &&
                    IsPreveouseDate(dayIndex + 1, dt.Month, dt.Year, today.Day, today.Month, today.Year)))
                    view.CellViewSetActiveCell(shiftIndex, true);
                else
                    view.CellViewSetActiveCell(shiftIndex, false);


                if (IsEquilDate(dayIndex + 1, dt.Month, dt.Year, today.Day, today.Month, today.Year))
                {
                    view.CellViewSetActiveCell(shiftIndex, true);
                  
                }
            }
        }
        private void SetCalendar(int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);

            int lastUnPlayedDay = -1;
            for (int i = 0; i < 42; i++)
            {
                view.CellViewShowHalo(i, false);
            }
            for (int dayIndex = 0; dayIndex < daysInMonth; dayIndex++)
            {
                int shiftIndex = dayIndex + cellBaseShift;
                int medalRank = medalRanking[dayIndex];



                if (view.CellViewIsActiveCell(shiftIndex))
                {
                    if (medalRank.Equals(0)) lastUnPlayedDay = dayIndex;
                    if (medalRank.Equals(1)) view.CellViewShowGoldMedal(shiftIndex);
                    if (medalRank.Equals(2)) view.CellViewShowGoldMedal(shiftIndex);
                }

                if (IsWonDay(dayIndex + 1, month, year) && GameSettings.Instance.isGameWon)
                {
                    if (medalRank.Equals(1)) view.CellViewAnimateGoldMedal(shiftIndex);
                    if (medalRank.Equals(2)) view.CellViewAnimateGoldMedal(shiftIndex);
                    SetGameDay(0, 0, 0);
                    GameSettings.Instance.isGameWon = false;
                }
            }
            if (!lastUnPlayedDay.Equals(-1))
            {
                view.CellViewShowHalo(lastUnPlayedDay + cellBaseShift, true);
       
                //    Debug.Log("cellBaseShift " + cellBaseShift);
                GameSettings.Instance.cellCurrentDay = lastUnPlayedDay + cellBaseShift;
               
                SetGameDay(lastUnPlayedDay +1, month, year);
                prevClickCellIndex = lastUnPlayedDay + cellBaseShift;
                    
                SetCalendarBottomInfor(lastUnPlayedDay + 1);
            }
        }

        private GameSettings.MedalType GetMedalDay(int day, int month, int year)
        {


            if (day >= medalRanking.Length) day = medalRanking.Length;
            int medalRank = medalRanking[day - 1];


            if (medalRank.Equals(1)) return GameSettings.MedalType.Gold;
            if (medalRank.Equals(2)) return GameSettings.MedalType.Gold;


            return GameSettings.MedalType.None;
        }
        private bool IsWonDay(int compareDay, int compareMonth, int compareYear)
        {
            if (GameSettings.Instance.calendarGameDay.Equals(compareDay) && GameSettings.Instance.calendarGameMonth.Equals(compareMonth) & GameSettings.Instance.calendarGameYear.Equals(compareYear))
                return true;
            return false;
        }
        private void SetGameDay(int day, int month, int year)
        {
            GameSettings.Instance.calendarGameDay = day;
            GameSettings.Instance.calendarGameMonth = month;
            GameSettings.Instance.calendarGameYear = year;
        }

        private bool IsPreveouseDate(int compareDay, int compareMonth, int compareYear, int fixDay, int fixMonth, int fixYear)
        {
            if (compareYear < fixYear) return true;
            if (compareYear.Equals(fixYear) && compareMonth < fixMonth) return true;
            if (compareYear.Equals(fixYear) && compareMonth.Equals(fixMonth) && compareDay < fixDay) return true;
            return false;
        }
        private bool IsAfterDate(int compareDay, int compareMonth, int compareYear, int fixDay, int fixMonth, int fixYear)
        {
            if (compareYear > fixYear) return true;
            if (compareYear.Equals(fixYear) && compareMonth > fixMonth) return true;
            if (compareYear.Equals(fixYear) && compareMonth.Equals(fixMonth) && compareDay > fixDay) return true;
            return false;
        }
        private bool IsEquilDate(int compareDay, int compareMonth, int compareYear, int fixDay, int fixMonth, int fixYear)
        {
            if (compareYear.Equals(fixYear) && compareMonth.Equals(fixMonth) && compareDay.Equals(fixDay))
                return true;
            else
                return false;
        }

        private void ShowArrowsPrevNextMonth(int month, int year)
        {
            DateTime today = DateTime.Today;
            bool isPrev = IsAfterDate(1, month, year, 1, GameSettings.Instance.startMonth, GameSettings.Instance.startYear);
            bool isNext = IsPreveouseDate(32, month, year, today.Day, today.Month, today.Year);
            view.ShowArrows(isPrev, isNext);
        }
        private void PutWinCalendarGame()
        {
            int day = GameSettings.Instance.calendarGameDay;
            int month = GameSettings.Instance.calendarGameMonth;
            int year = GameSettings.Instance.calendarGameYear;
            int earnPoint = GameSettings.Instance.calendarTryEarnPoints;

            int[] newRanking = ioManager.LoadMonthRanking(month, year);
            if (newRanking[day - 1].Equals(0))
            {
                newRanking[day - 1] = earnPoint;
                ioManager.SaveMonthRanking(month, year, newRanking);
            }
        }

        private void ShowRankingPanel(int month, int year, List<CalendarMonthData> tournamentData)
        {


            string playerName = GameSettings.Instance.playerName;

            //	CalendarInfoData infoData = (isMonthRankingPanel) ? ioManager.GetMonthRankingData (month, year, tournamentData) : ioManager.GetGlobalRankingData (tournamentData);
            CalendarInfoData infoData = ioManager.GetMonthRankingData(month, year, tournamentData);
            int medalType = (int)infoData.Medal;

            Sprite medalSpr = ImageSettings.Instance.medal[medalType];
            string rankText = infoData.Medal.ToString().ToUpper();
            string progressText = infoData.Earn.ToString() + "/" + infoData.Total.ToString();
           
            view.ShowRankingPanel(playerName, isMonthRankingPanel, medalSpr, rankText, progressText);

            bool isLastRank = (medalType < GameSettings.Instance.calendarRankLevelUp.Length);
            if (isLastRank)
            {
                string infoText = infoData.NeedToNext.ToString();
             
            }
            
        }

        private void SetCalendarBottomInfor(int day)
        {
            if (day >= DateTime.DaysInMonth(currentYear, currentMonth)) day = DateTime.DaysInMonth(currentYear, currentMonth);

            bool hasMedal = 0.Equals(ioManager.GetDayMedal(day, currentMonth, currentYear));

            for (int i = 0; i < txtDateTime.Length; i++)
            {
                txtDateTime[i].text = ioManager.monthName[currentMonth - 1].ToString() + " " + day.ToString();
            }
         

          

            
        }
        #endregion
        #region Public
        public void OnPrevMonthClick()
        {
            currentMonth--;
            if (currentMonth.Equals(0))
            {
                currentMonth = 12;
                currentYear--;
            }
            RefreshCalendar();
        }
        public void OnNextMonthClick()
        {
            currentMonth++;
            if (currentMonth.Equals(13))
            {
                currentMonth = 1;
                currentYear++;
            }
            RefreshCalendar();
        }

        public void OnCellCick(int clickCellIndex)
        {
            if (view.CellViewIsActiveCell(clickCellIndex))
            {
                view.CellViewShowHalo(prevClickCellIndex, false);
                view.CellViewShowHalo(clickCellIndex, true);
                prevClickCellIndex = clickCellIndex;
                Solitaire_GameStake.Sound.Instance.Creack();
                int day = view.CellViewGetDate(clickCellIndex);
                GameSettings.Instance.cellCurrentDay = clickCellIndex;
              //  Debug.Log(string.Format("{0}-{1}-{2} ",day,currentMonth,currentYear) );
                string[] solitaireData = ioManager.GetDayChallenge(day, currentMonth, currentYear);


                GameSettings.Instance.isCalendarGame = true;

                GameSettings.Instance.calendarData = solitaireData;
                SetGameDay(day, currentMonth, currentYear);

                bool isToday = ioManager.IsToday(day, currentMonth, currentYear);

                GameSettings.Instance.calendarTryEarnPoints =1;
                GameSettings.Instance.calendarIsOneCardSet = GameSettings.Instance.isOneCardSet;


                SetCalendarBottomInfor(day);





                GameSettings.MedalType medalType = GetMedalDay(day, currentMonth, currentYear);



            }
        }
        public void OnPlay()
        {	
			HUDController.instance.VisibleLayout(true);

            PopUpManager.Instance.Close();
            ContinueModeGame.instance.ClearAllDataCard();
            OnCellCick(GameSettings.Instance.cellCurrentDay);
            //GoogleMobileAdsScript.instance.ShowRewardBasedVideo();
            OnChalengePlay();
        }
        private void OnClose()
        {
            PopUpManager.Instance.Close();
        }
        private void OnChalengeBack()
        {
            isCalendarPanel = true;
            view.ShowMainPanel(isCalendarPanel);
        }

        private void OnChalengePlay()
        {
            GameFlowDispatcher.Instance.FromCalendarToGame();
        }

        public void OnTournament()
        {
            OnCollection();
        }
        public void OnEdit()
        {
            editName = GameSettings.Instance.playerName;
           
        }
        public void OnRanking(bool isMonth)
        {
            isMonthRankingPanel = isMonth;
            ShowRankingPanel(currentMonth, currentYear, tournamentData);
        }
        public void BackToMenu()
        {

            HUDController.instance.VisibleLayout(true);
            PopUpManager.Instance.Close();
        }
        #endregion

        #region PopUpDialogWindow
    
        private void OnOkGetMedal()
        {
            PopUpManager.Instance.Close();
        }

    
        private void OnInputField(string line)
        {
            editName = line;
        }
        private void OnCancel()
        {
            PopUpManager.Instance.Close();
        }
        private void OnSave()
        {
            if (IsNameValid(editName))
            {
                PopUpManager.Instance.Close();
                if (editName.Equals(""))
                    editName = GameSettings.Instance.playerName;
                GameSettings.Instance.playerName = editName;
                PlayerPrefs.SetString("Name", editName);
                PlayerPrefs.Save();
              

                ShowRankingPanel(currentMonth, currentYear, tournamentData);
                if (!isCalendarPanel)
                    view.InitTournamentRecord();
            }


        }
        private void SaveResultCallback(bool isSaved)
        {
            // TODO: think about isSaved
        }
        private bool IsNameValid(string name)
        {
            string validString = "1234567890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            foreach (char c in name)
            {
                if (!validString.Contains(c.ToString()))
                    return false;
            }
            return true;
        }
        public void OnCollection()
        {
            string head = "Collection";
            List<CollectionData> collectionData = CreateCollectionData(currentMonth, currentYear, tournamentData);

            List<ResultButtonData> buttonData = new List<ResultButtonData>();
            buttonData.Add(new ResultButtonData("Return to Callendar", 1, OnCallendar));
          
        }
        private void OnCallendar()
        {

            PopUpManager.Instance.Close();
        }
        private List<CollectionData> CreateCollectionData(int month, int year, List<CalendarMonthData> tournamentData)
        {
            List<CollectionData> collectionData = new List<CollectionData>();

            int workMonth = month - 5;
            int workYear = year;
            if (workMonth < 1)
            {
                workYear--;
                workMonth = 12 + workMonth;
            }

            for (int index = 0; index < 6; index++)
            {
                CalendarInfoData info = ioManager.GetMonthRankingData(workMonth, workYear, tournamentData);

                Sprite medalSprite = ImageSettings.Instance.medal[(int)info.Medal];
                string date = ioManager.monthName[workMonth - 1].Substring(0, 3).ToUpper() + " " + workYear.ToString();
                CollectionData medalData = new CollectionData(medalSprite, date);
                collectionData.Add(medalData);

                workMonth++;
                if (workMonth.Equals(13))
                {
                    workMonth = 1;
                    workYear++;
                }
            }
            return collectionData;
        }
        #endregion
    }
}