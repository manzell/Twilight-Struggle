using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAndCover : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1);
        int vpAward = Mathf.Clamp(5 - DEFCONtrack.Status, 0, 5);
        Game.AdjustVPs.Invoke(vpAward);

        Message($"US earns {vpAward} {(vpAward == 1 ? "point" : "points")} from Duck and Cover");

        command.callback.Invoke(); 
    }
}
