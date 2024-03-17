using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHeader : MonoBehaviour
{
    public int cardID { get; set; }
    public CardItem card { get; private set; }

    private List<DataCardResume> dataCards = new List<DataCardResume>();

    private DataCardResumeGroup dataCardResumeGroup = new DataCardResumeGroup();
    private List<DataCardResume> dataCardContainer = new List<DataCardResume>();

    public void Init(int ID,CardItem card)
    {
        this.cardID = ID;
        this.card = card;
        for (int i = 0; i <25; i++)
        {
            DataCardResume dataCard = new DataCardResume();
            dataCards.Add(dataCard);
        }
        dataCardResumeGroup.id = ID;
    }

    public DataCardResumeGroup GetAllChildCard(bool change =false)
    {
        if (dataCardResumeGroup.dataCardResumes.Count == 0) change = false;

        dataCardContainer.Clear();
        dataCardResumeGroup.dataCardResumes.Clear();
        List<CardItem> cards =  card.getChildCardsList();
   
        for (int i = 0; i < cards.Count; i++)
        {
         
            dataCards[i].id = cards[i].Id;
            dataCards[i].isOpen = cards[i].isOppened;
            dataCardResumeGroup.dataCardResumes.Add(dataCards[i]);
        }

        return dataCardResumeGroup;
    }
}
