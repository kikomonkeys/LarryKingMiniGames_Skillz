using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopUp : PopUp
{
    // Start is called before the first frame update

    public static SettingsPopUp instance;
    static void CheckInstance()
    {
        if (instance == null)
        {
            instance = Instantiate(
                Resources.Load<SettingsPopUp>("Prefabs/UI/Settings Window"),
                PopUp.CanvasPopup.transform,
                false);


        }
    }
    void Init()
    {

    }
    public static void show()
    {
        CheckInstance();
        instance.Init();
        instance.gameObject.SetActive(true);
    }

    public static void hide()
    {

        Destroy(instance.gameObject);
    }

    public override void Show()
    {

        base.Show();
        this.gameObject.SetActive(true);

    }

    public override void Hide()
    {
        base.Hide();
        Destroy(this.gameObject);
    }


}