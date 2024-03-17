using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance;

	
	// Use this for initialization
	void Awake()
	{

		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		SetMusic();
	}
	public void SetMusic()
	{
		//Debug.LogError("Music::" + GameManager.Music);
        if (GameManager.Music)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
        //gameObject.GetComponent<AudioSource>().enabled = GameManager.Music ? true : false;
    }
}
