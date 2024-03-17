using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsTest : MonoBehaviour
{
    public static int levelnumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LevelClick(int number)
    {
        PlayerPrefs.SetInt("OpenLevel", number);
        PlayerPrefs.Save();
        LevelData.GetTargetOnLevel(number);
        LevelData.LoadLevel(number);
        //if (MenuManager.Instance != null)
        SceneManager.LoadScene("game");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
