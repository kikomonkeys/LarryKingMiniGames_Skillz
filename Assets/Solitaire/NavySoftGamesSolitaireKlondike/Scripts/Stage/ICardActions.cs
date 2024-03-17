using System.Collections.Generic;

public interface ICardActions
{
	void OnClickCard(int id, int parent_id);
	void OnDropCard(int id, int parent_id, List<int> nearest_cards_id, CardItem cardItem);

	void OnClickDeck();
	void OnTurnDeck();

	void OnClickAnywhere();
}