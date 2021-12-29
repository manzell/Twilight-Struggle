using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Fidel : Card
    {
        [SerializeField] Country cuba;

        public override void CardEvent(GameCommand command)
        {
            Game.SetInfluence(cuba, Game.Faction.USA, 0);
            Game.SetInfluence(cuba, Game.Faction.USSR, Mathf.Max(cuba.influence[Game.Faction.USSR], cuba.stability));

            Message("Fidel overthrows Batista!");
            command.FinishCommand();
        }
    }
}