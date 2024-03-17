﻿using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
	LineRenderer line;
	public bool draw = false;
	Color col;

	public static Vector2[] waypoints = new Vector2[3];
	[SerializeField]
	float addAngle = 185;
	public GameObject pointer;
	public GameObject[,] pointers = new GameObject[3, 30];
	GameObject[,] pointers2 = new GameObject[30, 25];
	GameObject[,] pointers3 = new GameObject[3, 15];

	Vector3 lastMousePos;
	private bool startAnim;
	public float normalOffset = 100;
	public float addangle=10f;
	[SerializeField]
	LineRenderer rend;

	public Color blueBubbleColor, greenBubbleColor, orangeBubbleColor, redBubbleColor, violetBubbleColor, yellowBubbleColor, powerupBubbleClr;
	public Shader m_shader;
	// Use this for initialization
	public static DrawLine instance;

    private void Awake()
    {
		instance = this;
    }
    void Start()
	{
		line = GetComponent<LineRenderer>();
		waypoints[0] = GameObject.Find("boxCatapult").transform.position;
		waypoints[1] = waypoints[0] + Vector2.up * 5;
		for (int i = 0; i < 3; i++) {
			GeneratePoints(i);
			GeneratePositionsPoints(waypoints, i);
			HidePoints(i);
		}
		//rend.enabled = true;
		//rend.material.shader = m_shader;
	}

	int GetAimPoints()
	{
		if (!BoostVariables.AimBoost)
			return 30;
		else
			return pointers2.GetLength(1);
	}

	bool pointsHidden;
	void HidePoints(int num = 0)
	{
		
		//if (pointsHidden) return;
		for (int i = 0; i < pointers.GetLength(1); i++) {
			//pointers[num, i].GetComponent<SpriteRenderer>().enabled = false;
			//pointers[num, i].GetComponent<LinePoint>().light.SetActive(false);
		}

		for (int i = 0; i < pointers2.GetLength(1); i++) {
			//pointers2[num, i].GetComponent<SpriteRenderer>().enabled = false;
			//pointers2[num, i].GetComponent<LinePoint>().light.SetActive(false);
		}
		pointsHidden = true;
		//		for (int i = 0; i < pointers3.GetLength (1); i++) {
		//			pointers3 [num, i].GetComponent<SpriteRenderer> ().enabled = false;
		//		}

	}

	void HideAllPoints()
	{
		for (int i = 0; i < 3; i++) {
			HidePoints(i);
		}

	}

	void EnableBoostLight()
	{
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < pointers.GetLength(1); j++) {
				pointers[i, j].GetComponent<LinePoint>().light.SetActive(true);
			}

			for (int j = 0; j < pointers2.GetLength(1); j++) {
				pointers2[i, j].GetComponent<LinePoint>().light.SetActive(true);
			}
		}

	}

	private void GeneratePositionsPoints(Vector2[] waypoints, int num = 0)
	{
		if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy != null) {
			if(mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.powerUp == Powerups.FIRE ||
				mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.powerUp == Powerups.TRIPLE)
			{
				col = Color.red;
            }
            else
            {
				col = mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<SpriteRenderer>().sprite.texture.GetPixelBilinear(0.6f, 0.6f);
			}

			//Debug.Log(" name -> "+mainscript.Instance.boxCatapult.GetComponent<Square>().name);
			//Debug.Log("col:::" + col);
			col.a = 1;
		}

		HidePoints(num);
		// Removed Section dotted line
		/*for (int i = 0; i < pointers.GetLength(1); i++) {
			Vector2 AB = waypoints[1] - waypoints[0];
			AB = AB.normalized;
			float step = i / 2.75f;
			Vector2 newPos = waypoints[0] + (step * AB);
			if (step >= (waypoints[1] - waypoints[0]).magnitude) {
				newPos = waypoints[1];
			}
			pointsHidden = false;
			//			if (step <= (waypoints [1] - waypoints [0]).magnitude) {
			//pointers[num, i].GetComponent<SpriteRenderer>().enabled = true; //removed section =====================================
			if (BoostVariables.AimBoost)
				pointers[num, i].GetComponent<LinePoint>().light.SetActive(true);
			//removed section ==================================
			/*pointers[num, i].transform.position = newPos;
			pointers[num, i].GetComponent<SpriteRenderer>().color = col;
			pointers[num, i].GetComponent<LinePoint>().startPoint = pointers[num, i].transform.position;
			pointers[num, i].GetComponent<LinePoint>().nextPoint = pointers[num, i].transform.position;
			if (i > 0)
				pointers[num, i - 1].GetComponent<LinePoint>().nextPoint = pointers[num, i].transform.position;
			//			print (Vector2.Distance (waypoints [1], pointers [i].transform.position));
			//			}    // removed section

		}*/
        for (int i = 0; i < GetAimPoints(); i++)
        {
            Vector2 AB = waypoints[2] - waypoints[1];
            AB = AB.normalized;
            float step = i / 2.75f;
            if (step < (waypoints[2] - waypoints[1]).magnitude)
            {
				try
				{
					pointers2[num, i].GetComponent<SpriteRenderer>().enabled = true;
                
                if (BoostVariables.AimBoost)
                    pointers2[num, i].GetComponent<LinePoint>().light.SetActive(true);

                pointers2[num, i].transform.position = waypoints[1] + (step * AB);
                pointers2[num, i].GetComponent<SpriteRenderer>().color = col;
                pointers2[num, i].GetComponent<LinePoint>().startPoint = pointers2[num, i].transform.position;
                pointers2[num, i].GetComponent<LinePoint>().nextPoint = pointers2[num, i].transform.position;
                if (i > 0)
                    pointers2[num, i - 1].GetComponent<LinePoint>().nextPoint = pointers2[num, i].transform.position;
				}
				catch
				{

				}
			}
        }
    }

	void GeneratePoints(int num = 0)
	{
		/*for (int i = 0; i < pointers.GetLength(1); i++) {
			pointers[num, i] = Instantiate(pointer, transform.position, transform.rotation) as GameObject;
			pointers[num, i].transform.parent = transform;
			pointers[num, i].GetComponent<LinePoint>().light.SetActive(false);
			pointers[num, i].GetComponent<LinePoint>().SetDraw(this);
		}
		for (int i = 0; i < pointers2.GetLength(1); i++) {
			pointers2[num, i] = Instantiate(pointer, transform.position, transform.rotation) as GameObject;
			pointers2[num, i].transform.parent = transform;
			pointers2[num, i].GetComponent<LinePoint>().light.SetActive(false);
			pointers2[num, i].GetComponent<LinePoint>().SetDraw(this);

		}*/
	}

	bool boostEnabled;
	
	// Update is called once per frame
	void Update()
	{
		
		if (BoostVariables.AimBoost && !boostEnabled) {
			boostEnabled = true;
			//EnableBoostLight();//commented ro prevent double line
		}
		if (Input.GetMouseButtonDown(0) && mainscript.Instance.startTimer) {
			draw = true;
			
		} else if (Input.GetMouseButtonUp(0)) {
			draw = false;
			rend.enabled = false;
		}

		if (draw && !mainscript.StopControl && GameEvent.Instance.GameStatus == GameState.Playing) {
			Draw(waypoints);
			if (mainscript.Instance.lauchingBall) {
				if (mainscript.Instance.lauchingBall.PowerUp == Powerups.TRIPLE) {
					Draw(waypoints, 1);
					Draw(waypoints, 2);
				}
			}
		} else if (!draw) {
			for (int i = 0; i < 3; i++) {
				HidePoints(i);
			}
		}

        
		//Draw(waypoints);
	}
	void Draw(Vector2[] waypoints_, int num = 0)
	{
		Vector3 dir = Vector2.zero;
		dir = -Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.back * 10;
		//Debug.Log("dir.y::" + dir.y);

		if (dir.y > 3.9f)//mainscript.Instance.lowerLineAngle  4.5
		{
			HideAllPoints();
			rend.enabled = false;
			return;
		}
       

		rend.enabled = true;
		if (num == 1) {
			dir.x +=2f;
		}
		if (num == 2) {
			dir.x -= 1.5f;
		}

		   // if( dir.y - 2 < transform.position.y ) { HidePoints(); return; }
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (!mainscript.StopControl && dir.y > -5.5f && dir.y < 3.5f) {//dir.y < 15.5 && dir.y > - 2 && 

			dir.z = 0;
			if (num == 0) {
				if (lastMousePos == dir) {
					startAnim = true;
				} else
					startAnim = false;
				lastMousePos = dir;
			}
			

			for (int i=0;i<waypoints.Length;i++)
            {
				rend.SetPosition(i, new Vector3(waypoints[i].x, waypoints[i].y, 0));
            }
			float width = rend.startWidth;
			rend.material.mainTextureScale = new Vector2(1f / width, 1f);
			SetLineRendererColor();

			//				waypoints [0] = transform.position;
			//int layerMask = ~(1 << LayerMask.NameToLayer("Mesh"));
			//			if (num == 0) {
			RaycastHit2D[] hit = Physics2D.RaycastAll(waypoints_[0] ,waypoints_[0] + ((Vector2)dir + waypoints_[0]).normalized * normalOffset, ~(1 << LayerMask.NameToLayer("Mesh")));
			foreach (RaycastHit2D item in hit) {
				Debug.Log("item layer::" + item.collider.gameObject.layer);

				Vector2 point = item.point;

				addAngle = 185;

				if (waypoints_[1].x < 0)
					addAngle = -5;
				if (item.collider.gameObject.layer == LayerMask.NameToLayer("Border") && item.collider.gameObject.name != "GameOverBorder" && item.collider.gameObject.name != "borderForRoundedLevels") {
					if (item.collider.name == "TopBorder" && LevelData.GetTarget() != TargetType.Round)
						//GetReversSquare(waypoints_[1], waypoints_[0], num);
					mainscript.Instance.collidingPoints[num] = point;
					if (num == 0) {
						mainscript.Instance.ballOnLine[0] = null;
						mainscript.Instance.touchTheBorder[0] = false;
					}
					waypoints_[1] = point;
					waypoints_[2] = point;
					float angle = 0;
					angle = Vector2.Angle(waypoints_[0] - waypoints_[1], (point - Vector2.up * normalOffset) - (Vector2)point);
					if (waypoints_[1].x > 0)
						angle = Vector2.Angle(waypoints_[0] - waypoints_[1], (Vector2)point - (point - Vector2.up * 110));
					waypoints_[2] = Quaternion.AngleAxis(angle + addAngle, Vector3.back) * ((Vector2)point - (point - Vector2.down * normalOffset));
					RaycastHit2D hit2 = Physics2D.Raycast(waypoints_[1], waypoints_[2], 1000, (1 << LayerMask.NameToLayer("Ball")));
					Debug.DrawLine(waypoints_[1], waypoints_[2]);
					Debug.DrawRay(waypoints_[1], waypoints_[2],Color.green);
					if (hit2.collider != null) {
						mainscript.Instance.ballOnLine[num] = hit2.collider.gameObject.GetComponent<Ball>();
						mainscript.Instance.touchTheBorder[num] = true;

						waypoints_[2] = hit2.point;


					}
					break;

				} else if (item.collider.gameObject.layer == LayerMask.NameToLayer("Ball")) {
					if (num == 0) {
						mainscript.Instance.reverseMesh[0] = null;

					}
					mainscript.Instance.ballOnLine[num] = item.collider.GetComponent<Ball>();
					mainscript.Instance.touchTheBorder[num] = false;
					waypoints_[1] = point;
					waypoints_[2] = point;
					//					}
					break;

				} else {
					mainscript.Instance.ballOnLine[num] = null;
					if (num == 0) {

						mainscript.Instance.reverseMesh[0] = null;
					}
					mainscript.Instance.touchTheBorder[num] = false;
					waypoints_[1] = waypoints_[0] + ((Vector2)dir - waypoints_[0]).normalized * 10;
					waypoints_[2] = waypoints_[0] + ((Vector2)dir - waypoints_[0]).normalized * 10;
				}
			}
			//			}

			//mohith commented
			//if (!startAnim)
			//	GeneratePositionsPoints(waypoints_, num);
		}
		
	}

	void GetReversSquare(Vector2 firstPos, Vector2 endPos, int num)
	{
		//		Debug.DrawLine (firstPos, endPos);
		RaycastHit2D[] hit = Physics2D.RaycastAll(firstPos ,endPos, 1 << 10);
		foreach (RaycastHit2D item in hit) {
			if (item.collider.gameObject.GetComponent<Square>().Busy == null) {
				mainscript.Instance.reverseMesh[num] = item.collider.GetComponent<Square>();
				Debug.Log("item " + item.collider.GetComponent<Square>());
								Debug.DrawLine (firstPos, item.collider.transform.position, Color.green);
				return;
			}

		}
		mainscript.Instance.reverseMesh[num] = null;

	}

	void SetLineRendererColor()
    {
		if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.color == ItemColor.BLUE)
		{
			rend.startColor = blueBubbleColor;
			rend.endColor = blueBubbleColor;
		}
		else if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.color == ItemColor.GREEN)
		{
			rend.startColor = greenBubbleColor;
			rend.endColor = greenBubbleColor;
		}
		else if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.color == ItemColor.ORANGE)
		{
			rend.startColor = orangeBubbleColor;
			rend.endColor = orangeBubbleColor;
		}
		else if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.color == ItemColor.RED)
		{
			rend.startColor = redBubbleColor;
			rend.endColor = redBubbleColor;
		}
        else if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.color == ItemColor.VIOLET)
        {
			rend.startColor = violetBubbleColor;
			rend.endColor = violetBubbleColor;
		}
        else if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.color == ItemColor.YELLOW)
        {
			rend.startColor = yellowBubbleColor;
			rend.endColor = yellowBubbleColor;
		}
		else if (mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.powerUp == Powerups.FIRE ||
				mainscript.Instance.boxCatapult.GetComponent<Square>().Busy.GetComponent<Ball>().itemKind.powerUp == Powerups.TRIPLE)
        {
			rend.startColor = powerupBubbleClr;
			rend.endColor = powerupBubbleClr;
        }
	}

}
