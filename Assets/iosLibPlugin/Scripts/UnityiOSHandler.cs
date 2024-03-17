using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
public class UnityiOSHandler : MonoBehaviour
{
    // Start is called before the first frame update
 
    public UnityEngine.UI.Text debugTxt;

    public static UnityiOSHandler instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                //
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }
    void Start() {

        Debug.Log("GAME STARTED, LOG FROM UNITY PLAYER");
        SendScore(0, false);
        // NativeAPI.sendMessageToMobileApp("Game Has been started!! fired from unity");
        SetOtherGamesPhysics();
    }
    // Update is called once per frame
  
    int pressCount = 0;
  

    public void SendScore(int score,bool isComplete)
    {
        try
        {
            System.DateTime time = System.DateTime.Now;
            Debug.Log("{\"score\":" + score + ",\"timestamp\":" +"\""+ time+ "\"" + ", \"isFinal\":" + isComplete + "}");
            #if UNITY_IOS
            NativeAPI.sendMessageToMobileApp("{\"score\":" + score + ",\"timestamp\":" + "\"" + time + "\"" + ", \"isFinal\":" + isComplete + "}");
            #endif
        }
        catch
        {
            showDebugText("Error in finding the <color=RED>\"sendMessageToMobileApp(string)\"</color>");
        }
    }


    

 


    void showDebugText(string msg)
    {
        CancelInvoke(nameof(hideTxt));
        debugTxt.text = msg;
        Invoke(nameof(hideTxt), 2);
    }

    void hideTxt()
    {
        debugTxt.text = "";
    }

    System.Action VideoCompleteCallBack;
    public void SetCallBack(System.Action videoWatched) // attach call back viA unityIosHandler.instance.SetCallBack(ACtion) before invokeing RV video
    {
        VideoCompleteCallBack = videoWatched;
    }

    void rewardedAdDidEnd() //this will be invoked from iOS
    {
        if (VideoCompleteCallBack != null)
        {
            
            VideoCompleteCallBack.Invoke();
            VideoCompleteCallBack = null;
        }
    }



    //method names to call respective game scenes
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


public class NativeAPI
{
#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern void sendMessageToMobileApp(string message);

    [DllImport("__Internal")]
    public static extern void createRewardedAd(string rewardedVideo_id);

    [DllImport("__Internal")]
    public static extern void createBannerAd(string position, string banner_id);

    [DllImport("__Internal")]
    public static extern void createInterstitialAd(string interstitial_id);
#endif
}
