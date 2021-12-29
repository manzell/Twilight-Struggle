using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class UNIntervention : Card
    {
        public override void CardEvent(GameCommand command)
        {
            List<Card> eligibleCards = new List<Card>();

            foreach (Card card in FindObjectOfType<Game>().playerMap[command.faction].hand)
                if (command.faction == Game.Faction.USA && card.faction == Game.Faction.USSR || command.faction == Game.Faction.USSR && card.faction == Game.Faction.USA)
                    eligibleCards.Add(card);

            cardClickHandler = new UI.CardClickHandler(eligibleCards, Intervene);

            void Intervene(Card card)
            {
                // TODO - Make this work.
                Message($"UN intervenes to prevent {card.cardName}");
                command.FinishCommand();
            }
        }
    }
}