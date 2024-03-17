using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour 
{
	// The button to turn on/off sound.
	public Button btnSound;	
	// Image of the button on which sound sprite will get assigned. Default on
	public Image btnSoundImage; 
	// Sound on sprite.
	public Sprite soundOnSprite;
	// Sounf off sprite.
	public Sprite soundOffSprite;
	public GameObject parentBg;
	public Sprite parentBgOnSpr, parentBgOffSpr;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		btnSound.onClick.AddListener(() => 
		{
			Debug.LogError("sound btn clicked");
			AudioManager.Instance.PlayButtonClickSound();
			AudioManager.Instance.ToggleSoundStatus();
			if (InputManager.Instance.canInput ()) {
				
			}
			
		});
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		AudioManager.OnSoundStatusChangedEvent += OnSoundStatusChanged;
		initSoundStatus ();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		AudioManager.OnSoundStatusChangedEvent -= OnSoundStatusChanged;
	}

	/// <summary>
	/// Inits the sound status.
	/// </summary>
	void initSoundStatus()
	{
		btnSoundImage.sprite = (AudioManager.Instance.isSoundEnabled) ? soundOnSprite : soundOffSprite;
		parentBg.GetComponent<Image>().sprite= (AudioManager.Instance.isSoundEnabled) ? parentBgOnSpr : parentBgOffSpr;
		btnSoundImage.SetNativeSize();
	}

	/// <summary>
	/// Raises the sound status changed event.
	/// </summary>
	/// <param name="isSoundEnabled">If set to <c>true</c> is sound enabled.</param>
	void OnSoundStatusChanged (bool isSoundEnabled)
	{
		btnSoundImage.sprite = (isSoundEnabled) ? soundOnSprite : soundOffSprite;
		parentBg.GetComponent<Image>().sprite = (AudioManager.Instance.isSoundEnabled) ? parentBgOnSpr : parentBgOffSpr;

		btnSoundImage.SetNativeSize();

	}
}
