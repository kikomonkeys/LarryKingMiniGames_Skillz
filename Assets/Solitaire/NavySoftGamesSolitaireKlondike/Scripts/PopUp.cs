using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    static GameObject _canvasPopup;
    public static GameObject CanvasPopup
    {
        get
        {
            if (_canvasPopup == null)
                _canvasPopup = GameObject.FindWithTag("Canvas");

            return _canvasPopup;
        }
    }

    void Init()
    {

    }
    // Start is called before the first frame update
    public virtual void Show()
    {
        //show show
    }


    public virtual void Hide()
    {

    }
}