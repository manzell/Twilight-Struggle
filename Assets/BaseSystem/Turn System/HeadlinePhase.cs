using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle.TurnSystem
{
    public class HeadlinePhase : Phase
    {
        public Dictionary<Game.Faction, Card> headlines = new Dictionary<Game.Faction, Card>();

        public override void OnPhase(UnityAction callback)
        {
            // override this to do nothing
        }
    }
}