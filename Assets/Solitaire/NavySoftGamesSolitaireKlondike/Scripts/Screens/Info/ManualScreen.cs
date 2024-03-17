using UnityEngine;
using UserWindow;

public class ManualScreen : MonoBehaviour
{
	private void Start ()
	{
		if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Shift ();
	}
	public void OnRule ()
	{
		PopUpManager.Instance.ShowRule ();
	}
 
  
	public void OnBack ()
	{
		PopUpManager.Instance.Close ();
	}
	#region PopUpDialogWindow
	private void PopUpReviewApp()
	{
		RateUsController.instance.PopUpReviewApp ();
	}
	#endregion
}