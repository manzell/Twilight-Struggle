using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Containment : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        List<Card> adjustedCards = new List<Card>();
        
        foreach(Card card in FindObjectOfType<Game>().playerMap[Game.Faction.USA].hand)
        {
            if (card is ScoringCard || card.opsValue >= 4) break; 

            adjustedCards.Add(card);
            card.opsValue++; 
        }

        Game.currentTurn.turnEndEvent.AddListener(ResetInfluenceValues);

        command.callback.Invoke(); 

        void ResetInfluenceValues(Turn turn)
        {
            foreach (Card card in adjustedCards)
                card.opsValue--; 
        }
    }
}
