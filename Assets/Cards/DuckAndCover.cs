using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAndCover : Card
{
    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1);
        int vpAward = Mathf.Clamp(5 - DEFCON.status, 0, 5);
        Game.AwardVictoryPoints.Invoke(vpAward);

        Debug.Log($"US earns {vpAward} {(vpAward == 1 ? "point" : "points")} from Duck and Cover");

        callback.Invoke(); 
    }
}
