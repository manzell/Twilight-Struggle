using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class RomanianAbdication : Card
    {
        [SerializeField] Country romania;

        public override void CardEvent(GameCommand command)
        {
            Message("King Michael I of Romania abdicates!");

            Game.SetInfluence(romania, Game.Faction.USA, 0);
            Game.SetInfluence(romania, Game.Faction.USSR, Mathf.Max(romania.stability, romania.influence[Game.Faction.USSR]));

            command.FinishCommand();
        }
    }
}