using UnityEngine;
using System.Collections;
using InitScriptName;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{

	// Use this for initialization
	void OnEnable()
	{
		GameObject Off = transform.GetChild(0).gameObject;
		if (name == "MusicOn") {
			if (PlayerPrefs.GetFloat("Music") == 0f) {
				Off.SetActive(true);
				Off.transform.parent.GetComponent<Image>().enabled = false;
			}
			else {

				Off.SetActive(false);
				Off.transform.parent.GetComponent<Image>().enabled = true;


			}
		} else if (name == "SoundOn") {
			if (PlayerPrefs.GetInt("Sound") == 0) {
				SoundBase.Instance.mixer.SetFloat("soundVolume", -80);
				Off.SetActive(true);
				Off.transform.parent.GetComponent<Image>().enabled = false;

			}
			else {
				SoundBase.Instance.mixer.SetFloat("soundVolume", 1);
				Off.SetActive(false);
				Off.transform.parent.GetComponent<Image>().enabled = true;

			}
		}

	}

	// Update is called once per frame
	void Update()
	{

	}
}
