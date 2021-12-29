using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class DuckAndCover : Card
    {
        public override void CardEvent(GameCommand command)
        {
            DEFCONtrack.AdjustDefcon(Game.phasingPlayer, -1);
            int vpAward = Mathf.Clamp(5 - DEFCONtrack.status, 0, 5);
            VictoryTrack.AdjustVPs(vpAward);

            Message($"US earns {vpAward} {(vpAward == 1 ? "point" : "points")} from Duck and Cover");

            command.FinishCommand();
        }
    }
}