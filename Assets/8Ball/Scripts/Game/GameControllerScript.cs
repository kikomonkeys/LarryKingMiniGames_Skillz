using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class GameControllerScript : MonoBehaviour {

    private Image imageClock1;
    private Image imageClock2;

    private Animator messageBubble;
    private Text messageBubbleText;

    private int currentImage = 1;

    public float playerTime;

    public GameObject cueController;
    private CueController cueControllerScript;
    public GameObject shotPowerObject;
    private ShotPowerScript shotPowerScript;

    private float messageTime = 0;
    private AudioSource[] audioSources;
    private bool timeSoundsStarted = false;
    public bool isHowtoPlayClicked;
    int loopCount = 0;

    private float waitingOpponentTime = 0;
    public int randTableNum;
    // Use this for initialization
    
    private void Awake()
    {
        //mohith
        //randTableNum = Random.Range(0, 5);
        PlayerPrefs.SetInt("TableNum", PlayerPrefs.GetInt("TableNum") + 1);
        if (PlayerPrefs.GetInt("TableNum") % 2 == 0)
        {
            PoolGame_GameManager.Instance.tableNumber = 0;
        }
        else
        {
            PoolGame_GameManager.Instance.tableNumber = 1;
        }
        PoolGame_GameManager.Instance.offlineMode = true;
        PoolGame_GameManager.Instance.roomOwner = true;
        //mohith
    }
    void Start() {


      

        audioSources = GetComponents<AudioSource>();
        shotPowerScript = shotPowerObject.GetComponent<ShotPowerScript>();
        cueControllerScript = cueController.GetComponent<CueController>();
        playerTime = PoolGame_GameManager.Instance.playerTime;
        imageClock1 = GameObject.Find("AvatarClock1").GetComponent<Image>();
        imageClock2 = GameObject.Find("AvatarClock2").GetComponent<Image>();

        messageBubble = GameObject.Find("MessageBubble").GetComponent<Animator>();
        messageBubbleText = GameObject.Find("BubbleText").GetComponent<Text>();

        if (PoolGame_GameManager.Instance.offlineMode) {
            GameObject.Find("Name1").GetComponent<Text>().text = StaticStrings.offlineModePlayer1Name;
            // if (GameManager.Instance.avatarMy != null)
            //     GameObject.Find("Avatar1").GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

            GameObject.Find("Name2").GetComponent<Text>().text = StaticStrings.offlineModePlayer2Name;
            GameObject.Find("Avatar2").GetComponent<Image>().color = Color.red;

            // if (GameManager.Instance.avatarOpponent != null)
            //     GameObject.Find("Avatar2").GetComponent<Image>().sprite = GameManager.Instance.avatarOpponent;
        } else {
            GameObject.Find("Name1").GetComponent<Text>().text = PoolGame_GameManager.Instance.nameMy;
            if (PoolGame_GameManager.Instance.avatarMy != null)
                GameObject.Find("Avatar1").GetComponent<Image>().sprite = PoolGame_GameManager.Instance.avatarMy;

            GameObject.Find("Name2").GetComponent<Text>().text = PoolGame_GameManager.Instance.nameOpponent;

            if (PoolGame_GameManager.Instance.avatarOpponent != null)
                GameObject.Find("Avatar2").GetComponent<Image>().sprite = PoolGame_GameManager.Instance.avatarOpponent;
        }

        // GameObject.Find ("Name1").GetComponent <Text> ().text = GameManager.Instance.nameMy;
        // if (GameManager.Instance.avatarMy != null)
        //     GameObject.Find ("Avatar1").GetComponent <Image> ().sprite = GameManager.Instance.avatarMy;

        // GameObject.Find ("Name2").GetComponent <Text> ().text = GameManager.Instance.nameOpponent;

        // if (GameManager.Instance.avatarOpponent != null)
        //     GameObject.Find ("Avatar2").GetComponent <Image> ().sprite = GameManager.Instance.avatarOpponent;




        playerTime = playerTime * Time.timeScale;


        if (PoolGame_GameManager.Instance.roomOwner) {
            showMessage(StaticStrings.youAreBreaking);
        } else {
            showMessage(PoolGame_GameManager.Instance.nameOpponent + " " + StaticStrings.opponentIsBreaking);
        }

        if (!PoolGame_GameManager.Instance.roomOwner)
            currentImage = 2;
    }

    // Update is called once per frame
    void Update() {
        if (!PoolGame_GameManager.Instance.stopTimer) {
           // updateClock();//mo
        }
    }


    private void updateClock() {
        float minus;
        if (currentImage == 1) {
            playerTime = PoolGame_GameManager.Instance.playerTime;
            if (PoolGame_GameManager.Instance.offlineMode)
                playerTime = PoolGame_GameManager.Instance.playerTime + PoolGame_GameManager.Instance.cueTime;
            minus = 1.0f / playerTime * Time.deltaTime;

            imageClock1.fillAmount -= minus;

            if (imageClock1.fillAmount < 0.25f && !timeSoundsStarted) {
                audioSources[0].Play();
                timeSoundsStarted = true;
            }

            if (imageClock1.fillAmount == 0) {
                //				imageClock1.fillAmount = 1;
                //				currentImage = 2;
                //				showMessage (GameManager.Instance.nameOpponent + " turn");
                audioSources[0].Stop();
                PoolGame_GameManager.Instance.stopTimer = true;
                shotPowerScript.resetCue();
                if (!PoolGame_GameManager.Instance.offlineMode)
                {
                    //PhotonNetwork.RaiseEvent(9, cueControllerScript.cue.transform.position, true, null);
                }
                else {
                    PoolGame_GameManager.Instance.wasFault = true;
                    PoolGame_GameManager.Instance.cueController.setTurnOffline(true);
                }


                PoolGame_GameManager.Instance.cueController.ShotPowerIndicator.deactivate();
                PoolGame_GameManager.Instance.cueController.ShotPowerIndicator.resetCue();

                PoolGame_GameManager.Instance.cueController.cueSpinObject.GetComponent<SpinController>().hideController();

                PoolGame_GameManager.Instance.cueController.whiteBallLimits.SetActive(false);
                PoolGame_GameManager.Instance.ballHand.SetActive(false);

                showMessage("You " + StaticStrings.runOutOfTime);

                if (!PoolGame_GameManager.Instance.offlineMode) {
                    cueControllerScript.setOpponentTurn();
                }

            }

        } else {
            Debug.Log(PoolGame_GameManager.Instance.opponentCueTime);
            playerTime = PoolGame_GameManager.Instance.playerTime;
            if (PoolGame_GameManager.Instance.offlineMode)
                playerTime = PoolGame_GameManager.Instance.playerTime + PoolGame_GameManager.Instance.opponentCueTime;
            minus = 1.0f / playerTime * Time.deltaTime;
            imageClock2.fillAmount -= minus;

            if (PoolGame_GameManager.Instance.offlineMode && imageClock2.fillAmount < 0.25f && !timeSoundsStarted) {
                audioSources[0].Play();
                timeSoundsStarted = true;
            }

            if (imageClock2.fillAmount == 0) {
                PoolGame_GameManager.Instance.stopTimer = true;

                if (PoolGame_GameManager.Instance.offlineMode) {
                    showMessage("You " + StaticStrings.runOutOfTime);
                } else {
                    showMessage(PoolGame_GameManager.Instance.nameOpponent + " " + StaticStrings.runOutOfTime);
                }

                //				imageClock2.fillAmount = 1;
                //				currentImage = 1;
                //				showMessage ("Your turn");

                if (PoolGame_GameManager.Instance.offlineMode) {
                    PoolGame_GameManager.Instance.wasFault = true;
                    PoolGame_GameManager.Instance.cueController.setTurnOffline(true);
                }
            }
        }

    }

    public void showMessage(string message) {


        //Debug.Log ("Time " + (Time.time - messageTime));
        //        if(Time.time - messageTime > )

        float timeDiff = Time.time - messageTime;

        Debug.Log("Time diff: " + timeDiff);

        if (timeDiff > 6) {
            messageBubbleText.text = message;
            messageBubble.Play("ShowBubble");
            if (!message.Contains(StaticStrings.waitingForOpponent))
                Invoke("hideBubble", 5.0f);
            else {
                waitingOpponentTime = StaticStrings.photonDisconnectTimeout;
                StartCoroutine(updateMessageBubbleText());
            }
            messageTime = Time.time;
        } else {
            Debug.Log("Show message with delay");
            StartCoroutine(showMessageWithDelay(message, (6.0f - timeDiff) / 1.0f));
        }
    }

    public void hideBubble() {
        messageBubble.Play("HideBubble");
    }

    IEnumerator showMessageWithDelay(string message, float delayTime) {
        yield return new WaitForSeconds(delayTime);

        messageBubbleText.text = message;

        messageBubble.Play("ShowBubble");
        if (!message.Contains(StaticStrings.waitingForOpponent))
            Invoke("hideBubble", 5.0f);
        else {
            waitingOpponentTime = StaticStrings.photonDisconnectTimeout;
            StartCoroutine(updateMessageBubbleText());
        }
        messageTime = Time.time;

    }

    public IEnumerator updateMessageBubbleText() {
        yield return new WaitForSeconds(1.0f * 2);
        waitingOpponentTime -= 1;
        if (!PoolGame_GameManager.Instance.opponentDisconnected) {
            if (!messageBubbleText.text.Contains("disconnected from room"))
                messageBubbleText.text = StaticStrings.waitingForOpponent + " " + waitingOpponentTime;
        }
        if (waitingOpponentTime > 0 && !PoolGame_GameManager.Instance.opponentActive && !PoolGame_GameManager.Instance.opponentDisconnected) {
            StartCoroutine(updateMessageBubbleText());
        }
    }

    public void stopSound() {
        audioSources[0].Stop();
    }

    public void resetTimers(int currentTimer, bool showMessageBool) {

        stopSound();
        timeSoundsStarted = false;
        imageClock1.fillAmount = 1;
        imageClock2.fillAmount = 1;

        this.currentImage = currentTimer;

        if (PoolGame_GameManager.Instance.offlineMode) {
            if (showMessageBool) {

                if (currentTimer == 2) {
                    showMessage(StaticStrings.offlineModePlayer2Name + " turn");
                } else {
                    showMessage(StaticStrings.offlineModePlayer1Name + " turn");
                }

            }

        } else {
            if (currentTimer == 1 && showMessageBool) {
                showMessage("It's your turn");
            }
        }




        //        if (currentImage == 1) {
        //            currentImage = 2;
        //        } else {
        //            currentImage = 1;
        //            showMessage("It's your turn");
        //        }

        PoolGame_GameManager.Instance.stopTimer = false;
    }


}
