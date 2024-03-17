using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePage : MonoBehaviour
{
    private void OnEnable()
    {
        GameSettings.Instance.isGameStarted = false;
    }


}
