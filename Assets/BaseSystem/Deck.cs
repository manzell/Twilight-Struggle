using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class Deck : List<Card>
    {
        public List<Card> discards = new List<Card>();
        public List<Card> held = new List<Card>();
        public List<Card> removed = new List<Card>();

        public UnityEvent<Card> draw = new UnityEvent<Card>(),
            discard = new UnityEvent<Card>(),
            onShuffle = new UnityEvent<Card>();

        public void Discard(Card card)
        {
            discards.Add(card);
            held.Remove(card);
            Remove(card);

            discard.Invoke(card);
        }

        public void Discard(List<Card> cards)
        {
            foreach (Card card in cards)
                Discard(card);
        }

        public Card Draw()
        {
            if (Count == 0 && discards.Count > 0)
                Reshuffle();

            if (Count > 0)
            {
                Card card = this[0];

                draw.Invoke(card);
                Remove(card);
                held.Add(card);

                return card;
            }
            else
                return null;
        }

        public List<Card> Draw(int count)
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                Card card = Draw();

                if (card)
                {
                    cards.Add(card);
                    this.Remove(card);
                }
            }

            return cards;
        }

        public Deck Reshuffle()
        {
            AddRange(discards);
            discards.Clear();

            this.Shuffle();

            return this;
        }
    }
}