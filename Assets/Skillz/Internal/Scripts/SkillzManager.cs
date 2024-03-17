using SkillzSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SkillzManager : SkillzEventsHandler
{
    [Header("On Match Will Begin")]
    [Tooltip("Scene that will load when the match begins")]
    public SceneField gameScene;
    public UnityEvent<SkillzSDK.Match> onMatchWillBegin;

    [Header("On Skillz Will Exit")]
    [Tooltip("Scene that will load when the user returns to the start menu")]
    public SceneField startMenuScene;
    public UnityEvent onSkillzWillExit;

    [Header("On Progression Room Enter")]
    [Tooltip("Scene that will launch when the user clicks on a progression entry point")]
    public SceneField progressionRoomScene;
    public UnityEvent onProgressionRoomEnter;

    static SkillzManager instance;

    public static int gameTimerFromSkillz;

    public enum GameName
    {
        BlockPuzzle,
        KnifeHit,
        StackBall,
        Solitaire,
        BubbleShooter,
        PoolBall
    }
    public static GameName myGameName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
        }
    }

    public static bool ExistsInProject()
    {
        return (instance != null);
    }

    public static void LaunchSkillz()
    {
        SkillzCrossPlatform.LaunchSkillz();
    }

    protected override void OnMatchWillBegin(SkillzSDK.Match match)
    {
        onMatchWillBegin.Invoke(match);

        //if (gameScene != "")
        //{
        //    SceneManager.LoadScene(gameScene);
        //}

        if (match.GameParams.ContainsKey("gametimer"))
        {
            string timeLimitStr = match.GameParams["gametimer"];
            gameTimerFromSkillz = int.Parse(timeLimitStr);
        }

        if (match.GameParams.ContainsKey("gamename"))
        {
            string gamename;
            gamename = match.GameParams["gamename"];

            switch (gamename)
            {
                case "blockpuzzle":
                    myGameName = GameName.BlockPuzzle;
                    AllGamesMenu.SetOtherGamesPhysics();
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.01f;
                    SceneManager.LoadScene("blockpuzzle_mainmenu");
                    break;
                case "knifehit":
                    myGameName = GameName.KnifeHit;
                    AllGamesMenu.SetOtherGamesPhysics();
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.01f;
                    SceneManager.LoadScene("GameScene");
                    break;
                case "stackball":
                    myGameName = GameName.StackBall;
                    AllGamesMenu.SetOtherGamesPhysics();
                    Time.timeScale = 1.5f;
                    Time.fixedDeltaTime = 0.02f;
                    SceneManager.LoadScene("MainScene");
                    break;
                case "solitaire":
                    myGameName = GameName.Solitaire;
                    AllGamesMenu.SetOtherGamesPhysics();
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.01f;
                    SceneManager.LoadScene("Stage");
                    break;
                case "bubbleshooter":
                    myGameName = GameName.BubbleShooter;
                    AllGamesMenu.SetOtherGamesPhysics();
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.01f;
                    SceneManager.LoadScene("bubbleshootgame");
                    break;
                case "poolball":
                    myGameName = GameName.PoolBall;
                    AllGamesMenu.SetPoolGamePhysics();
                    Time.fixedDeltaTime = 0.01f;
                    SceneManager.LoadScene("PoolGame_GameScene");
                    break;
            }
        }
    }

    protected override void OnSkillzWillExit()
    {
        onSkillzWillExit.Invoke();

        if (startMenuScene != "")
        {
            SceneManager.LoadScene(startMenuScene);
        }

    }

    protected override void OnProgressionRoomEnter()
    {
        onProgressionRoomEnter.Invoke();

        if (progressionRoomScene != "")
        {
            SceneManager.LoadScene(progressionRoomScene);
        }
    }
}
