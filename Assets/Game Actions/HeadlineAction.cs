using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class HeadlineAction : GameAction
    {
        //public override void ExecuteCommandAction(Command command) // This is called from SetHeadline() once we have both Headlines and are ready to trigger. 
        //{
        //    UnityAction originalCallback = command.callback;
        //    HeadlineCommand headlineCommand = command as HeadlineCommand;
        //    Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headlinePhase.headlines;

        //    Game.Faction initiative = headlines[Game.Faction.USSR].cardOpsValue > headlines[Game.Faction.USA].cardOpsValue ? Game.Faction.USSR : Game.Faction.USA;

        //    FirstHeadline();
        //    void FirstHeadline()
        //    {
        //        Game.phasingPlayer = initiative;
        //        headlineCommand.actingPlayer = initiative;
        //        headlineCommand.callback = SecondHeadline;

        //        Debug.Log($"Headlining {headlines[headlineCommand.actingPlayer].card.cardName}");
        //        headlines[headlineCommand.actingPlayer].card.CardEvent(headlineCommand);
        //    }

        //    void SecondHeadline()
        //    {
        //        Game.phasingPlayer = headlineCommand.enemyPlayer;
        //        headlineCommand.actingPlayer = headlineCommand.enemyPlayer;
        //        headlineCommand.callback = FinishHeadline;

        //        Debug.Log($"Headlining {headlines[headlineCommand.actingPlayer].card.cardName}");
        //        headlines[headlineCommand.actingPlayer].card.CardEvent(headlineCommand);
        //    }

        //    void FinishHeadline()
        //    {

        //        Debug.Log("Finishing Headline Phase");
        //        //Game.currentPhase.NextPhase(originalCallback);
        //        originalCallback.Invoke(); // the callback SHOULD BE Game.currentPhase.nextPhase 
        //    }
        //}

        //public void SetHeadline(Game.Faction faction, Card card)
        //{
        //    HeadlineCommand headlineCommand = new HeadlineCommand(card, this);
        //    Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headlinePhase.headlines;
        //    Player player = FindObjectOfType<Game>().playerMap[faction];

        //    if (headlines.ContainsKey(faction))
        //    {
        //        // Readd the current Headline to the player's hand.             
        //        if (headlines[faction] != null)
        //            player.hand.Add(headlines[faction].card); // Todo We should never remove the card until we affirm we can headline it

        //        headlines[faction] = headlineCommand;
        //    }
        //    else
        //        headlines.Add(faction, headlineCommand);

        //    player.hand.Remove(card);

        //    // For now, automatically trigger the headlines if we have both; but we'll eventually put this into a button. 
        //    if (headlines[Game.Faction.USSR] != null && headlines[Game.Faction.USA] != null)
        //        ExecuteCommandAction(headlineCommand);
        //}

    }

    interface IHeadlineEvent
    {
        public void HeadlineEvent(TurnSystem.HeadlinePhase headline);
    }
}