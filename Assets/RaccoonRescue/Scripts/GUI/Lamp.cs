using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
	public Image fillRect;
	public ItemColor colorLamp;
	public GameObject light;
	public Image powerupImg;
	public Powerups powerup;
	public Animator anim;
	public GameObject lightningEffect;
	public GameObject handImg;
	public static Lamp instance;

    private void Awake()
    {
		instance = this;
        if (!PlayerPrefs.HasKey("powerupHelp"))
        {
			PlayerPrefs.SetInt("powerupHelp", 0);
        }
    }

    void Start()
	{
		fillRect.fillAmount = 0;
		powerupImg.color = Color.grey;

		light.SetActive(false);
		lightningEffect.SetActive(false);
		if (colorLamp == 0) {
			Debug.LogError("lamp's color not defined");
			return;
		}

	}

	// Use this for initialization
	void OnEnable()
	{
		//LightContainerLamp.OnFinished += ApplyPower;
		Ball.OnDestroy += Fill;
	}

	void OnDisable()
	{
		//LightContainerLamp.OnFinished -= ApplyPower;
		Ball.OnDestroy -= Fill;
	}


	public void Fill(ItemColor color)
	{
		//colorLamp == color && 
		if (fillRect.fillAmount < 1 && (GameEvent.Instance.GameStatus == GameState.BlockedGame || GameEvent.Instance.GameStatus == GameState.Playing)) {
			//fillRect.fillAmount += 0.066666666666667f;
			fillRect.fillAmount += 0.02f;
			powerupImg.color = Color.grey;
			if (fillRect.fillAmount == 1) {
				SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.powerup_fill);
                if (PlayerPrefs.GetInt("powerupHelp") == 0)
                {
					handImg.SetActive(true);
					iTween.ScaleTo(handImg.gameObject, iTween.Hash("x", 1.1, "y", 1.1, "looptype", iTween.LoopType.pingPong, "time", 0.35));
                }
                else
                {
					anim.SetTrigger("Play");
				}
				powerupImg.color = Color.white;
				light.SetActive(true);
			}
		}
	}

	public Ball ball;
	ItemColor clickedLampColor;

	public void OnClick()
	{
		GameObject catapult = GameObject.Find("boxCatapult");
		ball = catapult.GetComponent<Square>().Busy;
		clickedLampColor = colorLamp;
		PlayerPrefs.SetInt("powerupHelp", 1);
		handImg.SetActive(false);
		if (ball != null) {
			if (fillRect.fillAmount == 1 && GameEvent.Instance.GameStatus == GameState.Playing && ball.PowerUp == Powerups.NONE && !mainscript.Instance.lauchingBall.colorBoost) {
				light.SetActive(false);
				SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.powerup_click[Random.Range(0, SoundBase.Instance.powerup_click.Length)]);

				 //fillRect.fillAmount = 0;
				//lightningEffect.SetActive(true);
				StartCoroutine(FlyToTarget());
				ApplyPower();
			}
		}
	}

	int randPowerup;
	void ApplyPower()
	{
		if (ball != null) {
			if (fillRect.fillAmount == 1 && colorLamp == clickedLampColor) {
				//piper
				//randPowerup = Random.Range(0, 10);
				//if (randPowerup % 2 == 0)
				//	powerup = Powerups.FIRE;
				//else
				//	powerup = Powerups.TRIPLE;
				powerup = Powerups.FIRE;
				//piper
				mainscript.Instance.SetPower(powerup);
				fillRect.fillAmount = 0;
				powerupImg.color = Color.grey;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (/*!LevelData.colorsDict.ContainsValue(colorLamp) ||*/ LevelData.powerups[(int)powerup - 1] == 0)
			gameObject.SetActive(false);
		if (fillRect.fillAmount == 1 && Random.Range(0, 100) == 1)
			anim.SetTrigger("Play");

	}

	IEnumerator FlyToTarget()
	{
		Vector3 targetPos = GameObject.Find("boxCatapult").transform.position;

		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, fillRect.transform.position.x), new Keyframe(0.5f, targetPos.x));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, fillRect.transform.position.y), new Keyframe(0.5f, targetPos.y));
		curveY.AddKey(0.2f, fillRect.transform.position.y + 1);
		float startTime = Time.time;
		Vector3 startPos = transform.position;
		float speed = 0.2f;
		float distCovered = 0;
		// while (distCovered < 0.5f)
		// {
		//     distCovered = (Time.time - startTime);
		//     fillRect.transform.position = new Vector3(curveX.Evaluate(distCovered), curveY.Evaluate(distCovered), 0);
		//     fillRect.transform.Rotate(Vector3.back * 10);
		//     yield return new WaitForEndOfFrame();
		// }
		yield return new WaitForSeconds(1.5f);

		fillRect.rectTransform.localPosition = Vector3.zero;

	}

}
