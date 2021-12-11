using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Headline : Phase
{
    public Dictionary<Game.Faction, Card> headlineCards = new Dictionary<Game.Faction, Card>();

    public override void StartPhase(UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message("Headline Phase");
        Game.HeadlineStart.Invoke(this); 
        base.StartPhase(callback);
    }

    public override void EndPhase(UnityAction callback)
    {
        Game.HeadlineEnd.Invoke(this);
        base.EndPhase(callback);
    }

    [Button] public void SubmitHeadline(Game.Faction faction, Card card)
    {
        if (card.GetComponent<MayNotHeadline>() != null) return;

        if (headlineCards.ContainsKey(faction)) headlineCards[faction] = card;
        else headlineCards.Add(faction, card);

        if (headlineCards.Count == 2 && Game.currentPhase == this)
        {
            // Thise code calls the Events on the cards directly in order. But if the US headlines Defectors, we skip this phase
            // The issue is that we don't want to put that rule here, we want to keep it contained within the defector script.

            // What event to call: OnHeadline on each card? BeforeHeadline on the Game Object

            FindObjectOfType<UIMessage>().Message($"USA Headlines {headlineCards[Game.Faction.USA].cardName} (USA). USSR headlines {headlineCards[Game.Faction.USSR].cardName}"); 

            headlineCards[Game.Faction.USA].Headline.Invoke(headlineCards);
            headlineCards[Game.Faction.USSR].Headline.Invoke(headlineCards);

            if (headlineCards[Game.Faction.USSR].opsValue > headlineCards[Game.Faction.USSR].opsValue)
                headlineCards[Game.Faction.USSR].Event(() => headlineCards[Game.Faction.USA].Event(() => NextPhase(phaseCallback)));
            else
                headlineCards[Game.Faction.USA].Event(() => headlineCards[Game.Faction.USSR].Event(() => NextPhase(phaseCallback)));
        }
    }
}
