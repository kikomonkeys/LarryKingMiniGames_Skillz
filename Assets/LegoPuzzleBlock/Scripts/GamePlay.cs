using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Linq;

#if HBDOTween
using DG.Tweening;
#endif

// This script has main logic to run entire gameplay.
public class GamePlay : Singleton<GamePlay>, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
	[HideInInspector]
	public List<Block> blockGrid;

	ShapeInfo currentShape = null;
	Transform hittingBlock = null;

	List<Block> highlightingBlocks;

	public AudioClip blockPlaceSound;
	public AudioClip blockSelectSound;

	// Line break sounds.
	[SerializeField] private AudioClip lineClear1;
	[SerializeField] private AudioClip lineClear2;
	[SerializeField] private AudioClip lineClear3;
	[SerializeField] private AudioClip lineClear4;

	public AudioClip blockNotPlacedSound;

	[Tooltip("Max no. of times rescue can be used in 1 game. -1 is infinite")]
	[SerializeField] private int MaxAllowedRescuePerGame = 0;

	[Tooltip("Max no. of times rescue can be used in 1 game using watch video. -1 is infinite")]
	[SerializeField] private int MaxAllowedVideoWatchRescue = 0;

	[HideInInspector]
	public int TotalFreeRescueDone = 0;

	[HideInInspector]
	public int TotalRescueDone = 0;

	[HideInInspector]
	public int MoveCount = 0;

	public Sprite BombSprite;
	public BlockPuzzle_GameStake.Timer timeSlider;

	public bool isHelpOnScreen = false;

	public int currentID;
	//this bool is to check whether the line is filled before or not
	public bool isLineFilled;//mo
	public int streakCount;
	public int bonusScore;
	public GameObject bonusScoreObj;
	public Text bonusScoreText;
	public Text timerText;
	public float timer;
	public bool startTimer;
	public GameObject timeUp;
	public GameObject appreciationObj;
	public Sprite[] aprreciationSpr;
	public GameObject timeLeftObj, timeUpObj;
	GameObject currentShapeReflection;//piper
	public Sprite[] bonusStreakSpr;
	public List<Block> allBlocksList;

	public GameObject addTimerObj;
		

	void Start()
	{
		Application.targetFrameRate = 60;
		//Generate board from GameBoardGenerator Script Component.
		GetComponent<GameBoardGenerator>().GenerateBoard();
		highlightingBlocks = new List<Block>();
		Invoke(nameof(StartTimer), 3.0f);
		#region time mode
		// Timer will start with TIME and CHALLENGE mode.
		if (GameController.gameMode == GameMode.TIMED || GameController.gameMode == GameMode.CHALLENGE)
		{
			timeSlider.gameObject.SetActive(true);
		}
		#endregion

		#region check for help
		Invoke("CheckForHelp", 0.5F);
		#endregion


		//InvokeRepeating(nameof(ClearSomePercentOfTiles), 10,10);
	}

	#region IBeginDragHandler implementation

	/// <summary>
	/// Raises the begin drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (currentShape != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
			pos.z = currentShape.transform.localPosition.z;
			currentShape.transform.localPosition = pos;
		}
	}

	#endregion

	#region IPointerDownHandler implementation
	/// <summary>
	/// Raises the pointer down event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject != null) {
			Transform clickedObject = eventData.pointerCurrentRaycast.gameObject.transform;

			if (clickedObject.GetComponent<ShapeInfo>() != null)
			{
				if (clickedObject.transform.childCount > 0) {
					currentShape = clickedObject.GetComponent<ShapeInfo>();
					currentShapeReflection = currentShape.transform.Find("Reflection").gameObject;//piper
					currentShapeReflection.SetActive(false);
					Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
					currentShape.transform.localScale = Vector3.one;
					currentShape.transform.localPosition = new Vector3(pos.x, pos.y, 0);
					AudioManager.Instance.PlaySound(blockSelectSound);

					if (isHelpOnScreen) {
						GetComponent<InGameHelp>().StopHelp();
					}
				}
			}
		}
	}
	#endregion

	#region IPointerUpHandler implementation
	/// <summary>
	/// Raises the pointer up event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		if (currentShape != null) {

			if (highlightingBlocks.Count > 0)
			{
				SetImageToPlacingBlocks();
				//Debug.Log("on pointer up::" + currentShape + "is in the board");
				Destroy(currentShape.gameObject);
				currentShape = null;
				MoveCount += 1;
				Invoke("CheckBoardStatus", 0.1F);
			} else {
#if HBDOTween
				currentShape.transform.DOLocalMove(Vector3.zero, 0.5F);
				currentShape.transform.DOScale(Vector3.one * 0.6F, 0.5F);
				Invoke(nameof(EnableReflection), 0.35f);

#endif
				currentShape = null;
				AudioManager.Instance.PlaySound(blockNotPlacedSound);
				//Debug.Log("on pointer up::" + currentShape + "is not placed in the board");

			}
		}
	}
	#endregion
	void EnableReflection()
	{
		currentShapeReflection.SetActive(true);
	}
	#region IDragHandler implementation
	/// <summary>
	/// Raises the drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrag(PointerEventData eventData)
	{
		if (currentShape != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
			pos = new Vector3(pos.x, (pos.y + 1F), 0F);

			currentShape.transform.position = pos;

			RaycastHit2D hit = Physics2D.Raycast(currentShape.GetComponent<ShapeInfo>().firstBlock.block.position, Vector2.zero, 1);

			if (hit.collider != null)
			{
				if (hittingBlock == null || hit.collider.transform != hittingBlock) {
					hittingBlock = hit.collider.transform;
					CanPlaceShape(hit.collider.transform);
				}
			} else {
				StopHighlighting();
			}
		}
	}
	#endregion

	/// <summary>
	/// Determines whether this instance can place shape the specified currentHittingBlock.
	/// </summary>
	public bool CanPlaceShape(Transform currentHittingBlock)
	{
		Block currentCell = currentHittingBlock.GetComponent<Block>();

		int currentRowID = currentCell.rowID;
		int currentColumnID = currentCell.columnID;

		StopHighlighting();

		bool canPlaceShape = true;
		foreach (ShapeBlock c in currentShape.ShapeBlocks)
		{
			Block checkingCell = blockGrid.Find(o => o.rowID == currentRowID + (c.rowID + currentShape.startOffsetX) && o.columnID == currentColumnID + (c.columnID - currentShape.startOffsetY));

			if ((checkingCell == null) || (checkingCell != null && checkingCell.isFilled)) {
				canPlaceShape = false;
				highlightingBlocks.Clear();
				break;
			} else {
				if (!highlightingBlocks.Contains(checkingCell)) {
					highlightingBlocks.Add(checkingCell);
				}
			}
		}

		if (canPlaceShape) {
			SetHighLightImage();
		}

		return canPlaceShape;
	}

	/// <summary>
	/// Sets the high light image.
	/// </summary>
	void SetHighLightImage()
	{
		foreach (Block c in highlightingBlocks) {
			c.SetHighlightImage(currentShape.blockImage);
		}
	}

	/// <summary>
	/// Stops the highlighting.
	/// </summary>
	void StopHighlighting()
	{
		if (highlightingBlocks != null && highlightingBlocks.Count > 0) {
			foreach (Block c in highlightingBlocks) {
				c.StopHighlighting();
			}
		}
		hittingBlock = null;
		highlightingBlocks.Clear();
	}

	/// <summary>
	/// Sets the image to placing blocks.
	/// </summary>
	void SetImageToPlacingBlocks()
	{
		if (highlightingBlocks != null && highlightingBlocks.Count > 0)
		{
			foreach (Block c in highlightingBlocks) {
				c.SetBlockImage(currentShape.blockImage, currentShape.ShapeID);
				//allBlocksList.Add(c);//mo
			}
		}
		currentID = int.Parse(currentShape.blockImage.name) - 1;
		Debug.Log("current iD::" + currentID);
		AudioManager.Instance.PlaySound(blockPlaceSound);
	}
	[SerializeField]
	List<Block> tempBlockList;
	int a, b;
	public IEnumerator ClearSomePercentOfTiles(float waittime)
    {
		yield return new WaitForSeconds(waittime);
        for (int i = 0; i < allBlocksList.Count; i++)
        {
            if (allBlocksList[i].blockID != -1)
            {
				tempBlockList.Add(allBlocksList[i]);
            }
        }
		yield return new WaitForSeconds(0.5f);

		int num;
		num = Random.Range(0, 4);
        if (num % 2 == 0)
        {
			a = 0;
			b = tempBlockList.Count / 2;
		}
        else
        {
			a = tempBlockList.Count / 2;
			b = tempBlockList.Count;
		}

		for (int i = a; i < b; i++)   
        {
			Debug.LogError("block nameL::" + tempBlockList[i].name);
			tempBlockList[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
			tempBlockList[i].transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
			tempBlockList[i].name = "block-" + i;
			tempBlockList[i].ClearBlock(Random.Range(1, 7));
			yield return new WaitForSeconds(0.01f);
			//tempBlockList[i].blockID = -1;
			//tempBlockList[i].isFilled = false;
			
        }
    }

	public void ClearSomePercentOfTilesNow()
    {
		StartCoroutine(ClearSomePercentOfTiles(0f));
    }

	/// <summary>
	/// Checks the board status.
	/// </summary>
	void CheckBoardStatus()
	{
		int placingShapeBlockCount = highlightingBlocks.Count;
		List<int> updatedRows = new List<int>();
		List<int> updatedColumns = new List<int>();

		List<List<Block>> breakingRows = new List<List<Block>>();
		List<List<Block>> breakingColumns = new List<List<Block>>();

		foreach (Block b in highlightingBlocks) {
			if (!updatedRows.Contains(b.rowID)) {
				updatedRows.Add(b.rowID);
			}
			if (!updatedColumns.Contains(b.columnID)) {
				updatedColumns.Add(b.columnID);
			}
		}
		highlightingBlocks.Clear();

		foreach (int rowID in updatedRows) {
			List<Block> currentRow = GetEntireRow(rowID);
			if (currentRow != null) {
				breakingRows.Add(currentRow);
			}
		}

		foreach (int columnID in updatedColumns) {
			List<Block> currentColumn = GetEntireColumn(columnID);
			if (currentColumn != null) {
				breakingColumns.Add(currentColumn);
			}
		}

		if (breakingRows.Count > 0 || breakingColumns.Count > 0) {
			isLineFilled = true;//mo
			StartCoroutine(BreakAllCompletedLines(breakingRows, breakingColumns, placingShapeBlockCount));
		} else {
			BlockShapeSpawner.Instance.FillShapeContainer();
			//Debug.LogError("cube placed in the box");
			isLineFilled = false;
			streakCount = 0;
			//bonusScore = 0;

			ScoreManager.Instance.AddScore(10 * placingShapeBlockCount);
		}

		if (GameController.gameMode == GameMode.BLAST || GameController.gameMode == GameMode.CHALLENGE) {
			Invoke("UpdateBlockCount", 0.5F);
		}
	}

	/// <summary>
	/// Gets the entire row.
	/// </summary>
	/// <returns>The entire row.</returns>
	/// <param name="rowID">Row I.</param>
	List<Block> GetEntireRow(int rowID)
	{
		List<Block> thisRow = new List<Block>();
		for (int columnIndex = 0; columnIndex < GameBoardGenerator.Instance.TotalColumns; columnIndex++) {
			Block block = blockGrid.Find(o => o.rowID == rowID && o.columnID == columnIndex);

			if (block.isFilled) {
				thisRow.Add(block);
			} else {
				return null;
			}
		}
		return thisRow;
	}

	/// <summary>
	/// Gets the entire row for rescue.
	/// </summary>
	/// <returns>The entire row for rescue.</returns>
	/// <param name="rowID">Row I.</param>
	List<Block> GetEntireRowForRescue(int rowID)
	{
		List<Block> thisRow = new List<Block>();
		for (int columnIndex = 0; columnIndex < GameBoardGenerator.Instance.TotalColumns; columnIndex++) {
			Block block = blockGrid.Find(o => o.rowID == rowID && o.columnID == columnIndex);
			thisRow.Add(block);
		}
		return thisRow;
	}

	/// <summary>
	/// Gets the entire column.
	/// </summary>
	/// <returns>The entire column.</returns>
	/// <param name="columnID">Column I.</param>
	List<Block> GetEntireColumn(int columnID)
	{
		List<Block> thisColumn = new List<Block>();
		for (int rowIndex = 0; rowIndex < GameBoardGenerator.Instance.TotalRows; rowIndex++) {
			Block block = blockGrid.Find(o => o.rowID == rowIndex && o.columnID == columnID);
			if (block.isFilled) {
				thisColumn.Add(block);
			} else {
				return null;
			}
		}
		return thisColumn;
	}

	/// <summary>
	/// Gets the entire column for rescue.
	/// </summary>
	/// <returns>The entire column for rescue.</returns>
	/// <param name="columnID">Column I.</param>
	List<Block> GetEntireColumnForRescue(int columnID)
	{
		List<Block> thisColumn = new List<Block>();
		for (int rowIndex = 0; rowIndex < GameBoardGenerator.Instance.TotalRows; rowIndex++) {
			Block block = blockGrid.Find(o => o.rowID == rowIndex && o.columnID == columnID);
			thisColumn.Add(block);
		}
		return thisColumn;
	}

	/// <summary>
	/// Breaks all completed lines.
	/// </summary>
	/// <returns>The all completed lines.</returns>
	/// <param name="breakingRows">Breaking rows.</param>
	/// <param name="breakingColumns">Breaking columns.</param>
	/// <param name="placingShapeBlockCount">Placing shape block count.</param>
	/// <param name="placingShapeBlockCount">Placing shape block count.</param>
	IEnumerator BreakAllCompletedLines(List<List<Block>> breakingRows, List<List<Block>> breakingColumns, int placingShapeBlockCount)
	{
		int TotalBreakingLines = breakingRows.Count + breakingColumns.Count;
		//Debug.LogError("total breaking lines::" + TotalBreakingLines);
		if (TotalBreakingLines == 1) {
			AudioManager.Instance.PlaySound(lineClear1);
			ScoreManager.Instance.AddScore(120 + (placingShapeBlockCount * 10));
		} else if (TotalBreakingLines == 2) {
			AudioManager.Instance.PlaySound(lineClear2);
			ScoreManager.Instance.AddScore(300 + (placingShapeBlockCount * 10));
		}
		else if (TotalBreakingLines == 3) {
			AudioManager.Instance.PlaySound(lineClear3);
			ScoreManager.Instance.AddScore(600 + (placingShapeBlockCount * 10));
		}
		else if (TotalBreakingLines >= 4) {
			AudioManager.Instance.PlaySound(lineClear4);
			if (TotalBreakingLines == 4) {
				ScoreManager.Instance.AddScore(1000 + (placingShapeBlockCount * 10));
			}
			else {
				ScoreManager.Instance.AddScore((300 * TotalBreakingLines) + (placingShapeBlockCount * 10));
			}

		}
		if (isLineFilled)
			GetStreakBonus();//mo



		yield return 0;
		if (breakingRows.Count > 0) {
			foreach (List<Block> thisLine in breakingRows) {
				StartCoroutine(BreakThisLine(thisLine));
			}
		}
		if (breakingRows.Count > 0) {
			yield return new WaitForSeconds(0.1F);
		}

		if (breakingColumns.Count > 0) {
			foreach (List<Block> thisLine in breakingColumns) {
				StartCoroutine(BreakThisLine(thisLine));
			}
		}


		BlockShapeSpawner.Instance.FillShapeContainer();

		#region time mode
		if (GameController.gameMode == GameMode.TIMED || GameController.gameMode == GameMode.CHALLENGE) {
			timeSlider.AddSeconds(TotalBreakingLines * 5);
		}
		#endregion
	}
	/// <summary>
	/// show appreciation if a row is cleared//mohith
	/// </summary>
	IEnumerator ShowAppreciation(float waittime, int rowCount = 0)
	{
		//string[] firstRowStr = new string[] { "Good Job", "Great Going", "Sweet" };
		//string[] multiRowStr = new string[] { "Fantastic", "Superb", "Well Done", "Awesome", "Amazing" };
		
		yield return new WaitForSeconds(waittime);
		appreciationObj.SetActive(true);
		appreciationObj.GetComponentInChildren<Image>().sprite = aprreciationSpr[Random.Range(0, aprreciationSpr.Length)];
		//if (rowCount == 1)
		//      {
		//	string randStr;
		//	randStr = firstRowStr[Random.Range(0, firstRowStr.Length)];
		//	appreciationObj.GetComponentInChildren<Text>().text = randStr;
		//}
		//      else
		//      {
		//	string randStr;
		//	randStr = multiRowStr[Random.Range(0, multiRowStr.Length)];
		//	appreciationObj.GetComponentInChildren<Text>().text = randStr;
		//}
		yield return new WaitForSeconds(1f);
		appreciationObj.SetActive(false);
	}
	/// <summary>
	/// This method is to give streak bonus to the user 
	/// </summary>

	public int totalBonusScore;
	int prevBonusScore, tempBonus;
	public bool isShowingBonusStreak;
	public void GetStreakBonus()
	{
		streakCount++;
		//Debug.LogError("islinefilled in getstreakbonus method::" + isLineFilled);
		if (isLineFilled)
		{
			if (streakCount == 1)
			{
				//bonusScore = 0;
				StartCoroutine(ShowAppreciation(0.1f));

			}
			else if (streakCount == 2)
			{
				//ScoreManager.Instance.AddScore(200);
				bonusScore = prevBonusScore + 200;
				tempBonus = 200;
				StartCoroutine(ShowBonusScore(0.1f, "2 in a row"));
			}
			else if (streakCount == 3)
			{
				//ScoreManager.Instance.AddScore(400);
				bonusScore = prevBonusScore + 400;
				tempBonus = 400;
				StartCoroutine(ShowBonusScore(0.1f, "3 in a row"));
			}
			else if (streakCount == 4)
			{
				//ScoreManager.Instance.AddScore(600);
				bonusScore = prevBonusScore + 600;
				tempBonus = 600;
				StartCoroutine(ShowBonusScore(0.1f, "4 in a row"));
			}
			else if (streakCount == 5)
			{
				//ScoreManager.Instance.AddScore(1000);
				bonusScore = prevBonusScore + 1000;
				tempBonus = 1000;
				StartCoroutine(ShowBonusScore(0.1f, "5 in a row"));
			}
			else
			{
				//ScoreManager.Instance.AddScore(1000);
				bonusScore = prevBonusScore + 1000;
				tempBonus = 1000;
				StartCoroutine(ShowBonusScore(0.1f, "Bonus Streak"));
			}
			prevBonusScore = bonusScore;

			totalBonusScore = prevBonusScore;
			//isLineFilled = false;
		}
		//Debug.LogError("Bonus score is::" + bonusScore + "total score::" + totalBonusScore);
	}
	IEnumerator ShowBonusScore(float waittime, string bonusStr)
	{
		yield return new WaitForSeconds(waittime);
		isShowingBonusStreak = true;
		bonusScoreObj.SetActive(true);
		if (bonusStr == "2 in a row") bonusScoreObj.GetComponentInChildren<Image>().sprite = bonusStreakSpr[0];
		if (bonusStr == "3 in a row") bonusScoreObj.GetComponentInChildren<Image>().sprite = bonusStreakSpr[1];
		if (bonusStr == "4 in a row") bonusScoreObj.GetComponentInChildren<Image>().sprite = bonusStreakSpr[2];
		if (bonusStr == "5 in a row") bonusScoreObj.GetComponentInChildren<Image>().sprite = bonusStreakSpr[3];
		if (bonusStr == "Bonus Streak") bonusScoreObj.GetComponentInChildren<Image>().sprite = bonusStreakSpr[4];
		bonusScoreObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "+" + bonusStr;
		bonusScoreText.text = tempBonus.ToString();
		yield return new WaitForSeconds(1);
		bonusScoreObj.SetActive(false);
		isShowingBonusStreak = false;

	}
	/// <summary>
	/// Breaks the this line.
	/// </summary>
	/// <returns>The this line.</returns>
	/// <param name="breakingLine">Breaking line.</param>
	IEnumerator BreakThisLine(List<Block> breakingLine)
	{
		foreach (Block b in breakingLine) {
			//b.ClearBlock (int.Parse(breakingLine[breakingLine.Count-1].blockImage.GetComponent<Image>().sprite.name) -1);
			Debug.Log("current id::" + currentID);
            b.ClearBlock(currentID);
			yield return new WaitForSeconds (0.02F);
		}
		yield return 0;
	}

	/// <summary>
	/// Determines whether this instance can existing blocks placed the specified OnBoardBlockShapes.
	/// </summary>
	/// <returns><c>true</c> if this instance can existing blocks placed the specified OnBoardBlockShapes; otherwise, <c>false</c>.</returns>
	/// <param name="OnBoardBlockShapes">On board block shapes.</param>
	public bool CanExistingBlocksPlaced(List<ShapeInfo> OnBoardBlockShapes)
	{
		foreach (Block block in blockGrid) {
			if (!block.isFilled) {
				foreach (ShapeInfo info in OnBoardBlockShapes) {
					bool canPlace = CheckShapeCanPlace (block, info);
					if (canPlace) {
						return true;
					}
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Checks the shape can place.
	/// </summary>
	/// <returns><c>true</c>, if shape can place was checked, <c>false</c> otherwise.</returns>
	/// <param name="placingBlock">Placing block.</param>
	/// <param name="placingBlockShape">Placing block shape.</param>
	bool CheckShapeCanPlace(Block placingBlock, ShapeInfo placingBlockShape)
	{
		int currentRowID = placingBlock.rowID;
		int currentColumnID = placingBlock.columnID;

		if (placingBlockShape != null && placingBlockShape.ShapeBlocks != null) {
			foreach (ShapeBlock c in placingBlockShape.ShapeBlocks) {
				Block checkingCell = blockGrid.Find (o => o.rowID == currentRowID + (c.rowID + placingBlockShape.startOffsetX) && o.columnID == currentColumnID + (c.columnID - placingBlockShape.startOffsetY));

				if ((checkingCell == null) || (checkingCell != null && checkingCell.isFilled)) {
					return false;
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Raises the unable to place shape event.
	/// </summary>
	public void OnUnableToPlaceShape()
	{
		if ((TotalRescueDone < MaxAllowedRescuePerGame) || MaxAllowedRescuePerGame < 0) {
			GamePlayUI.Instance.ShowRescue (GameOverReason.OUT_OF_MOVES);
		} else {
			OnGameOver ();
		}
	}

	/// <summary>
	/// Raises the bomb counter over event.
	/// </summary>
	public void OnBombCounterOver(){
		if ((TotalRescueDone < MaxAllowedRescuePerGame) || MaxAllowedRescuePerGame < 0) {
			GamePlayUI.Instance.ShowRescue (GameOverReason.BOMB_COUNTER_ZERO);
		} else {
			OnGameOver ();
		}
	}

	/// <summary>
	/// Executes the rescue.
	/// </summary>
	void ExecuteRescue()
	{
		if (GamePlayUI.Instance.currentGameOverReson == GameOverReason.OUT_OF_MOVES) {
			int TotalBreakingColumns = 3;
			int TotalBreakingRows = 3;

			int totalColumns = GameBoardGenerator.Instance.TotalColumns;
			int totalRows = GameBoardGenerator.Instance.TotalRows;

			int startingColumn = (int)((totalColumns / 2F) - (TotalBreakingColumns / 2F));
			int startingRow = (int)((totalRows / 2F) - (TotalBreakingRows / 2F));

			List<List<Block>> breakingColums = new List<List<Block>> ();

			for (int columnIndex = startingColumn; columnIndex <= (startingColumn + (TotalBreakingColumns - 1)); columnIndex++) {
				breakingColums.Add (GetEntireColumnForRescue (columnIndex));
			}

			List<List<Block>> breakingRows = new List<List<Block>> ();

			for (int rowIndex = startingRow; rowIndex <= (startingRow + (TotalBreakingRows - 1)); rowIndex++) {
				breakingRows.Add (GetEntireRowForRescue (rowIndex));
			}
			StartCoroutine (BreakAllCompletedLines (breakingRows, breakingColums, 0));
		}

		#region bomb mode
		if ((GameController.gameMode == GameMode.BLAST || GameController.gameMode == GameMode.CHALLENGE) && GamePlayUI.Instance.currentGameOverReson == GameOverReason.BOMB_COUNTER_ZERO) {
			List<Block> bombBlocks = blockGrid.FindAll (o => o.isBomb);
			foreach (Block block in bombBlocks) {
				if (block.bombCounter <= 1) {
					block.SetCounter (block.bombCounter + 4);
				}
				block.DecreaseCounter ();
			}
		}
		#endregion

		#region time mode
		if (GameController.gameMode == GameMode.TIMED || GameController.gameMode == GameMode.CHALLENGE) 
		{
			if(GamePlayUI.Instance.currentGameOverReson == GameOverReason.TIME_OVER)
			{
				timeSlider.AddSeconds(30);
			}
			timeSlider.ResumeTimer();
		}
		#endregion
	}

	/// <summary>
	/// Ises the free rescue available.
	/// </summary>
	/// <returns><c>true</c>, if free rescue available was ised, <c>false</c> otherwise.</returns>
	public bool isFreeRescueAvailable()
	{
		if((TotalFreeRescueDone < MaxAllowedVideoWatchRescue) || (MaxAllowedVideoWatchRescue < 0))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Raises the rescue done event.
	/// </summary>
	/// <param name="isFreeRescue">If set to <c>true</c> is free rescue.</param>
	public void OnRescueDone(bool isFreeRescue) 
	{
		if (isFreeRescue) {
			TotalFreeRescueDone += 1;
		} else {
			TotalRescueDone += 1;
		}
		CloseRescuePopup ();
		Invoke ("ExecuteRescue", 0.5F);
	}

	/// <summary>
	/// Raises the rescue discarded event.
	/// </summary>
	public void OnRescueDiscarded()
	{
        CloseRescuePopup();
		OnGameOver ();
	}

	/// <summary>
	/// Closes the rescue popup.
	/// </summary>
	void CloseRescuePopup()
	{
		GameObject currentWindow = StackManager.Instance.WindowStack.Peek ();
		if (currentWindow != null) {
			if (currentWindow.OnWindowRemove () == false) {
				Destroy (currentWindow);
			}
			StackManager.Instance.WindowStack.Pop ();
			
		}
	}

	/// <summary>
	/// Raises the game over event.
	/// </summary>
	public void OnGameOver()
	{

		//GameObject gameOverScreen = StackManager.Instance.SpawnUIScreen ("GameOver");
		//GameController.instance.ShowLCOptionNow();
	} 
	public void ShowLC()
    {
		GameObject gameOverScreen = GameController.instance.gameOverPage;
		gameOverScreen.SetActive(true);

		gameOverScreen.GetComponent<GameOver>().SetLevelScore(FindObjectOfType<ScoreManager>().GetScore(), 10, totalBonusScore);
		startTimer = false;

	}
	IEnumerator ShowAds()
    {
        yield return new WaitForSeconds(0.5f);
       // AdsControl.Instance.showAds();
    }

    private void Update()
    {
        if (startTimer)
        {
			SetTimer();
        }
    }
	public void StartTimer()
    {
		startTimer = true;
    }
	float min, sec;
	bool b1, b2, b3;
	void SetTimer()
    {
		timer -= Time.deltaTime;
		min = Mathf.CeilToInt(timer) / 60;
		sec = Mathf.CeilToInt(timer) % 60;
		timerText.text = "" + min.ToString("0") + ":" + sec.ToString("00");
        if (timer <= 120 && timer >= 60)
        {
            if (!b1)
            {
                if (isShowingBonusStreak)
                {
					StartCoroutine(ShowTimeLeft(1.2f, "2MinLeft"));
				}
                else
                {
					StartCoroutine(ShowTimeLeft(0f, "2MinLeft"));
				}
				b1 = true;
			}
		}
		else if (timer <= 60 && timer >= 30)
        {
			if(!b2)
            {
                if (isShowingBonusStreak)
                {
					StartCoroutine(ShowTimeLeft(1.2f, "1minLeft"));
				}
				else
                {
					StartCoroutine(ShowTimeLeft(0f, "1minLeft"));
				}
				b2 = true;
			}
		}
		else if (timer <= 30 && timer>=0)
        {
            if (!b3)
            {
                if (isShowingBonusStreak)
                {
					StartCoroutine(ShowTimeLeft(1.2f, "30sLeft"));
				}
				else
                {
					StartCoroutine(ShowTimeLeft(0f, "30sLeft"));
				}
				b3 = true;
			}
		}

		if (timer <= 0)
		{
			startTimer = false;
            if (isShowingBonusStreak)
            {
				StartCoroutine(LoadTimeUpGameOVer(1.2f));
			}
            else
            {
				StartCoroutine(LoadTimeUpGameOVer(0.1f));
			}
		}
	}
	IEnumerator LoadTimeUpGameOVer(float waittime)
    {
		yield return new WaitForSeconds(waittime);
		timeUp.SetActive(true);
		yield return new WaitForSeconds(2f);
		timeUp.SetActive(false);
		GameController.instance.isTimeUp = true;
		GameController.instance.ShowLCOptionNow();
		//OnGameOver();

	}
	public Sprite[] timerSpr;
	IEnumerator ShowTimeLeft(float waittime, string str)
    {
		yield return new WaitForSeconds(waittime);
		timeLeftObj.SetActive(true);
		//timeLeftObj.GetComponentInChildren<Text>().text = str;
		if(str == "2MinLeft")
        {
			timeLeftObj.GetComponentInChildren<Image>().sprite = timerSpr[0];
        }
		else if(str== "1minLeft")
        {
			timeLeftObj.GetComponentInChildren<Image>().sprite = timerSpr[1];
		}
		else if(str== "30sLeft")
        {
			timeLeftObj.GetComponentInChildren<Image>().sprite = timerSpr[2];
		}
		yield return new WaitForSeconds(1.5f);
		timeLeftObj.SetActive(false);
	}
	#region Bomb Mode Specific
	/// <summary>
	/// Updates the block count.
	/// </summary>
	void UpdateBlockCount()
	{
		List<Block> bombBlocks = blockGrid.FindAll (o => o.isBomb);
		foreach (Block block in bombBlocks) {
			block.DecreaseCounter ();
		}

		bool doPlaceBomb = false;
		if (MoveCount <= 10) {
			if (MoveCount % 5 == 0) {
				doPlaceBomb = true;
			}
		} else if (MoveCount <= 30) {
			if (((MoveCount-10) % 4 == 0)) {
				doPlaceBomb = true;
			}

		} else if (MoveCount <= 48) {
			if (((MoveCount-30) % 3 == 0)) {
				doPlaceBomb = true;
			}
		} else {
			if (((MoveCount-48) % 2 == 0)) {
				doPlaceBomb = true;
			}
		}

		if (doPlaceBomb) {
			PlaceAtRandomPlace ();
		}
	}

	/// <summary>
	/// Places at random place.
	/// </summary>
	void PlaceAtRandomPlace()
	{
		List<int> BombPlacingRows = new List<int> ();

		for (int rowIndex = 0; rowIndex < GameBoardGenerator.Instance.TotalRows; rowIndex++) {
			bool canPlace = CheckRowForBombPlace (rowIndex);
			if (canPlace) {
				BombPlacingRows.Add (rowIndex);
			}
		}

		int RandomRowForPlacingBomb = 0;
		if (BombPlacingRows.Count > 0) {
			RandomRowForPlacingBomb = BombPlacingRows [UnityEngine.Random.Range (0, BombPlacingRows.Count)];
		} else {
			RandomRowForPlacingBomb = UnityEngine.Random.Range (0, GameBoardGenerator.Instance.TotalRows);
		}

		PlaceBombAtRow (RandomRowForPlacingBomb);
	}

	/// <summary>
	/// Places the bomb at row.
	/// </summary>
	/// <param name="rowIndex">Row index.</param>
	void PlaceBombAtRow(int rowIndex)
	{
		List<Block> emptyBlockForThisRow = blockGrid.FindAll (o => o.rowID == rowIndex && o.isFilled == false); 
		Block randomBlock = emptyBlockForThisRow [UnityEngine.Random.Range (0, emptyBlockForThisRow.Count)];

		if (randomBlock != null) {
			randomBlock.ConvertToBomb ();
		}
	}

	/// <summary>
	/// Checks the row for bomb place.
	/// </summary>
	/// <returns><c>true</c>, if row for bomb place was checked, <c>false</c> otherwise.</returns>
	/// <param name="rowIndex">Row index.</param>
	bool CheckRowForBombPlace(int rowIndex)
	{
		Block block = blockGrid.Find (o => o.rowID == rowIndex && o.isFilled == true);
		int totalEmptyBlocks = blockGrid.FindAll (o => o.rowID == rowIndex && o.isFilled == false).Count;

		if (block != null && totalEmptyBlocks >= 2) {
			return true;
		} else {
			return false;
		}
	}
	#endregion


	#region show help if mode opened first time
	/// <summary>
	/// Checks for help.
	/// </summary>
	public void CheckForHelp()
	{
		bool isHelpShown = false;
		if (GameController.gameMode == GameMode.BLAST || GameController.gameMode == GameMode.ADVANCE || GameController.gameMode == GameMode.TIMED) {
			isHelpShown = PlayerPrefs.GetInt ("isHelpShown_" + GameController.gameMode.ToString (), 0) == 0 ? false : true;
			if (!isHelpShown) {
				InGameHelp inGameHelp = gameObject.AddComponent<InGameHelp> ();
				inGameHelp.StartHelp ();
			} else {
				ShowBasicHelp ();
			}
		} else {
			ShowBasicHelp ();
		}
	}

	/// <summary>
	/// Raises the help popup closed event.
	/// </summary>
	public void OnHelpPopupClosed()
	{
		if (GameController.gameMode == GameMode.TIMED) {
			timeSlider.ResumeTimer ();
		} 
		ShowBasicHelp ();
	}

	/// <summary>
	/// Shows the basic help.
	/// </summary>
	void ShowBasicHelp()
	{
		bool isBasicHelpShown = PlayerPrefs.GetInt ("isBasicHelpShown", 0) == 0 ? false : true;
		if (!isBasicHelpShown) {
			InGameHelp inGameHelp = gameObject.GetComponent<InGameHelp> ();
			if (inGameHelp == null) {
				inGameHelp = gameObject.AddComponent<InGameHelp> ();
			}
			inGameHelp.ShowBasicHelp ();
		}
	}
	#endregion

	
}