using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenDistanceCard : MonoBehaviour
{


    private bool trigger = false;
    private int length = 0;
    public void CheckDistanceCard(bool resetLength)
    {
        StopAllCoroutines();
        if (resetLength)
        {
            length = 0;
        }
        StartCoroutine(CaculatorDistance());
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        CardItem[] cardItems = GetComponentsInChildren<CardItem>();
        length = cardItems.Length;
    }

  



    private IEnumerator CaculatorDistance()
    {
        yield return new WaitForSeconds(.1f);
        CardItem[] cardItems = GetComponentsInChildren<CardItem>();
        if (length != cardItems.Length)
        {
            length = cardItems.Length;
            for (int j = 2; j < cardItems.Length; j++)
            {
          

                Vector2 offSet = Vector2.zero;
                bool close = (!cardItems[j].isOppened )|| (!cardItems[j].parentCard.isOppened);


                offSet.y = cardItems[j].OffSetChildCard(cardItems.Length, close);
           
                if (close)
                {
                    cardItems[j].childOffset = offSet;
                }
                else
                {
                    cardItems[j].childOffsetOpened = offSet;
                }
                if (!ContinueModeGame.instance.LoadSuccess)
                    cardItems[j].rect.localPosition = offSet;
                else
                {
                    //LeanTween.move(cardItems[j].GetComponent<RectTransform>(), offSet, 0.1f);
                }


               
            }
        }
    }
}

