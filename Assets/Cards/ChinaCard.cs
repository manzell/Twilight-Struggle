using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class ChinaCard : Card
    {
        public bool faceUp = true;
        public Game.Faction currentFaction = Game.Faction.USSR;

        public override void CardEvent(GameCommand command)
        {
            faceUp = false;
            currentFaction = currentFaction == Game.Faction.USSR ? Game.Faction.USA : Game.Faction.USSR;

            command.FinishCommand();
        }
    }
}