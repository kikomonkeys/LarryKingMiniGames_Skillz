/**
 * Apple.cs
 * Created by: #FreeBird#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

	public ParticleSystem splatApple;
	public SpriteRenderer Sprite;
	public AudioClip appleHitSfx;
	bool cut;
	public enum Multiplier
    {
		multiplier2x,
		multiplier3x,
		multiplier5x
	}
	public Multiplier scoreMultiplier;
	public Sprite multiplier2xSpr, multiplier3xSpr, multiplier5xSpr;
	void EnableMultiplier()
    {
        if (GameManager.Stage >= 4 && GameManager.Stage <= 10)
        {
			int rand;
			rand = Random.Range(0, 10);
            if (rand % 2 == 0)
            {
				scoreMultiplier = Multiplier.multiplier2x;
				Sprite.sprite = multiplier2xSpr;
            }
            else
            {
				scoreMultiplier = Multiplier.multiplier3x;
				Sprite.sprite = multiplier3xSpr;
			}
		}
        else if(GameManager.Stage >= 11)
        {
			int rand;
			rand = Random.Range(0, 20);
            if (rand % 2 == 0)
            {
				scoreMultiplier = Multiplier.multiplier2x;
				Sprite.sprite = multiplier2xSpr;
			}
			else if (rand % 3 == 0)
            {
				scoreMultiplier = Multiplier.multiplier3x;
				Sprite.sprite = multiplier3xSpr;
			}
            else
            {
				scoreMultiplier = Multiplier.multiplier5x;
				Sprite.sprite = multiplier5xSpr;
			}
        }
    }
	// Use this for initialization
	public Rigidbody2D rb;
	void Start () {
		//Debug.LogError("stage number:" + GameManager.Stage);
		rb = GetComponentInChildren<Rigidbody2D> ();    
		rb.isKinematic = true;
		EnableMultiplier();
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Knife") && !cut) {
			cut = true;
			GamePlayManager.isKnifeHitToBubble = true;
			//Debug.LogError("Called Apple"+ GamePlayManager.isKnifeHitToBubble);
			SoundManager.instance.PlaySingle(appleHitSfx);
			GameManager.Apple++;
			transform.parent = null;
			GetComponent<CircleCollider2D>().enabled = false;
			Sprite.enabled = false;
			splatApple.Play();
			//FindObjectOfType<GamePlayManager>().storeAppleScore(GameManager.Stage + 1);
			//GameManager.score += (GameManager.Stage + 1);
			if (scoreMultiplier == Multiplier.multiplier2x)
			{
				//Debug.LogError("scoremultiplier::" + scoreMultiplier);
				FindObjectOfType<GamePlayManager>().storeAppleScore((GameManager.Stage + 1) * 2);
				GameManager.score += ((GameManager.Stage + 1) * 2);
				GamePlayManager.instance.SpawnPointsText((GameManager.Stage + 1) * 2, 100);
			}
			else if (scoreMultiplier == Multiplier.multiplier3x)
            {
				FindObjectOfType<GamePlayManager>().storeAppleScore(GameManager.Stage * 3);
				GameManager.score += (GameManager.Stage * 3);
				GamePlayManager.instance.SpawnPointsText(GameManager.Stage * 3, 100);
			}
			else if (scoreMultiplier == Multiplier.multiplier5x)
            {
				FindObjectOfType<GamePlayManager>().storeAppleScore(GameManager.Stage * 5);
				GameManager.score += (GameManager.Stage * 5);
				GamePlayManager.instance.SpawnPointsText(GameManager.Stage * 5, 100);
			}
			Destroy(gameObject, 3f);

			//if (!other.gameObject.GetComponent<Knife> ().isHitted) {


			//}
		}
	}
}


