using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpritePos : MonoBehaviour
{
    public float val;

    void Start()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - val, Screen.height - val));
        gameObject.transform.position = worldPoint;
    }



}
