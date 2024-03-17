using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuPause : MonoBehaviour
{

	int count;
	[SerializeField]
	Text lvlNumText;
	// Use this for initialization
	void OnEnable()
	{
		Time.timeScale = 0;
		//mainscript.Instance.startTimer = false;
		//GameEvent.Instance.GameStatus = GameState.Pause;
	}

	// Update is called once per frame
	void OnDisable()
	{
		Time.timeScale = 1;
		GameEvent.Instance.GameStatus = GameState.WaitAfterClose;
		lvlNumText.gameObject.SetActive(false);
	}
	public void PauseTitleClicked()
    {
		count++;
        if (count >= 10)
        {
			count = 0;
			lvlNumText.gameObject.SetActive(true);
			lvlNumText.text = "" + PlayerPrefs.GetInt("OpenLevel");
		}
    }
}
