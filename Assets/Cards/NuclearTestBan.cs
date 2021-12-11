using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearTestBan : Card
{
    Game.Faction actingPlayer; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    { 

        int vpAward = Mathf.Max(0, DEFCON.status - 2);
        Debug.Log($"Eventing Nuclear Test Ban for {vpAward} VPs");

        Game.AwardVictoryPoints.Invoke(actingPlayer == Game.Faction.USA ? vpAward : -vpAward);
        Game.AdjustDEFCON.Invoke(actingPlayer, 2);

        callback.Invoke(); 
    }
}
