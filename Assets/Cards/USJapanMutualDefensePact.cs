using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class USJapanMutualDefensePact : Card
    {
        [SerializeField] Country japan;

        public override void CardEvent(GameCommand command)
        {
            Message("US/Japan Mutual Defense Pact signed!");
            Game.setInfluenceEvent.Invoke(japan, Game.Faction.USA, Mathf.Max(japan.influence[Game.Faction.USA], japan.stability + japan.influence[Game.Faction.USSR]));

            japan.gameObject.AddComponent<MayNotCoup>().faction = Game.Faction.USSR;
            japan.gameObject.AddComponent<MayNotRealign>().faction = Game.Faction.USSR;

            command.FinishCommand();
        }
    }
}