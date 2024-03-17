using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongAnim : MonoBehaviour {

	public float AnimSpeed;
	public float ScaleVal=1.1f;
	public float delay;
	void OnEnable () 
	{
		if(gameObject.activeInHierarchy)
		Invoke("TweenButton", delay);
	}
	private void OnDisable()
	{
		//iTween.Stop();
	}
	void TweenButton()
	{
		//Debug.Log("tween btn");
		iTween.ScaleTo(gameObject, iTween.Hash("x", ScaleVal, "y", ScaleVal, "looptype", iTween.LoopType.pingPong, "speed", AnimSpeed));
	}


}
