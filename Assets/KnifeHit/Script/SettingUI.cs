﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour {

	[Header("Setting View")]
	public  Toggle soundToggle;
	public  Toggle vibrationToggle;
    public Toggle musicToggle;
	public  GameObject UIParent;
    public Text removeAdPriceText;
    public GameObject bg;
    public GameObject removeAdSection;

	public static SettingUI intance;
    public Text CurrentScoreText;

    [SerializeField]
    bool isWatchVideoPopup;
   

	void Awake()
	{
		if (intance == null) 
		{
			intance = this;
		}
	}

	void Start()
	{
        //mo
        if (CurrentScoreText != null)
        {
            CurrentScoreText.text = "" + GameManager.score;
        }

        if (isWatchVideoPopup)
            return;

        soundToggle.onValueChanged.RemoveAllListeners ();
		vibrationToggle.onValueChanged.RemoveAllListeners ();
        musicToggle.onValueChanged.RemoveAllListeners();
		UpdateUI ();
		soundToggle.onValueChanged.AddListener ((arg0) =>{ 
			GameManager.Sound=arg0;
			if(arg0)
				SoundManager.instance.PlaybtnSfx ();
		} );
		vibrationToggle.onValueChanged.AddListener ((arg0) =>{ 
			GameManager.Vibration=arg0;
			if(arg0)
				SoundManager.instance.playVibrate();
		} );


        musicToggle.onValueChanged.AddListener((arg0) => {
            GameManager.Music = arg0;
            Debug.Log("arg0::" + arg0);
            //if (arg0)
            MusicManager.instance.SetMusic();
        });

        

//#if IAP && UNITY_PURCHASING
//        Purchaser.instance.onItemPurchased += OnItemPurchased;
//        removeAdPriceText.text = "$" + Purchaser.instance.iapItems[0].price;
//#endif

        //removeAdSection.SetActive(Purchaser.instance.isEnabled && !CUtils.IsAdsRemoved());
    }

    public void WatchVideo_SubmitBtnCLicked()
    {
        GamePlayManager.instance.ShowLC();
        gameObject.SetActive(false);
    }

    public void ShowUI()
	{
		UIParent.SetActive (true);
        bg.SetActive(true);
        CUtils.ShowInterstitialAd();
	}

    public void CloseUI()//ot pause resumebtn click event
    {
        UIParent.SetActive(false);
        if(bg != null)
        bg.SetActive(false);
        Time.timeScale = 1;
    }
    public void Pause_ResumeBtn()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public static bool isGameQuitByUser;
    public void Pause_QuitBtn()
    {
        Time.timeScale = 1;
        if(bg != null)
        bg.SetActive(false);
        UIParent.SetActive(false);
        isGameQuitByUser = true;
        //GamePlayManager.instance.gameOverSingle.SetActive(true);
        GamePlayManager.instance.SetScoreForTheGame();
        //Invoke(nameof(ShowLc), 3f);
        GamePlayManager.instance.ShowLC();

    }
    
	public void UpdateUI()
	{
		soundToggle.isOn = GameManager.Sound;
		vibrationToggle.isOn = GameManager.Vibration;
        musicToggle.isOn = GameManager.Music;
	}

	public void OnRestorPurchases()
	{
#if IAP && UNITY_PURCHASING
        Purchaser.instance.RestorePurchases();
#endif
    }

    public void OnRemoveAdCall()
	{
#if IAP && UNITY_PURCHASING
        SoundManager.instance.PlaybtnSfx();
        Purchaser.instance.BuyProduct(0);
#else
        Debug.Log("Please enable, import and install Unity IAP to use this function");
#endif
    }


#if IAP && UNITY_PURCHASING
    private void OnItemPurchased(IAPItem item, int index)
    {
        // A consumable product has been purchased by this user.
        if (item.productType == PType.Consumable)
        {
            
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (item.productType == PType.NonConsumable)
        {
            CUtils.SetRemoveAds(true);
            Toast.instance.ShowMessage("Removing ads is successful");

            removeAdSection.SetActive(false);
        }
        // Or ... a subscription product has been purchased by this user.
        else if (item.productType == PType.Subscription)
        {
            // TODO: The subscription item has been successfully purchased, grant this to the player.
        }
    }
#endif

#if IAP && UNITY_PURCHASING
    private void OnDestroy()
    {
        Purchaser.instance.onItemPurchased -= OnItemPurchased;
    }
#endif
}
