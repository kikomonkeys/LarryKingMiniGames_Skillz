using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class SmoothMovementManager : MonoBehaviour {

    public static SmoothMovementManager instance;

 
	[SerializeField]
	private float smoothTimeMove = 10f;
	[SerializeField]
	public float distMove = 5f;
    
    private void Awake()
    {
        instance = this;
    }

    public float GetDistanceMove { get { return distMove; } }

 

	public enum Speed
	{
		Deal,
		Deck,
		Move,
		Undo,
		Hint,
		AutoComplete,
		Solution1x,
		Solution2x,
		Solution3x
	}

	[Space(20f)]
	[SerializeField]
	private Speed currentSpeedType;


	[Header("Speed Consts")]
	[SerializeField]
	private float speedDeal;
	[SerializeField]
	private float speedDeck;
	[SerializeField]
	private float speedMove;
	[SerializeField]
	private float speedUndo;
	[SerializeField]
	private float speedHint;
	[SerializeField]
	private float speedAutoComplete;
	[SerializeField]
	private float speedSolution1x;
	[SerializeField]
	private float speedSolution2x;
	[SerializeField]
	private float speedSolution3x;

 
    public float GetSmoothSpeed
    {
        get
        { return smoothTimeMove; }
    }
 

 





	// FOLLOW
 

	 



	// MOVES

	public void SetMovingSpeed(Speed speed){
		currentSpeedType = speed;
		speed = currentSpeedType;
		switch (speed) {
		case Speed.Deal:
			smoothTimeMove = speedDeal;
			break;
		case Speed.Move:
			smoothTimeMove = speedMove;
			break;
		case Speed.Deck:
			smoothTimeMove = speedDeck;
			break;
		case Speed.Undo:
			smoothTimeMove = speedUndo;
			break;
		case Speed.Hint:
			smoothTimeMove = speedHint;
			break;
		case Speed.AutoComplete:
			smoothTimeMove = speedAutoComplete;
			break;
		case Speed.Solution1x:
			smoothTimeMove = speedSolution1x;
			break;
		case Speed.Solution2x:
			smoothTimeMove = speedSolution2x;
			break;
		case Speed.Solution3x:
			smoothTimeMove = speedSolution3x;
			break;
		}
 
	}


 
 
 
		
}
