using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// All this does is Manage Highlights and Turn a Listener on or off. Should it exist? 
public class CardClickHandler
{
    Dictionary<Card, Outline> outlines = new Dictionary<Card, Outline>();
    UnityAction<Card> callback;
    Color color = Color.yellow;

    // This is specific for coming from a Card Place Influence Action
    public CardClickHandler(List<Card> cards, UnityAction<Card> callback, Color color)
    {
        this.color = color;

        // We need to choose between showing highlighted cards in the players hand area or a new hand area. For now, let's always create the new windows

        foreach (Card card in cards)
        {
            card.clickEvent.AddListener(this.callback = callback);
            AddHighlight(card);
        }
    }

    public CardClickHandler(List<Card> cards, UnityAction<Card> callback)
    {
        foreach (Card card in cards)
        {
            card.clickEvent.AddListener(this.callback = callback);
            AddHighlight(card);
        }
    }

    public void Refresh(List<Card> cards)
    {
        foreach (Card card in cards)
            if (!outlines.ContainsKey(card))
                AddHighlight(card);

        foreach (Card card in outlines.Keys)
            if (!cards.Contains(card))
                RemoveHighlight(card);
    }

    public void Close()
    {
        foreach (Card card in outlines.Keys)
        {
            card.clickEvent.RemoveListener(callback); 
            RemoveHighlight(card);
        }
    }

    public void AddHighlight(Card card)
    {
        if (!outlines.ContainsKey(card))
        {
            /*
            Outline outline = card.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(5f, 5f);
            outlines.Add(card, outline);
            */
        }
    }

    public void RemoveHighlight(Card card)
    {
        if (outlines.ContainsKey(card))
            GameObject.Destroy(outlines[card]);
    }
}