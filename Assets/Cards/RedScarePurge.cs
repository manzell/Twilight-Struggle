using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedScarePurge : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        List<Card> adjustedCards = new List<Card>();

        foreach (Card card in FindObjectOfType<Game>().playerMap[Game.Faction.USA].hand)
        {
            if (card is ScoringCard || card.opsValue < 2) break;

            adjustedCards.Add(card);
            card.opsValue--;
        }

        Game.currentTurn.phaseEndEvent.AddListener(ResetInfluenceValues);

        command.callback.Invoke();

        void ResetInfluenceValues(Phase turn)
        {
            foreach (Card card in adjustedCards)
                card.opsValue++;
        }
    }
}
