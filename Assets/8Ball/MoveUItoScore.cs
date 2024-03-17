using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoveUItoScore : MonoBehaviour
{
    public GameObject scoreMainUI;

    public Color fadeColor;

    bool canMove = false;
    TextMeshPro currentScoreUI;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 thisPosition = transform.position;
        //GameObject trickShotText = Instantiate(ScoreController.instance.trickShotTextPrefab, thisPosition, Quaternion.identity);
        scoreMainUI = ScoreController.instance.scoreUIPosition;
        currentScoreUI = transform.Find("ScoredTextUI").GetComponent<TextMeshPro>();
        Invoke("CallMoveFunction", 1.5f);

        //trickShotText.transform.DOScale()
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, scoreMainUI.transform.position) < 0.001f)
        {
            //play score animation...
            //ScoreController.instance.mainScoreAnim.Play("ScorePopAnimation");//mohith
            ScoreController.instance.mainScoreAnim.Play("ScoreAnim");//mohith
            Destroy(gameObject);
        }

        if(canMove)
        {
            MoveUI();
            ChangeAlpha();
        }
        
    }

    void CallMoveFunction()
    {
        canMove = true;
    }

    public void MoveUI()
    {
        transform.position = Vector3.MoveTowards(transform.position, scoreMainUI.transform.position, 7f * Time.deltaTime);
    }

    public void ChangeAlpha()
    {
        currentScoreUI.color = Color.Lerp(currentScoreUI.color, fadeColor, 0.25f * Time.deltaTime);
    }

}
