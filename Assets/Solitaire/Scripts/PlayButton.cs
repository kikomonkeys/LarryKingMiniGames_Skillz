using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
///using PiperAptoidePlugin;
using System;
using TMPro;

public class PlayButton : MonoBehaviour
{
	[Header("Options View")]
	//public InputField userName;
	public GameObject options;
	public GameObject nameSelection, playNow;
	public GameObject howToPlayScreen;
	public string userNameSaves = string.Empty;
	public string imName = "0";
	//public AptiodeManager aptiodeManager;

	public TextMeshProUGUI userNameView;

	public List<GameObject> allSlots = new List<GameObject>();

	public List<string> userNames;
	public List<Sprite> userProfiles;
	public List<TextMeshProUGUI> allnames;
	public List<Image> allProfiles;

	public Sprite userSprite;
	private StatusBar statusBar;
	private void Start()
	{


		//if (aptiodeManager != null)
		//	aptiodeManager.CompletedPurchase += CompletedPurchase;

		if (PlayerPrefs.HasKey("MyName"))
		{
			userNameSaves = PlayerPrefs.GetString("MyName");
			imName = PlayerPrefs.GetString("imName");
			ShowView(imName);
			userSprite = userProfiles[int.Parse(imName)];

			//userNameView.text = userNameSaves;
			Debug.Log("userNameSaves == > " + userNameSaves);
			Debug.Log("imNameimName == > " + imName);
			options.SetActive(true);
			nameSelection.SetActive(false);
			hasName = true;


		}
        else
		{
            //if (userNames.Count > 0) {
			//	userNameSaves = userNames[UnityEngine.Random.Range(0, userNames.Count)];
				//userName.text = userNameSaves;
			//}


			if (allProfiles.Count > 0)
            {
				int pind = UnityEngine.Random.Range(0, allProfiles.Count);
				userSprite = userProfiles[pind];
				imName = pind.ToString();
			}


		}
		//UpdateAllUserData();piper
		//ShowView(imName);piper
		
		
	}
	bool hasName = false;
	public void UpdateAllUserData()
    {
		foreach(TextMeshProUGUI textMesh in allnames)
        {
			textMesh.text = userNameSaves;

		}
		foreach (Image image in allProfiles)
		{
			image.sprite = userSprite;

		}
	}
	public void ShowView( string slotName)
    {
		foreach(GameObject slot in allSlots)
        {
            if (slot.name.Contains(slotName))
            {
				slot.transform.GetChild(1).gameObject.SetActive(true);
				imName = slotName.ToString();
			}
            else
            {
                slot.transform.GetChild(1).gameObject.SetActive(false);
            }

        }
    }

	float usdCost;
	float prizeMoney;
	int playersCount = 0;
	string leaugeName = string.Empty;


	private void CompletedPurchase(Dictionary<string, string> data)
	{
        if (!PlayerPrefs.HasKey("Games"))
        {
			PlayerPrefs.SetString("Games", data["ROOM_ID"]);

			string gameData = leaugeName + "," + usdCost + "," + prizeMoney + "," + leaugeName;

			PlayerPrefs.SetString(data["ROOM_ID"], gameData);

		}
        {
			string games =  PlayerPrefs.GetString("Games");
			games = games + data["ROOM_ID"];
			PlayerPrefs.SetString("Games", games);
			string gameData = leaugeName + "," + usdCost + "," + prizeMoney + "," + leaugeName;

			PlayerPrefs.SetString(data["ROOM_ID"], gameData);

		}
		if (data != null && data.Count > 0 && data.ContainsKey("ROOM_ID"))
			SceneManager.LoadScene("Splash");
        else
        {
			SceneManager.LoadScene("Solitaire_Temp");

		}

	}
	public GameObject dummybg;
	public void PLayGame()
	{
		//Debug.Log("username::" + PlayerPrefs.GetString("MyName"));
		//PlayerPrefs.SetString("MyName", "Mohith");//mo

		if (PlayerPrefs.HasKey("MyName"))
		{
			userNameSaves = PlayerPrefs.GetString("MyName");
			imName = PlayerPrefs.GetString("imName");
			Debug.Log("userNameSaves == > " + userNameSaves);
			options.SetActive(true);
			nameSelection.SetActive(false);

			playNow.SetActive(false);

		}
		else
		{
			//Debug.Log("firsttime::" + PlayerPrefs.GetString("firstTime"));

			if (PlayerPrefs.GetInt("firstTime") == 0)
			{
				howToPlayScreen.SetActive(true);
			}
			else
			{
				//options.SetActive(false);
				//nameSelection.SetActive(true);

				//Debug.Log("start game");
				//SceneManager.LoadScene("Stage");
				dummybg.SetActive(true);

				StageManager.instance.EnableCountDown(0f);
				gameObject.SetActive(false);
				//StageManager.instance.InitViewer();
				Invoke(nameof(InitViewer), 3f);
				return;
			}
			//if (userName.text.Length >= 0)
            //{
                //PlayerPrefs.SetString("MyName", userName.text);
               // PlayerPrefs.SetString("imName", imName);

               // userNameSaves = userName.text;
                //Debug.Log("userNameSaves == > " + userNameSaves);
                //Debug.Log("imNameimName == > " + imName);
				//UpdateAllUserData();

				//userNameView.text = userName.text;
			//}
			//options.SetActive(false);
			//nameSelection.SetActive(true);

		}
		//SceneManager.LoadScene("Splash");
	}
	void InitViewer()
    {
		dummybg.SetActive(false);

		StageManager.instance.InitViewer();
	}
	public static bool rulesBtnClicked;
	public void RulesBtnClicked()
    {
		rulesBtnClicked = true;
		howToPlayScreen.SetActive(true);
    }
	public void OnPlayClick(int players)
	{
		playersCount = players;


		if (PlayerPrefs.HasKey("MyName"))
		{
			userNameSaves = PlayerPrefs.GetString("MyName");
			imName = PlayerPrefs.GetString("imName");


		}
        else
        {
			return;

        }
		//if (userName.text.Length >= 0)
		//{
		//	PlayerPrefs.SetString("MyName", userName.text);
		//	PlayerPrefs.SetString("imName", imName);

		//	userNameSaves = userName.text;
		//	Debug.Log("userNameSaves == > " + userNameSaves);
		//	Debug.Log("imNameimName == > " + imName);

		//	//userNameView.text = userName.text;
		//}
		//if (userNameSaves.Length > 0)
		//{			PlayerPrefs.SetString("MyName", userName.text);
		//	userName.transform.gameObject.SetActive(false);
		//	options.SetActive(true);
		//	nameSelection.SetActive(false);

		//}

		if (userNameSaves.Length <= 0)
		{
			return;
		}

		//GameManager.Stage = players == 1 ? 1 : 1;
		//GameManager.Stage = players == 2 ? 10 : GameManager.Stage;
		//GameManager.Stage = players == 3 ? 20 : GameManager.Stage;
		//GameManager.Stage = players == 4 ? 30 : GameManager.Stage;




		//if (userName.text.Length > 0 && players <= 1)
		//	GeneralFunction.intance.LoadSceneWithLoadingScreen("GameScene");
		//else
		{

			if (players >= 2)
			{
				usdCost = players == 2 ? usdCost = 1.0f : usdCost;
				usdCost = players == 200 ? usdCost = 4.0f : usdCost;
				usdCost = players == 3 ? usdCost = 4.0f : usdCost;
				usdCost = players == 4 ? usdCost = 10.0f : usdCost;

			}
			else
			{
				players = 2;

			}
			if(players == 2)
            {
				leaugeName = "Duel";

			}
			if (players == 200)
			{
				leaugeName = "Duel Pro";

			}
			if (players == 3)
			{
				leaugeName = "Multiplayer";

			}
			if (players == 4)
			{
				leaugeName = "Expert";

			}
			if (players == 200) players = 2;
			  prizeMoney = (usdCost * players) * 0.75f;
           // if (AptiodeManager.instance)
            //{
			//	AptiodeManager.instance.prizeMoney = prizeMoney;

			//}
#if UNITY_EDITOR
			SceneManager.LoadScene("Splash");
#else
			//if (userNameSaves.Length > 0 && players >= 1)
			//{
			//	AptiodeManager.instance.StartWalletPurchase(userNameSaves, "1", 120, usdCost, players);
			//}
#endif
		}
	}
	
}
