using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blockade : Card
{
    [SerializeField] Country westGermany;

    public override void CardEvent(GameAction.Command command)
    {
        List<Card> eligibleCards = new List<Card>();

        foreach(Card card in FindObjectOfType<Game>().playerMap[Game.Faction.USA].hand)
            if(card.opsValue >= 3) 
                eligibleCards.Add(card);

        if(eligibleCards.Count > 0)
            cardClickHandler = new CardClickHandler(eligibleCards, onCardClick);
        else
        {
            Game.SetInfluence.Invoke(westGermany, Game.Faction.USA, 0); 
            command.callback.Invoke();
        }

        void onCardClick(Card card)
        {
            if (card && card.opsValue >= 3)
            {
                Player.USA.hand.Remove(card);
                Game.deck.discards.Add(card);
            }
            else
                Game.SetInfluence.Invoke(westGermany, Game.Faction.USA, 0);

            countryClickHandler.Close(); 
            command.callback.Invoke();
        }
    }
}
