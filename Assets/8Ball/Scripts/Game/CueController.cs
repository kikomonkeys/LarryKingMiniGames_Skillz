using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class CueController : MonoBehaviour
{

    public GameObject cue;
    public GameObject posDetector;

    public UnityEngine.UI.Text text;

    private Rigidbody myRigidbody;
    public bool isServer;
    private LineRenderer lineRenderer;
    private LineRenderer lineRenderer2;
    private LineRenderer lineRenderer3;
    public GameObject ShotPowerInd;
    public ShotPowerScript ShotPowerIndicator;
    public GameObject targetLine;
    private GameObject circle;
    private RaycastHit hitInfo = new RaycastHit();
    private Ray ray = new Ray();
    public LayerMask layerMask;
    private Vector3 endPosition;
    private float radius;
    private bool displayTouched = false;
    private bool isFirstTouch = false;
    public int steps = 0;
    public bool shouldShot = false;
    private bool firstShotDone = false;
    public int fixedCounter = 0;
    public bool startShoot = false;
    public bool dataSent = false;
    private bool canCount;
    public bool ballsInMovement = false;
    public bool canRun = false;
    private ArrayList multiplayerDataArray = new ArrayList();
    private bool startMovement;
    private int counterFixPositionMulti = 0;
    private Vector3 multiFirstShotPower;
    private Vector3 multiFirstShotDirection;
    private Vector3 circleShotPos = Vector3.zero;
    private Vector3 initShotPos = Vector3.zero;
    private bool firstShot = true;
    int updateCount = 0;
    private Vector3 shotDirection;
    private bool ballCollideFirst;
    public Vector3 trickShotAdd = Vector3.zero;
    public bool spinShowed;
    public GameObject cueSpinObject;
    public bool opponentShotStart = false;
    public bool opponentBallsStoped = false;
    public bool shotMyTurnDone = false;
    private bool raisedSixEvent = false;
    public GameObject gameControllerObject;
    private GameControllerScript gameControllerScript;
    private bool movingWhiteBall = false;
    public GameObject whiteBallLimits;
    public GameObject ballHand;
    public GameObject youWonMessage;
    public PotedBallsGUIController potedBallsGUI;
    public GameObject whiteBallTrigger;
    public GameObject[] callPocketsButtons;
    public GameObject[] calledPockets;
    private bool canShowControllers = true;
    public GameObject prizeText;
    private AudioSource[] audioSources;
    public GameObject audioController;
    public GameObject invitiationDialog;
    public GameObject wrongBall;
    public Sprite[] cueTextures;
    public GameObject chatButton;
    public bool opponentResumed = false;
    private Vector3 touchMousePos;
    void Start()
    {



        if(PoolGame_GameManager.Instance.roomOwner) {
            changeCueImage(PoolGame_GameManager.Instance.cueIndex);
        } else {
            changeCueImage(PoolGame_GameManager.Instance.opponentCueIndex);
        }

        if(PoolGame_GameManager.Instance.offlineMode) {
            chatButton.SetActive(false);
        }
        


        Debug.Log("Minus Coins");

        

        //if(!GameManager.Instance.offlineMode)
        //    GameManager.Instance.playfabManager.addCoinsRequest(-GameManager.Instance.payoutCoins);


        PoolGame_GameManager.Instance.audioSources = audioController.GetComponents<AudioSource>();
        audioSources = GetComponents<AudioSource>();

        PoolGame_GameManager.Instance.iWon = false;
        PoolGame_GameManager.Instance.iLost = false;


        setPrizeText();

        potedBallsGUI = GameObject.Find("PotedBallsGUI").GetComponent<PotedBallsGUIController>();
        PoolGame_GameManager.Instance.ballHand = ballHand;
        PoolGame_GameManager.Instance.cueController = this;
        ShotPowerIndicator = ShotPowerInd.GetComponent<ShotPowerScript>();
        gameControllerScript = gameControllerObject.GetComponent<GameControllerScript>();
        PoolGame_GameManager.Instance.gameControllerScript = gameControllerScript;
        circle = GameObject.Find("Circle");
        targetLine = GameObject.Find("TargetLine");
        lineRenderer = GameObject.Find("Line").GetComponent<LineRenderer>();
        lineRenderer2 = GameObject.Find("Line2").GetComponent<LineRenderer>();
        lineRenderer3 = GameObject.Find("Line3").GetComponent<LineRenderer>();

        targetLine.SetActive(false);
        // Configure when target lines will be invisible - table number
        if (PoolGame_GameManager.Instance.tableNumber > 6)
        {
            PoolGame_GameManager.Instance.showTargetLines = false;
        }
        else
        {
            PoolGame_GameManager.Instance.showTargetLines = true;
        }


        // Configure when calling pocket for black ball is required - table number
        if (PoolGame_GameManager.Instance.tableNumber > 1)
        {
            PoolGame_GameManager.Instance.callPocketBlack = true;
        }
        else
        {
            PoolGame_GameManager.Instance.callPocketBlack = false;
        }


        // Configure when calling pocket for all balls is required - table number
        if (PoolGame_GameManager.Instance.tableNumber > 3)
        {
            PoolGame_GameManager.Instance.callPocketAll = true;
        }
        else
        {
            PoolGame_GameManager.Instance.callPocketAll = false;
        }

        if (!PoolGame_GameManager.Instance.showTargetLines)
        {
            lineRenderer2.enabled = false;
            lineRenderer3.enabled = false;
        }

        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;


        myRigidbody = GetComponent<Rigidbody>();

        // Set cue position to ball position
        Vector3 cueInitialPos = transform.position;
        cueInitialPos.x = cueInitialPos.x;
        cueInitialPos.z = cue.transform.position.z;
        cue.transform.position = cueInitialPos;

        isServer = false;




        if (PoolGame_GameManager.Instance.roomOwner)
        {
            if (!ballHandShownOnce)//mohith
            {
                ballHand.SetActive(true);
                PoolGame_GameManager.Instance.hasCueInHand = true;
            }
            //ballHand.SetActive(true);//mohith
            //GameManager.Instance.hasCueInHand = true;//mohith
            isServer = true;
            opponentBallsStoped = true;
            PoolGame_GameManager.Instance.audioSources[0].Play();
        }

        PoolGame_GameManager.Instance.audioSources[1].Play();



        canCount = false;

        //drawTargetLines();

    }
    public bool ballHandShownOnce;
    /// <summary>
    /// Callback sent to all game objects when the player pauses.
    /// </summary>
    /// <param name="pauseStatus">The pause state of the application.</param>
    void OnApplicationPause(bool pauseStatus)
    {
        //if (pauseStatus) {
        //    PhotonNetwork.RaiseEvent(151, 1, true, null);

        //    PhotonNetwork.SendOutgoingCommands();
        //    Debug.Log("Application pause");
        //} else {
        //    PhotonNetwork.RaiseEvent(152, 1, true, null);
        //    PhotonNetwork.SendOutgoingCommands();
        //    Debug.Log("Application resume");
        //}
    }

    public void changeCueImage(int index) {
        cue.GetComponent<SpriteRenderer>().sprite = cueTextures[index];
    }

    private void setPrizeText()
    {

        

        int prizeCoins = PoolGame_GameManager.Instance.payoutCoins * 2;

        if (prizeCoins >= 1000)
        {
            if (prizeCoins >= 1000000)
            {
                if (prizeCoins % 1000000.0f == 0)
                {
                    prizeText.GetComponent<Text>().text = (prizeCoins / 1000000.0f).ToString("0") + "M";

                }
                else
                {
                    prizeText.GetComponent<Text>().text = (prizeCoins / 1000000.0f).ToString("0.0") + "M";

                }

            }
            else
            {
                if (prizeCoins % 1000.0f == 0)
                {
                    prizeText.GetComponent<Text>().text = (prizeCoins / 1000.0f).ToString("0") + "k";
                }
                else
                {
                    prizeText.GetComponent<Text>().text = (prizeCoins / 1000.0f).ToString("0.0") + "k";
                }

            }
        }
        else
        {
            prizeText.GetComponent<Text>().text = prizeCoins + "";
        }

        if(PoolGame_GameManager.Instance.offlineMode) {
            prizeText.GetComponent<Text>().text = "Practice";
        }
    }




    void Awake()
    {
       // PhotonNetwork.OnEventCall += this.OnEvent;

    }

    public void removeOnEventCall()
    {
       // PhotonNetwork.OnEventCall -= this.OnEvent;
    }


    void Update()
    {
        //mohith
        //Debug.LogError("startPotCheckTimer::" + startPotCheckTimer);
        if (startPotCheckTimer)
        {
            ScoreController.instance.canCheckForPotedBalls = true;
        }
        else
        {
            ScoreController.instance.canCheckForPotedBalls = false;
        }
        //mohith

        if (!spinShowed && canCheckAnotherCueRotation)
            StartCoroutine(rotateCue());
        

        //

        if(!isServer) {
            drawTargetLines();
        }



        if (Input.GetMouseButtonDown(0))
        {
            steps = 0;
            isFirstTouch = true;
            displayTouched = true;
            touchMousePos = Vector3.zero;
        }


        if (Input.GetMouseButtonUp(0))
        {
            //isServer = true;
            canRun = true;
            displayTouched = false;
        }
        //mohith
        if (canRotateBalls)
        {

            RotateBallsToFaceForward();
        }
    }

    void FixedUpdate()
    {
        checkFirstCollision();
        shotOpponentTurn();
        shotMyTurn();

    }

    private void checkFirstCollision()
    {
        //have we moved more than our minimum extent? 

        if (ballCollideFirst && gameObject.layer == 11)
        {
            RaycastHit hitInfo;


            if (Physics.SphereCast(initShotPos, radius, shotDirection, out hitInfo, Vector3.Distance(initShotPos, transform.position), layerMask.value))
            {

                if (!hitInfo.transform.tag.Equals(transform.tag))
                {

                    if (!hitInfo.collider)
                        return;

                    if (hitInfo.collider.isTrigger)
                    {
                        //hitInfo.collider.SendMessage ("OnTriggerEnter", myCollider);

                    }

                    if (!hitInfo.collider.isTrigger)
                    {
                        Debug.Log("fix pos");
                        //
                        //						Vector3 vel = myRigidbody.velocity;
                        //						Vector3 angVel = myRigidbody.angularVelocity;
                        //						myRigidbody.Sleep ();
                        ballHandShownOnce = true;//mohith
                        Vector3 fixedPos = circleShotPos;
                        fixedPos.z = transform.position.z;
                        myRigidbody.transform.position = fixedPos;
                        gameObject.layer = 8;
                        //						myRigidbody.velocity = vel;
                        //						myRigidbody.angularVelocity = angVel;
                    }

                }
            }
        }
    }

    public void callPocket()
    {
        PoolGame_GameManager.Instance.gameControllerScript.showMessage(StaticStrings.callPocket);
        for (int i = 0; i < callPocketsButtons.Length; i++)
        {
            callPocketsButtons[i].SetActive(true);
            callPocketsButtons[i].GetComponent<Animator>().Play("CallPocketShowAnimation");
        }
    }

    public void hidePocketButtons()
    {
        for (int i = 0; i < callPocketsButtons.Length; i++)
        {
            callPocketsButtons[i].SetActive(false);
        }
    }

    bool firstTime = true;
    private void shotMyTurn()
    {

        //		if (!initShotPos.Equals (Vector3.zero) && !circleShotPos.Equals (Vector3.zero)) {
        //			if (Vector3.Distance (transform.position, initShotPos) > Vector3.Distance (initShotPos, circleShotPos) * 0.5f) {
        //				Debug.Log ("Aa");
        //				//myRigidbody.Sleep ();
        //				gameObject.layer = 8;
        //				//GetComponent <SphereCollider> ().isTrigger = false;
        //				Vector3 fixedPos = circleShotPos;
        //				fixedPos.z = transform.position.z;
        //				myRigidbody.transform.position = fixedPos;
        //				initShotPos = Vector3.zero;
        //				circleShotPos = Vector3.zero;
        //			}
        //		}

        // Shot when its your turn

        if (PoolGame_GameManager.Instance.opponentActive && !PoolGame_GameManager.Instance.stopTimer && shouldShot && steps > 0 && isServer)
        {
            startPotCheckTimer = true;//mohith
            shotMyTurnDone = true;
            //GameManager.Instance.iWon = true;

            for (int i = 0; i < PoolGame_GameManager.Instance.balls.Length; i++)
            {
                PoolGame_GameManager.Instance.balls[i].GetComponent<Rigidbody>().maxAngularVelocity = 150;
            }

            // GameManager.Instance.whiteBall.GetComponent<Rigidbody>().maxAngularVelocity = 150;
            // Debug.Log(GameManager.Instance.whiteBall.GetComponent<Rigidbody>().maxAngularVelocity + " MAX ANG");

            startShoot = false;

            opponentBallsStoped = false;

            myRigidbody.velocity = Vector3.zero;
            Vector3 dir = transform.position - posDetector.transform.position;
            dir = dir.normalized;
            dir.z = 0;

            Vector3 trickShot = transform.position;
            trickShot = trickShot + trickShotAdd;
            //			trickShot.z -= 0.01f;
            //			trickShot.z += 0.5f;
            //			trickShot.z -= 1.0f;

            //			float multipleBy = 0.7f;
            //			if (firstShot) {
            //				multipleBy = 1f;
            //				firstShot = false;
            //			} 

            float randNum;//mo

            if (firstTime)
            {
                randNum = UnityEngine.Random.Range(0.03f, 0.04f);//mo
                firstTime = false;
            }
            else
            {
                randNum = 0.03f;
            }
            Debug.Log("randnum::" + randNum);

            float multipleBy = randNum + (PoolGame_GameManager.Instance.cuePower + 1) * 0.001f;



            Vector3 shotPower = dir * steps * multipleBy;

            if (ballCollideFirst)
                gameObject.layer = 11;

            initShotPos = transform.position;
            circleShotPos = circle.transform.position;
            shotDirection = dir;


            float velSum = Mathf.Abs(shotPower.x) + Mathf.Abs(shotPower.y) + Mathf.Abs(shotPower.z);
            audioSources[audioSources.Length - 1].volume = velSum / 1.8f;
            audioSources[audioSources.Length - 1].Play();

            gameControllerScript.stopSound();

            

            myRigidbody.AddForceAtPosition(shotPower, trickShot, ForceMode.Impulse);
            ballsInMovement = true;
            ShotPowerScript.instance.HidePowerBar();//mohith

            PoolGame_GameManager.Instance.hasCueInHand = false;



            byte evCode = 0;
            Vector3[] content = new Vector3[] { shotPower, trickShot };
            //if (!GameManager.Instance.offlineMode)
            //    PhotonNetwork.RaiseEvent(evCode, content, true, null);

            targetLine.SetActive(false);

            PoolGame_GameManager.Instance.stopTimer = true;

            //cue.GetComponent<Renderer> ().enabled = false;
            dataSent = true;
            shouldShot = false;
        }
    }


    private void shotOpponentTurn()
    {

        // Begin movement after multiplayer data is received
        if (startMovement)
        {
            if (!firstShotDone)
            {
                //                myRigidbody.AddForceAtPosition (multiFirstShotPower, multiFirstShotDirection, ForceMode.Impulse);
                firstShotDone = true;
                float velSum = Mathf.Abs(multiFirstShotPower.x) + Mathf.Abs(multiFirstShotPower.y) + Mathf.Abs(multiFirstShotPower.z);
                audioSources[audioSources.Length - 1].volume = velSum / 1.8f;
                audioSources[audioSources.Length - 1].Play();
                gameControllerScript.stopSound();
            }
            else
            {
                //if (fixedCounter == 1) {
                if (fixedCounter == 1)
                {


                    if (multiplayerDataArray.Count > 0)
                    {


                        Vector3[] balls = (Vector3[])multiplayerDataArray[0];
                        multiplayerDataArray.RemoveAt(0);
                        GameObject spawnBalls = GameObject.Find("SpawnBalls");
                        SpawnBallsScript script = spawnBalls.GetComponent<SpawnBallsScript>();
                        for (int i = 0; i <= balls.Length - 3; i += 3)
                        {

                            if (!isServer /*|| shotMyTurnDone == false*/)
                            {
                                float dist = Vector3.Distance(balls[i], script.balls[i / 3].transform.position);

                                script.balls[i / 3].GetComponent<Rigidbody>().velocity = balls[i + 1];
                                script.balls[i / 3].GetComponent<Rigidbody>().angularVelocity = balls[i + 2];
                                script.balls[i / 3].transform.position = balls[i];
                            }

                        }

                    }
                    else
                    {

                        startMovement = false;

                    }


                }

                if (canCount)
                    fixedCounter++;
                if (fixedCounter > 15)
                    fixedCounter = 0;
            }
        }
    }


    void OnDestroy()
    {
        //PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    // Multiplayer data received
    private void OnEvent(byte eventcode, object content, int senderid)
    {

        Debug.Log("isServer: " + isServer + "  code: " + eventcode);
        if (!isServer && eventcode == 0)
        {
            Vector3[] data = (Vector3[])content;
            multiFirstShotPower = data[0];
            multiFirstShotDirection = data[1];
            firstShotDone = false;
            multiplayerDataArray.Clear();
        }
        else if (!isServer && eventcode == 1)
        {
            Debug.Log("1");
            Vector3[] balls = (Vector3[])content;

            cue.GetComponent<Renderer>().enabled = true;

            GameObject spawnBalls = GameObject.Find("SpawnBalls");
            SpawnBallsScript script = spawnBalls.GetComponent<SpawnBallsScript>();

            //transform.position = balls [0];

            Debug.Log("Received positions x-" + balls[0].x + "  y-" + balls[0].y);

            for (int i = 0; i < balls.Length; i++)
            {
                script.balls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                script.balls[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                //script.balls [i].transform.position = Vector3.Lerp(balls [i], script.balls [i].transform.position, 0.5f);
                script.balls[i].transform.position = balls[i];
                //script.setBallTransform (i, balls[i].transform.position);
            }
        }
        else if (!isServer && eventcode == 2)
        {
            Debug.Log("2");



            cue.GetComponent<Renderer>().enabled = false;



            GameObject spawnBalls = GameObject.Find("SpawnBallsScript");
            SpawnBallsScript script = spawnBalls.GetComponent<SpawnBallsScript>();
            Vector3[] balls = (Vector3[])content;
            for (int i = 0; i <= balls.Length - 3; i += 3)
            {

                float dist = Vector3.Distance(balls[i], script.balls[i / 3].transform.position);
                script.balls[i / 3].GetComponent<Rigidbody>().velocity = balls[i + 1];
                script.balls[i / 3].GetComponent<Rigidbody>().angularVelocity = balls[i + 2];
                script.balls[i / 3].transform.position = balls[i];

            }


        }
        else if (!isServer && eventcode == 4)
        {
            Debug.Log("3");



            cue.GetComponent<Renderer>().enabled = true;



            GameObject spawnBalls = GameObject.Find("SpawnBallsScript");
            SpawnBallsScript script = spawnBalls.GetComponent<SpawnBallsScript>();
            Vector3[] balls = (Vector3[])content;


            multiplayerDataArray = (ArrayList)content;

        }
        else if (!isServer && eventcode == 5)
        {
            multiplayerDataArray.Add((Vector3[])content);
            counterFixPositionMulti++;
            PoolGame_GameManager.Instance.stopTimer = true;

            if (counterFixPositionMulti == 20)
            {
                Debug.Log("AAAA");
                raisedSixEvent = false;
                startMovement = true;
                opponentShotStart = true;
                cue.GetComponent<Renderer>().enabled = false;
                targetLine.SetActive(false);
                ballsInMovement = true;
                canCount = true;
            }
        }
        else if (!isServer && eventcode == 6)
        {

            counterFixPositionMulti = 0;
            multiplayerDataArray.Add((Vector3[])content);


        }
        else if (!isServer && eventcode == 7)
        { // Opponent rotated cue
            cue.transform.rotation = (Quaternion)content;
        }
        else if (!isServer && eventcode == 8)
        { // Shot power - move main cue
            cue.transform.position = (Vector3)content;
        }
        else if (!isServer && eventcode == 9)
        { // My turn - show cue and lines


            cue.transform.position = (Vector3)content;


            StartCoroutine(setMyTurn(true));


        }
        else if (!isServer && eventcode == 12)
        { // Opponents turn - no fault


            cue.transform.position = (Vector3)content;
            setOpponentTurn();

        }
        else if (isServer && eventcode == 10)
        { // Opponents balls stoped movement. Can show cue and lines

            

            checkShot();
        }
        else if (!isServer && eventcode == 11)
        { // Cue spin controller changed value
            cueSpinObject.GetComponent<SpinController>().changePositionOpponent((Vector3)content);
        }
        else if (eventcode == 13)
        { // Fault message
            string message = (string)content;

            //if (message.Equals (StaticStrings.potedCueBall)) {
            if (!ballHandShownOnce)
            {
                ballHand.SetActive(true);
                PoolGame_GameManager.Instance.hasCueInHand = true;
            }
            //ballHand.SetActive(true);
            //GameManager.Instance.hasCueInHand = true;
            //}

            if (message.Equals(StaticStrings.invalidStrike) || (!PoolGame_GameManager.Instance.ballsStriked && message.Equals(StaticStrings.faultCueBallDidntStrike)))
            {

                getNewWhiteBallPosition(false);
            }
            else
            {
                if (message.Equals(StaticStrings.potedCueBall))
                {


                    getNewWhiteBallPosition(true);
                }
            }

            Vector3 newBallPos = PoolGame_GameManager.Instance.whiteBall.transform.position;
            newBallPos.z = ballHand.transform.position.z;
            ballHand.transform.position = newBallPos;





            if (message.Contains("You"))
            {
                message = message.Replace("You", PoolGame_GameManager.Instance.nameOpponent);
            }




            PoolGame_GameManager.Instance.gameControllerScript.showMessage(message);
            PoolGame_GameManager.Instance.faultMessage = "";
        }
        else if (eventcode == 14)
        { // Opponent moving white ball before strike - show limits
            whiteBallLimits.SetActive(true);
            HideAllControllers();
        }
        else if (eventcode == 15)
        { // Opponent moving white ball
            transform.position = (Vector3)content;
        }
        else if (eventcode == 16)
        { // Opponent stoped moving white ball - hide limits
            whiteBallLimits.SetActive(false);
            ShowAllControllers();
        }
        else if (eventcode == 17)
        { // Opponent moving white ball after strike - hide controllers
            HideAllControllers();
        }
        else if (eventcode == 18)
        { // Balls was striked correctly
            PoolGame_GameManager.Instance.ballsStriked = true;
        }
        else if (eventcode == 19)
        { // Opponent Won!
            HideAllControllers();
            //PoolGame_GameManager.Instance.audioSources[3].Play();
            youWonMessage.SetActive(true);
            youWonMessage.GetComponent<YouWinMessageChangeSprite>().changeSprite();
            youWonMessage.GetComponent<Animator>().Play("YouWinMessageAnimation");
            PoolGame_GameManager.Instance.iWon = false;
        }
        else if (eventcode == 20)
        { // Opponent Poted 8 ball - you won!
            HideAllControllers();
            //PoolGame_GameManager.Instance.audioSources[3].Play();
            youWonMessage.SetActive(true);
            youWonMessage.GetComponent<Animator>().Play("YouWinMessageAnimation");
            PoolGame_GameManager.Instance.iWon = true;
        }
        else if (!isServer && eventcode == 21)
        { // Player got types
            checkBallsTypes(false, false, (bool)content);
        }
        else if (!isServer && eventcode == 22)
        { // Opponent called pocket
            PoolGame_GameManager.Instance.gameControllerScript.showMessage(PoolGame_GameManager.Instance.nameOpponent + " " + StaticStrings.opponentCalledPocket);
            calledPockets[(int)content].SetActive(true);
        }
        else if (eventcode == 192)
        {
            //invitiationDialog.GetComponent<PhotonChatListener2>().showInvitationDialog(null, null, null);
        }
        else if (eventcode == 151)
        {
            if(isServer)
                ShotPowerIndicator.anim.Play("ShotPowerAnimation");
            PoolGame_GameManager.Instance.opponentActive = false;
            PoolGame_GameManager.Instance.stopTimer = true;
            PoolGame_GameManager.Instance.gameControllerScript.showMessage(StaticStrings.waitingForOpponent + " " + StaticStrings.photonDisconnectTimeout );
        }
        else if (eventcode == 152)
        {
            if(canShowControllers && isServer && !shotMyTurnDone)
                ShotPowerIndicator.anim.Play("MakeVisible");
            PoolGame_GameManager.Instance.opponentActive = true;

            if((isServer && !shotMyTurnDone) || !isServer)
                PoolGame_GameManager.Instance.stopTimer = false;
            PoolGame_GameManager.Instance.gameControllerScript.hideBubble();
            opponentResumed = true;
        }
        
    }
    List<String> collides = new List<String>();
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag.Contains("Ball"))
            collides.Add(other.tag);
    }


    public void checkShot() {
        
        if (PoolGame_GameManager.Instance.iWon)
            {

                if(!PoolGame_GameManager.Instance.wasFault) {
                    HideAllControllers();
                    //PoolGame_GameManager.Instance.audioSources[3].Play();
                    youWonMessage.SetActive(true);
                    youWonMessage.GetComponent<Animator>().Play("YouWinMessageAnimation");
                    //if (!GameManager.Instance.offlineMode)
                    //    PhotonNetwork.RaiseEvent(19, null, true, null);
                } else {
                    HideAllControllers();
                    //PoolGame_GameManager.Instance.audioSources[3].Play();
                    youWonMessage.SetActive(true);
                    youWonMessage.GetComponent<YouWinMessageChangeSprite>().changeSprite();
                    youWonMessage.GetComponent<Animator>().Play("YouWinMessageAnimation");
                    //if (!GameManager.Instance.offlineMode)
                    //    PhotonNetwork.RaiseEvent(20, null, true, null);
                }
                
            }
            else if (PoolGame_GameManager.Instance.iLost)
            {
                HideAllControllers();
                //PoolGame_GameManager.Instance.audioSources[3].Play();
                youWonMessage.SetActive(true);
                youWonMessage.GetComponent<YouWinMessageChangeSprite>().changeSprite();
                youWonMessage.GetComponent<Animator>().Play("YouWinMessageAnimation");
                //if (!GameManager.Instance.offlineMode)
                //    PhotonNetwork.RaiseEvent(20, null, true, null);
            }
            else
            {
                bool changedWhitePos = false;




                if (!PoolGame_GameManager.Instance.ballsStriked && !PoolGame_GameManager.Instance.validPot && PoolGame_GameManager.Instance.ballTouchBeforeStrike.Count < 4)
                {
                    PoolGame_GameManager.Instance.wasFault = true;
                    //if (GameManager.Instance.faultMessage.Length == 0)
                    PoolGame_GameManager.Instance.faultMessage = StaticStrings.invalidStrike;

                    //if (GameManager.Instance.ballTouchBeforeStrike.Count > 0) { // tutaj bylo 0
                    PoolGame_GameManager.Instance.ballsStriked = true;
                    //if (!GameManager.Instance.offlineMode)
                    //    PhotonNetwork.RaiseEvent(18, 1, true, null);
                    //}
                    PoolGame_GameManager.Instance.ballTouchBeforeStrike.Clear();


                    if (!changedWhitePos)
                    {
                        //                        GameManager.Instance.whiteBall.GetComponent<Rigidbody> ().transform.position = new Vector3 (-2.9f, -0.69f, -0.24f);
                        //                        GameManager.Instance.whiteBall.GetComponent<Rigidbody> ().velocity = Vector3.zero;
                        //                        GameManager.Instance.whiteBall.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
                        //                        GameManager.Instance.whiteBall.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ;
                        //                        GameManager.Instance.whiteBall.GetComponent<LockZPosition> ().ballActive = true;
                        //                        GameManager.Instance.whiteBall.SetActive (true);
                        getNewWhiteBallPosition(false);
                    }
                    //GameManager.Instance.whiteBall.transform.position = new Vector3 (-2.9f, -0.69f, -0.24f);


                }
                else
                {
                    //if (!GameManager.Instance.ballsStriked && GameManager.Instance.firstBallTouched) {
                    PoolGame_GameManager.Instance.ballsStriked = true;
                    //if (!GameManager.Instance.offlineMode)
                    //    PhotonNetwork.RaiseEvent(18, 1, true, null);
                    //}

                    checkBallsTypes(PoolGame_GameManager.Instance.wasFault, true, false);

                    if (!PoolGame_GameManager.Instance.firstBallTouched)
                    {
                        PoolGame_GameManager.Instance.wasFault = true;
                        if (PoolGame_GameManager.Instance.faultMessage.Length == 0)
                            PoolGame_GameManager.Instance.faultMessage = StaticStrings.faultCueBallDidntStrike;

                    }

                    if (PoolGame_GameManager.Instance.firstBallTouched && PoolGame_GameManager.Instance.ballTouchedBand == 0)
                    {
                        PoolGame_GameManager.Instance.wasFault = true;
                        if (PoolGame_GameManager.Instance.faultMessage.Length == 0)
                            PoolGame_GameManager.Instance.faultMessage = StaticStrings.invalidShotNoBandContact;

                    }

                    if (!PoolGame_GameManager.Instance.validPot)
                    {
                        PoolGame_GameManager.Instance.wasFault = true;
                        if (PoolGame_GameManager.Instance.faultMessage.Length == 0)
                            PoolGame_GameManager.Instance.faultMessage = "";
                    }

                    if (PoolGame_GameManager.Instance.wasFault && PoolGame_GameManager.Instance.faultMessage.Equals(StaticStrings.potedCueBall))
                    {

                        getNewWhiteBallPosition(true);

                    }
                }






                opponentBallsStoped = true;
                if(PoolGame_GameManager.Instance.offlineMode) {

                } else {
                    if (PoolGame_GameManager.Instance.wasFault)
                    {
                        if (PoolGame_GameManager.Instance.faultMessage.Length > 0)
                            PoolGame_GameManager.Instance.audioSources[2].Play();
                        setOpponentTurn();
                    }
                    else
                    {
                        StartCoroutine(setMyTurn(false));
                    }
                }

                
            }
    }
    public void EnableWhiteBallCollider()
    {
        GetComponent<SphereCollider>().enabled = true;
    }

    Vector3 newPos;
    private void getNewWhiteBallPosition(bool center)
    {
        PoolGame_GameManager.Instance.whiteBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PoolGame_GameManager.Instance.whiteBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        PoolGame_GameManager.Instance.whiteBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        PoolGame_GameManager.Instance.whiteBall.GetComponent<Rigidbody>().maxAngularVelocity = 150;

        // Debug.Log(GameManager.Instance.whiteBall.GetComponent<Rigidbody>().maxAngularVelocity + " MAX ANG");

        bool collides = true;

        //Vector3 newPos;
        //Vector3 newPos = new Vector3(-0f, -0.69f, -0.24f);
        if (PoolGame_GameManager.Instance.firstBallTouched)//mohith
        {
            newPos = new Vector3(-0f, -0.69f, -0.24f);
        }
        else
        {

            if (!center)
                newPos = new Vector3(-2.9f, -0.69f, -0.24f);
            else
                newPos = new Vector3(0, -0.69f, -0.24f);

        }
        //mohith
        PoolGame_GameManager.Instance.whiteBall.GetComponent<LockZPosition>().whiteBallPotted = false;//mohith
        //newPos = new Vector3(-0f, -0.69f, -0.24f);
        //if (!center)
        //    newPos = new Vector3(-2.9f, -0.69f, -0.24f);
   
           




        while (collides)
        {
            collides = false;
            for (int i = 1; i < PoolGame_GameManager.Instance.balls.Length; i++)
            {
                if (Vector2.Distance(PoolGame_GameManager.Instance.balls[i].transform.position, newPos) < 0.38f)
                {
                    collides = true;
                    break;
                }
            }

            if (!collides)
            {
                Debug.LogError("change white ball position ");
                PoolGame_GameManager.Instance.whiteBall.GetComponent<Rigidbody>().transform.position = newPos;
                PoolGame_GameManager.Instance.whiteBall.GetComponent<LockZPosition>().ballActive = true;
                PoolGame_GameManager.Instance.whiteBall.SetActive(true);
                Vector3 newBallPos1 = PoolGame_GameManager.Instance.whiteBall.transform.position;
                newBallPos1.z = ballHand.transform.position.z;
                ballHand.transform.position = newBallPos1;
            }
            else
            {
                newPos.x -= 0.1f;
            }
        }

        PoolGame_GameManager.Instance.whiteBall.GetComponent<Rigidbody>().transform.position = newPos;
        PoolGame_GameManager.Instance.whiteBall.GetComponent<LockZPosition>().CancelInvoke();
    }

    public void checkBallsTypes(bool fault, bool wasServer, bool setSolid)
    {
        Debug.Log("Checking types " + PoolGame_GameManager.Instance.wasFault);
        if (!fault && !PoolGame_GameManager.Instance.playersHaveTypes)
        {
            Debug.Log("Checking types inside if");
            if (PoolGame_GameManager.Instance.noTypesPotedSolid && !PoolGame_GameManager.Instance.noTypesPotedStriped)
            {
                PoolGame_GameManager.Instance.playersHaveTypes = true;
                potedBallsGUI.potedBallsVisible = true;

                if(PoolGame_GameManager.Instance.offlineMode) {
                    if(PoolGame_GameManager.Instance.offlinePlayerTurn == 1) {
                        PoolGame_GameManager.Instance.offlinePlayer1OwnSolid = true;
                        PoolGame_GameManager.Instance.ownSolids = true;
                        potedBallsGUI.showPotedBalls(true);
                        
                        
                    } else {
                        PoolGame_GameManager.Instance.offlinePlayer1OwnSolid = false;
                        PoolGame_GameManager.Instance.ownSolids = true;
                        potedBallsGUI.showPotedBalls(false);
                        
                    }
                } else {
                    // You got solid balls
                    //if (GameManager.Instance.cueController.isServer)
                    if(wasServer)
                        potedBallsGUI.showPotedBalls(true);
                    else
                        potedBallsGUI.showPotedBalls(setSolid);
                        //potedBallsGUI.showPotedBalls(false);

                    //if (!GameManager.Instance.offlineMode && wasServer)
                    //    PhotonNetwork.RaiseEvent(21, false, true, null);
                }
                
            }
            else if (!PoolGame_GameManager.Instance.noTypesPotedSolid && PoolGame_GameManager.Instance.noTypesPotedStriped)
            {
                PoolGame_GameManager.Instance.playersHaveTypes = true;
                potedBallsGUI.potedBallsVisible = true;

                if(PoolGame_GameManager.Instance.offlineMode) {
                    if(PoolGame_GameManager.Instance.offlinePlayerTurn == 1) {
                        PoolGame_GameManager.Instance.offlinePlayer1OwnSolid = false;
                        PoolGame_GameManager.Instance.ownSolids = false;
                        potedBallsGUI.showPotedBalls(false);
                    } else {
                        PoolGame_GameManager.Instance.ownSolids = false;
                        PoolGame_GameManager.Instance.offlinePlayer1OwnSolid = true;
                        potedBallsGUI.showPotedBalls(true);
                    }
                } else {
                    // You got striped balls
                    //if (GameManager.Instance.cueController.isServer)
                    if(wasServer)
                        potedBallsGUI.showPotedBalls(false);
                    else
                        potedBallsGUI.showPotedBalls(setSolid);
                        //potedBallsGUI.showPotedBalls(true);
                    //if (!GameManager.Instance.offlineMode && wasServer)
                    //    PhotonNetwork.RaiseEvent(21, true, true, null);
                }
                
            }
        }

        PoolGame_GameManager.Instance.noTypesPotedSolid = false;
        PoolGame_GameManager.Instance.noTypesPotedStriped = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Ball"))
            collides.Remove(other.tag);
    }




    void OnMouseDown()
    {
        if (PoolGame_GameManager.Instance.hasCueInHand)
        {
            ballHand.SetActive(false);
            ballHandShownOnce = true;
            ballHand.GetComponent<SpriteRenderer>().enabled = false;
            HideAllControllers();
            movingWhiteBall = true;
            if (!PoolGame_GameManager.Instance.ballsStriked)
            {
                whiteBallLimits.SetActive(true);
                //if (!GameManager.Instance.offlineMode)
                //    PhotonNetwork.RaiseEvent(14, 1, true, null);
            }
            else
            {
                //if (!GameManager.Instance.offlineMode)    
                //    PhotonNetwork.RaiseEvent(17, 1, true, null);
            }
        }
    }

    Vector3 lastCorrectPosition;
    void OnMouseDrag()
    {
        //if (collides.Count == 0) {
        if (PoolGame_GameManager.Instance.hasCueInHand && isServer)
        {
            bool canMove = true;
            Vector3 collPos = Vector3.zero;
            float distance = 0.0f;

            //Debug.Log(Vector2.Distance(PoolGame_GameManager.Instance.balls[1].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));

            for (int i = 1; i < PoolGame_GameManager.Instance.balls.Length; i++)
            {
                if (Vector2.Distance(PoolGame_GameManager.Instance.balls[i].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.38f)
                {
                    canMove = false;
                    collPos = PoolGame_GameManager.Instance.balls[i].transform.position;
                    distance = Vector2.Distance(PoolGame_GameManager.Instance.balls[i].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;
                }
            }

            if (canMove)
            {

                float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));


                float newX = transform.position.x;
                float newY = transform.position.y;


                if (PoolGame_GameManager.Instance.ballsStriked)
                {
                    if (pos_move.x > -4.55f && pos_move.x < -2.65f)
                    {
                        newX = pos_move.x;
                    }
                    else
                    {
                        if (pos_move.x < -4.55f)
                        {
                            newX = -4.55f;
                        }
                        else
                        {
                            newX = -2.65f;
                        }
                    }
                }
                else
                {
                    if (pos_move.x > -4.55f && pos_move.x < -2.65f)
                    {
                        newX = pos_move.x;
                    }
                    else
                    {
                        if (pos_move.x < -4.55f)
                        {
                            newX = -4.55f;
                        }
                        else
                        {
                            newX = -2.65f;
                        }
                    }
                }

                if (pos_move.y > -2.75f && pos_move.y < 1.45f)
                {
                    newY = pos_move.y;
                }
                else
                {
                    if (pos_move.y < -2.75f)
                    {
                        newY = -2.75f;
                    }
                    else
                    {
                        newY = 1.45f;
                    }
                }

                lastCorrectPosition = transform.position;
                transform.position = new Vector3(newX, newY, transform.position.z);
                //if (!GameManager.Instance.offlineMode)
                //    PhotonNetwork.RaiseEvent(15, transform.position, true, null);
            }
        }
    }

    void OnMouseUp()
    {
        Debug.Log("hascueinhand::" + PoolGame_GameManager.Instance.hasCueInHand);
        if (PoolGame_GameManager.Instance.hasCueInHand)
        {
            if (ballHandShownOnce)
            {
                ballHand.SetActive(true);
                ballHand.GetComponent<SpriteRenderer>().enabled = true;
                Vector3 newBallPos = PoolGame_GameManager.Instance.whiteBall.transform.position;
                newBallPos.z = ballHand.transform.position.z;
                ballHand.transform.position = newBallPos;
            }

            ShowAllControllers();
            movingWhiteBall = false;

            whiteBallLimits.SetActive(false);
            //if (!GameManager.Instance.offlineMode)
            //    PhotonNetwork.RaiseEvent(16, 1, true, null);
        }
    }

    private void hideCalledPockets()
    {
        for (int i = 0; i < calledPockets.Length; i++)
        {
            calledPockets[i].SetActive(false);
        }
    }

    public bool startPotCheckTimer = false;

    public bool canRotateBalls;
    int blackBallPottedCounter;
    int firstMultiplierCounter;

    public void setTurnOffline(bool showTurnMessage) {

        checkShot();

        PoolGame_GameManager.Instance.audioSources[0].Play();

        hidePocketButtons();

        if(PoolGame_GameManager.Instance.wasFault) {
            if(PoolGame_GameManager.Instance.faultMessage.Length > 0)
                PoolGame_GameManager.Instance.gameControllerScript.showMessage(PoolGame_GameManager.Instance.faultMessage);
            Debug.Log("Fault: " + PoolGame_GameManager.Instance.faultMessage);
            if(PoolGame_GameManager.Instance.offlinePlayerTurn == 1) {
                if(PoolGame_GameManager.Instance.offlinePlayer1OwnSolid) {
                    PoolGame_GameManager.Instance.ownSolids = false;
                } else {
                    PoolGame_GameManager.Instance.ownSolids = true;
                }
                PoolGame_GameManager.Instance.offlinePlayerTurn = 2;
                if (!PoolGame_GameManager.Instance.iWon && !PoolGame_GameManager.Instance.iLost) 
                    gameControllerScript.resetTimers(2, showTurnMessage);
                
            } else {
                if(PoolGame_GameManager.Instance.offlinePlayer1OwnSolid) {
                    PoolGame_GameManager.Instance.ownSolids = true;
                } else {
                    PoolGame_GameManager.Instance.ownSolids = false;
                }
                PoolGame_GameManager.Instance.offlinePlayerTurn = 1;
                if (!PoolGame_GameManager.Instance.iWon && !PoolGame_GameManager.Instance.iLost) 
                    gameControllerScript.resetTimers(1, showTurnMessage);
                
            }

            //if (message.Equals (StaticStrings.potedCueBall)) {

            if (!PoolGame_GameManager.Instance.iWon && !PoolGame_GameManager.Instance.iLost) {
                if (!ballHandShownOnce)
                {
                    ballHand.SetActive(true);
                    PoolGame_GameManager.Instance.hasCueInHand = true;
                }

                //ballHand.SetActive(true);
                //GameManager.Instance.hasCueInHand = true;
                Vector3 newBallPos = PoolGame_GameManager.Instance.whiteBall.transform.position;
                newBallPos.z = ballHand.transform.position.z;
                ballHand.transform.position = newBallPos;
            }
            //}
        } else {
            if (!PoolGame_GameManager.Instance.iWon && !PoolGame_GameManager.Instance.iLost) {
                gameControllerScript.resetTimers(PoolGame_GameManager.Instance.offlinePlayerTurn, false);
                Debug.Log("Turn: " + PoolGame_GameManager.Instance.offlinePlayerTurn);
            }
                
        }

        
        cueSpinObject.GetComponent<SpinController>().resetPositions();
        PoolGame_GameManager.Instance.noTypesPotedSolid = false;
        PoolGame_GameManager.Instance.noTypesPotedStriped = false;
        ballsInMovement = false;
        PoolGame_GameManager.Instance.firstBallTouched = false;
        PoolGame_GameManager.Instance.ballTouchedBand = 0;
        PoolGame_GameManager.Instance.wasFault = false;
        PoolGame_GameManager.Instance.faultMessage = "";
        PoolGame_GameManager.Instance.validPot = false;
        isServer = true;
        canShowControllers = true;
        hideCalledPockets();
        PoolGame_GameManager.Instance.calledPocket = true;
        canShowControllers = true;

        if(!PoolGame_GameManager.Instance.iWon && !PoolGame_GameManager.Instance.iLost)
        {
            //mohith
            ScoreController.instance.MultiplierMeshRendererOn();

            ScoreController.instance.firstBallPotedCounter++;

            Invoke("ShowAllControllers", 0.25f);
            //ShotPowerIndicator.anim.Play("MakeVisible");
           // ShotPowerIndicator.ShowCue();

            //rotate all balls to forward facing...
            canRotateBalls = true;

            //stopping to check whether any ball is poted....
            startPotCheckTimer = false;

            ScoreController.instance.pocketedBallCounter = 0;

            //switch score multipliers.....
            Debug.Log("First ball pocket counter::" + ScoreController.instance.firstBallPotedCounter);
            ScoreController.instance.RandomizePots();


            ScoreController.instance.commonTrickScore = 0;

            if (firstMultiplierCounter == 0)
            {
                firstMultiplierCounter++;
                ScoreController.instance.TurnOffFirstMultiplier();
            }

            if (ScoreController.instance.blackBallPotted)
            {
                if (blackBallPottedCounter <= 1)
                {
                    if (blackBallPottedCounter == 0)
                    {
                        ScoreController.instance.BlackBallBonusMessage();
                    }

                    if (blackBallPottedCounter == 1)
                    {
                        ScoreController.instance.blackBallPotted = false;
                    }

                    blackBallPottedCounter++;
                    ScoreController.instance.BlackBallBonusOn();
                }
            }

            ScoreController.instance.multiplierValue++;

            ScoreController.instance.wallTrickshot = false;
            ScoreController.instance.ballTrickShot = false;

            foreach (var item in FindObjectsOfType<LockZPosition>())
            {
                item.wallCollided = false;
                item.ballCount = 0;
                item.ballCounted = false;
            }
            ////reduce player life if white ball is poted....
            ///ScoreController.instance.callDecreasePlayerLife();

            //mohith
        }

    }
    //mohith
    public void RotateBallsToFaceForward()
    {

        Quaternion ballFaceForwardrot = Quaternion.Euler(0, 0, 0);
        Quaternion whiteBallFaceForwardrot = Quaternion.Euler(0, 90, 0);


        for (int i = 0; i < PoolGame_GameManager.Instance.balls.Length; i++)
        {
            if (i == 0)
            {
                PoolGame_GameManager.Instance.balls[i].transform.rotation =
                Quaternion.RotateTowards(PoolGame_GameManager.Instance.balls[i].transform.rotation, whiteBallFaceForwardrot, 150 * Time.deltaTime);
            }
            else
            {
                PoolGame_GameManager.Instance.balls[i].transform.rotation =
                Quaternion.RotateTowards(PoolGame_GameManager.Instance.balls[i].transform.rotation, ballFaceForwardrot, 150 * Time.deltaTime);
            }
        }
        Invoke("StopBallRotation", 2f);
    }

    void StopBallRotation()
    {

        canRotateBalls = false;

    }


    public IEnumerator setMyTurn(bool showTurnMessage)
    {

        Debug.Log("My turn");

        PoolGame_GameManager.Instance.audioSources[0].Play();

        changeCueImage(PoolGame_GameManager.Instance.cueIndex);

        hidePocketButtons();
        gameControllerScript.resetTimers(1, showTurnMessage);
        cueSpinObject.GetComponent<SpinController>().resetPositions();
        PoolGame_GameManager.Instance.noTypesPotedSolid = false;
        PoolGame_GameManager.Instance.noTypesPotedStriped = false;
        ballsInMovement = false;
        PoolGame_GameManager.Instance.firstBallTouched = false;
        PoolGame_GameManager.Instance.ballTouchedBand = 0;
        PoolGame_GameManager.Instance.wasFault = false;
        PoolGame_GameManager.Instance.validPot = false;
        isServer = true;
        canShowControllers = true;
        hideCalledPockets();
        PoolGame_GameManager.Instance.calledPocket = true;

        if (PoolGame_GameManager.Instance.callPocketAll)
        {
            PoolGame_GameManager.Instance.calledPocket = false;
            canShowControllers = false;
            callPocket();
        }


        if (PoolGame_GameManager.Instance.callPocketBlack)
        {
            if (PoolGame_GameManager.Instance.ownSolids && PoolGame_GameManager.Instance.solidPoted >= 7)
            {
                PoolGame_GameManager.Instance.calledPocket = false;
                canShowControllers = false;
                callPocket();
            }
            else if (!PoolGame_GameManager.Instance.ownSolids && PoolGame_GameManager.Instance.stripedPoted >= 7)
            {
                PoolGame_GameManager.Instance.calledPocket = false;
                canShowControllers = false;
                callPocket();
            }
        }


        while (PoolGame_GameManager.Instance.calledPocket == false)
        {
            Debug.Log("CALLED: " + PoolGame_GameManager.Instance.calledPocket);
            yield return new WaitForSeconds(0.2f);
        }

        canShowControllers = true;

        if(!PoolGame_GameManager.Instance.iWon && !PoolGame_GameManager.Instance.iLost) {
            Invoke("ShowAllControllers", 0.25f);
            ShotPowerIndicator.anim.Play("MakeVisible");
        }

    }

    public void setOpponentTurn()
    {
        int oppoCueIndex = PoolGame_GameManager.Instance.opponentCueIndex;
        if(PoolGame_GameManager.Instance.opponentCueIndex >= cueTextures.Length - 1) {
            oppoCueIndex = cueTextures.Length-1;
        }
        changeCueImage(oppoCueIndex);

        hidePocketButtons();
        canShowControllers = true;
        hideCalledPockets();
        PoolGame_GameManager.Instance.noTypesPotedSolid = false;
        PoolGame_GameManager.Instance.noTypesPotedStriped = false;
        //opponentBallsStoped = true;
        cueSpinObject.GetComponent<SpinController>().resetPositions();
        ballsInMovement = false;
        //GameManager.Instance.wasFault = false;
        cue.GetComponent<Renderer>().enabled = true;
        Invoke("ShowAllControllers", 0.25f);
        isServer = false;
        multiplayerDataArray.Clear();
        startMovement = false;
        ShotPowerIndicator.anim.Play("ShotPowerAnimation");
        gameControllerScript.resetTimers(2, true);
    }

    private void setCuePosition()
    {
        
    }

    private void ShowAllControllers()
    {

        if (canShowControllers)
        {
            Debug.Log("Showing controllers");
            if (ScoreController.instance.isShowingSomeUI)
            {
                Invoke(nameof(EnableTargetLine), 4f);
            }
            else
            {
                targetLine.SetActive(true);
            }

            cue.transform.position = new Vector3(PoolGame_GameManager.Instance.balls[0].transform.position.x, PoolGame_GameManager.Instance.balls[0].transform.position.y, cue.transform.position.z);
            cue.GetComponent<Renderer>().enabled = true;
            ShotPowerScript.instance.ShowPowerBar();
        }

    }
    void EnableTargetLine()
    {
        targetLine.SetActive(true);
    }
    public void HideAllControllers()
    {
        targetLine.SetActive(false);
        cue.GetComponent<Renderer>().enabled = false;
    }

    public void stopTimer()
    {
        PoolGame_GameManager.Instance.stopTimer = true;
    }


    public void noMoreTime()
    {

    }

    private bool canCheckAnotherCueRotation = true;

    int aa;

    private IEnumerator rotateCue()
    {
        canCheckAnotherCueRotation = false;
        //yield return new WaitForSeconds(0.1f);

        if (!movingWhiteBall && !ballsInMovement && isServer)
        {
            float ang = 0;

            Quaternion initRot = cue.transform.rotation;
            Quaternion rot = cue.transform.rotation;

            
            

            if (!ShotPowerIndicator.mouseDown && displayTouched & !isFirstTouch)
            {
                if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")))
                {
                    ang = Input.GetAxis("Mouse X");
                    if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y > transform.position.y)
                    {
                        ang = -ang;
                    }
                }
                else
                {
                    ang = Input.GetAxis("Mouse Y");
                    if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
                    {
                        ang = -ang;
                    }
                }

                float multAng = Vector2.Distance(touchMousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                multAng *= 300.0f;

                if(multAng < 1.5f) multAng = 1.5f;

            
                multAng = multAng * 0.05f;

                
                if(multAng > 6.0f) multAng = 6.0f;



                ang *=  multAng;

                touchMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                isFirstTouch = false;
            }



            //if(touchMousePos != Vector3.zero) {
                

            // } else {

            // }

            // float multAng = Vector2.Distance(touchMousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            // multAng *= 300.0f;

            // if(multAng < 1.5f) multAng = 1.5f;

        
            // multAng = multAng * 0.05f;

            
            // if(multAng > 8.0f) multAng = 8.0f;


            // if(displayTouched)
            //     Debug.Log("Andgle: " + (aa++) + "    -     " + multAng);

            // ang *=  multAng;

            // touchMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //ang *= 0.9f;


            yield return new WaitForSeconds(0.01f);

            if (displayTouched) {
               // Debug.LogError("angle::" + ang);//mohith
                ang = ang / 1.5f;//mohith
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z + ang);
                cue.transform.rotation = rot;


                if (isServer && initRot.eulerAngles != rot.eulerAngles) {
                    //if (!GameManager.Instance.offlineMode)
                    //    PhotonNetwork.RaiseEvent(7, rot, true, null);
                }
            }

            canCheckAnotherCueRotation = true;

            drawTargetLines();
        } else {
            canCheckAnotherCueRotation = true;
        }
    }

    private IEnumerator rotateCueInvoke(Quaternion rot, Quaternion initRot) {

        yield return new WaitForSeconds(0.1f);

        if (displayTouched) {

            cue.transform.rotation = rot;

            

            if (isServer && initRot.eulerAngles != rot.eulerAngles) {
                //if (!GameManager.Instance.offlineMode)
                //    PhotonNetwork.RaiseEvent(7, rot, true, null);
            }
        } 

        yield return true;
    }

    // DRAW TARGET LINES
    private void drawTargetLines()
    {
        
        if (!ballsInMovement)
        {
            Vector3 dir = transform.position - posDetector.transform.position;
            dir.z = 0;
            dir = dir.normalized;

            Vector3 linePos = transform.position;
            linePos.z = -3;

            lineRenderer.SetPosition(0, linePos);

            ray.origin = transform.position;
            ray.direction = dir;

            if (Physics.SphereCast(ray, radius, out hitInfo, 10, layerMask))
            {

                endPosition = ray.origin + (ray.direction.normalized * hitInfo.distance);
                endPosition.z = -3;


                circle.transform.position = endPosition;
                Vector3 linePos1 = endPosition;
                linePos1.z = -3;
                lineRenderer.SetPosition(1, linePos1);


                if (hitInfo.transform.tag.Contains("Ball"))
                {
                    int ballNumber = System.Int32.Parse(hitInfo.transform.tag.Replace("Ball", ""));
                    if (PoolGame_GameManager.Instance.playersHaveTypes)
                    {
                        if (isServer)
                        {
                            if ((PoolGame_GameManager.Instance.ownSolids && ballNumber < 8) || (!PoolGame_GameManager.Instance.ownSolids && ballNumber > 8) || ballNumber == 8)
                            {
                                drawLine(linePos);
                            }
                            else
                            {
                                //wrongBall.SetActive(true);
                                //circle.GetComponent<LineRenderer>().enabled = false;
                                //ballCollideFirst = false;
                                //lineRenderer2.enabled = false;
                                //lineRenderer3.enabled = false;
                                drawLine(linePos);
                            }
                        }
                        else
                        {
                            if ((!PoolGame_GameManager.Instance.ownSolids && ballNumber < 8) || (PoolGame_GameManager.Instance.ownSolids && ballNumber > 8) || ballNumber == 8)
                            {
                                drawLine(linePos);
                            }
                            else
                            {
                                //wrongBall.SetActive(true);
                                //circle.GetComponent<LineRenderer>().enabled = false;
                                //ballCollideFirst = false;
                                //lineRenderer2.enabled = false;
                                //lineRenderer3.enabled = false;
                                drawLine(linePos);
                            }
                        }

                    }
                    else
                    {
                        drawLine(linePos);
                    }


                }
                else
                {
                    wrongBall.SetActive(false);
                    circle.GetComponent<LineRenderer>().enabled = true;
                    ballCollideFirst = false;
                    lineRenderer2.enabled = false;
                    lineRenderer3.enabled = false;
                }
            }
        }
    }

    public void drawLine(Vector3 linePos)
    {
        wrongBall.SetActive(false);
        circle.GetComponent<LineRenderer>().enabled = true;
        Vector3 hitBallPosition = hitInfo.transform.position;
        hitBallPosition.z = -3;
        //hitBallPosition.y += 0.1f;
        lineRenderer3.SetPosition(0, hitBallPosition);

        Vector3 r2dir = hitBallPosition - endPosition;
        Ray r2 = new Ray(hitBallPosition, r2dir);

        Vector3 pos3 = r2.origin + (1.5f + PoolGame_GameManager.Instance.cueAim * 0.25f) * r2dir;//added 1.5f in place of 3
        pos3.z = -3;
        lineRenderer3.SetPosition(1, pos3);


       
        Vector3 l = (3 + PoolGame_GameManager.Instance.cueAim * 0.25f) * r2dir;
        l = Quaternion.Euler(0, 0, -90) * l + endPosition;
        
        float angleBeetwen = AngleBetweenThreePoints(linePos, endPosition, l);//mo
       
        l.z = -3;
        lineRenderer2.SetPosition(0, endPosition);
        //float angle = 90.0f;\
        //float angleBeetwen = AngleBetweenThreePoints(linePos, endPosition, l);//mo

        //Debug.LogError("Angle Between::" + angleBeetwen);

        if (angleBeetwen < 90.0f || angleBeetwen > 270.0f)
        {
            float val2;//mo
            val2 = Mathf.Lerp(1.5f, 0.1f, Mathf.InverseLerp(0, 90, angleBeetwen));//mo  //added 1.5f in place of 3
                                                                                  // Debug.LogError("val::" + val2);
            l = (val2 + PoolGame_GameManager.Instance.cueAim * 0.25f) * r2dir;
            l = Quaternion.Euler(0, 0, 90) * l + endPosition;
            l.z = -3;
        }
        else if (angleBeetwen > 90f && angleBeetwen < 270f)
        {
            float val3;//mo
            val3 = Mathf.Lerp(1.5f, 0.1f, Mathf.InverseLerp(180, 90, angleBeetwen));//mo  //added 1.5f in place of 3
                                                                                    // Debug.LogError("val::" + val2);
            l = (val3 + PoolGame_GameManager.Instance.cueAim * 0.25f) * r2dir;
            l = Quaternion.Euler(0, 0, -90) * l + endPosition;
            l.z = -3;
        }
        else//mo
        {
            Debug.LogError("else condition");
            float val3;
            val3 = Mathf.Lerp(1.5f, 0.1f, Mathf.InverseLerp(270, 360, angleBeetwen));  //added 1.5f in place of 3
            // Debug.LogError("val::" + val2);
            l = (val3 + PoolGame_GameManager.Instance.cueAim * 0.25f) * r2dir;
            l = Quaternion.Euler(0, 0, -90) * l + endPosition;
            l.z = -3;
        }

        lineRenderer2.SetPosition(1, l);
        ballCollideFirst = true;


        if (PoolGame_GameManager.Instance.showTargetLines)
        {
            lineRenderer2.enabled = true;
            lineRenderer3.enabled = true;
        }
    }

    public float AngleBetweenThreePoints(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        float a = pointB.x - pointA.x;
        float b = pointB.y - pointA.y;
        float c = pointB.x - pointC.x;
        float d = pointB.y - pointC.y;

        float atanA = Mathf.Atan2(a, b) * Mathf.Rad2Deg;
        float atanB = Mathf.Atan2(c, d) * Mathf.Rad2Deg;

        float output = atanB - atanA;
        output = Mathf.Abs(output);



        return output;
    }
}
