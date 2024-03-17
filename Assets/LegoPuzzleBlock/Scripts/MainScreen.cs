using UnityEngine;
using System.Collections;

public class MainScreen : UIScreen 
{	
	/// <summary>
	/// Init this instance.
	/// </summary>
	public override void init()
	{
		base.init ();
        GameController.gameMode = GameMode.CLASSIC;
        StackManager.Instance.LoadGamePlayFromModeSelection();
	}

	/// <summary>
	/// Raises the play button pressed event.
	/// </summary>
	public void OnPlayButtonPressed()
	{
		if (InputManager.Instance.canInput ()) {
			AudioManager.Instance.PlayButtonClickSound ();
			StackManager.Instance.SpawnUIScreen ("SelectMode");
		}
	}
}
