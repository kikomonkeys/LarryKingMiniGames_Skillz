using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingpongAnim : MonoBehaviour
{
    void Start()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1.1, "y", 1.1,  "looptype", iTween.LoopType.loop,"delay",2f));
    }

    
}
