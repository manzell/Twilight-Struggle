using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class VietnamRevolts : Card
    {
        [SerializeField] Country vietnam;
        [SerializeField] List<Country> southeastAsia = new List<Country>();

        public override void CardEvent(GameCommand command)
        {
            Message("Hi Chi Minh expels French");
            Game.AdjustInfluence(vietnam, Game.Faction.USSR, 2);

            // TODO Make this work
            command.FinishCommand();
        }
    }
}