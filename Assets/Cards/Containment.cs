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
            if (card is ScoringCard) break; 

            adjustedCards.Add(card);
            card.bonusOps++; 
        }

        if(adjustedCards.Count > 0)
            Game.currentTurn.phaseEndEvent.AddListener(ResetInfluenceValues);

        command.callback.Invoke(); 

        void ResetInfluenceValues(Phase phase)
        {
            foreach (Card card in adjustedCards)
                card.bonusOps = Mathf.Clamp(card.bonusOps - 1, 0, 5); 
        }
    }
}
