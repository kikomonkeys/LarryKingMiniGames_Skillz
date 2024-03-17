﻿using System.Collections.Generic;
using SolitaireEngine;
using SolitaireEngine.Model;
using UserWindow;


public partial class StageManager
{

    #region Debug and Test
    public void OnComplete80()
    {
        List<ICommand> solution_commands = commandBuilder.ConvertContractCommandToICommand(solitaire.GetSolution(), GameSettings.Instance.isOneCardSet);
        int count = solution_commands.Count - 5;

        for (int i = 0; i < count; i++)
        {
            executor.Execute(solution_commands[i]);

        }
        // TODO: fix undo after that code
    }
    #endregion

    #region HUD Actions
    // back menu/shop
    void IHUDActions.OnBackToMenuPressed()
    {
        if (GameSettings.Instance.isCalendarGame) LoadCalendarScene();
        else GeneralBackToMenu();
    }
    void IHUDActions.OnStorePressed()
    {
        ShowStorePopUp();
    }
    void IHUDActions.OnShowManualPressed()
    {

    }


    // bottom buttons
    void IHUDActions.OnOptionsPressed()
    {
        GeneralBackToSettings();
    }
    void IHUDActions.OnSolutionPressed()
    {
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Up();
        viewer.UnblinkAll();
        StartCoroutine(ShowSolution());
    }

    void IHUDActions.OnRestartPressed()
    {
        ContinueModeGame.instance.ClearAllDataCard(false);
        //if (GameSettings.Instance.isSoundSet) Sound.Instance.Distribute ();
        GeneralRestartGame();
    }
    void IHUDActions.OnNewGamePressed()
    {
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Up();

        GeneralNewGame();
    }
    void IHUDActions.OnDailyChallengePressed()
    {
        LoadCalendarScene();
    }
    void IHUDActions.OnStatisticPressed()
    {
        PopUpManager.Instance.ShowStats();
    }

    void IHUDActions.OnHintsPressed()
    {
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Creack();
        managerLogic.StartCountGame();
        // TODO remove it

        commandUpdater.Reset();
        List<ICommand> listToExecute = new List<ICommand>();

        foreach (ContractCommand element in GetHints())
        {

            if (element.Action.Equals(ContractCommand.State.ShiftDeckOnece))
            {

                viewer.BlinkCard(element.IdFrom);
                managerLogic.isAllowBlinkHint = false;
                return;
            }

            ICommand command = new MoveHintCommand(viewer, element.IdFrom, element.IdTo);
            listToExecute.Add(command);
        }
        SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Hint);


        HUDController.instance.setTrigger(false, () =>
        {
            commandUpdater.Reset();

            HUDController.instance.triggerLess.SetActive(false);
        });

        commandUpdater.ExecuteList(listToExecute, null, () =>
        {

            HUDController.instance.triggerLess.SetActive(false);
        });


    }
    void IHUDActions.OnUndoPressed()
    {




        if (GameSettings.Instance.isSoundSet)
        {

            Solitaire_GameStake.Sound.Instance.Up();
        }

        if (executor.HasCommands())
        {
            // TODO: remove it
            SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Undo);


            executor.Unexecute();

            if (GameSettings.Instance.isSoundSet)
            {

                Solitaire_GameStake.Sound.Instance.Shift();
            }
            managerLogic.AddMoves();
        }


    }
    void IHUDActions.OnNoUsefulHintsPressed()
    {
        managerLogic.isGameWin = false;
        managerLogic.SetStopStatsAndSetting();
        HUDController.instance.SetSolutionLayout(false);
        OnGameCompletedAction();
    }

    // solution
    void IHUDActions.OnExitSolution()
    {
        // pause executing solution commands
        StageManager.instance.commandUpdater.Pause();

        PopUpExitSolution();
    }
    #endregion


    #region Actions received from view
    CardItem carditem, clickedCardItem;//piper
    void ICardActions.OnClickCard(int id, int parent_id)
    {
        managerLogic.StartCountGame();
        //GameSettings.Instance.isAutoCompleteSet = false;
        // check from settingss
        if (!GameSettings.Instance.isAutoTapMoveSet)
            return;

        if (solitaire.HasBetterPlace(id))
        {
            int best_place_id = solitaire.GetBetterPlace(id);

            carditem = SolitaireStageViewHelperClass.instance.FindCardItem(best_place_id);//piper
            clickedCardItem = SolitaireStageViewHelperClass.instance.FindCardItem(id);//piper
            ////
            ///
            if (clickedCardItem.getRootCard().isTableu && carditem.getRootCard().isFoundation)
            {
                SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Move);

                ICommand move_command = commandBuilder.CreateMoveCardAndTryOpenParentCommand(id, best_place_id, parent_id, SolitaireStageViewHelperClass.instance.FindCardItem(parent_id).isOppened, true);
                ContinueModeGame.instance.AddDataStep(id, best_place_id, parent_id, SolitaireStageViewHelperClass.instance.FindCardItem(parent_id).isOppened);
                executor.Execute(move_command);
                managerLogic.AddMoves();

                //piper
                if (carditem.getRootCard().isFoundation && !carditem.isGivenScoreToCard)
                {
                    AddScore(id);
                    carditem.isGivenScoreToCard = true;
                }
                //piper
            }
            else
            {
                if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.MissCard();
                ICommand command = new ShakeCardCommand(viewer, id);
                executor.Execute(command, false);
            }

        }
        else
        {
            if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.MissCard();
            ICommand command = new ShakeCardCommand(viewer, id);
            executor.Execute(command, false);
        }

    }
    //piper
    void AddScore(int cardId)
    {
        int mycardId;
        mycardId = cardId % 13;
        //DebugGame.print("add score if the card added to the foundation");
        if (mycardId < 10 && mycardId > 0)
        {
            int score;
            score = 100 - ((mycardId - 1) * 10);
            //DebugGame.print("score value::" + score);
            managerLogic.AddScore(score);
        }
    }
    CardItem dropNearCard, dropcard;
    void ICardActions.OnDropCard(int id, int parent_id, List<int> near_ids, CardItem cardItem)
    {

        if (cardItem.childCard == null || cardItem.GetComponentInParent<CardHasChild>() == null)
        {
            managerLogic.StartCountGame();
            foreach (int near_id in near_ids)
            {
                if (near_id == id || near_id == parent_id || near_id.Equals(Data.NULL_CARD))
                {
                    throw new UnityEngine.UnityException("Invalid near card!");
                }
                print("can attach::" + solitaire.CanAttach(id, near_id));
                dropNearCard = SolitaireStageViewHelperClass.instance.FindCardItem(near_id);//piper
                dropcard = SolitaireStageViewHelperClass.instance.FindCardItem(id);//piper
                if (dropcard.getRootCard().isFoundation && dropNearCard.getRootCard().isFoundation)//piper
                {
                    if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.MissCard();

                    ICommand command_1 = new MoveCardCommand(viewer, id, parent_id, -1);
                    executor.Execute(command_1, false);
                    return;
                }//piper

                if (solitaire.CanAttach(id, near_id))
                {

                    ContinueModeGame.instance.AddDataStep(id, near_id, parent_id, SolitaireStageViewHelperClass.instance.FindCardItem(parent_id).isOppened);
                    int best_place_id = solitaire.GetBetterPlace(id);

                    if ((dropNearCard.Rank > dropcard.Rank) && dropNearCard.getRootCard().isFoundation)//piper
                    {
                        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.MissCard();

                        ICommand command_1 = new MoveCardCommand(viewer, id, parent_id, -1);
                        executor.Execute(command_1, false);
                        return;
                    }
                    SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Move);
                    ICommand move_command = commandBuilder.CreateMoveCardAndTryOpenParentCommand(id, near_id, parent_id, SolitaireStageViewHelperClass.instance.FindCardItem(parent_id).isOppened, true);

                    executor.Execute(move_command);
                    //  DebugGame.print("card parent::" + cardItem.getRootCard().isFoundation);
                    //piper
                    if (cardItem.getRootCard().isFoundation && !cardItem.isGivenScoreToCard)
                    {
                        AddScore(id);
                        cardItem.isGivenScoreToCard = true;
                    }
                    //}
                    //piper
                    managerLogic.AddMoves();

                    return;
                }
            }
        }

        // move back it no cards found to be attached
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.MissCard();

        ICommand command = new MoveCardCommand(viewer, id, parent_id, -1);


        executor.Execute(command, false);


    }
    void ICardActions.OnClickDeck()
    {
        Solitaire_GameStake.Sound.Instance.TouchCard();

        if (!solitaire.IsDeckEmpty())
        {
            managerLogic.StartCountGame();
            SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.Deck);

            ICommand shiftDeckCommand;
            if (GameSettings.Instance.isOneCardSet)
            {
                shiftDeckCommand = commandBuilder.CreateShiftDeckCommand();
            }
            else
            {
                int cardsInDeckCount = solitaire.DeckRestCardsCount();
                int deckTurnCount = (cardsInDeckCount > 3) ? 3 : cardsInDeckCount;
                //DebugGame.print("deckTurnCount::" + deckTurnCount);
                shiftDeckCommand = commandBuilder.CreateShiftDeckCommand(deckTurnCount);
            }

            ContinueModeGame.instance.AddDataStep(-99, -99, GameSettings.Instance.isOneCardSet ? 1 : 3, false);

            executor.Execute(shiftDeckCommand);
            managerLogic.AddMoves();
        }
    }
    int deckTurnCount;//piper
    void ICardActions.OnTurnDeck()
    {
        Solitaire_GameStake.Sound.Instance.TouchCard();
        deckTurnCount++;
        CardItemsDeck.instance.SetDeckImage(true, deckTurnCount);
        //StageManager.instance.ChangeDeckImageSpr(deckTurnCount);
        if (deckTurnCount > 3)
        {
            DebugGame.print("decrease 20 points from total points");
            managerLogic.AddScore(-20);
        }
        if (solitaire.IsDeckEmpty())
        {
            managerLogic.StartCountGame();
            if (!GameSettings.Instance.isStandardSet && !managerLogic.HasDeckTurn)  //countDeckTurn >= DECK_TURN_LIMIT)
            {
                Solitaire_GameStake.Sound.Instance.MissCard();
                return;
            }
            ICommand command_turn_deck = commandBuilder.CreateTurnDeckCommand();
            // DebugGame.print("piper on turn deck");
            ContinueModeGame.instance.AddDataStep(0, 0, 0, false);
            executor.Execute(command_turn_deck);
            managerLogic.AddMoves();
        }
    }
    void ICardActions.OnClickAnywhere()
    {
        viewer.UnblinkAll();
        managerLogic.StartCountGame();
    }
    #endregion


    #region Actions received from command
    public void Move(int idFrom, int idTo)
    {

        if (!ContinueModeGame.instance.LoadSuccess) return;
        solitaire.Move(idFrom, idTo);
    }
    public void TurnCard(int id)
    {
        if (!ContinueModeGame.instance.LoadSuccess) return;
        //managerLogic.AddScore(20);//piper
        solitaire.TurnCard(id);
    }
    public void ShiftDeck()
    {
        if (!ContinueModeGame.instance.LoadSuccess) return;
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Foley();

        solitaire.ShiftDeck();
    }
    public void UndoShiftDeck()
    {
        if (!ContinueModeGame.instance.LoadSuccess) return;
        if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Foley();
        solitaire.UndoShiftDeck();
    }
    public void ReverseDeck(bool isOpenCard)
    {

        if (!ContinueModeGame.instance.LoadSuccess) return;


        if (isOpenCard)
        {
            if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Up();
            GameSettings.Instance.countDeckTurn--;
        }
        else
        {
            if (GameSettings.Instance.isSoundSet) Solitaire_GameStake.Sound.Instance.Down();
            GameSettings.Instance.countDeckTurn++;
        }

        PlayerPrefAPI.Set();
        bool isTurnImage = (GameSettings.Instance.isStandardSet || (!GameSettings.Instance.isStandardSet && managerLogic.HasDeckTurn));


        CardItemsDeck.instance.SetDeckImage(isTurnImage, deckTurnCount);

        solitaire.ReverseDeck(isOpenCard);
    }
    public void Score(int score)
    {
        managerLogic.AddScore(score);
    }
    public void SetThronCardMove(int id, bool isMoved)
    {
        solitaire.SetThronCardMove(id, isMoved);
    }
    public void SetCommunityCardMove(int id, bool isMoved)
    {
        solitaire.SetCommunityCardMove(id, isMoved);
    }
    #endregion

    void AutoCompleteGame()
    {

        managerLogic.isAllowBlinkHint = false;
        viewer.UnblinkAll();

        SolitaireStageViewHelperClass.instance.movementManager.SetMovingSpeed(SmoothMovementManager.Speed.AutoComplete);

        StartCoroutine(SolitaireStageViewHelperClass.instance.AutoMoveCard());

        HUDController.instance.VisibleButtonComplete(false);
        // block touches
        HUDController.instance.setTrigger(false, null);




    }



    // TODO: move it to shop builder

    #region PopUpWindow
    private void ShowStorePopUp()
    {
        string titleLineData = "STORE";
        string listLinesData = "Here you can remove ads" + "\n" + "and restore purchase.";

        // Build store pop up
        List<ResultButtonData> buttonData = new List<ResultButtonData>();
        buttonData.Add(new ResultButtonData("Remove ads", 1, OnShopRemove));
        buttonData.Add(new ResultButtonData("Restore purchase", 0, OnShopRestore));
        buttonData.Add(new ResultButtonData("Cancel", 2, OnShopCancel));

        // show store pop up
        PopUpManager.Instance.ShowDialog(titleLineData, listLinesData, buttonData);
    }
    private void OnShopRemove()
    {
        // TODO: Remove ads
        PopUpManager.Instance.Close();
    }
    private void OnShopRestore()
    {
        // TODO: Restore purchase
        PopUpManager.Instance.Close();
    }
    private void OnShopCancel()
    {
        PopUpManager.Instance.Close();
    }
    #endregion
}