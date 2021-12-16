using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defectors : Card, IHeadlineEvent
{
    public override void CardEvent(GameAction.Command command)
    {
        if(Game.phasingPlayer == Game.Faction.USSR)
            Game.AdjustVPs.Invoke(1);

        command.callback.Invoke(); 
    }

    public void HeadlineEvent(HeadlinePhase headline)
    {
        if(headline.headlines[Game.Faction.USA].card == this)
        {
            // Discard the USSR Headline Card
            Game.deck.Add(headline.headlines[Game.Faction.USSR].card);

            // TODO: Skip Headline Phase

            FindObjectOfType<UIMessage>().Message($"USSR Headline ({headline.headlines[Game.Faction.USSR].card.cardName}) is canceled by Defectors!"); 
        }
    }
}
