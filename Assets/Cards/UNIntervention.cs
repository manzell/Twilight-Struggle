using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UNIntervention : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        List<Card> eligibleCards = new List<Card>();

        foreach(Card card in FindObjectOfType<Game>().playerMap[command.actingPlayer].hand)
            if(command.actingPlayer == Game.Faction.USA && card.faction == Game.Faction.USSR || command.actingPlayer == Game.Faction.USSR && card.faction == Game.Faction.USA)
                eligibleCards.Add(card);

        cardClickHandler = new CardClickHandler(eligibleCards, Intervene);                

        void Intervene(Card card)
        {
            // TODO - Make this work.
            Message($"UN intervenes to prevent {card.cardName}");
            command.callback.Invoke();
        }
    }
}
