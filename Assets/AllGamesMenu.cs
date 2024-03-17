using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllGamesMenu : MonoBehaviour
{
    

    void Start()
    {
        SetOtherGamesPhysics();
    }

    public void PlayBtnClicked()
    {

       // SkillzManager.myGameName = SkillzManager.GameName.PoolBall;
        SkillzCrossPlatform.LaunchSkillz();
    }


    public void OpenBlockGame()
    {
        SetOtherGamesPhysics();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
        SceneManager.LoadScene("blockpuzzle_mainmenu");
    }
    public void OpenKnifeGame()
    {
        SetOtherGamesPhysics();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
        SceneManager.LoadScene("GameScene");
    }
    public void OpenStackballGame()
    {
        SetOtherGamesPhysics();
        Time.timeScale = 1.5f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("MainScene");
    }
    public void OpenSolitaireGame()
    {
        SetOtherGamesPhysics();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
        SceneManager.LoadScene("Stage");
    }
    public void OpenBubbleShooterGame()
    {
        SetOtherGamesPhysics();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
        SceneManager.LoadScene("bubbleshootgame");
    }
    public void OpenBallPoolGame()
    {
        SetPoolGamePhysics();
        Time.fixedDeltaTime = 0.01f;
        SceneManager.LoadScene("PoolGame_GameScene");
    }

    public static void SetPoolGamePhysics()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Physics.gravity = new Vector3(0, 0, 10);
        Physics.bounceThreshold = 0;
        Physics.defaultMaxDepenetrationVelocity = 10;//10
        Physics.sleepThreshold = 0.005f;// 1000000;
        Physics.defaultContactOffset = 0.0001f;
        Physics.defaultSolverIterations = 10;
        Physics.defaultSolverVelocityIterations = 1;
        Physics.autoSyncTransforms = true;
        Physics.reuseCollisionCallbacks = false;
        Time.timeScale = 2;
    }
    public static void SetOtherGamesPhysics()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.bounceThreshold = 2;
        Physics.defaultMaxDepenetrationVelocity = 10;//10
        Physics.sleepThreshold = 0.005f;// 1000000;
        Physics.defaultContactOffset = 0.01f;
        Physics.defaultSolverIterations = 6;
        Physics.defaultSolverVelocityIterations = 1;
        Physics.autoSyncTransforms = false;
        Physics.reuseCollisionCallbacks = true;
    }
    
}
