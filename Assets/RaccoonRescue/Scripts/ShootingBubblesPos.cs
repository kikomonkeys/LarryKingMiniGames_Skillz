using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBubblesPos : MonoBehaviour
{

    public static ShootingBubblesPos instance;
    public float offsetVal;
    
    private void Awake()
    {
        instance = this;
        SetPosition();
    }
    
    public void SetPosition()
    {
        Vector3 pos;
        float yVal;
        yVal = (Screen.height * 10) / 100;
        if (yVal > 220)
            yVal = 25;
        else if (yVal > 200 && yVal < 220)
            yVal = 100;
        pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, yVal, 0));
        Debug.LogError("pos::" + pos + "yval::" + yVal); 
        gameObject.transform.localPosition = pos;
    }
   
}
