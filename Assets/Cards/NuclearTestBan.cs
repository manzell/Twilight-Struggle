using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearTestBan : Card
{
    public override void CardEvent(GameAction.Command command)
    { 
        int vpAward = Mathf.Max(0, DEFCON.Status - 2);
        Message($"{command.phasingPlayer} Events Nuclear Test Ban for {vpAward} VPs");

        Game.AdjustVPs.Invoke(command.phasingPlayer == Game.Faction.USA ? vpAward : -vpAward);
        Game.AdjustDEFCON.Invoke(Game.actingPlayer, 2);

        command.callback.Invoke(); 
    }
}
