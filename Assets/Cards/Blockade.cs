using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockade : Card
{
    [SerializeField] Country westGermany;

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        // Prompt US player to discard a 3-op Card. 
        callback.Invoke(); 
    }

    public void AirLift(Card card)
    {
        if (card && card.opsValue >= 3)
        {
            Player.USA.hand.Remove(card);
            Game.deck.discards.Add(card);
        }
        else 
            Game.AdjustInfluence.Invoke(westGermany, Game.Faction.USA, -westGermany.influence[Game.Faction.USA]); 
    }
}
