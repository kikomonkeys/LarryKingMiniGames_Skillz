using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItemsDeck : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{

    [SerializeField]
    Image image;

    [SerializeField]
    Sprite sprire_refresh;

    //piper
    [SerializeField]
    Sprite sprire_3left;
    [SerializeField]
    Sprite sprire_2left;
    [SerializeField]
    Sprite sprire_1left;
    [SerializeField]
    Sprite sprire_20points;
    //piper

    [SerializeField]
    Sprite sprire_lock;


    // create singleton
    public static CardItemsDeck instance;
    void Awake()
    {
        instance = this;
    }

    private ICardActions manager;
    void Start()
    {
        manager = StageManager.instance;
    }

    public CardItem deckHiddenContainer;
    public CardItem deckOpenedContainer;

    public CardItem initDeck(int containerId)
    {
        deckHiddenContainer.initCard(containerId, false, -1, -1);
        return deckHiddenContainer;
    }
    public CardItem initDeckFaceUp(int containerId)
    {
        deckOpenedContainer.initCard(containerId, false, -1, -1);
        return deckOpenedContainer;
    }

    public int ShiftDeck(bool direction_forward)
    {


#if UNITY_EDITOR
        if (direction_forward)
        {
            if (deckHiddenContainer.hasChildCard == false)
            {
                throw new UnityException("Can't shift deck forward. Deck is empty.");
            }
        }

#endif
        // show next card
        CardItem cardFrom = direction_forward ? deckHiddenContainer.getChildestCard() : deckOpenedContainer.getChildestCard();
        CardItem cardTo = !direction_forward ? deckHiddenContainer.getChildestCard() : deckOpenedContainer.getChildestCard();

#if UNITY_EDITOR
        if (cardFrom == null || cardTo == null)
            throw new UnityException("Can't find card for shift");

#endif


        // TODO: remove it
        if (cardFrom.parentCard != null)
            cardFrom.parentCard.childCardNull();

        // open or close card

        if (!cardFrom.isRoot)
        {
            cardFrom.openCard(direction_forward);
        }


        // move card
        SolitaireStageViewHelperClass.instance.MoveCard(cardFrom, cardTo, true);
        return cardFrom.Id;
    }

    public void TurnDeck(bool forward)
    {
#if UNITY_EDITOR
        if (forward)
        {
            if (deckHiddenContainer.hasChildCard || !deckOpenedContainer.hasChildCard)
            {
                throw new UnityException("Can't turn deck. Decks aren't ready yet!");
            }
        }
        else
        {
            if (!deckHiddenContainer.hasChildCard || deckOpenedContainer.hasChildCard)
            {
                throw new UnityException("Can't turn deck. Decks aren't ready yet!");
            }
        }
#endif
        // turn card and attach to deck
        if (forward)
        {
            MoveDeckCard(deckOpenedContainer, deckHiddenContainer, false);
        }

        else
        {

            MoveDeckCard(deckHiddenContainer, deckOpenedContainer, true);

        }
        // update offsets if undo
        if (!forward)
            updateDeckOffsets(false);

    }

    private void MoveDeckCard(CardItem CardFrom, CardItem cardTo, bool openCard)
    {
        CardItem[] cardInStocks = CardFrom.GetComponentsInChildren<CardItem>();
        CardItem cardParent = cardTo;
        for (int i = cardInStocks.Length - 1; i >= 0; i--)
        {
            if (cardInStocks[i].isRoot) continue;

            SolitaireStageViewHelperClass.instance.MoveCard(cardInStocks[i], cardParent, true);
            cardParent = cardInStocks[i];
            cardParent.openCard(openCard);
        }
    }


    public void updateDeckOffsets(bool anim = true)
    {

       // Debug.LogError("deckOpenedContainer.getChildCardsList()::" + deckOpenedContainer.getChildCardsList());
        foreach (var c in deckOpenedContainer.getChildCardsList())
        {

            int child_count = c.getChildCardsList().Count;

            // set offset only for 2 last cards
            if (child_count < 3)
            {
                c.childOffsetOpened = new Vector2(75f, 0);//new Vector2(75f, 0);
            }
            else
            {
                c.childOffsetOpened = Vector2.zero;
            }
        }

        // apply
        foreach (var c in deckOpenedContainer.getChildCardsList())
        {
            SolitaireStageViewHelperClass.instance.updateOffset(c, anim);
        }
        // allow touch
        enableOnlyLast();
    }

    private void enableOnlyLast()
    {
        foreach (var c in deckOpenedContainer.getChildCardsList())
        {
            c.allowClick = false;
        }
        int cound = deckOpenedContainer.getChildCardsList().Count;
        if (cound > 0)
        {
            deckOpenedContainer.getChildCardsList()[cound - 1].allowClick = true;
        }
    }


    #region OnPointerDown handler

    bool isDeckClicked = false;//piper
    public void OnPointerDown(PointerEventData eventData)
    {
        // for touck anywhere
        if (!isDeckClicked)//piper
        {
            SolitaireStageViewHelperClass.instance.pressByDeck();
            if (deckHiddenContainer.hasChildCard)
            {
                manager.OnClickDeck();
                if (GameSettings.Instance.isCumulativeVegasSet && !deckHiddenContainer.hasChildCard)
                {
                    SetDeckImage(false, 0);
                }
            }
            else
            {
                manager.OnTurnDeck();
            }
            isDeckClicked = true;//piper

        }
        

    }
    #endregion
    #region OnPointer up Handler 
    //onpointer up handler written by piper

    public void OnPointerUp(PointerEventData eventData)//piper
    {
        isDeckClicked = false;
    }
    #endregion
    public void SetDeckImage(bool b, int num)
    {
        if (b)
        {
          //  Debug.LogError("num::" + num);
            if (num == 0)
            {
                image.sprite = sprire_3left;//sprire_refresh
            }
            else if (num == 1)
            {
                image.sprite = sprire_2left;
            }
            else if (num == 2)
            {
                image.sprite = sprire_1left;
            }
            //else if (num == 3)
            //{
            //    image.sprite = sprire_1left;
            //}
            else
            {
                image.sprite = sprire_20points;
            }
        }
        else
        {
            image.sprite = sprire_lock;
        }
    }
    
}
