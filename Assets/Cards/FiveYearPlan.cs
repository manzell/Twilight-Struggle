using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveYearPlan : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        Player USSR = FindObjectOfType<Game>().playerMap[Game.Faction.USSR];
        int i = Random.Range(0, USSR.hand.Count); 
        Card card = USSR.hand[i];

        USSR.hand.Remove(card);
        
        Message($"5-Year Plan discarded {card.cardName} from USSR hand");
                
        if(card.faction == Game.Faction.USA)
        {
            command.actingPlayer = Game.Faction.USA; // TODO - Maybe do some more manipulation of the Command event. Double check other cards for overreliance on the Command. 
            card.CardEvent(command); // TODO : Warning I have no idea if this is stable. Five year plan is can be an CardEventCommand from either faction, will that mess things up? 
        }
        else
            command.callback.Invoke();  
    }
}
