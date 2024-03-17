namespace SolitaireEngine
{
	using UnityEngine.Events;
	using System.Collections.Generic;
	using SolitaireEngine.Model;
	using SolitaireEngine.Utility;


	public class Logic
	{
		private Data data;
		private Utils utils;
		private Logic(){}
		public Logic (Data _data)
		{
			this.data = _data;
			utils = new Utils ();
			//ReverseDeck (false); 
		}

		#region CanAttach
		private bool CanPutCardToCommunityBase (int idFrom, int idTo)
        {
            CardItem cardFrom = SolitaireStageViewHelperClass.instance.FindCardItem(idFrom);
            CardItem cardTo = SolitaireStageViewHelperClass.instance.FindCardItem(idTo);

            bool tableu = cardTo.Id == -105 || cardTo.Id == -106 || cardTo.Id == -107 || cardTo.Id == -108 || cardTo.Id == -109 || cardTo.Id == -110 || cardTo.Id == -111;

            if (tableu && !cardTo.hasChildCard   &&  cardFrom.King)
            {
                return true;

            }

            return false;
        }
		private bool CanPutCardToThronBase (int idFrom, int idTo)
		{
            int idSuit = -1;
            CardItem cardFrom = SolitaireStageViewHelperClass.instance.FindCardItem(idFrom);
            CardItem cardTo = SolitaireStageViewHelperClass.instance.FindCardItem(idTo);
          
            if (!cardTo.hasChildCard)
            {
                switch (cardTo.Id)
                {
                    case -102:
                        //diamonds
                        idSuit = 0;
                        break;
                    case -101:
                        //hearts
                        idSuit = 1;
                        break;
                    case -103:
                        //clubs
                        idSuit = 2;
                        break;
                    case -104:
                        //spades
                        idSuit = 3;
                        break;
                }
                if (idSuit == cardFrom.Suit && cardFrom.Ace) return true;
            }



            return false;
        }
		private bool CanPutCardToCommunityCards(int idFrom, int idTo)
        {
            CardItem cardFrom = SolitaireStageViewHelperClass.instance.FindCardItem(idFrom);
            CardItem cardTo = SolitaireStageViewHelperClass.instance.FindCardItem(idTo);

            if ((cardTo.isRedSuit != cardFrom.isRedSuit) && (cardTo.Rank - cardFrom.Rank).Equals(1))
            {
                return true;
            }
            return false;
        }
		private bool CanPutCardToThronCards(int idFrom, int idTo)
        {
            CardItem cardFrom = SolitaireStageViewHelperClass.instance.FindCardItem(idFrom);
            CardItem cardTo = SolitaireStageViewHelperClass.instance.FindCardItem(idTo);
           //  UnityEngine.Debug.Log(cardFrom.Id +"____"+ cardTo.Id);
            if (!cardTo.getRootCard().isFoundation) return false;
            if (cardFrom.hasChildCard) return false;




            if (cardTo.Suit.Equals(cardFrom.Suit) && (cardFrom.Rank - cardTo.Rank).Equals(1))
            {
                return true;
            }
            return false;
        }
		private bool SetOpenCard(int id, bool value)
		{
			bool isSet = data.SetOpenCard (id, value);
			return isSet;
		}

		#endregion

		#region GetBetterPlace
		private List<int> FindSolverPlace (int id) // attantion solver use it
		{
			List<int> findAllPlaces = utils.ShuffleList(FindAllPlaces (id));
			if (findAllPlaces.Count.Equals(0)) return new List<int> ();
			// bufferisate findPlace 
			List<IdResult> resultFindPlaces = new List<IdResult> ();
			foreach (int element in findAllPlaces)
				resultFindPlaces.Add (data.IdAnalizator (element));
			int totalFindPlaces = resultFindPlaces.Count;

			IdResult resultTab = data.IdAnalizator (id);
			List<int> sencePlaces = new List<int> ();

           //  UnityEngine.Debug.Log("Place");
			// ALL GOOD
			// Cards in Thron Base or in Thron do not return back
			if (resultTab.isInThronBase || resultTab.isInThron) return new List<int> ();
				
			// TO THRON
			// card from deck or from community is last find place in Thron
			if (resultTab.isInDeck || (resultTab.isInCommunity && resultTab.isLast))
			for (int index = 0; index < totalFindPlaces; index++)
				if (resultFindPlaces [index].isInThron) sencePlaces.Add (findAllPlaces [index]);
			// TO COMMUNITY BASE
			if (resultTab.card.IsKing)
			{
				// if king in base and find befor thron place its cool, if not dont move end return empty move
				if (resultTab.isFirst) return sencePlaces;
				// if king in deck or in community find olso one in communitybase
				for (int index = 0; index < totalFindPlaces; index++)
					if (resultFindPlaces [index].isInCommunityBase)
					{
						sencePlaces.Add (findAllPlaces [index]);
						break;
					}
				// so king can one or two position (one in thron and another one in community base) send it imediatly
				return sencePlaces;
			}

			// TO THRON BASE
			// Ace find one position in ThronBase
			if (resultTab.card.IsAce)
			{
				for (int index = 0; index < totalFindPlaces; index++)
					if (resultFindPlaces [index].isInThronBase)
					{
						sencePlaces.Add (findAllPlaces [index]);
						break;
					}
			}
			// TO COMMUNITY
			if (resultTab.isInDeck)
			{
				for (int index = 0; index < totalFindPlaces; index++)
					if (resultFindPlaces [index].isInCommunity) sencePlaces.Add (findAllPlaces [index]); // move from dack to none or one card or two cards 
			}
			if (resultTab.isInCommunity)
			{
				IdResult resultParent = data.IdAnalizator (resultTab.parentId);
				if (!(resultParent.isCard && resultParent.card.IsOpened))
				for (int index = 0; index < totalFindPlaces; index++)
					if (resultFindPlaces [index].isInCommunity) sencePlaces.Add (findAllPlaces [index]);// move from community to none or one card at first and last time 

			}
			return sencePlaces;
		}
        private List<int> FindUserPlace(int id)
        {
            List<int> findAllPlaces = utils.ShuffleList(FindAllPlaces(id));
         
            if (findAllPlaces.Count.Equals(0)) return new List<int>();
            return findAllPlaces;
        }



        private List<int> FindAllPlaces (int id)
		{
			
			List<int> totalPlaces = new List<int> ();
			IdResult resultTab = data.IdAnalizator (id);
            CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(id);
 
			if (card.isRoot ||card.isDeck) return totalPlaces;
         

            if (card.Ace)
            {
                if (card.CardInFoundationStack()) return totalPlaces;
                List<int> thronBase = FindPlacesInThronBase(id);
                totalPlaces = utils.AddTo(thronBase, totalPlaces);
              // //  UnityEngine.Debug.Log("totalPlaces Ace " + totalPlaces.Count);
                if (totalPlaces.Count > 0) return totalPlaces;
            }

             List<int> thron = FindPlacesInThron(id);
             totalPlaces = utils.AddTo(thron, totalPlaces);
      
            if (totalPlaces.Count == 0)
            {
                //UnityEngine.Debug.Log("card.King " + card.King);
                if (card.King)
                {

                    List<int> communityBase = FindPlacesInCommunityBase(id);
                    totalPlaces = utils.AddTo(communityBase, totalPlaces);
                   ////  UnityEngine.Debug.Log("totalPlaces King " + totalPlaces.Count);
                }
                else
                {
                    List<int> community = FindPlacesInCommunity(id);
                    totalPlaces = utils.AddTo(community, totalPlaces);

                }
            }
         


             
            return totalPlaces;
        }
		private List<int> FindPlacesInCommunity (int id)
		{
			List<int> result= new List<int>();
            CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(id);

            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
            {
                CardItem[] tableuStacks = SolitaireStageViewHelperClass.instance.GetTableuStacks[i].GetComponentsInChildren<CardItem>();
                CardItem lastCard = tableuStacks[tableuStacks.Length - 1];

                if (!lastCard.isRoot)
                {

                  // //  UnityEngine.Debug.Log(string.Format("FindPlacesInCommunity {0}-{1}", lastCard, card));
                    bool rank = (lastCard.Rank - card.Rank).Equals(1);
                    bool suit = lastCard.isBlackSuit != card.isBlackSuit || lastCard.isRedSuit != card.isRedSuit;
                  
                    if (rank && suit)
                    {

                        result.Add(lastCard.Id);


                    }
                }


            }

 
			return result;
		}
		private List<int> FindPlacesInThronBase (int id)
        {
            List<int> result = new List<int>();
            CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(id);
           
            switch (card.Suit)
            {
                case 0:
                    //diamonds
                    result.Add(-102);
                   // card.Hide = true;
                    break;
                case 1:
                    //hearts
                    result.Add(-101);
                  //  card.Hide = true;
                    break;
                case 2:
                    //clubs
                    result.Add(-103);
                 //   card.Hide = true;
                    break;
                case 3:
                    //spades
                    result.Add(-104);
                  //  card.Hide = true;
                    break;
            }

            return result;
        }

    
    private List<int> FindPlacesInThron(int id)
    {

        List<int> result = new List<int>();
        CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(id);
        for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetFoundationStacks.Count; i++)
        {
            CardItem[] cardItems = SolitaireStageViewHelperClass.instance.GetFoundationStacks[i].GetComponentsInChildren<CardItem>();
            CardItem lastCard = cardItems[cardItems.Length - 1];
          //     //  UnityEngine.Debug.Log(string.Format("PlaceInThorn {0}-{1}", card, lastCard));
            if ((card.Rank - lastCard.Rank).Equals(1) && card.Suit.Equals(lastCard.Suit) && !card.hasChildCard)
            {
              //  card.Hide = true;
                result.Add(lastCard.Id);
               
            }
        }



        return result;
    }
    private List<int> FindPlacesInCommunityBase (int id)
		{
            List<int> result = new List<int>();

            CardItem cardItem = SolitaireStageViewHelperClass.instance.FindCardItem(id);



            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
            {
//               //  UnityEngine.Debug.Log(string.Format("PlacesInCommunityBase {0}-{1}", !SolitaireStageViewHelperClass.instance.GetTableuStacks[i].hasChildCard, cardItem.King));
                if (!SolitaireStageViewHelperClass.instance.GetTableuStacks[i].hasChildCard && cardItem.King)
                {
                    result.Add(SolitaireStageViewHelperClass.instance.GetTableuStacks[i].Id);
                    break;
                }
            }


            return result;
        }
		#endregion
		#region GetHints
		private List<ContractCommand> GetDeckCardHints() // attantion solver use it
		{
			List<ContractCommand> hintsCommand = new List<ContractCommand> ();
			int length = data.GetCountElementsInHolder (data.DeckIndex);
			if (length.Equals(0)) return hintsCommand;
			if (data.IsDeckOpen())
			{
				int deckCardId = data.Holder [data.DeckIndex].Element [length - 1].Id;
				List<int> deckHints = FindSolverPlace (deckCardId);
				foreach (int idTo in deckHints) hintsCommand.Add(new ContractCommand(ContractCommand.State.Move,deckCardId, idTo));
			}
			return hintsCommand;
		}

        private List<ContractCommand> GetFirstAndLastOpenCommunityCardHints() // attantion solver use it
        {
            List<ContractCommand> hintsCommand = new List<ContractCommand>();
          


       

            List<CardItem> lastCards = new List<CardItem>();
            List<int> firstCommunityHints = new List<int>();
            CardItem[] cardsChidDeck = SolitaireStageViewHelperClass.instance.GetDeckStack.GetComponentsInChildren<CardItem>();
            CardItem lastCardDeck = cardsChidDeck[cardsChidDeck.Length - 1];

            List<int> firstThornHints = FindPlacesInThron(lastCardDeck.Id);
            if (firstThornHints.Count > 0)
            {
                hintsCommand.Add(new ContractCommand(ContractCommand.State.Move, lastCardDeck.Id, firstThornHints[0]));
                return hintsCommand;
            }


            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
            {
                CardItem[] tableuStacks = SolitaireStageViewHelperClass.instance.GetTableuStacks[i].GetComponentsInChildren<CardItem>();
                lastCards.Add(tableuStacks[tableuStacks.Length - 1]);
            }

            for (int i = 0; i < lastCards.Count; i++)
            {
                if (lastCards[i].Ace)
                {
                   
                    IdResult idResult = new IdResult();
                    idResult.id = lastCards[i].Id;
                    idResult.card.Suit = lastCards[i].Suit;
                    List<int> thornBase = FindPlacesInThronBase(idResult.id);
                 
                    for (int h = 0; h < thornBase.Count; h++)
                    {
                        hintsCommand.Add(new ContractCommand(ContractCommand.State.Move, lastCards[i].Id, thornBase[h]));
                        return hintsCommand;
                    }
                }
                else
                {
                  
                
                    if (!lastCardDeck.isRoot)
                    {
                        if (lastCards[i].Rank - lastCardDeck.Rank == 1 && (lastCardDeck.isRedSuit && lastCards[i].isBlackSuit || lastCardDeck.isBlackSuit && lastCards[i].isRedSuit))
                        {
                            
                            hintsCommand.Add(new ContractCommand(ContractCommand.State.Move, lastCardDeck.Id, lastCards[i].Id));
                            return hintsCommand;

                        }
                    }
                }
            }


            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
            {
                CardItem[] tableuStacks = SolitaireStageViewHelperClass.instance.GetTableuStacks[i].GetComponentsInChildren<CardItem>();

                for (int j = 0; j < tableuStacks.Length; j++)
                {

                    if (!tableuStacks[j].isRoot && tableuStacks[j].isOppened)
                    {
                       
                        for (int h = 0; h < lastCards.Count; h++)
                        {
                            if (h == j) continue;
                            if (lastCards[h].Rank - tableuStacks[j].Rank == 1 && (tableuStacks[j].isRedSuit && lastCards[h].isBlackSuit || tableuStacks[j].isBlackSuit && lastCards[h].isRedSuit))
                            {
                                firstCommunityHints.Add(lastCards[h].Id);
                                hintsCommand.Add(new ContractCommand(ContractCommand.State.Move, tableuStacks[j].Id, lastCards[h].Id));
                                return hintsCommand;
                    
                            }


                        }
                    }

                }
            }


            
            

 



            return hintsCommand;

        }
        #endregion

        #region Public
        public bool CanAttach (int idFrom, int idTo)
		{
			IdResult resultFrom = data.IdAnalizator (idFrom);
			IdResult resultTo = data.IdAnalizator (idTo);

         //  UnityEngine.Debug.Log(string.Format("{0}-{1}-{2}-{3}", CanPutCardToCommunityBase(idFrom, idTo), CanPutCardToThronBase(idFrom, idTo), CanPutCardToCommunityCards(idFrom, idTo), CanPutCardToThronCards(idFrom, idTo)));
			if (CanPutCardToCommunityBase (idFrom, idTo) ||
			    CanPutCardToThronBase (idFrom, idTo) ||
			    CanPutCardToCommunityCards (idFrom, idTo) ||
			    CanPutCardToThronCards (idFrom, idTo))
				return true;
			return false;
		}

		public int ShouldOpenDownCard (int id)
		{
 
            CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(id);
            if(card.parentCard!=null && !card.parentCard.isRoot)
            {
                return card.parentCard.Id;
            }
		 
			return Data.NULL_CARD;
		}
		public List<ContractCommand> GetHints () // attantion solver use it
		{
            List<ContractCommand> hints = new List<ContractCommand>();



            List<ContractCommand> communityHint = GetFirstAndLastOpenCommunityCardHints();
            hints = utils.AddTo(communityHint, hints);

            return hints;
        }


		public bool HasBetterPlace (int id)
		{
			List<int> allCombs = FindUserPlace (id);
			return (allCombs.Count.Equals(0)) ? false : true;
		}
		public int GetBetterPlace (int id)
		{
			List<int> allCombs = FindUserPlace (id);
			if (allCombs.Count>0) return utils.RandomElement(allCombs);
			return Data.NULL_CARD;
		}


		public void ReverseDeck(bool isOpenCard)
		{
			foreach (Card element in data.Holder [data.DeckIndex].Element) element.IsOpened = isOpenCard;
		}

		public bool Move (int idFrom, int  idTo)
		{
			return data.Move (idFrom, idTo);
		}

		public void TurnCard (int id)
		{
			data.TurnCard (id);
		}
		public int GetNextDeckCard ()
		{
			int nextCardId = Data.NULL_CARD;
			int deckLength = data.GetCountElementsInHolder (data.DeckIndex);
			if (deckLength > 0)
			{
				nextCardId = data.Holder [data.DeckIndex].Element [0].Id;
				if (deckLength > 1) data.MoveToEndList (nextCardId);
				data.Holder [data.DeckIndex].Element [deckLength-1].IsOpened = true;
			}
    
			return nextCardId;
		}
		public void UndoShiftDeck ()
		{
			int deckLength = data.GetCountElementsInHolder (data.DeckIndex);
			Card undoCard = data.Holder [data.DeckIndex].Element [deckLength - 1];
			undoCard.IsOpened = false;
			data.MoveToHomeList (undoCard.Id);
		}
		public bool IsComplete ()
		{
        
            int count = 0;
            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetFoundationStacks.Count; i++)
            {
                CardItem[] cards = SolitaireStageViewHelperClass.instance.GetFoundationStacks[i].GetComponentsInChildren<CardItem>();
                if (cards[cards.Length - 1].King )count++;
            }
//           //  UnityEngine.Debug.Log("count " + count);
            return count >= 4;
        }
		public bool IsCommunityHasAllOpenCard()
		{
            for (int i = 0; i < SolitaireStageViewHelperClass.instance.GetTableuStacks.Count; i++)
            {
                CardItem[] cardItems = SolitaireStageViewHelperClass.instance.GetTableuStacks[i].GetComponentsInChildren<CardItem>();
                for (int j = 1; j < cardItems.Length; j++)
                {
                    if (!cardItems[j].isOppened) return false;
                }
            }
            if (SolitaireStageViewHelperClass.instance.GetDeckStack.hasChildCard || SolitaireStageViewHelperClass.instance.GetStockStack.hasChildCard) return false;

            return true;
        }
		public bool IsDeckEmpty()
		{
            bool isDeckEmpty = !SolitaireStageViewHelperClass.instance.GetStockStack.hasChildCard;
			return isDeckEmpty;
		}
		public bool IsDeckOpenedAllCards()
		{
			if (IsDeckEmpty ())
				return true;
			bool isDeckOpenedAllCards = true;
			foreach (Card element in data.Holder [data.DeckIndex].Element)
				if (!element.IsOpened)
					isDeckOpenedAllCards = false;
			return isDeckOpenedAllCards;
		}
		public int DeckRestCardsCount()
		{
            CardItem[] cardItems = SolitaireStageViewHelperClass.instance.GetStockStack.GetComponentsInChildren<CardItem>();

			return cardItems.Length-1;
		} 
		public int DeckTotalCardsCount()
		{
			return data.GetCountElementsInHolder (data.DeckIndex);
		}
		public bool HasHintInDeck (bool isOneCardSet)
		{
			int length = data.GetCountElementsInHolder (data.DeckIndex);
			if (length.Equals(0)) return false;

			if (isOneCardSet)
						
			for (int index = 0; index < length; index++)
			{
				int deckCardId = data.Holder [data.DeckIndex].Element [index].Id;
				List<int> deckHints = FindSolverPlace (deckCardId);
				if (!deckHints.Count.Equals (0))
					return true;
			}
			
			else
			{
				int index = -1;
				do {
					index = (index + 3 < length) ? index + 3 : length - 1;

					int deckCardId = data.Holder [data.DeckIndex].Element [index].Id;
					List<int> deckHints = FindSolverPlace (deckCardId);
					if (!deckHints.Count.Equals (0))
						return true;
				 
				}
				while (index < (length - 1));
			}
			return false;
		}			
		#endregion
		public int GetLastClosedCardInDeck()
		{
			int deckCloseId = data.GetStackHolderId ().DECK_STACK_HOLDER;
			int deckIndex = data.DeckIndex;
			foreach (Card element in data.Holder [deckIndex].Element)
				if (!element.IsOpened) deckCloseId = element.Id;
			
			return deckCloseId;
		}
	}
}