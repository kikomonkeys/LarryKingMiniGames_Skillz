using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreTutorial : MonoBehaviour {
	public Sprite[] pictures;
	public Text countDownText;
	Image image;
	// Use this for initialization
	void OnEnable () {
		//		image = transform.GetChild (0).GetComponent<Image> ();
		//		image.sprite = pictures [(int)LevelData.GetTarget ()];//TODO: set pre tutorial picture
		//		image.SetNativeSize ();
		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.swish [0]);
		StartCoroutine(StartCountDown(0f));
	}
	IEnumerator StartCountDown(float waittime)
    {
		yield return new WaitForSeconds(waittime);
		countDownText.text = "3";
		yield return new WaitForSeconds(0.5f);
		countDownText.text = "2";
		yield return new WaitForSeconds(0.5f);
		countDownText.text = "1";
		yield return new WaitForSeconds(0.5f);
		countDownText.text = "GO";
        yield return new WaitForSeconds(0.5f);
        Stop();
    }
	// Update is called once per frame
	public void Stop () {
		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.swish [1]);
		mainscript.Instance.startTimer = true;
		mainscript.Instance.EnableHeaderAndFooter();
		//GameEvent.Instance.GameStatus = GameState.Tutorial;
		gameObject.SetActive (false);
	}
}
