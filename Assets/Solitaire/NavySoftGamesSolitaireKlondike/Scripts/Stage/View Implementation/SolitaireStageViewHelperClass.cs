using System.Collections.Generic;

using SmartOrientation;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
 
public class SolitaireStageViewHelperClass : MonoBehaviour, ICardItemActions, IDebugGame
{
    // responsabilities:
    // - drag n drop cards
    // - manage movements
    // - controll animations 
    public const int rangeBetweenOpenCard = -90;
    public const int rangeBetweenCloseCard = -40;
    private static SolitaireStageViewHelperClass _instance;
    public static SolitaireStageViewHelperClass instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject viewHelper = GameObject.Find("Stage Vew Helper");
                // GameObject obj = Instantiate(PopUpPref);

                _instance = viewHelper.GetComponent<SolitaireStageViewHelperClass>();
            }
            return _instance;
        }
    }

    // 
    //	*********************************
    //	*****  DRAG'n'DROP FIELDS  ******
    //	*********************************
    // 
    [SerializeField]
	private CardItem originCard;
	[SerializeField]
	private CardItemsDeck deckContainer;
	[SerializeField]
	private List<CardItem> foundationStacks;
	[SerializeField]
	private List<CardItem> tableuStacks;
    [SerializeField]
    private CardItem stockStack;
    [SerializeField]
    private CardItem containerStack;
    [SerializeField]
	public SmoothMovementManager movementManager;
	[SerializeField]
	private SmartOrientationStage smartOrientation;
	[SerializeField]
	private DanceIniter dance;

    private bool isNewGame = false;
	float maxRadiusToAttach = 90f;
	public Transform touchPoint;

    public List<CardItem> GetTableuStacks { get { return tableuStacks; } }
    public List<CardItem> GetFoundationStacks { get { return foundationStacks; } }
    public CardItemsDeck GetDeckContainer { get { return deckContainer; } }
    public CardItem GetDeckStack { get { return containerStack; } }
    public CardItem GetStockStack { get { return stockStack; } }
    private ICardActions manager;
	private CardItemsBuilder builder;
	private Dictionary<int, CardItem> cardsOnStage = new Dictionary<int, CardItem>();
	private ManagerLogic managerLogic;
	public Image bg;

	private string[,] dealContract;


	void Awake() {
		 
		Input.multiTouchEnabled = false;
        HUDController.instance.gameObject.name = "HUD";

    }
	void Start(){
		manager = StageManager.instance;
		managerLogic = new ManagerLogic();//piper
#if !UNITY_WEBGL
		Application.targetFrameRate = 60;
#endif
    }

	public void ShowDance (UnityAction action)
	{
		HUDController.instance.setTrigger (true,
			//DanceCallback(action)
			() =>
			{
				HUDController.instance.triggerLess.SetActive(false);
				HUDController.instance.triggerFull.SetActive(false);
				dance.Hide ();
				action();
			}
		);

		SetActiveCards (false);
		dance.Show (
			foundationStacks[0].rect,
            foundationStacks[1].rect,
            foundationStacks[2].rect,
            foundationStacks[3].rect,
			//DanceCallback (action)

			() =>
			{
				HUDController.instance.triggerLess.SetActive(false);
				HUDController.instance.triggerFull.SetActive(false);
				dance.Hide ();
				action();
			}
		);
	}
		
	// help functions to sort foundations by suit
 

    public void UpdateViewSettings ()
	{
		
		// change backgroung
		int bg_id = GameSettings.Instance.visualPlayBackgroundSet;
		bg.sprite = ImageSettings.Instance.background [bg_id];


		// update suits
		if (cardsOnStage.Count == 0) {
			originCard.UpdateCardBack ();
		} else {
			foreach (var c in cardsOnStage) {
				if(!c.Value.isRoot)
					c.Value.UpdateCardBack ();
			}
		}

		// update status bar
		bool isMoveTime = GameSettings.Instance.isMoveTimeSet;
		bool isScore = GameSettings.Instance.isScoreSet;
		HUDController.instance.SetBarsVisibility(isMoveTime, isMoveTime, isScore);

		// update left-right hand layouts
		smartOrientation.Refresh();
		HUDController.instance.smartOrientationHUD.Refresh();
	}

	public void ResetView(){
 
		// TODO move it to ResetView()
		blinkCards.Clear ();


        HUDController.instance.SetSolutionLayout(false);
 
	}
	public void RemoveCards(){
//		List<CardItem> rootCardsTemtList = new List<CardItem> ();

		foreach (var item in cardsOnStage) {
			if (item.Value.isRoot) {
				// save root card reference
//				rootCardsTemtList.Add (item.Value);
			}
			else {
				// destroy not root card
				Destroy (item.Value.gameObject);
			}
		}

		// clear list of all cards
		cardsOnStage.Clear ();

//		// restore root cards reference
//		foreach (var root in rootCardsTemtList) {
//			cardsOnStage.Add (root.Id, root);
//		}
	}
     

    // 
    //	*********************************
    //	*******  MANAGER'S CALLS  *******
    //	*********************************
    // 
#region IView implementation
    public void Init(int null_card, int deck_stack_holder, int[] foundation_stack_holder, int[] tableau_stack_holder,
        List<SolitaireEngine.Model.Card> startingCards, string[,] deal_contract)
    {

        isNewGame = true;
 
        // set up settings
        UpdateViewSettings();

        originCard.gameObject.SetActive(true);

        // Card Items Builder
        builder = new CardItemsBuilder(originCard, deckContainer.GetComponent<Transform>());

        //Deck

        CardItem deck_card = deckContainer.initDeck(deck_stack_holder);
        cardsOnStage.Add(deck_stack_holder, deck_card);

        int unique_id = -300;
        CardItem deck_card_face_up = deckContainer.initDeckFaceUp(unique_id);
        cardsOnStage.Add(unique_id, deck_card_face_up);

        // Create All Cards
        for (int i = startingCards.Count - 1; i >= 0; i--)
        {
 
            CardItem card = builder.BuildNewCardItem(
                startingCards[i].Id, startingCards[i].IsOpened,
                startingCards[i].Suit, startingCards[i].Rank);
            card.attachListener(this);
 
            card.openCard(false);

            if (!ContinueModeGame.instance.HasDataInMatch)
            {
                MoveCard(card, deckContainer.deckHiddenContainer.getChildestCard());
            }
            else
            {
                setOffset(card, FindCardItem(-100), Vector2.zero);
            }
            cardsOnStage.Add(startingCards[i].Id, card);
        }

        originCard.gameObject.SetActive(false);

        // Fundation
        for (int i = 0; i < foundation_stack_holder.Length; i++)
        {
            //			Card c = new Card(foundation_stack_holder[i], -1,-1);
            foundationStacks[i].initCard(foundation_stack_holder[i], false, -1, -1);//c);
            cardsOnStage.Add(foundation_stack_holder[i], foundationStacks[i]);
        }

        // Tableu
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            //			Card c = new Card(tableau_stack_holder[i], -1,-1);
            tableuStacks[i].initCard(tableau_stack_holder[i], false, -1, -1);//(c);
            cardsOnStage.Add(tableau_stack_holder[i], tableuStacks[i]);
        }

        // measure radius to nearest card when we drop one
        float distance_between_cards = Vector3.Distance(tableuStacks[0].transform.position, tableuStacks[1].transform.position);
        Assert.AreNotEqual(0, distance_between_cards);
        float magic_number = 1.5f;
        maxRadiusToAttach = distance_between_cards * magic_number;


        dealContract = deal_contract;
        



        if (!ContinueModeGame.instance.HasDataInMatch)
        {
            StartCoroutine(DealCards(dealContract, 0));
        }
        else
        {
            StartCoroutine(ContinueModeGame.instance.LoadContinueMatch());
            HUDController.instance.VisibleButtonComplete(false);
            
        }
        if (ContinueModeGame.instance.LoadSuccess)
        {
            GameSettings.Instance.countDeckTurn = 0;
            PlayerPrefAPI.Set();
        }


    }


   


    IEnumerator DealCards(string[,] contract,float waitTime = 0, UnityAction callback = null){
        HUDController.instance.VisibleButtonsAction(false);
        GameSettings.Instance.countDeckTurn = 0;
		movementManager.SetMovingSpeed (SmoothMovementManager.Speed.Deal);
        activeAutoMove = false;
		Solitaire_GameStake.Sound.Instance.StartNew();
        HUDController.instance.VisibleButtonComplete(false);
        HUDController.instance.UpdateGameMode();
        yield return new WaitForSeconds(waitTime);
		HUDController.instance.setTrigger(true, null);
		for (int i = 0; i < 7; i++) 
		{
			for (int j = 0; j < 7; j++) {
				string c = contract [j, i];

				bool deal = c == "0" || c == "1";

				if(deal)
				{
					// to open last card
					bool isFaceUp = c == "1";

					// get last card from deck
					CardItem cardFromDeck = deckContainer.deckHiddenContainer.getChildestCard ();

					// card to
					CardItem cTo = tableuStacks[i].getChildestCard();

                    if(!cardFromDeck.isRoot)
					// deal
					DealOneCard (cardFromDeck, cTo, isFaceUp);
					yield return new WaitForSeconds (.03f);
				}else{
					continue;
				}
			}
		}
		HUDController.instance.triggerLess.SetActive(false);
		HUDController.instance.triggerFull.SetActive(false);
      
        if (callback != null)
			callback ();
        yield return new WaitForSeconds(.1f);
        HUDController.instance.VisibleButtonsAction(true);
        yield return null;
	}

	void DealOneCard (CardItem card, CardItem cardTo, bool isFaceUp){
#if UNITY_EDITOR
        if (card == null) {
			throw new UnityException ("Card from deck is null!");
		}
#endif
      
       
        // TODO remove it
        if (card.parentCard != null)
			card.parentCard.childCardNull ();
          
   
       
		// attach to tableu
		MoveCard(card, cardTo, true, () => {
            if (isFaceUp)
            {
				//Debug.LogError("card is opening now::");
				card.openCardAnim();
			}
		});
	}


	public void Reset(List<int> originCards, UnityAction callback = null){
        isNewGame = true;
		MoveToHeap ();
		MoveToDeck (originCards);
		SetActiveCards (true);

   
		  StartCoroutine(DealCards (dealContract,0, callback));
	}


	public void SetActiveCards (bool isActive)
	{
//		int cc = 0;
		// detach all cards
		foreach (var c in cardsOnStage) {
			if (!c.Value.isRoot) {
				c.Value.gameObject.SetActive (isActive);
			}
		}
	}
	public void MoveToHeap ()
	{
 
		foreach (var c in cardsOnStage) {
			if (!c.Value.isRoot) {
				attachCard (c.Value, bg.transform);
				c.Value.openCard (false);
				c.Value.transform.localPosition = new Vector3 (10000f, 0);
				c.Value.childCardNull();
				c.Value.parentCard = null;
			}
		}
		
	}

	public void MoveToDeck(List<int> originCardsOrder){
        CardItem cardTo = deckContainer.deckHiddenContainer.getChildestCard();
        for (int i = originCardsOrder.Count - 1; i >= 0; i--) {
			// find card on stage ordered by id
			CardItem cardFrom = getCardById (originCardsOrder [i]);
			// find last card in deck
		

           
			// attach card into deck
			MoveCard (cardFrom, cardTo);
            cardTo = cardFrom;

        }
	}
		





	// MOVING
	public void MoveCard(CardItem cardFrom, CardItem cardTo, bool animation = false, UnityAction unityAction = null){
		
		bool should_show_salute_effect = false;
		if (GameSettings.Instance.isEffectSet) {	
			// NOTE: we have to check it before attach card because card will be attached to foundation
			bool targetFromFoundation = cardFrom.getRootCard ().isFoundation;
			bool destinationFromFundation = cardTo.getRootCard ().isFoundation;
           
            // Show salute effect only if card from deck or tableu and will be attached to foundation
            if ( ! targetFromFoundation && destinationFromFundation) {
				should_show_salute_effect = true;
			}
		}
      
		attachCard (cardFrom, cardTo);
		cardFrom.childOffset = cardTo.childOffset;
		cardFrom.childOffsetOpened = cardTo.childOffsetOpened;

 
        if (animation) {

			SetTopLayerFor (cardFrom);

			Vector2 offset = GetOffset (cardTo);
           
            cardFrom.GetComponent<CardMove>().Move(cardFrom.Id, cardFrom.transform, cardTo.transform, offset, () =>
            {
                // try to remove
                setOffset(cardFrom, cardTo, offset);

                if (should_show_salute_effect)
                {
                    cardFrom.showSalute();
                }

                selectedEffectStack(cardFrom, false);

                if (unityAction != null)
                    unityAction();

            }
            );

          
		} else {
			setOffset(cardFrom, cardTo, GetOffset(cardTo));
			selectedEffectStack (cardFrom, false);
		}
	}





	public void ShowMoveHint (CardItem cardFrom, CardItem cardTo, CardItem parentCard)
	{
		cardFrom.allowClick = false;
		selectedEffectStack (cardFrom, true);
		SetTopLayerFor (cardFrom);
		HUDController.instance.VisibleButtonsAction(false);
		Vector2 offset = GetOffset (cardTo);
        cardFrom.GetComponent<CardMove>().Move(cardFrom.Id, cardFrom.transform, cardTo.transform, offset, () =>
        {
            offset = GetOffset(cardFrom.getParentCard());
            setOffset(cardFrom, cardFrom.getParentCard(), offset);
            selectedEffectStack(cardFrom, false);
            cardFrom.allowClick = true;
			HUDController.instance.VisibleButtonsAction(true);
		}
        );
       
	}




	private Vector2 GetOffset(CardItem cardTo){
		Vector2 offset;
		if(cardTo.isRoot){
			offset = Vector2.zero;
		}else{
			offset = cardTo.isOppened ? cardTo.childOffsetOpened : cardTo.childOffset;
           
		}
		return offset;
	}

	public void updateOffset(CardItem cardFrom, bool anim = false){
		CardItem cardTo = cardFrom.getParentCard ();
		Vector2 offset = GetOffset (cardTo);
		if (!anim) {
			 cardFrom.rect.localPosition = offset;
		} else {
            cardFrom.GetComponent<CardMove>().Move(cardFrom.Id, cardFrom.transform, cardTo.transform, offset, () =>
            {
                // ???
            }
            );

           
		}
	}

    public void setOffset(CardItem cardFrom, CardItem cardTo, Vector2 offset)
    { 
        cardFrom.rect.localPosition = offset;

        if (!ContinueModeGame.instance.LoadSuccess || isNewGame ) return;
        if (cardTo.rootCard != null && cardTo.rootCard.isTableu)
        {

            cardTo.rootCard.GetComponent<BetweenDistanceCard>().CheckDistanceCard(false);
        }
        if (cardFrom.rootCard != null && cardFrom.rootCard.isTableu)
            cardFrom.rootCard.GetComponent<BetweenDistanceCard>().CheckDistanceCard(false);
        //Save Card In Match
        ContinueModeGame.instance.SaveDataCardInMatch(false,cardFrom.rootCard, cardTo.rootCard);
        ContinueModeGame.instance.SaveTimeAndMove(StageManager.instance.GetManagerLogic.moves, (int)StageManager.instance.GetManagerLogic.timer, StageManager.instance.GetManagerLogic.score);
    }




    public void TurnCard (CardItem card,bool isOpen)
	{
 
        if (!card.isOppened && isOpen)
        {
			Timer.Schedule(this, GameSettings.Instance.isSolutionMode?0.23f:0.02f, () => {
                card.openCardAnim();
				//Invoke(nameof(AddTurnScore), 1f);
            });
          
      
        }
        else
        {
            card.openCard(isOpen);
        }
	}
	void AddTurnScore()//piper
    {
		managerLogic.AddScore(20);
    }
	public void ShakeCard(CardItem card){
		CardShakerAnim shakerAnim = card.GetComponent<CardShakerAnim> ();
#if UNITY_EDITOR
        if (shakerAnim == null) {
			throw new UnityException ("Can't find shaker anim");
		}
#endif
        shakerAnim.Shake ();
	}

	public void HighlightCard (CardItem card)
	{
		blinkStack (card, true);
	}

	public void UnblinkAll ()
	{
		// guard clouse for null reference
		bool hasNullReference = false;
		foreach(var c in blinkCards){
			if (c.Equals (null)) {
				hasNullReference = true;
				break;
			}
		}
		if(hasNullReference){	
			blinkCards.Clear ();
			Debug.LogError ("Detected null reference in blink cards. List forced cleared!");
		}


		foreach(var c in blinkCards){
			blinkStack(c, false);
		}
		blinkCards.Clear ();
	}

#endregion




// 
//	*********************************
//	*******  CARD'S ACTIONS  ********
//	*********************************
// 
#region RECEIVED ACTIONS FORM PARTICULAR CARD

	CardItem lastParent;
	CardItem clickedLast;

	public void pressByCard (CardItem targetCard)
	{
		touchAnywhere ();

		SetTopLayerFor (targetCard);

		if (targetCard.isOppened)
			selectedEffectStack (targetCard, true);
	}

	public void clickByCard(CardItem clicked){
       
		if(!clicked.clickable )
        {
			return;
		}
        GameSettings.Instance.isSolutionMode = false;
        isNewGame = false;       
        // to prefent when card still highlighted
        selectedEffectStack(clicked, false);
       
       
        manager.OnClickCard(clicked.Id, clicked.parentCard.Id);
     

	}

	// START
	public void startDragCard(CardItem clicked)
    {
         
        if (!clicked.clickable  )
        {
			return;
		}
        isNewGame = false;
        GameSettings.Instance.isSolutionMode = false;
        clicked.allowClick = false;


		lastParent = clicked.getParentCard ();
		clickedLast = clicked;

      

		HUDController.instance.triggerLess.SetActive(true);
	}

	// DRAG
	public void dragCard(Vector3 position){
		if (clickedLast == null)
			return;
        clickedLast.GetComponent<CardMove>().MoveDragCard(true);

        touchPoint.transform.position = position;
	}

    // DROP
    public void endDragCard(CardItem clicked)
    {
        if (clickedLast == null)
            return;

        if (!clickedLast.clickable)
        {
            return;
        }
        clickedLast.allowClick = true;
        clickedLast.GetComponent<CardMove>().MoveDragCard(false);
        HUDController.instance.triggerLess.SetActive(false);
        List<int> near_cards = getNearestCards(clickedLast);
        manager.OnDropCard(clickedLast.Id, lastParent.Id, near_cards, clickedLast);
        clickedLast = null;

    }
#endregion







    // 
    //	*********************************
    //	***********  HELPERS  ***********
    //	*********************************
    // 


    bool ableToAttachBack = true;
	public void attachCard(CardItem card, CardItem cardTo){

#if UNity_EDITOR
        if(cardTo.hasChildCard){
			if (ableToAttachBack && cardTo.childCard.Id != card.Id)
				throw new UnityException("Can't attach card id " + card.Id + " to card id " + cardTo.Id + ". It already contains a card!");
		}
#endif

		attachCard (card, cardTo.transform);

		SetTopLayerFor (cardTo);

     
		cardTo._childCard = card;
        if (cardTo.isRoot)
        {
            card.rootCard = cardTo;
        }
        else if (cardTo.rootCard != null)
        {
            List<CardItem> cards = cardTo.getChildCardsList();
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].rootCard = cardTo.rootCard;
            }
           
        }
        card.parentCard = cardTo;
   
        CardItemsDeck.instance.updateDeckOffsets();
        

     
    }
	public void attachCard(CardItem card, Transform t){
       
      
		if(card.parentCard != null)
			card.parentCard.childCardNull();
          


        card.rect.SetParent(t, true);
	}

	private void SetTopLayerFor(CardItem card){
		card.getRootCard ().transform.SetAsLastSibling ();		
	}

	List<CardItem> blinkCards = new List<CardItem>();

	public void selectedEffectStack (CardItem card, bool b)
    {
#if UNITY_EDITOR
        if (card.Equals (null)) {
			throw new UnityException ("Can't highlight card is null!");
		}
#endif
        
	}
	public void blinkStack (CardItem card, bool b)
	{
		if (card.Equals (null)) {
			throw new UnityException ("Can't blink card is null!");
		}

		foreach (var c in card.GetComponentsInChildren<CardItem>()) {
		 
			if(b)
				blinkCards.Add (c);
		}
	}



	public void pressByVoid ()
	{
		touchAnywhere ();
	}

	public void pressByDeck ()
	{
		touchAnywhere ();
	}

	void touchAnywhere(){
		manager.OnClickAnywhere ();
	}


// 
//	*********************************
//	***********  GETTERS  ***********
//	*********************************
// 

	public bool hasCardById(int id){
		return cardsOnStage.ContainsKey (id);
	}

	public CardItem getCardById(int id){
		if (!hasCardById (id)) {
			throw new UnityException ("Can't find card with id " + id);
		}
		return cardsOnStage [id];
	}

	List<CardItem> getChildestCardsInStage()
	{
		List<CardItem> c = new List<CardItem> ();
		foreach (var cont in foundationStacks) {
			c.Add (cont.getChildestCard ());
		}
		foreach (var cont in tableuStacks) {
			c.Add (cont.getChildestCard ());
		}
		return c;
	}

	List<int> getNearestCards (CardItem card){
		Dictionary<float, CardItem> cardsDistances = new Dictionary<float, CardItem> ();
		List<float> distances = new List<float> ();

		List<CardItem> cards = getChildestCardsInStage ();

		foreach (var c in cards) {
			// ignore current stack
			if (c.getRootCard().Id == card.getRootCard().Id) {
				continue;
			}

			float dist = Vector3.Distance (c.rect.position, card.rect.position);

			cardsDistances.Add(dist, c);
			distances.Add (dist);
		}

		// sort
		distances.Sort();

		List<int> nearCards = new List<int>();

		for (int i = 0; i < distances.Count; i++) {
			float dist = distances [i];
			if (dist < maxRadiusToAttach) {
				nearCards.Add (cardsDistances[dist].Id);
			}
		}

		return nearCards;
	}




#region IDebugGame implementation

	List<IDebugCardInfo> IDebugGame.GetAllCards ()
	{
		List<IDebugCardInfo> allCards = new List<IDebugCardInfo> ();

		foreach (var c in cardsOnStage) {
			CardItem cardView = c.Value;
			if (cardView.isRoot)
				continue;

			IDebugCardInfo cInfo = new IDebugCardInfo ();
			cInfo.id = cardView.Id;
			cInfo.isOpen = cardView.isOppened;
			cInfo.rank = getDebugRank(cardView);
			cInfo.suit = getDebugSuit(cardView);
			cInfo.zone = getDebugZone(cardView);
			cInfo.zoneIndex = getDebugZoneIndex(cardView);
			cInfo.cardIndexInStack = getDebugIndexInStack(cardView);

			allCards.Add (cInfo);
		}

		return allCards;
	}

	private IDebugCardInfo.Rank getDebugRank(CardItem card){
		switch (card.Rank) {
		case 1:
			return IDebugCardInfo.Rank.Ace;
		case 2:
			return IDebugCardInfo.Rank.Two;
		case 3:
			return IDebugCardInfo.Rank.Three;
		case 4:
			return IDebugCardInfo.Rank.Four;
		case 5:
			return IDebugCardInfo.Rank.Five;
		case 6:
			return IDebugCardInfo.Rank.Six;
		case 7:
			return IDebugCardInfo.Rank.Seven;
		case 8:
			return IDebugCardInfo.Rank.Eight;
		case 9:
			return IDebugCardInfo.Rank.Nine;
		case 10:
			return IDebugCardInfo.Rank.Ten;
		case 11:
			return IDebugCardInfo.Rank.Jack;
		case 12:
			return IDebugCardInfo.Rank.Quen;
		case 13:
			return IDebugCardInfo.Rank.King;
		default:
			throw new UnityException ("Can't parse card item rank!");
		}
	}
	private IDebugCardInfo.Suit getDebugSuit(CardItem card){
		switch (card.Suit) {
		case 0:
			return IDebugCardInfo.Suit.Diamonds;
		case 1:
			return IDebugCardInfo.Suit.Heart;
		case 2:
			return IDebugCardInfo.Suit.Clubs;
		case 3:
			return IDebugCardInfo.Suit.Spades;
		default:
			throw new UnityException ("Can't parse card item suit!");
		}
	}
	private IDebugCardInfo.Zone getDebugZone(CardItem card){
		if (card.getRootCard().isFoundation) {
			return IDebugCardInfo.Zone.Foundation;
		}else if(card.getRootCard().isDeck){
			return IDebugCardInfo.Zone.Deck;
		}
		return IDebugCardInfo.Zone.Tableu;
	}
	private int getDebugZoneIndex(CardItem card){
		return card.getRootCard().debugZoneIndex;
	}
	private int getDebugIndexInStack(CardItem carrd){

		CardItem rood = carrd.getRootCard ();

		int ind = 0;
		// TODO remove deck case
		if (rood.isDeck && rood.debugZoneIndex == 0) {
			foreach (var c in rood.getChildCardsList()) {
				if (c.Id == carrd.Id) {
					return rood.getChildCardsList().Count - ind - 1;
				}
				ind++;
			}		
		} else {
			foreach (var c in rood.getChildCardsList()) {
				if (c.Id == carrd.Id) {
					return ind;
				}
				ind++;
			}
		}


		return -1;
	}


    public void ChangeCardFace()
    {
        CardItem[] cardItems = FindObjectsOfType<CardItem>();
        for (int i = 0; i < cardItems.Length; i++)
        {
            if (cardItems[i].isRoot) continue;
            cardItems[i].GetComponent<CardImage>().SetCard(cardItems[i].Suit, cardItems[i].Rank - 1);
        }
    }

    [SerializeField]
    private RectTransform sizeCardPortrait;
    [SerializeField]
    private RectTransform sizeCardLandSpace;
    public Vector2 ConvertSizeCard(bool isPortrait)
    {
        float sizeX = 0;
        float sizeY = 0;
        float max = Mathf.Max(Screen.height, Screen.width);
        float min = Mathf.Min(Screen.height, Screen.width);
        float ratioResolution = max / min;
        if (isPortrait)
        {
            float minSize = Mathf.Min(sizeCardPortrait.sizeDelta.x, sizeCardPortrait.sizeDelta.y);

            sizeX = sizeY = minSize / foundationStacks[0].GetComponent<RectTransform>().sizeDelta.x;

        }
        else
        {
            float minSize = sizeCardLandSpace.sizeDelta.x;


           
            sizeX = sizeY = minSize / foundationStacks[0].GetComponent<RectTransform>().sizeDelta.x;

            //caculator scale when card out size screen
            float heightCards = originCard.GetComponent<RectTransform>().sizeDelta.y * sizeX * foundationStacks.Count;
            float heightUse = Screen.height - (Screen.height * 0.15f);

           //    Debug.Log("heightUse " + heightUse);
            //  Debug.Log("heightCards " + heightCards);
            if (minSize > 199 && heightCards > heightUse)
            {
                heightUse = Screen.height - (Screen.height * 0.1f);
                float sizeLandSpace = heightCards / heightUse;

                sizeLandSpace = (sizeLandSpace < 1) ? ratioResolution : sizeLandSpace;

                sizeLandSpace = (sizeLandSpace > 1.2f) ? 1.2f : sizeLandSpace;
                sizeX = sizeY = sizeLandSpace;


            }
            sizeX = sizeY = (sizeX > 1.2f) ? 1.2f : sizeX;
        }
        Vector2 size = new Vector2(sizeX, sizeY);

        return size;

    }
    public void SaveMedalCurrentMonth()
    {
        int day = GameSettings.Instance.calendarGameDay - 1;
        int month = GameSettings.Instance.calendarGameMonth;
        int year = GameSettings.Instance.calendarGameYear;


        string fileName = string.Format("{0}{1}{2}", day, month, year);

        PlayerPrefs.SetInt(fileName, 1);


    }
    public void ScaleCard(bool isPortrait)
    {


        for (int i = 0; i < tableuStacks.Count; i++)
        {
            tableuStacks[i].transform.localScale =  ConvertSizeCard(isPortrait);
        }

        for (int i = 0; i < foundationStacks.Count; i++)
        {
            foundationStacks[i].transform.localScale =  ConvertSizeCard(isPortrait);
        }

        containerStack.transform.localScale = ConvertSizeCard(isPortrait);
        stockStack.transform.localScale = ConvertSizeCard(isPortrait);
    }
    public CardItem FindCardItem(int id)
    {
         if(cardsOnStage.ContainsKey(id))
        {
            return cardsOnStage[id];
        }
        return null;
    }
    public float RatioResolution()
    {
        float max = Mathf.Max(Screen.height, Screen.width);
        float min = Mathf.Min(Screen.height, Screen.width);
        float ratioResolution = max / min;
        return ratioResolution;
    }
    private List<CardItem> cardItemAutoMove = new List<CardItem>();
    private bool activeAutoMove = false;
    private void GetCardAutoMove()
    {
        cardItemAutoMove.Clear();
		Debug.LogError("Tabluea cards count::" + tableuStacks.Count);
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            CardItem[] cardItems = tableuStacks[i].GetComponentsInChildren<CardItem>();
            CardItem lastCast = cardItems[cardItems.Length - 1];
            if(!lastCast.isRoot)
            cardItemAutoMove.Add(lastCast);
			Debug.LogError("count of cards int he table stack is::" + (cardItems.Length - 1));

		}
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            if (tableuStacks[i].transform.childCount == 0)
            {
				Debug.LogError("table is empty. Load Gameover screen");
				Invoke(nameof(LoadGameoverAfterTableisEmpty), 8f);
			}
        }
		//if(tableuStacks.Count==0 && )
	}

	void LoadGameoverAfterTableisEmpty()
	{
		HUDController.instance.triggerFull.SetActive(false);
		HUDController.instance.triggerLess.SetActive(false);
		StageManager.instance.LoadGameover();
	}
	public IEnumerator AutoMoveCard()
    {
        activeAutoMove = true;
        while (activeAutoMove)
        {
            GetCardAutoMove();
            yield return new WaitForSeconds(0.02f);
            for (int i = 0; i < cardItemAutoMove.Count; i++)
            {
                yield return new WaitForSeconds(0.05f);
				Solitaire_GameStake.Sound.Instance.TouchCard();
                // cardItemAutoMove[i].autoMove = true;
                // cardItemAutoMove[i].Hide = true;
                clickByCard(cardItemAutoMove[i]);
            }
        }
        
    }
    public void SetDistanceBetweenCard(bool resetLength)
    {

       
        
        for (int i = 0; i < tableuStacks.Count; i++)
        {
            BetweenDistanceCard betweenDistanceCard = tableuStacks[i].GetComponent<BetweenDistanceCard>();
            betweenDistanceCard.CheckDistanceCard(resetLength);
        }
          


    }
    public void SaveAllCardInMatch( CardItem cardFrom,CardItem cardTo )
    {
        ContinueModeGame.instance.ClearDataGroup();


        for (int i = 0; i < tableuStacks.Count; i++)
        {
            bool change = false;
            if (tableuStacks[i] == cardFrom || tableuStacks[i] == cardTo ) change = true;
            ContinueModeGame.instance.AddDataGroup(tableuStacks[i].GetComponent<CardHeader>().GetAllChildCard(change));
        }
        for (int i = 0; i < foundationStacks.Count; i++)
        {
            bool change = false;
            if (tableuStacks[i] == cardFrom || tableuStacks[i] == cardTo) change = true;
            ContinueModeGame.instance.AddDataGroup(foundationStacks[i].GetComponent<CardHeader>().GetAllChildCard(change));
        }

        ContinueModeGame.instance.AddDataGroup(stockStack.GetComponent<CardHeader>().GetAllChildCard((stockStack == cardFrom || stockStack == cardTo)));

        ContinueModeGame.instance.AddDataGroup(containerStack.GetComponent<CardHeader>().GetAllChildCard((containerStack == cardFrom || containerStack == cardTo)));
    }

#endregion
}