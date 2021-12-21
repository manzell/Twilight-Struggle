using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro; 

public class Blockade : Card
{
    [SerializeField] Country westGermany;

    public override void CardEvent(GameAction.Command command)
    {
        List<Card> eligibleCards = new List<Card>();

        foreach(Card card in FindObjectOfType<Game>().playerMap[Game.Faction.USA].hand)
            if(card.OpsValue >= 3) 
                eligibleCards.Add(card);

        if(eligibleCards.Count > 0)
        {
            cardClickHandler = new CardClickHandler(eligibleCards, onCardClick);
            FindObjectOfType<UIManager>().SetButton(FindObjectOfType<UIManager>().primaryButton, "Don't Discard", onPass);
        }
        else
        {
            Game.SetInfluence.Invoke(westGermany, Game.Faction.USA, 0); 
            command.callback.Invoke();
        }

        void onCardClick(Card card) // TODO: We can't pass null card 
        {
            Player.USA.hand.Remove(card);
            Game.deck.discards.Add(card);
            countryClickHandler.Close(); 
            command.callback.Invoke();

            FindObjectOfType<UIMessage>().Message($"US discarded {card.cardName} to Blockade"); 
        }

        void onPass()
        {
            Game.SetInfluence.Invoke(westGermany, Game.Faction.USA, 0);
            command.callback.Invoke();

            FindObjectOfType<UIManager>().UnsetButton(FindObjectOfType<UIManager>().primaryButton);


            FindObjectOfType<UIMessage>().Message($"USSR Blockade eliminates US influence in West Germany!");
        }
    }
}
