using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardItem : MonoBehaviour {

    private const int limitCardLandSpace = 11;
    private const int limitCardPortrait = 22;
    // 
    //  *********************************
    //  ***********  FIELDS  ************
    //  *********************************
    // 
    public bool isRoot = false;
    public bool isTableu = false;
	public bool isFoundation = false;
	public bool isDeck = false;
	public int debugZoneIndex = 0;

	[SerializeField]
	private Image faceImage;
	[SerializeField]
	private Image backImage;
	[SerializeField]
	private SimpleEffectAbstract highlightEffect;
	[SerializeField]
	private SimpleEffectAbstract blinkEffect;
	[SerializeField]
	private SaluteAnimator salute;
	[SerializeField]
	private Vector2 offsetForAllChilds;
	[SerializeField]
	private Vector2 offsetForAllChildsOpened;

	[Header("Debug")]
    [HideInInspector]
	public CardItem parentCard;	
	[HideInInspector]
	public CardItem _childCard;
 
    public CardItem rootCard;

	public int Id;
	public bool isOppened;
    public bool Hide;
    public int Suit;
	public int Rank;
	public bool isGivenScoreToCard = false;





	private RectTransform _rect;

	public bool clickable{
		get{ 
			return isOppened;
		}
	}
 
    public bool King { get { return Rank ==13; } }
    public bool Ace { get { return Rank == 1; } }

    public bool isRedSuit { get { return Suit == 0 || Suit == 1; } }
    public bool isBlackSuit { get { return Suit == 2 || Suit == 3; } }




    // 
    //	*********************************
    //	*******  INITIALISATION  ********
    //	*********************************
    //

    private CardItem(){}
	private ManagerLogic managerLogic;
	public void initCard(int id, bool isOpen, int suit, int rank)//(Card card)
	{
		Id = id;
		isOppened = isOpen;
		Suit = suit;
		Rank = rank;
		managerLogic = new ManagerLogic();
//		cardModel = card;
		if (!isRoot) {

			//piper generate random card bg here
			GameSettings.Instance.visualCardBacksSet = StageManager.instance.randomCardBackImage;

			backImage.sprite = ImageSettings.Instance.cardbackHiResolurion[GameSettings.Instance.visualCardBacksSet];
			openCard (isOpen);//(card.IsOpened);
		}
        {
            CardHeader cardHeader = GetComponent<CardHeader>();
            if (cardHeader != null)  
            cardHeader.Init(id, this);
        }
		_rect = GetComponent<RectTransform>();
		//Debug.Log("card name is::" + transform.name);
		transform.name = "Card id: " + Id;//card.Id;
		
        _rect.transform.localScale = SolitaireStageViewHelperClass.instance.ConvertSizeCard(DeviceOrientationHandler.instance.isVertical);

        
    }


    public bool CardInFoundationStack()
    {
        CardItem parent = FirstCard();

        return (parent.Id == -101 || parent.Id == -102 || parent.Id == -103 || parent.Id == -104);
    }

    public bool CanClick()
    {
        CardItem parent = FirstCard();
        
        return (parent.Id == -300 && !this.hasChildCard);
    }

    public CardItem FirstCard()
    {
        CardItem parent = parentCard;
        while (parent != null)
        {
            if (parent.parentCard == null)
            {
                break;
            }
            parent = parent.parentCard;
        }
        return parent;
    }

    public void attachListener(ICardItemActions listener){
		// add touch listener
		CardTouchHandler touchHandler = gameObject.AddComponent<CardTouchHandler> ();
		touchHandler.Init (this, listener);
	}

	public void UpdateCardBack (){
#if UNITY_EDITOR
        if (isRoot) {
			throw new UnityException ("Root card hasn't back image");
		}
#endif

        backImage.sprite = ImageSettings.Instance.cardbackHiResolurion[GameSettings.Instance.visualCardBacksSet];
	}


// 
//	*********************************
//	*********  PROPERTIES  **********
//	*********************************
// 

	public bool allowClick{
		get{ 
			return faceImage.raycastTarget || backImage.raycastTarget;
		}
		set{ 
			faceImage.raycastTarget = backImage.raycastTarget = value;
		}
	}

//	public int Id {
//		get {
//			return cardModel.Id;
//		}
//	}
//	public bool isOppened {
//		get {
//			if (cardModel == null)
//				return false;
//			return cardModel.IsOpened;
//		}
//	}
	public RectTransform rect {
		get {
			return _rect;
		}
	}

	public bool hasChildCard {
		get {
			return _childCard != null;
		}
	}

	public CardItem childCard{
		get{
			return _childCard;
		}
	}
	public List<CardItem> getChildCardsList(){
		List<CardItem> childCards = new List<CardItem> ();
		if(!hasChildCard){
			return childCards;
		}else{
            CardItem[] childs = GetComponentsInChildren<CardItem>();
            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] == this) continue;
                childCards.Add(childs[i]);
            }

           
			return childCards;
		}
	}
	public CardItem getChildestCard(){
		if(!hasChildCard){
			return this;
		}else{
            CardItem[] childs = GetComponentsInChildren<CardItem>();

            CardItem c = childs[childs.Length - 1];
            /*
            CardItem c = childCard;

            while (c.hasChildCard){
				c = c.childCard;
			}
            */
			return c;
		}
	}

	public void childCardNull () {
		_childCard = null;
	}

	public CardItem getParentCard(){
		Transform t = gameObject.transform.parent;
		CardItem c = t.GetComponentInParent<CardItem>();
		return c;
	}

	public CardItem getRootCard (){
		CardItem c = this;
      
		while(c.getParentCard () != null){
			c = c.getParentCard ();
		}
		return c;
	}
	public Vector2 childOffset{
		set{ 
			offsetForAllChilds = value;
		}
		get{ 
			return offsetForAllChilds;
		}
	}
	public Vector2 childOffsetOpened{
		set{ 
			offsetForAllChildsOpened = value;
		}
		get{ 
			return offsetForAllChildsOpened;
		}
	}




    // 
    //	*********************************
    //	***********  ACTIONS  *********** 
    //	*********************************
    //
   public float OffSetChildCard(int length, bool close)
    {
        float range = 0;

        int portrait = limitCardPortrait;
        int landSpace = limitCardLandSpace;



        if (SolitaireStageViewHelperClass.instance.RatioResolution() <= 1.5)
        {
            portrait = landSpace;
        }


        if (DeviceOrientationHandler.instance.isVertical && length < portrait)
        {

            if (close)
            {

                return SolitaireStageViewHelperClass.rangeBetweenCloseCard;
            }
            else
            {

                return SolitaireStageViewHelperClass.rangeBetweenOpenCard;
            }
        }

        else if (length < landSpace)
        {

            if (close)
            {
                return SolitaireStageViewHelperClass.rangeBetweenCloseCard;
            }
            else
            {

                return SolitaireStageViewHelperClass.rangeBetweenOpenCard;
            }

        }
		float ratio = 50;
		if (DeviceOrientationHandler.instance.isVertical)
        {
			if (SolitaireStageViewHelperClass.instance.RatioResolution() <= 1.5)
			{
				ratio = 13;
			}

		}
		else
        {
			ratio = 9;
        }
		 
        // Debug.Log("ratio  "+ratio);
        if (close)
        {
            range = (SolitaireStageViewHelperClass.rangeBetweenCloseCard * ratio) / (length * 2);
            //           Debug.Log(string.Format("Range {0}-{1}",Id,range));
            range = (Mathf.Abs(range) > Mathf.Abs(SolitaireStageViewHelperClass.rangeBetweenCloseCard)) ? (float)SolitaireStageViewHelperClass.rangeBetweenCloseCard : range;
        }
        else
        {

            range = ((SolitaireStageViewHelperClass.rangeBetweenOpenCard * ratio * 1.1f) / length );
		  //  Debug.Log("range1: " + SolitaireStageViewHelperClass.rangeBetweenOpenCard * ratio * 1.1f);
			//Debug.Log("length: " + length);
			range = (Mathf.Abs(range) > Mathf.Abs(SolitaireStageViewHelperClass.rangeBetweenOpenCard)) ? (float)SolitaireStageViewHelperClass.rangeBetweenOpenCard : range;
			//Debug.Log("range2 : " + range);

		}
      
        return range;
    }
    public void showSalute(){

		Assert.IsFalse (Suit < 0 || Suit > 3);
//        Debug.Log("Show 2");
        salute.Show (Suit);		
	}
 

    public void openCard(bool isOpen = true)
	{
 
		isOppened = isOpen;
        if(faceImage!=null)
		faceImage.gameObject.SetActive(isOpen);
        if (backImage != null)
            backImage.gameObject.SetActive(!isOpen);
    }

	public void openCardAnim(bool isOpen = true){
		turn = true;
		turnOpen = isOpen;
        if (GameSettings.Instance.isGameStarted)
        {
			//managerLogic.AddScore(20);
			/////Debug.LogError("open card or flip card here");
		}

	}

  



	bool turn = false;
	bool turnOpen = false;
	private float speed = 8f;
	private float time = 1f;
	// ANIMATION UPDATE
	private void Update()
	{
		if (!turn)
			return;
		
		// increase timer
		time -= Time.deltaTime * speed;

		// first part of animation
		if (time > 0f) {
			SetScale (time);
		} 
		// second part of animation
		else {
 
			 openCard(turnOpen);
			// shift time for 1 second back 
			float new_time01 = time + 1f;
			// change time from 0-1 to 1-0
			float inverted_time = 1f - new_time01;
			// fade in
			SetScale (inverted_time);
			// finish
			if (time < -1f) {
				time = 1f;
				turn = false;
				SetScale (1f);
			}
		}
	}

	private void SetScale(float value)
	{
        if(faceImage!=null)
		faceImage.transform.localScale = new Vector3 (value, 1f);
        if (backImage != null)
            backImage.transform.localScale = new Vector3 (value, 1f);
	}
}