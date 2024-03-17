using UnityEngine;
using System.Collections;

public class SetTableTexture : MonoBehaviour {

    public GameObject downside;

    public Sprite[] sprites;

	// Use this for initialization
	void Start () {
	    gameObject.GetComponent<SpriteRenderer>().sprite = sprites[PoolGame_GameManager.Instance.tableNumber];
        downside.GetComponent<SpriteRenderer>().sprite = sprites[PoolGame_GameManager.Instance.tableNumber];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
