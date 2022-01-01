using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class SuezCrisis : Card
    {
        [SerializeField] List<Country> countries;

        public override void CardEvent(GameCommand command)
        {
            RemoveInfluence(countries, Game.Faction.USA, 4, 2, command.FinishCommand);
        }
    }
}