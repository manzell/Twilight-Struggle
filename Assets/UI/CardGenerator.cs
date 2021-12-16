using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    Dictionary<Card, GameObject> cardsDisplayed = new Dictionary<Card, GameObject>();

    public void Setup(List<Card> cards)
    {
        // Firstly, we look through our list of instantiated prefabs and remove any that aren't still in our hand. 
        foreach (Card card in cardsDisplayed.Keys)
        {
            if (!cards.Contains(card))
            {
                cardsDisplayed.Remove(card);
                Destroy(card.gameObject);
            }
        }

        // Then we go through our list and instantiate any of the cards that are not present
        foreach (Card card in cards)
        {
            if (!cardsDisplayed.ContainsKey(card))
            {
                GameObject cardObject = Instantiate(cardPrefab, transform);
                cardObject.GetComponent<UICardDisplay>().Setup(card);
                cardsDisplayed.Add(card, cardObject);
            }
        }
    }
}