using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(PostScore), 1f);
            
    }
    void PostScore()
    {
        UnityiOSHandler.instance.SendScore(StageManager.instance.finalScore, true);
    }
   
}
