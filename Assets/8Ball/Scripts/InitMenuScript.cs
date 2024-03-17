using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
//using Photon.Chat;
using UnityEngine.SceneManagement;
//using PlayFab.ClientModels;
//using PlayFab;
using System.Collections.Generic;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using AssemblyCSharp;

public class InitMenuScript : MonoBehaviour {

    public GameObject playerName;
    public GameObject videoRewardText;
    public GameObject playerAvatar;
    public GameObject fbFriendsMenu;
    public GameObject matchPlayer;
    public GameObject backButtonMatchPlayers;
    public GameObject MatchPlayersCanvas;
    public GameObject menuCanvas;
    public GameObject tablesCanvas;
    public GameObject gameTitle;
    public GameObject changeDialog;
    public GameObject inputNewName;
    public GameObject tooShortText;
    public GameObject coinsText;
    public GameObject coinsTextShop;
    public GameObject coinsTab;

    public GameObject dialog;
    // Use this for initialization
    void Start() {


        PoolGame_GameManager.Instance.dialog = dialog;
        videoRewardText.GetComponent<Text>().text = "+" + StaticStrings.rewardForVideoAd;
        PoolGame_GameManager.Instance.tablesCanvas = tablesCanvas;
        //GameManager.Instance.facebookFriendsMenu = fbFriendsMenu.GetComponent<FacebookFriendsMenu>(); ;
        PoolGame_GameManager.Instance.matchPlayerObject = matchPlayer;
        PoolGame_GameManager.Instance.backButtonMatchPlayers = backButtonMatchPlayers;
        playerName.GetComponent<Text>().text = PoolGame_GameManager.Instance.nameMy;
        PoolGame_GameManager.Instance.MatchPlayersCanvas = MatchPlayersCanvas;

        if (PoolGame_GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = PoolGame_GameManager.Instance.avatarMy;


        PoolGame_GameManager.Instance.coinsTextMenu = coinsText;
        PoolGame_GameManager.Instance.coinsTextShop = coinsTextShop;
        //GameManager.Instance.playfabManager.updateCoinsTextMenu();
        //GameManager.Instance.playfabManager.updateCoinsTextShop();
        PoolGame_GameManager.Instance.initMenuScript = this;

        if (StaticStrings.hideCoinsTabInShop) {
            coinsTab.SetActive(false);
        }

#if UNITY_WEBGL
        coinsTab.SetActive(false);
#endif
        //coinsText.GetComponent<Text>().text = GameManager.Instance.coinsCount + "";
    }

    // Update is called once per frame
    void Update() {

    }

    //public void showAdStore() 
    //{
           // UnityAds.instance.ShowInterstitialAd();
   // }

    public void backToMenuFromTableSelect() {
        tablesCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        gameTitle.SetActive(true);
    }

    public void showSelectTableScene(bool challengeFriend) {
        if (!challengeFriend)
            PoolGame_GameManager.Instance.inviteFriendActivated = false;
        //if (StaticStrings.showAdOnSelectTableScene)
            //UnityAds.instance.ShowInterstitialAd();
        menuCanvas.SetActive(false);
        tablesCanvas.SetActive(true);
        gameTitle.SetActive(false);
    }

    public void playOffline() {
        PoolGame_GameManager.Instance.tableNumber = 0;
        PoolGame_GameManager.Instance.offlineMode = true;
        PoolGame_GameManager.Instance.roomOwner = true;
        SceneManager.LoadScene("GameScene");
    }

    public void switchUser() {
        //GameManager.Instance.playfabManager.destroy();
        //GameManager.Instance.facebookManager.destroy();
        PoolGame_GameManager.Instance.connectionLost.destroy();
        //GameManager.Instance.adsScript.destroy();
        PoolGame_GameManager.Instance.avatarMy = null;
        //PhotonNetwork.Disconnect();

        PlayerPrefs.DeleteAll();
        PoolGame_GameManager.Instance.resetAllData();
        PoolGame_GameManager.Instance.coinsCount = 0;
        SceneManager.LoadScene("LoginSplash");
    }

    public void showChangeDialog() {
        changeDialog.SetActive(true);
    }

    public void changeUserName() {
        Debug.Log("Change Nickname");

        //string newName = inputNewName.GetComponent<Text>().text;
        //if (newName.Equals(StaticStrings.addCoinsHackString)) {
        //    GameManager.Instance.playfabManager.addCoinsRequest(1000000);
        //    changeDialog.SetActive(false);
        //} else {
        //    if (newName.Length > 0) {
        //        UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest() {
        //            //DisplayName = newName
        //            DisplayName = GameManager.Instance.playfabManager.PlayFabId
        //        };

        //        PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (response) => {
        //            Dictionary<string, string> data = new Dictionary<string, string>();
        //            data.Add("PlayerName", newName);
        //            UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest() {
        //                Data = data,
        //                Permission = UserDataPermission.Public
        //            };

        //            PlayFabClientAPI.UpdateUserData(userDataRequest, (result1) => {
        //                Debug.Log("Data updated successfull ");
        //                Debug.Log("Title Display name updated successfully");
        //                PlayerPrefs.SetString("GuestPlayerName", newName);
        //                PlayerPrefs.Save();
        //                GameManager.Instance.nameMy = newName;
        //                playerName.GetComponent<Text>().text = newName;
        //            }, (error1) => {
        //                Debug.Log("Data updated error " + error1.ErrorMessage);
        //            }, null);

        //        }, (error) => {
        //            Debug.Log("Title Display name updated error: " + error.Error);

        //        }, null);

        //        changeDialog.SetActive(false);
        //    } else {
        //        tooShortText.SetActive(true);
        //    }
        //}



    }

    public void startQuickGame() {
        //GameManager.Instance.facebookManager.startRandomGame();
    }

    public void startQuickGameTableNumer(int tableNumer, int fee) {
        PoolGame_GameManager.Instance.payoutCoins = fee;
        PoolGame_GameManager.Instance.tableNumber = tableNumer;
       // GameManager.Instance.facebookManager.startRandomGame();
    }

    public void showFacebookFriends() 
    {
        //UnityAds.instance.ShowInterstitialAd();
       // GameManager.Instance.playfabManager.GetPlayfabFriends();
    }

    public void setTableNumber() {
        PoolGame_GameManager.Instance.tableNumber = Int32.Parse(GameObject.Find("TextTableNumber").GetComponent<Text>().text);
    }

}
