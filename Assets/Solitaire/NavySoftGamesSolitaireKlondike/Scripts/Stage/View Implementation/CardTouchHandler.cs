using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardItem))]
public class CardTouchHandler : MonoBehaviour, 
								IPointerDownHandler, 
								IPointerUpHandler, 
								IPointerClickHandler, 
								IBeginDragHandler, 
								IDragHandler, 
								IEndDragHandler 
{
	private const bool PRINT_DEBUG = false;

	private bool draggable = true;
	// find view
	private CardItem targetCard;
	private ICardItemActions view;

	public void Init(CardItem ownerCard, ICardItemActions listener){
		
//		view = SolitaireStageViewHelperClass.instance;
//		targetCard = GetComponent<CardItem> ();
		targetCard = ownerCard;
		view = listener;
	}
	
// 
//  *********************************
//  *******  TOUCH  HANDLERS  ******* 
//  *********************************
// 

	bool pressedIn = false;
    #region IPointerDownHandler and IPointerUpHandler implementation for onClick
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {

        if(targetCard.FirstCard().Id==-300)
        {
            if (targetCard.hasChildCard)
            {
                SolitaireStageViewHelperClass.instance.ShakeCard(targetCard);
                return;
            }
        }
         

        view.pressByCard(targetCard);
        pressedIn = true;
        if (GameSettings.Instance.isSoundSet)
        {

			Solitaire_GameStake.Sound.Instance.TouchCard();
        }
    }
	void IPointerUpHandler.OnPointerUp (PointerEventData eventData)	{
		if(pressedIn){
			pressedIn = false;
			log ("OnPointerClick");
			view.clickByCard (targetCard);
		}

	}
	void IPointerClickHandler.OnPointerClick (PointerEventData eventData) {}
	#endregion

	#region IBeginDragHandler implementation
	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
    {
        if (targetCard.FirstCard().Id == -300)
        {
            if (targetCard.hasChildCard)
            {
                SolitaireStageViewHelperClass.instance.ShakeCard(targetCard);
                return;
            }
        }
        pressedIn = false;
		if (draggable) {
			log ("OnBeginDrag");
			view.startDragCard (targetCard);
		}
	}
	#endregion

	#region IDragHandler implementation
	void IDragHandler.OnDrag (PointerEventData eventData)
    {
         
        if (draggable) {
		
			view.dragCard (Input.mousePosition);
		}
	}
	#endregion

	#region IEndDragHandler implementation
	void IEndDragHandler.OnEndDrag (PointerEventData eventData)
    {
        
        if (draggable) {
			log ("OnEndDrag");
			view.endDragCard (targetCard);
		}
	}
	#endregion


	private void log(string msg){
		if (!PRINT_DEBUG)
			return;

		string c = "card id: " + targetCard.Id + " ";
		Debug.Log(c + msg);
	}
}
