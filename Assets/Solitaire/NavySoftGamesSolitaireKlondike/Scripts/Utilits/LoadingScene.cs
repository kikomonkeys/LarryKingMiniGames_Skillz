using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private float eplapsedTime = 0;
    public GameObject menuPage;//piper

 
    private void Update()
    {
        eplapsedTime += Time.deltaTime;
        if (eplapsedTime >= 5)
        {
            ContinueModeGame.instance.SetLoadSuccess(true);
        }
        if (ContinueModeGame.instance.LoadSuccess)//ContinueModeGame.instance!=null && 
        {
            //Destroy(gameObject);//piper
            gameObject.SetActive(false);
            menuPage.SetActive(true);
        }
    }
}
