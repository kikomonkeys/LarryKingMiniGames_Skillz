using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class MainMenu : MonoBehaviour 
{
	[Header("Main View")]
	public Button giftButton;
	public Text giftLable;
	public CanvasGroup giftLableCanvasGroup;
	public GameObject giftBlackScreen;
	public GameObject giftParticle;
	public Image selectedKnifeImage;
	public AudioClip giftSfx;
	public GameObject howToPlayObj;
	public GameObject mainMenu;
	public static MainMenu intance;

	// Gift Setting

	int timeForNextGift = 60*8;
	int minGiftApple = 40;// Minimum Apple for Gift
	int maxGiftApple = 70;// Maxmum Apple for Gift


	[Header("Options View")]
	public  InputField userName;
	public GameObject options;

	string userNameSaves;
	void Awake()
	{
		intance = this;
	}
	void Start()
	{
        //CUtils.ShowInterstitialAd();
		//InvokeRepeating ("updateGiftStatus", 0f, 1f);
		if(KnifeShop.intance != null)
		KnifeShop.intance.UpdateUI ();
        if (GameManager.Knife_HowToPlay == 0)
        {
			//howToPlayObj.SetActive(true);
			mainMenu.SetActive(false);
        }
        else
        {
			if (GamePlayManager.homeBtnClicked)
			{
				GamePlayManager.homeBtnClicked = false;
				mainMenu.SetActive(true);
				//howToPlayObj.SetActive(false);
			}
			else
			{
				GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
			}
		}
		//Debug.LogError("Help::" + GameManager.Knife_HowToPlay);

		/*if (PlayerPrefs.HasKey("MyName"))
        {
			userName.gameObject.SetActive(false);
			userNameSaves = PlayerPrefs.GetString("MyName");
			Debug.Log("userNameSaves == > " + userNameSaves);
			options.SetActive(true);


		}*/
	}
	public void OnPlayerNameEntered()
    {
        if (PlayerPrefs.HasKey("MyName"))
        {
			userName.gameObject.SetActive(false);
			options.SetActive(true);

		}
        else
        {
			if (userName.text.Length > 0)
			{
				PlayerPrefs.SetString("MyName", userName.text);
				options.SetActive(true);
			}

		}

	}
	float usdCost = 0;

	public void OnPlayClick()//int players
	{
		//Debug.LogError("helpppppp::" + GameManager.Knife_HowToPlay);
        if (GameManager.Knife_HowToPlay == 0)
        {
			//if(howToPlayObj!=null)
			//howToPlayObj.SetActive(true);
			//else
			//	GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");

		}
		else
        {
			
			GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
		}
		/*
		if (userName.text.Length <= 0 && userNameSaves.Length <= 0) return;

		GameManager.Stage = players == 1 ? 1 : 1;
		GameManager.Stage = players == 2 ? 10 : GameManager.Stage;
		GameManager.Stage = players == 3 ? 20 : GameManager.Stage;
		GameManager.Stage = players == 4 ? 30 : GameManager.Stage;


/*        if (userNameSaves.Length > 0)
        {

        }
            else if(userName.text.Length > 0 && userNameSaves.Length<=0)
            {
				userNameSaves = userName.text;
				PlayerPrefs.SetString("MyName", userName.text);
			userName.transform.gameObject.SetActive(false);

		}*/


		//if (userName.text.Length > 0 && players <= 1)
		//	GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
		//else
		/*{
			SoundManager.instance.PlaybtnSfx();

			if(players >= 2)
            {
				usdCost = players == 2 ? usdCost = 0.6f : usdCost;
				usdCost = players == 3 ? usdCost = 1.2f : usdCost;
				usdCost = players == 4 ? usdCost = 3.0f : usdCost;

			}
            else
            {
				players = 2;

			}


#if UNITY_EDITOR
			
#else
if (userNameSaves.Length > 0 && players >=1)
		{
			AptiodeManager.instance.StartWalletPurchase(userNameSaves,"1", 120,	usdCost , players);
		}
#endif
		}*/
	}
	public void RateGame()
	{
		SoundManager.instance.PlaybtnSfx ();
        CUtils.OpenStore();
	}

	private void OnEnable()
	{
#if !UNITY_EDITOR
	//	FindObjectOfType<AptiodeManager>().CompletedPurchase += OnCompletedCoinsPurchage;
#endif
	}
	private void OnCompletedCoinsPurchage(Dictionary<string, string> data)
	{
		Debug.Log("OnCompletedCoinsPurchage == ");
		GameManager.UserResData = data;

		Debug.Log("SESSION >>>>>>> " + GameManager.UserResData["SESSION"]);
		Debug.Log("USER_ID >>>>>>> " + GameManager.UserResData["USER_ID"]);
		Debug.Log("ROOM_ID >>>>>>> " + GameManager.UserResData["ROOM_ID"]);
		Debug.Log("WALLET_ADDRESS >>>>>>> " + GameManager.UserResData["WALLET_ADDRESS"]);
		if(data.ContainsKey("ROOM_ID") && data["ROOM_ID"].Length>0)
		GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
        else
        {
			GeneralFunction.intance.LoadSceneWithLoadingScreen("HomeScene");

		}
	}
	void updateGiftStatus()
	{
		if (GameManager.GiftAvalible) {
			giftButton.interactable = true;
			LeanTween.alphaCanvas (giftLableCanvasGroup, 0f, .4f).setOnComplete (() => {
				LeanTween.alphaCanvas (giftLableCanvasGroup, 1f, .4f);
			});
			giftLable.text="READY!";
		} else {
			giftButton.interactable = false;
			giftLable.text = GameManager.RemendingTimeSpanForGift.Hours.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Minutes.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Seconds.ToString("00");
		}
	}
	[ContextMenu("Get Gift")]
	public void OnGiftClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		int Gift = UnityEngine.Random.Range (minGiftApple, maxGiftApple);
        Toast.instance.ShowMessage("You got "+Gift+" Apples");
		GameManager.Apple += Gift;
		GameManager.NextGiftTime = DateTime.Now.AddMinutes(timeForNextGift);

        updateGiftStatus ();
		giftBlackScreen.SetActive (true);
		Instantiate<GameObject>(giftParticle);
		SoundManager.instance.PlaySingle (giftSfx);
		Invoke("HideGiftParticle",2f);
	}
	public void HideGiftParticle()
	{
		giftBlackScreen.SetActive (false);
	}
	public void OpenShopUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		KnifeShop.intance.ShowShop ();	
	}
	public void OpenSettingUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		//howToPlayObj.SetActive(true);	
		SettingUI.intance.UIParent.SetActive(true);
	}
	public bool helpBtnClicked;
	public void HowtoPlayBtnClicked()
    {
		helpBtnClicked = true;
		//howToPlayObj.SetActive(true);
	}
}

