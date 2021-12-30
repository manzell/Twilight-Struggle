using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle.TurnSystem
{
    public class HeadlinePhase : Phase // There is just one of these
    {
        public Dictionary<Game.Faction, Card> headlines = new Dictionary<Game.Faction, Card>();

        public override void OnPhase(UnityAction callback)
        {
            FindObjectOfType<UI.UIMessage>().Message("Headline Phase");
        }
    }
}