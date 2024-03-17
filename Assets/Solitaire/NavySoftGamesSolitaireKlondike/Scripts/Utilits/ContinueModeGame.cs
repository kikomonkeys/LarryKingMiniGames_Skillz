using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataCardResumeGroup
{
    public int id;
    public List<DataCardResume> dataCardResumes = new List<DataCardResume>();
}
[System.Serializable]
public class DataCardResume
{
    public int id;
    public int bestPlaceId;
    public int parentID;
    public bool isOpen;


    public bool ShiftDeck()
    {
        return id == -99 && bestPlaceId == -99;
    }

    public bool TurnDeck()
    {
        return id == 0 && bestPlaceId == 0;
    }

}
public class ContinueModeGame : MonoBehaviour
{
    public static ContinueModeGame instance;

    [System.Serializable]
    public class StatusInMatch
    {
        public int score;
        public int timer;
        public int move;
    }

    [SerializeField]
    private List<DataCardResumeGroup> dataInMatch = new List<DataCardResumeGroup>();

    [SerializeField]
    private StatusInMatch statusInMatch;
    [SerializeField]
    private DataCardResumeGroup dataUserClickCard = new DataCardResumeGroup();

    
    [SerializeField]
    private bool loadSuccess = false;
    public bool LoadSuccess { get { return loadSuccess; } }
    [SerializeField]
    private bool isDataInMatch = false;
    public bool HasDataInMatch { get { return isDataInMatch; } }
    private void Awake()
    {
        instance = this;
  



      
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        //string data = PlayerPrefAPI.LoadDataInMatch();
        //string dataClick = PlayerPrefAPI.LoadDataUserClickCard();
        //if(!string.IsNullOrEmpty(dataClick))
        //dataUserClickCard = JsonUtility.FromJson<DataCardResumeGroup>(dataClick);
        //if (data != string.Empty)
        //{
        //    DataCardResumeGroup[] dataCardResumeGroups = JsonHelper.FromJson<DataCardResumeGroup>(PlayerPrefAPI.LoadDataInMatch());


        //    for (int i = 0; i < dataCardResumeGroups.Length; i++)
        //    {
        //        dataInMatch.Add(dataCardResumeGroups[i]);
        //    }
        //    isDataInMatch = true;
          

        //    statusInMatch = JsonUtility.FromJson<StatusInMatch>(PlayerPrefs.GetString("StatusInMatch"));
        //}
        //else
        //{

        //    StartCoroutine(StartNewGame());
        //}

        StartCoroutine(StartNewGame());

    }


    private IEnumerator StartNewGame()
    {
        yield return new WaitForSeconds(2f);

        loadSuccess = true;
        dataUserClickCard.dataCardResumes.Clear();
        StageManager.instance.GetManagerLogic.StartCountGame();

    }
    public void AddDataGroup(DataCardResumeGroup group)
    {
        dataInMatch.Add(group);
    }
    public void ClearDataGroup()
    {
        dataInMatch.Clear();
    }
    public void SetLoadSuccess(bool value)
    {
        loadSuccess = value;
    }
    public void AddDataStep(int id, int best_place_id, int parent_id,bool isOpen)
    {
        DataCardResume dataCard = new DataCardResume();
        dataCard.id = id;
        dataCard.bestPlaceId = best_place_id;
        dataCard.parentID = parent_id;
        dataCard.isOpen = isOpen;
        dataUserClickCard.dataCardResumes.Add(dataCard);
    }

  


    public void ClearAllDataCard(bool resetGameData = true)
    {

        if (resetGameData)
        {
           GameSettings.Instance.calendarData =new string[2];
            for (int i = 0; i < GameSettings.Instance.calendarData.Length; i++)
            {
                GameSettings.Instance.calendarData[i] = string.Empty;
            }
           PlayerPrefAPI.Set();

            
        }
        dataUserClickCard.dataCardResumes.Clear();
        dataInMatch.Clear();
        
        SaveTimeAndMove(0, 0,0);
        SaveDataCardInMatch(true);
    
        isDataInMatch = false;
        loadSuccess = true;
     

    }
    public void UndoCardInMatch()
    {
        if (dataUserClickCard.dataCardResumes.Count > 0)
        {
            dataUserClickCard.dataCardResumes.RemoveAt(dataUserClickCard.dataCardResumes.Count - 1);
        }
      
    }


    public void SaveDataCardInMatch(bool stringEmpty =false,CardItem cardFrom=null ,CardItem cardTo=null )
    {
        SolitaireStageViewHelperClass.instance.SaveAllCardInMatch(cardFrom,cardTo);

        string dataUserClick = JsonUtility.ToJson(dataUserClickCard);
        string data = JsonHelper.ToJson<DataCardResumeGroup>(dataInMatch.ToArray());

        if (stringEmpty)
        {
            data = string.Empty;
        }
        PlayerPrefAPI.SaveDataUserClickCard(dataUserClick);
        PlayerPrefAPI.SaveDataInMatch(data);
       
    }

    public void SaveTimeAndMove(int move, int timer,int score)
    {

        statusInMatch.move = move;
        statusInMatch.timer = timer;
        statusInMatch.score = score;
        string rawSetData = JsonUtility.ToJson(statusInMatch);

        PlayerPrefs.SetString("StatusInMatch", rawSetData);
    }

    public void DealCardContinueMode()
    {
        for (int i = 0; i < dataInMatch.Count; i++)
        {
            CardItem cardFrom = null;
            CardItem cardTo = null;
            //Firtst Move Card To Root
 
            if (dataInMatch[i].dataCardResumes.Count == 0) continue;
            cardFrom = SolitaireStageViewHelperClass.instance.FindCardItem(dataInMatch[i].dataCardResumes[0].id);
            cardTo = SolitaireStageViewHelperClass.instance.FindCardItem(dataInMatch[i].id);
            cardFrom.openCard(dataInMatch[i].dataCardResumes[0].isOpen);

            SolitaireStageViewHelperClass.instance.MoveCard(cardFrom, cardTo);
            for (int j = 0; j < dataInMatch[i].dataCardResumes.Count - 1; j++)
            {



                cardFrom = SolitaireStageViewHelperClass.instance.FindCardItem(dataInMatch[i].dataCardResumes[j + 1].id);
                cardTo = SolitaireStageViewHelperClass.instance.FindCardItem(dataInMatch[i].dataCardResumes[j].id);

                cardFrom.openCard(dataInMatch[i].dataCardResumes[j + 1].isOpen);
                cardTo.openCard(dataInMatch[i].dataCardResumes[j].isOpen);
                SolitaireStageViewHelperClass.instance.MoveCard(cardFrom, cardTo);
            }
           
         
        }

       
 

    }



    public IEnumerator LoadContinueMatch()
    {

        yield return new WaitForSeconds(.1f);

        if (isDataInMatch)
        {

            for (int i = 0; i < dataUserClickCard.dataCardResumes.Count; i++)
            {
                DataCardResume dataCard = dataUserClickCard.dataCardResumes[i];
                CardItem card = SolitaireStageViewHelperClass.instance.FindCardItem(dataCard.id);
                if(dataCard.isOpen)
                card.openCard(dataCard.isOpen);

                if (dataCard.ShiftDeck())
                {
                    Debug.LogError(":::::shiftdeck:::::::");
                    StageManager.instance.CreateCommand(dataCard.id, dataCard.bestPlaceId, dataCard.parentID, 1, dataCard.isOpen);
                }
                else if (dataCard.TurnDeck())
                {
                    Debug.LogError(":::::Turndeck:::::::");
                    StageManager.instance.CreateCommand(dataCard.id, dataCard.bestPlaceId, dataCard.parentID, 2, dataCard.isOpen);
                }
                else
                {
                    Debug.LogError(":::::else condition:::::::");
                    StageManager.instance.CreateCommand(dataCard.id, dataCard.bestPlaceId, dataCard.parentID,0, dataCard.isOpen);
                }
            }
        }



        CardItem[] cardsInDeck = SolitaireStageViewHelperClass.instance.GetDeckStack.GetComponentsInChildren<CardItem>();

        for (int i = 0; i < cardsInDeck.Length - 2; i++)
        {
            cardsInDeck[i].rect.anchoredPosition = Vector2.zero;
        }



        CardItem[] cardsInStock = SolitaireStageViewHelperClass.instance.GetStockStack.GetComponentsInChildren<CardItem>();

        for (int i = 0; i < cardsInStock.Length ; i++)
        {
            cardsInStock[i].openCard(false);
        }


        DealCardContinueMode();

        yield return new WaitForSeconds(1f);
       
        loadSuccess = true;
      
        RateUsController.instance.CheckRatePopUp();
        HUDController.instance.SetMove(statusInMatch.move);
        
        HUDController.instance.SetTime(statusInMatch.timer);
        StageManager.instance.AddStatusGame(statusInMatch.score, statusInMatch.timer, statusInMatch.move);

    }

   
}
