using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveYearPlan : Card
{
    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        Player USSR = FindObjectOfType<Game>().playerMap[Game.Faction.USSR];

        int i = Random.Range(0, USSR.hand.Count); 

        Card card = USSR.hand[i];

        USSR.hand.Remove(card);
        Debug.Log($"5-Year Plan took {card.cardName} from USSR hand");
                
        if(card.faction == Game.Faction.USA)
            card.Event(callback);
        else
        {
            Game.deck.discards.Add(card);
            callback.Invoke(); 
        }
    }
}
