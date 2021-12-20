using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class HeadlineAction : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new HeadlineCommand(card, action);

    public override void ExecuteCommandAction(Command command) // This is called from SetHeadline() once we have both Headlines and are ready to trigger. 
    {
        Debug.Log($"Headling Executing {command.card.cardName}");
        Debug.Log(command.callback); 

        UnityAction originalCallback = command.callback; 
        HeadlineCommand headlineCommand = command as HeadlineCommand;
        Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headlinePhase.headlines;

        Game.Faction initiative = headlines[Game.Faction.USSR].cardOpsValue > headlines[Game.Faction.USA].cardOpsValue ? Game.Faction.USSR : Game.Faction.USA;

        FirstHeadline();
        void FirstHeadline()
        {
            Game.phasingPlayer = initiative;
            headlineCommand.actingPlayer = initiative;
            headlineCommand.callback = SecondHeadline;

            headlines[initiative].card.CardEvent(headlineCommand);
        }

        void SecondHeadline()
        {
            Game.phasingPlayer = headlineCommand.enemyPlayer;
            headlineCommand.actingPlayer = headlineCommand.enemyPlayer;
            headlineCommand.callback = FinishHeadline;

            headlines[headlineCommand.actingPlayer].card.CardEvent(headlineCommand);
        }

        void FinishHeadline()
        {
            originalCallback.Invoke(); 
        }
    }

    public void SetHeadline(Game.Faction faction, Card card)
    {
        HeadlineCommand headlineCommand = new HeadlineCommand(card, this);
        Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headlinePhase.headlines;
        Player player = FindObjectOfType<Game>().playerMap[faction];

        if (headlines.ContainsKey(faction))
        {
            // Readd the current Headline to the player's hand.             
            if(headlines[faction] != null)
                player.hand.Add(headlines[faction].card);

            headlines[faction] = headlineCommand;
        }
        else
            headlines.Add(faction, headlineCommand);

        player.hand.Remove(card);

        // For now, automatically trigger the headlines if we have both; but we'll eventually put this into a button. 
        if (headlines[Game.Faction.USSR] != null && headlines[Game.Faction.USA] != null)
            ExecuteCommandAction(headlineCommand);
    }

    public class HeadlineCommand : Command // in Our case we will have two headline commands
    {
        public HeadlineCommand(Card card, GameAction a) : base(card, a) { }
    }
}

interface IHeadlineEvent
{
    public void HeadlineEvent(HeadlinePhase headline);
}