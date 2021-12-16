using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class HeadlineAction : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new HeadlineCommand(card, action);

    public override void onCommandExecute(Command command) // This is called from SetHeadline() once we have both Headlines and are ready to trigger. 
    { 
        HeadlineCommand headlineCommand = command as HeadlineCommand;
        Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headline.headlines;

        Game.Faction initiative = headlines[Game.Faction.USSR].cardOpsValue > headlines[Game.Faction.USA].cardOpsValue ? Game.Faction.USSR : Game.Faction.USA;

        // Call our card headline events. // This is way to ensure that our Before/On/After events get called on each of the events in the list. 
        List<GameEvent<HeadlinePhase>> actionList = new List<GameEvent<HeadlinePhase>>();

        actionList.Add(Game.currentTurn.headline.headlineEvent);
        actionList.Add(Game.headlineEvent);
        actionList.Add(headlines[Game.Faction.USA].card.headlineEvent);
        actionList.Add(headlines[Game.Faction.USSR].card.headlineEvent);

        foreach(GameEvent<HeadlinePhase> geph in actionList)
            geph.before.Invoke(Game.currentTurn.headline);
        foreach (GameEvent<HeadlinePhase> geph in actionList)
            geph.BaseInvoke(Game.currentTurn.headline);

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
            foreach (GameEvent<HeadlinePhase> geph in actionList)
                geph.after.Invoke(Game.currentTurn.headline);

            command.callback.Invoke(); 
        }
    }

    public void SetHeadline(Game.Faction faction, Card card)
    {
        HeadlineCommand headlineCommand = new HeadlineCommand(card, this);
        Dictionary<Game.Faction, HeadlineCommand> headlines = Game.currentTurn.headline.headlines;

        if (headlines.ContainsKey(faction))
            headlines[faction] = headlineCommand;
        else
            headlines.Add(faction, headlineCommand);

        // For now, automatically trigger the headlines if we have both; but we'll eventually put this into a button. 
        if (headlines.Count == 2)
            onCommandExecute(headlineCommand);
    }

    public class HeadlineCommand : Command // in Our case we will have two headline commands
    {
        public HeadlineCommand(Card c, GameAction a) : base(c, a) { }
    }
}

interface IHeadlineEvent
{
    public void HeadlineEvent(HeadlinePhase headline);
}