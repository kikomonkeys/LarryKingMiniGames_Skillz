using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class YouWinMessageChangeSprite : MonoBehaviour {

    public Sprite other;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void changeSprite() {
        GetComponent<Image>().sprite = other;
    }

    public void loadWinnerScene() {
        if (PoolGame_GameManager.Instance.offlineMode) {
           // GameManager.Instance.playfabManager.roomOwner = false;
            PoolGame_GameManager.Instance.roomOwner = false;
            PoolGame_GameManager.Instance.resetAllData();
            SceneManager.LoadScene("Menu");
            Debug.Log("Timeout 7");
            //PhotonNetwork.BackgroundTimeout = 0;
            //if (GameManager.Instance.offlineMode && StaticStrings.showAdWhenLeaveGame)
                //UnityAds.instance.ShowInterstitialAd();

        } else {
            SceneManager.LoadScene("WinnerScene");
        }

    }
}
