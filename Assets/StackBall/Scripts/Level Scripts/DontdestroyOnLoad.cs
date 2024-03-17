using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontdestroyOnLoad : MonoBehaviour
{
    public static DontdestroyOnLoad instance;
    void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    
}
