using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelOpener : MonoBehaviour {

    public GameObject Panel;
    public GameObject Panelhide;

    public void openPanel() 
    {
        if (Panel != null) 
        {
            Panel.SetActive(true);
            Panelhide.SetActive(false);
        }
    }
}
