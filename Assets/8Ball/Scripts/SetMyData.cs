using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetMyData : MonoBehaviour {

	public GameObject avatar;
	public GameObject name;
	public GameObject matchCanvas;
	public GameObject controlAvatars;
	public GameObject backButton;
	// Use this for initialization


	public void MatchPlayer() {
		
		name.GetComponent <Text>().text = PoolGame_GameManager.Instance.nameMy;
		if(PoolGame_GameManager.Instance.avatarMy != null)
			avatar.GetComponent <Image>().sprite = PoolGame_GameManager.Instance.avatarMy;


		controlAvatars.GetComponent <ControlAvatars> ().reset ();
		//matchCanvas.SetActive (true);

	}

	public void setBackButton(bool active) {
		backButton.SetActive (active);
	}
}
