using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;
//using PlayFab.ClientModels;
using DG.Tweening;

public class LockZPosition : MonoBehaviour
{

    public bool potted;

    public bool reducedPlayerLife = false;

    private Rigidbody rigid;
    private bool poted = false;
    private float minVelocity = 0.005f;//0.002f;
    private float minAngularVelocity = 0.25f; //0.15f;
    public bool ballActive = true;
    public PhysicMaterial material;
    private PotedBallsGUIController potedBallsGUI;
    public GameObject youWonMessage;
    private bool ballMoved = false;
    private bool audioDisabled = false;
    // Use this for initialization
    public AudioSource[] audioSources;
    private bool ballInactivePlayedAudio = false;

    GameObject thisCollider;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        rigid = GetComponent<Rigidbody>();

        // Set the sorting layer and order.
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sortingLayerName = "tableLayer";
        potedBallsGUI = GameObject.Find("PotedBallsGUI").GetComponent<PotedBallsGUIController>();

    }

    // Update is called once per frame
    bool ballInMovement = false;

    void Update()
    {
        if (ballActive && rigid.velocity.sqrMagnitude < minVelocity &&
            rigid.angularVelocity.magnitude < minAngularVelocity)
        {

            rigid.Sleep();
            ballInMovement = false;
        }

        if (!rigid.IsSleeping() && !rigid.velocity.Equals(Vector3.zero) && !rigid.angularVelocity.Equals(Vector3.zero))
            ballInMovement = true;

        if (ballMoved && !audioDisabled && rigid.velocity.sqrMagnitude < minVelocity &&
            rigid.angularVelocity.magnitude < minAngularVelocity)
        {
            Debug.Log("AUDIO STOP");
            audioSources[3].Stop();
            audioDisabled = true;
        }




    }
   

    private bool firstBallPot = false;

    public bool wallCollided;
    public int ballCount;
    public bool whiteBallPotted;
    void OnTriggerEnter(Collider other)
    {
        
        thisCollider = other.gameObject;
        if (this.gameObject.tag == "WhiteBall" && other.tag.Equals("Wall"))
        {
            wallCollided = true;
        }


        //Debug.Log("TRIGGER: " + other.tag);



        if (!poted && other.tag.Contains("Pot"))
        {

            audioSources[1].Play();
            PoolGame_GameManager.Instance.ballTouchedBand++;
            Debug.Log("Firstballpot::" + firstBallPot);
            //if (!potedBallsGUI.potedBallsVisible) {
            if (!firstBallPot)
            {
                if (transform.tag.Equals("WhiteBall"))
                {
                    if (whiteBallPotted)
                        return;

                    //if(other.gameObject.GetComponent<PotController>().pocketSet == 1)
                    {
                        Debug.Log("WhiteBall");
                        // White Ball Poted - remove life
                        ScoreController.instance.ballPotted = true;
                        //ScoreController.instance.canReduceLife = true;
                        //ScoreController.instance.callDecreasePlayerLife();
                        whiteBallPotted = true;
                        if (ScoreController.instance.firstBallPotedCounter >= 1)
                        {
                            ScoreController.instance.canReduceLife = true;
                        }

                    }

                }
                else
                {
                    int ballNumber = System.Int32.Parse(transform.tag.Replace("Ball", ""));
                    // if (GameManager.Instance.ballsStriked) {
                    if (ballNumber == 8)
                    {
                        // Black ball poted
                        ScoreController.instance.blackBallPotted = true;
                    }
                    else if (ballNumber <= 8)
                    {
                        firstBallPot = true;
                        PoolGame_GameManager.Instance.noTypesPotedSolid = true;
                    }
                    else if (ballNumber >= 8)
                    {
                        firstBallPot = true;
                        PoolGame_GameManager.Instance.noTypesPotedStriped = true;
                    }
                }

            }


           // Debug.Log("POT");

            if (transform.tag.Equals("WhiteBall"))
            {
                PoolGame_GameManager.Instance.wasFault = true;
                PoolGame_GameManager.Instance.faultMessage = StaticStrings.potedCueBall;
                DisableWhiteBall();
            }
            else
            {
                int ballNumber = System.Int32.Parse(transform.tag.Replace("Ball", ""));
                if (PoolGame_GameManager.Instance.cueController.isServer)
                {




                    if (PoolGame_GameManager.Instance.playersHaveTypes)
                    {

                        //if (GameManager.Instance.validPotsCount >= 7) {
                        if ((PoolGame_GameManager.Instance.ownSolids && PoolGame_GameManager.Instance.solidPoted >= 7) ||
                            (!PoolGame_GameManager.Instance.ownSolids && PoolGame_GameManager.Instance.stripedPoted >= 7))
                        {
                            if (ballNumber == 8)
                            {
                                //if (GameManager.Instance.callPocketBlack) {
                                //    if (other.tag.Equals("Pot" + GameManager.Instance.calledPocketID)) {
                                //        GameManager.Instance.iWon = true;
                                //    } else {
                                //        GameManager.Instance.iLost = true;
                                //    }
                                //}
                                //// else if (GameManager.Instance.callPocketAll)
                                //// {
                                ////     if (other.tag.Equals("Pot" + GameManager.Instance.calledPocketID))
                                ////     {
                                ////         GameManager.Instance.iWon = true;
                                ////     }
                                ////     else
                                ////     {
                                ////         GameManager.Instance.iLost = true;
                                ////     }
                                //// }
                                //else {
                                //    GameManager.Instance.iWon = true;
                                //}
                            }
                        }
                        else
                        {


                            if (PoolGame_GameManager.Instance.ownSolids)
                            {
                                if (ballNumber <= 8)
                                {
                                    if (PoolGame_GameManager.Instance.callPocketAll)
                                    {
                                        if (other.tag.Equals("Pot" + PoolGame_GameManager.Instance.calledPocketID))
                                        {
                                            PoolGame_GameManager.Instance.validPotsCount++;
                                            PoolGame_GameManager.Instance.validPot = true;
                                        }
                                    }
                                    else
                                    {
                                        PoolGame_GameManager.Instance.validPotsCount++;
                                        PoolGame_GameManager.Instance.validPot = true;
                                    }
                                }
                                else if (ballNumber == 8)
                                {
                                    //Debug.Log("Poted 8 ball - game over");
                                    //GameManager.Instance.iLost = true;

                                }
                            }
                            else
                            {
                                if (ballNumber >= 8)
                                {
                                    if (PoolGame_GameManager.Instance.callPocketAll)
                                    {
                                        if (other.tag.Equals("Pot" + PoolGame_GameManager.Instance.calledPocketID))
                                        {
                                            PoolGame_GameManager.Instance.validPotsCount++;
                                            PoolGame_GameManager.Instance.validPot = true;
                                        }

                                    }
                                    else
                                    {
                                        PoolGame_GameManager.Instance.validPotsCount++;
                                        PoolGame_GameManager.Instance.validPot = true;
                                    }
                                }
                                else if (ballNumber == 8)
                                {
                                    //Debug.Log("Poted 8 ball - game over");
                                    //GameManager.Instance.iLost = true;
                                }
                            }
                        }



                    }
                    else
                    {
                        if (ballNumber != 8)
                        {
                            PoolGame_GameManager.Instance.validPotsCount++;
                            PoolGame_GameManager.Instance.validPot = true;
                        }
                        else
                        {
                            //GameManager.Instance.iLost = true;
                        }
                    }


                }

                if (ballNumber < 8)
                {
                    PoolGame_GameManager.Instance.solidPoted++;
                }
                else if (ballNumber > 8)
                {
                    PoolGame_GameManager.Instance.stripedPoted++;
                }

                poted = true;
                DisableBall(transform.tag);
            }







            //Invoke ("DisableBall", 0.0f);
        }
    }

    int whiteBallHit = 0;

    public bool ballCounted = false;

    int ballNumber;
    void OnCollisionEnter(Collision collision)
    {
        //Debug.LogError("Mo's name1::" + collision.gameObject.name + " name2 ::" + gameObject.name);
        if (collision.collider.tag.Contains("Ball") && !collision.collider.CompareTag("WhiteBall"))
        {
            if (!ballCounted)
            {
                ballCounted = true;

                LockZPosition colorBall = collision.collider.GetComponent<LockZPosition>();
                int bC = colorBall.ballCount;
                if (bC > ballCount)
                {
                    //ScoreController.instance.ballTrickShot = true;
                    ballCount = bC + 1;
                    if (ballCount > 3)
                    {
                        ballCount = 3;
                    }
                }
            }
        }
        else if (collision.collider.CompareTag("WhiteBall"))
        {
            Debug.Log("white ball pocketed");
            LockZPosition whiteBall = collision.collider.GetComponent<LockZPosition>();
            wallCollided = whiteBall.wallCollided;
            if (ballCount == 0)
            {
                ballCount++;
            }
        }
        //Debug.Log("COLLISION: " + collision.transform.tag);

        if (transform.tag.Contains("Ball") && collision.collider.tag.Equals("bumper"))
        {
            Vector3 v = GetComponent<Rigidbody>().velocity;
            float velSum = Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
            audioSources[2].volume = velSum / 8.0f;
            audioSources[2].Play();
        }

        if (PoolGame_GameManager.Instance.firstBallTouched && transform.tag.Contains("Ball") && collision.collider.tag.Equals("bumper"))
        {
            PoolGame_GameManager.Instance.ballTouchedBand++;
        }

        if (collision.collider.tag.Contains("Ball") && transform.tag.Contains("Ball"))
        {

            if (ballActive)
            {
                // Ball - ball collision
                if (audioSources.Length > 0)
                {
                    Vector3 v = GetComponent<Rigidbody>().velocity;

                    float velSum = Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);

                    Vector3 v2 = collision.gameObject.GetComponent<Rigidbody>().velocity;

                    float velSum2 = Mathf.Abs(v2.x) + Mathf.Abs(v2.y) + Mathf.Abs(v2.z);

                    if (velSum > velSum2) velSum = velSum2;

                    audioSources[0].volume = velSum / 10.0f;

                    audioSources[0].Play();

                }
            }
            else
            {
                if (!ballInactivePlayedAudio && audioSources.Length > 0)
                {
                    ballInactivePlayedAudio = true;
                    audioSources[0].volume = 0.6f;
                    audioSources[0].Play();
                }
            }



        }

        if (transform.tag.Equals("WhiteBall"))
        {
            if (collision.collider.tag.Contains("Ball") && !collision.collider.tag.Equals("WhiteBall") && PoolGame_GameManager.Instance.cueController.isServer)
            {
                // Check if touched my ball - otherwise fault
                if (!PoolGame_GameManager.Instance.firstBallTouched)
                {
                    PoolGame_GameManager.Instance.firstBallTouched = true;
                    Debug.Log("touched the other balls first time");
                    ballNumber = System.Int32.Parse(collision.collider.tag.Replace("Ball", ""));

                    if (PoolGame_GameManager.Instance.playersHaveTypes)
                    {
                        Debug.Log("Inside");

                        //if (GameManager.Instance.validPotsCount >= 7) {
                        if ((PoolGame_GameManager.Instance.ownSolids && PoolGame_GameManager.Instance.solidPoted >= 7) ||
                            (!PoolGame_GameManager.Instance.ownSolids && PoolGame_GameManager.Instance.stripedPoted >= 7))
                        {
                            if (ballNumber != 8)
                            {
                                PoolGame_GameManager.Instance.wasFault = true;
                                PoolGame_GameManager.Instance.faultMessage = StaticStrings.failedToHit8Ball;
                            }
                        }
                        else
                        {
                            if (PoolGame_GameManager.Instance.ownSolids)
                            {
                                if (ballNumber >= 8)
                                {
                                    // Touched not mine ball first - fault
                                    PoolGame_GameManager.Instance.wasFault = true;
                                    PoolGame_GameManager.Instance.faultMessage = StaticStrings.failedToHitSolid;
                                }
                            }
                            else
                            {
                                if (ballNumber <= 8)
                                {
                                    // Touched not mine ball first - fault
                                    PoolGame_GameManager.Instance.wasFault = true;
                                    PoolGame_GameManager.Instance.faultMessage = StaticStrings.failedToHitStriped;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (transform.tag.Contains("Ball") && collision.collider.tag.Equals("bumper"))
            {
                if (!PoolGame_GameManager.Instance.ballTouchBeforeStrike.Contains(transform.tag))
                    PoolGame_GameManager.Instance.ballTouchBeforeStrike.Add(transform.tag);
            }
        }
    }
    void ShowNoBallPocketed()
    {
        ScoreController.instance.ShowNoBallPocked();
    }
    
    private void DisableWhiteBall()
    {

        ScoreController.instance.ballStreakBonus = 0;
        ScoreController.instance.DeactivateStreakBonusUI();
        Debug.Log("ballstreak bonus cancelled bceauase white ball pocketed");
    
        GameObject normalVfx = Instantiate(ScoreController.instance.normalVfxPrefab, thisCollider.transform.position, Quaternion.identity);
        normalVfx.transform.position = new Vector3(thisCollider.transform.position.x, thisCollider.transform.position.y, -1f);
        normalVfx.GetComponent<SpriteRenderer>().color = Color.red;
        
        //GetComponent <SphereCollider>().material = material;

        //rigid.Sleep ();
        ballActive = false;
        rigid.constraints = RigidbodyConstraints.None;
        Vector3 vel = rigid.velocity;
        //      if(vel.x > 1)
        //          vel.x = vel.x / (vel.x * vel.x);
        //      if(vel.y > 1)
        //          vel.y = vel.y / (vel.y * vel.y);
        vel.z = 5.0f;
        //vel.z = 0;
        rigid.velocity = vel;

        //rigid.angularVelocity = rigid.angularVelocity / 5;


        Invoke("deactiveWhite", 1.0f);
        //Invoke ("movePosition", 3.5f);

    }

    private void DisableBall(string ii)
    {



       // Debug.Log("Disable");

        int i = 0;

        try
        {
            i = int.Parse(ii.Replace("Ball", ""));
        }
        catch (System.Exception e) { }

        if (i > 0 && i != 8)
        {
            potedBallsGUI.hidePotedBall(i - 1);
        }

        GetComponent<SphereCollider>().material = material;
        ballActive = false;
        //rigid.Sleep ();
        rigid.constraints = RigidbodyConstraints.None;
        Vector3 vel = rigid.velocity;
        //		if(vel.x > 1)
        //			vel.x = vel.x / (vel.x * vel.x);
        //		if(vel.y > 1)
        //			vel.y = vel.y / (vel.y * vel.y);
        vel.z = 5.0f;
        //vel.z = 0;
        rigid.velocity = vel;

        //rigid.angularVelocity = rigid.angularVelocity / 5;


        StartCoroutine("disableMeshRenderer");//mo this is to disable the ball rolling after pocketed
        Invoke("showMessage", 3.5f);

    }



    public void showMessage()
    {


        //Debug.Log ("Time " + (Time.time - messageTime));
        //        if(Time.time - messageTime > )

        float timeDiff = Time.time - PoolGame_GameManager.Instance.messageTime;

        //Debug.Log("Time diff: " + timeDiff);


        if (timeDiff > 2)
        {
            movePosition();
            PoolGame_GameManager.Instance.messageTime = Time.time;
        }
        else
        {
            //Debug.Log("Show message with delay");
            StartCoroutine(showMessageWithDelay((2.0f - timeDiff) / 1.0f));
        }
    }

    IEnumerator showMessageWithDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);



        movePosition();
        PoolGame_GameManager.Instance.messageTime = Time.time;

    }


    public void EnableBall()
    {

    }

    private void deactiveWhite()
    {
        gameObject.SetActive(false);
        //StartCoroutine("AnimateWhiteBallOff");
    }



    private IEnumerator disableMeshRenderer()
    {
        Debug.LogError("disablemesh");
        //yield return new WaitForSeconds(0.3f);
        //rigid.velocity = Vector3.zero;
        //rigid.angularVelocity = Vector3.zero;
        rigid.transform.DOScale(0, 0);
        yield return new WaitForSeconds(1f);
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        rigid.transform.DOScale(0.33f, 0);
        GetComponent<MeshRenderer>().enabled = true;
    }


    //IEnumerator AnimateWhiteBallOff()
    //{
    //    transform.DOScale(0, 1);
    //    yield return new WaitForSeconds(1f);
    //    gameObject.SetActive(false);
    //}

    //IEnumerator AnimateWhiteBallOn()
    //{
    //    transform.DOScale(0.33f, 1);
    //    yield return new WaitForSeconds(1f);
    //    gameObject.SetActive(true);
    //}

    private void movePosition()
    {
        // Debug.Log("Play sound!!");
        audioSources[0].PlayOneShot(audioSources[3].clip, 1.0f);
        audioSources[3].Play();

        rigid.Sleep();
        GetComponent<MeshRenderer>().enabled = true;
        //		rigid.useGravity = false;


        rigid.transform.position = new Vector3(5.61f, 1.317f, 5.45f);

        //ballMoved = true;
        Invoke("setBallMoved", 1.0f);
    }

    private void setBallMoved()
    {
        ballMoved = true;
    }


}
