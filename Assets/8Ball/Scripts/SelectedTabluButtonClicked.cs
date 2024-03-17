using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectedTabluButtonClicked : MonoBehaviour {

    public int tableNumber;
    public int fee;

    // Use this for initialization

    void Start() {
        Debug.Log("start");
        gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        gameObject.GetComponent<Button>().onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void Update() {

    }


    public void startGame() {


        Debug.Log("Fee: " + fee + "  Coins: " + PoolGame_GameManager.Instance.coinsCount);
        if (PoolGame_GameManager.Instance.coinsCount >= fee) {

            if (PoolGame_GameManager.Instance.inviteFriendActivated) {
                PoolGame_GameManager.Instance.tableNumber = tableNumber;
                PoolGame_GameManager.Instance.payoutCoins = fee;
                PoolGame_GameManager.Instance.initMenuScript.backToMenuFromTableSelect();
                //GameManager.Instance.playfabManager.challengeFriend(GameManager.Instance.challengedFriendID, "" + fee + ";" + tableNumber);

            } else {
                PoolGame_GameManager.Instance.tableNumber = tableNumber;
                PoolGame_GameManager.Instance.payoutCoins = fee;
                //GameManager.Instance.facebookManager.startRandomGame();
            }

        } else {
            PoolGame_GameManager.Instance.dialog.SetActive(true);
        }

    }


}
