using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnifeCounter : MonoBehaviour 
{
	public GameObject knifeIcon;
	public Color activeColor;
	public Color deactiveColor;
	public static KnifeCounter intance;
	public Text knivesLeftText;
	//public TextMeshProUGUI knivesleftTxt;
	public TextMeshPro knivesleftTxt;

	List<GameObject> iconList;
	void Awake()
	{
		if (intance == null) {
			intance = this;
			iconList = new List<GameObject> ();
		}
		else
			Destroy (gameObject);

		//Invoke(nameof(EnableKnifeLeftText), 0.35f);
	}
	void EnableKnifeLeftText()
    {
		//knivesLeftText.gameObject.SetActive(true);
		knivesleftTxt.gameObject.SetActive(true);
    }
	void DisableKnifeLeftText()
    {
		//knivesLeftText.gameObject.SetActive(false);
		knivesleftTxt.gameObject.SetActive(false);
	}
	int totalKnives;
	public void setUpCounter(int totalKnife)
	{
		Invoke(nameof(EnableKnifeLeftText), 0.35f);

		totalKnives = totalKnife;
		//knivesLeftText.text = "" + totalKnife;
		knivesleftTxt.text = "" + GamePlayManager.instance.currentCircle.totalKnife;
		//knivesleftTxt.SetText()
		/*
		foreach (var item in iconList) {
			Destroy (item);
		}
		iconList.Clear ();

		for (int i = 0; i < totalKnife; i++) 
		{
			GameObject temp = Instantiate<GameObject> (knifeIcon, transform);
			temp.GetComponent<Image> ().color = activeColor;
			iconList.Add (temp);
		}
		*/

	}
	int knivesLeft;

	public void  setHitedKnife(int val)
	{
		knivesLeft = totalKnives - val;
		knivesleftTxt.text = "" + knivesLeft;
		Invoke(nameof(DisableWithDelay), 0.3f);
		
		/*
		for (int i = 0; i <iconList.Count; i++) {
			iconList[i].GetComponent<Image> ().color =i<val?deactiveColor:activeColor;
		}
		*/
	}
	void DisableWithDelay()
    {
		if (knivesLeft == 0)
		{
			DisableKnifeLeftText();
		}
	}
}
