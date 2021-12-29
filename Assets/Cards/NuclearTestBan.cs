using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class NuclearTestBan : Card
    {
        public override void CardEvent(GameCommand command)
        {
            int vpAward = Mathf.Max(0, DEFCONtrack.status - 2);
            Message($"{command.faction} Events Nuclear Test Ban for {vpAward} VPs");

            VictoryTrack.AdjustVPs(command.faction == Game.Faction.USA ? vpAward : -vpAward);
            DEFCONtrack.AdjustDefcon(Game.actingPlayer, 2);

            command.FinishCommand();
        }
    }
}