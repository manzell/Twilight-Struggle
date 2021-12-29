using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class NuclearTestBan : Card
    {
        public override void CardEvent(GameCommand command)
        {
            int vpAward = Mathf.Max(0, DEFCONtrack.Status - 2);
            Message($"{command.faction} Events Nuclear Test Ban for {vpAward} VPs");

            VictoryTrack.AdjustVPs(command.faction == Game.Faction.USA ? vpAward : -vpAward);
            Game.AdjustDEFCON.Invoke(Game.actingPlayer, 2);

            command.FinishCommand();
        }
    }
}