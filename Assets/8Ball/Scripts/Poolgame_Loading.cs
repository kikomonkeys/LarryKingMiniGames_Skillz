using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Poolgame_Loading : MonoBehaviour
{
    void OnEnable()
    {
        SetPoolGamePhysicsValues();
    }

    public static void SetPoolGamePhysicsValues()
    {
        Physics.gravity = new Vector3(0, 0, 10);
        Physics.bounceThreshold = 0.5f;//0
        Physics.defaultMaxDepenetrationVelocity = 10f;//10
        Physics.sleepThreshold = 0.05f;// 1000000;
        Physics.defaultContactOffset = 0.0001f;//0.0001f
        Physics.defaultSolverIterations = 10;
        //Physics.defaultSolverVelocityIterations = 1;
        //Physics.autoSyncTransforms = true;
        //Physics.reuseCollisionCallbacks = false;
        Time.timeScale = 2f;
        
    }
    void LoadGameScene()
    {
        SceneManager.LoadScene("PoolGame_GameScene");
    }
    public static void SetOriginalPhysicsValues()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.bounceThreshold = 2;
        Physics.defaultMaxDepenetrationVelocity = 10;//10
        Physics.sleepThreshold = 0.005f;// 1000000;
        Physics.defaultContactOffset = 0.01f;
        Physics.defaultSolverIterations = 6;
        Physics.defaultSolverVelocityIterations = 1;
        Physics.autoSyncTransforms = false;
        Physics.reuseCollisionCallbacks = true;
        Time.timeScale = 1;
    }
}
