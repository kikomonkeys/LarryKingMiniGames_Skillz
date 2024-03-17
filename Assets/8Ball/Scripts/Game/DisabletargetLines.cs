using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabletargetLines : MonoBehaviour
{
    public GameObject targetLines;
    [SerializeField]
    bool isTimeUp;
    private void OnEnable()
    {
        targetLines.SetActive(false);
        if(isTimeUp)
        ScoreController.instance.DisablePots();
    }
    private void OnDisable()
    {
        targetLines.SetActive(true);

    }


}
