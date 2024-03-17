using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
	private void Awake()
	{ 
		GameProgress gp = new GameProgress ();
		gp.InitPlayerPref ();
		gp.InitNamePref ();
		gp.InitCalendarPref ();


        Debug.LogError(13 % 13 + "::" + 2 % 13);

      
      StartCoroutine(LoadStage());
         
	}


    private IEnumerator LoadStage()
    {
        yield return new WaitForSeconds(.1f);

        // note: this requires "using UnityEngine.SceneManagement;"
        SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }
}