using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmanagerScript : MonoBehaviour
{
    // Start is called before the first frame update
  
    public static PmanagerScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
