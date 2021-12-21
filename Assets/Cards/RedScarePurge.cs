using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedScarePurge : Card
{
    // TODO: Fix the underlying system whereby we add a Modifier to the Ops Value of the card. 
    public override void CardEvent(GameAction.Command command)
    {
        if (command.actingPlayer == Game.Faction.USSR)
            Message("Red Scare grips the US!");
        else
            Message("USSR Purges reactionaries from the party"); 

        List<Card> adjustedCards = new List<Card>();

        foreach (Card card in FindObjectOfType<Game>().playerMap[command.enemyPlayer].hand)
        {
            if (card is ScoringCard) break;

            adjustedCards.Add(card);
            card.bonusOps--; 
        }

        Game.currentTurn.phaseEndEvent.AddListener(ResetInfluenceValues);

        command.callback.Invoke();

        void ResetInfluenceValues(Phase turn)
        {
            foreach (Card card in adjustedCards)
                card.bonusOps++;
        }
    }
}
