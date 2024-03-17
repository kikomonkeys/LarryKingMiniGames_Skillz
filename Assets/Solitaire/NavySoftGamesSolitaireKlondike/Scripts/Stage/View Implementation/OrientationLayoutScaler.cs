using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OrientationLayoutScaler : MonoBehaviour {

	public RectTransform canvasRect;

	public RectTransform port;
	public RectTransform land;
//
	float width;
	float height;
		
	protected float aspectRatio {
		get {
			//				if (height > width) {
			//					return width / height;
			//				} else {
			//					return height / width;
			//				}
			return Mathf.Min (width, height) / Mathf.Max(width, height);
		}
	}

	// Use this for initialization
	void Start () {

		width = Screen.width;//canvasRect.sizeDelta.x;
		height = Screen.height;//canvasRect.sizeDelta.y;

		// MAGIC FOR PORT
		port.anchoredPosition = new Vector2(0, port.sizeDelta.x / aspectRatio / 2);


		// MAGIC FOR LAND
		land.sizeDelta = new Vector2 (land.sizeDelta.y / aspectRatio, land.sizeDelta.y);

	}
//
//	void Update(){
//		print ("width: " + Screen.width + " h:" + Screen.height + "/ w: "+ land.sizeDelta.x + " h: " + land.sizeDelta.y);
//	}

}
