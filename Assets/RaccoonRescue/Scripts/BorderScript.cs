using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour
{
    public bool isLeftBorder;
    public GameObject LeftObj, rightObj;
    [SerializeField]
    float val;
    private void Start()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        if (!isLeftBorder)
        {
            //Vector2 rightworldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + val, val));
            Vector2 rightworldPoint = Camera.main.ScreenToWorldPoint(rightObj.transform.position);
            gameObject.transform.position = rightworldPoint;
        }
        else
        {
            //Vector2 leftworldPoint = Camera.main.ScreenToWorldPoint(new Vector2(-val, val));
            Vector2 leftworldPoint = Camera.main.ScreenToWorldPoint(LeftObj.transform.position);
            gameObject.transform.position = leftworldPoint;
        }
        
    }
}
