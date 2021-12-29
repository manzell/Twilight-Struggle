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
                FindObjectOfType<UI.UIManager>().SetButton(FindObjectOfType<UI.UIManager>().primaryButton, "Don't Discard", onPass);
            }
            else
            {
                Game.SetInfluence(westGermany, Game.Faction.USA, 0);
                command.FinishCommand();
            }

            void onCardClick(Card card) // TODO: We can't pass null card 
            {
                Player.USA.hand.Remove(card);
                Game.deck.discards.Add(card);
                command.FinishCommand();

                FindObjectOfType<UI.UIMessage>().Message($"US discarded {card.cardName} to Blockade");
            }

            void onPass()
            {
                Game.SetInfluence(westGermany, Game.Faction.USA, 0);
                command.FinishCommand();

                FindObjectOfType<UI.UIManager>().UnsetButton(FindObjectOfType<UI.UIManager>().primaryButton);


                FindObjectOfType<UI.UIMessage>().Message($"USSR Blockade eliminates US influence in West Germany!");
            }
        }
    }
}