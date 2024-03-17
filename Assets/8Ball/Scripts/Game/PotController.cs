using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{
    public int pocketSet;

    public int PotColliderNumber;

    public int PotValue;

    int firstPotValue = 10;

    int blackBallBonusVal = 15;

    public bool isBallPoted = false;

    GameObject thisCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        thisCollider = this.gameObject;
    }

    
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.tag.Contains("Ball") && !collision.CompareTag("WhiteBall"))
        {
           
            foreach (Renderer r in thisCollider.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
            int ballNumber = System.Int32.Parse(collision.transform.tag.Replace("Ball", ""));
           // print("Ball Number is " + ballNumber);
            isBallPoted = true;
            LockZPosition b = collision.GetComponent<LockZPosition>();
            int ballCount = b.ballCount;
            if (b.wallCollided)
            {
                if(ScoreController.instance.firstBallPotedCounter >= 1)
                    ScoreController.instance.wallTrickshot = true;
            }


            Debug.LogError("b.potted::" + b.potted);
            if (!b.potted)
            {
                b.potted = true;
                ScoreController.instance.pocketedBallCounter++;
                Debug.Log("pocketedBallCounter" + ScoreController.instance.pocketedBallCounter);
                //piper for ball streak bonus
                ScoreController.instance.ballStreakBonus++;
                Debug.LogError("ballstreakBonus::" + ScoreController.instance.ballStreakBonus);
                //
                if (/*ScoreController.instance.ballPotted &&*/ ScoreController.instance.pocketedBallCounter >= 2)
                {
                    //ball trickshot bonus...
                    ScoreController.instance.doubleBallBonus = true;
                }
                if (ScoreController.instance.multiplierValue == 0)
                {
                    ScoreController.instance.AddScore(firstPotValue, ballNumber, thisCollider, ballCount);
                }
                else
                {
                    if (ScoreController.instance.blackBallBonus[0].activeSelf)
                    {
                        ScoreController.instance.AddScore(blackBallBonusVal, ballNumber, thisCollider, ballCount);
                    }
                    else
                    {
                        ScoreController.instance.AddScore(PotValue, ballNumber, thisCollider, ballCount);
                    }
                }
            }
           
        }
        //mohith commented
        if (collision.CompareTag("WhiteBall"))//piper
        {
            Debug.Log("white ball potted");
            if (ScoreController.instance.firstBallPotedCounter >= 1)
                Invoke(nameof(ShowNoBallPocketed), 1f);
        }
        //mohith commented

        if (ScoreController.instance.canCheckForPotedBalls)
        {
            if (collision.tag.Contains("Ball"))
            {
                ScoreController.instance.ballPotted = true;
                //ScoreController.instance.canAddScore = true;
            }
        }

    }
    void ShowNoBallPocketed()
    {
        ScoreController.instance.ShowNoBallPocked();
    }
}
