using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defectors : Card
{
    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        if(Game.phasingPlayer == Game.Faction.USSR)
            Game.AwardVictoryPoints.Invoke(1);
    }

    public override void OnHeadline(Dictionary<Game.Faction, Card> headlines)
    {
        if(headlines[Game.Faction.USA] == this)
        {
            Game.deck.AddRange(headlines.Values);
            headlines.Clear();
        }
    }
}
