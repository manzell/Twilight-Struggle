using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Blockade : Card
    {
        [SerializeField] Country westGermany;

        public override void CardEvent(GameCommand command)
        {
            List<Card> eligibleCards = new List<Card>();

            foreach (Card card in FindObjectOfType<Game>().playerMap[Game.Faction.USA].hand)
                if (card.opsValue >= 3)
                    eligibleCards.Add(card);

            if (eligibleCards.Count > 0)
            {
                cardClickHandler = new UI.CardClickHandler(eligibleCards, onCardClick);
            }
            else
            {
                Game.SetInfluence(westGermany, Game.Faction.USA, 0);
                command.FinishCommand();
            }

            void onCardClick(Card card) // TODO: We can't pass null card 
            {
                FindObjectOfType<UI.UIMessage>().Message($"US discarded {card.cardName} to Blockade");
                Player.USA.hand.Remove(card);
                Game.deck.discards.Add(card);
                command.FinishCommand();
            }

            void onPass()
            {
                FindObjectOfType<UI.UIMessage>().Message($"USSR Blockade eliminates US influence in West Germany!");
                Game.SetInfluence(westGermany, Game.Faction.USA, 0);
                command.FinishCommand();


            }
        }
    }
}