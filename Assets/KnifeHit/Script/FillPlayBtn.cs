using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillPlayBtn : MonoBehaviour
{
    bool startFilling;
    [SerializeField]
    Image img;
    void OnEnable()
    {
        startFilling = true;
    }

    void Update()
    {
        if (startFilling)
        {
            img.fillAmount += 0.55f * Time.deltaTime;
            if (img.fillAmount >= 1)
            {
                startFilling = false;
                img.fillAmount = 1;
            }
        }
    }
}
