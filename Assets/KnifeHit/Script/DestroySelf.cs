using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float destroyTime;
    void Start()
    {
        SelfDestroy();
    }

    void SelfDestroy()
    {
        Destroy(gameObject, destroyTime);
    }
}
