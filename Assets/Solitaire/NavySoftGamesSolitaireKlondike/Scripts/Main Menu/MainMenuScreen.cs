using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserWindow;
using Calendar;
using System.Collections;
public class MainMenuScreen : MonoBehaviour
{
 

	 
	#region Public
	public void PlayGame ()
	{
		GameFlowDispatcher.Instance.FromMenuToGame ();
	}
	public void ShowSettings ()
	{
		GameFlowDispatcher.Instance.FromMenuToSettings ();
	}
	public void ShowCallendar ()
	{
       


        if (IsCalendarApproveGame ())
			GameFlowDispatcher.Instance.FromMenuToCalendar ();
		else
			PopUpChangeScoring ();
	}
	public void ShowStats ()
	{
		PopUpManager.Instance.ShowStats ();
	}

 
	#endregion

	private bool IsCalendarApproveGame()
	{
		return (GameSettings.Instance.isStandardSet);
	}
	#region PopUpWindow
	private void PopUpChangeScoring()
	{
		string titleLineData = "CHANGE SCORING";
		string listLinesData = "The score setting will be changed\nto standard type for Daily Challenge.\nWould you like to continue?";

		List<ResultButtonData> buttonData = new List<ResultButtonData> ();
		buttonData.Add (new ResultButtonData ("Ok", 1, OnOk));
		buttonData.Add (new ResultButtonData ("Cancel", 2, OnCancel));

		PopUpManager.Instance.ShowDialog (titleLineData, listLinesData, buttonData);
	}
	private void OnOk()
	{
		GameSettings.Instance.isStandardSet = true;
		GameSettings.Instance.isVegasSet = false;
		GameSettings.Instance.isOneCardSet = true;
		PopUpManager.Instance.Close ();
		GameFlowDispatcher.Instance.FromMenuToCalendar ();
	}
	private void OnCancel()
	{
		PopUpManager.Instance.Close ();
	}
	#endregion   
	#region Debug // Production Delete it
	public void OnDebug()
	{
		GameSettings.Instance.startDay = 1;
		GameSettings.Instance.startMonth = 11;
		GameSettings.Instance.startYear = 2016;
	}
	public void OnDebugSettingsClear()
	{
		GameProgress gp = new GameProgress ();
		gp.Clear ();
	}
	#endregion
	private bool IsShow2x()
	{/*
		CalendarIOManager ioManager = new CalendarIOManager ();
		DateTime dt = DateTime.Today;
		int ranking = ioManager.GetDayMedal (dt.Day, dt.Month, dt.Year);
		bool isMedal = (ranking.Equals (1) || ranking.Equals (2));
        */
		return false;
	}
	private void Show2x(bool isShow)
	{
		 
	}
}