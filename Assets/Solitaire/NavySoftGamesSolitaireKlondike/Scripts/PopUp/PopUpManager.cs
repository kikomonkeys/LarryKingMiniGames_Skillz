using UnityEngine;
using System;
using System.Collections.Generic;
using UserWindow;
using System.Collections;
public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance = null;
    public static PopUpManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject PopUpPref = GameObject.Find("PopUpWindow");

                PopUpPref.name = "PopUpWindow";
                _instance = PopUpPref.GetComponentInChildren<PopUpManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private RectTransform holder;
    [SerializeField]
    private PopUpAnimator animator;

    [SerializeField]
    private GameObject settingScreen;
    [SerializeField]
    private GameObject cardBackScreen;
    [SerializeField]
    private GameObject cardFaceScreen;
    [SerializeField]
    private GameObject playBackGroundScreen;
    [SerializeField]
    private GameObject statsScreen;
    [SerializeField]
    private GameObject calendarScreen;
    [SerializeField]
    private GameObject dialogScreen;
    [SerializeField]
    private GameObject winEffectScreen;
    [SerializeField]
    private GameObject winNormalScreen;

    [SerializeField]
    private GameObject winDailyScreen;
    #region Main
    //	public RectTransform Holder {get{ return holder;}}

    public void Show(GameObject origin, PopUpAnimator.Direction direction, Action callback)
    {
        animator.Show(origin, direction, callback);
    }
    public void Close()
    {
        animator.CloseLastWindow();

        BackScreen.instance.RemoveWindow();
    }
    private GameObject SpawnPref(GameObject pref, RectTransform holderRT)
    {
        GameObject obj = pref;
        obj.name = pref.name;
        obj.SetActive(true);
        obj.transform.SetParent(holderRT);
        return obj;
    }
    #endregion
    #region Panel
 
    public void ShowStats()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = statsScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromDown, null);
    }
    public void ShowSettings()
    {
      BackScreen.instance.AddWindow();
        GameObject origin = settingScreen;
      
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, null);
        
    }
    public void ShowCardBack(Action func)
    {
        BackScreen.instance.AddWindow();
        GameObject origin = cardBackScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, func);
    }

    public void ShowCardFace(Action func)
    {
        BackScreen.instance.AddWindow();
        GameObject origin = cardFaceScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, func);
    }
    public void ShowPlayBackground(Action func)
    {
        BackScreen.instance.AddWindow();
        GameObject origin =playBackGroundScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromLeft, func);
    }
 
    public void ShowRule()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = WindowSettings.Instance.RulePref;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.FromRight, null);
    }
  
 
   
 
 
    public void ShowCalendar()
    {
        BackScreen.instance.AddWindow();
        GameObject origin = calendarScreen ;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.Central, null);
    }
    public void ShowWin(Action func)
    {
        GameObject origin = winEffectScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
            animator.Show(SpawnPref(origin, holder), PopUpAnimator.Direction.Central, func);
    }
    #endregion
    #region Dialog
    public void ShowResult(ResultTextLineData titleData, List<ResultTextLineData> listData, bool isAwarded, List<ResultButtonData> buttonData)
    {
        GameObject origin = winNormalScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            ResultWindowScreen manager = obj.GetComponentInChildren<ResultWindowScreen>();
            manager.Init(titleData, listData, isAwarded, buttonData);
            StartCoroutine(manager.Build());
            Show(obj, PopUpAnimator.Direction.FromLeft, null);
        }
    }
    public void ShowScroll(ResultTextLineData titleData, List<ResultTextLineData> listData, bool isAwarded, List<ResultButtonData> buttonData)
    {
        GameObject origin = winDailyScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            ScrollWindowScreen manager = obj.GetComponentInChildren<ScrollWindowScreen>();
            manager.Init(titleData, listData, isAwarded, buttonData);
            Show(obj, PopUpAnimator.Direction.FromLeft, null);
            manager.Build();

        }
    }
    public void ShowDialog(string headData, string infoData, List<ResultButtonData> buttonData)
    {
        GameObject origin =dialogScreen;
        if (!animator.IsPresentWindowInPopUp(origin))
        {
            GameObject obj = SpawnPref(origin, holder);
            DialogWindowScreen manager = obj.GetComponentInChildren<DialogWindowScreen>();
            manager.Init(headData, infoData, buttonData);
            manager.Build();
            Show(obj, PopUpAnimator.Direction.FromRight, null);
        }
    }
 
 
    #endregion
}
