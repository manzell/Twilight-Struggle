using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAndCover : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        int vpAward = Mathf.Clamp(5 - DEFCON.Status, 0, 5);

        Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1);
        Game.AdjustVPs.Invoke(vpAward);

        FindObjectOfType<UIMessage>().Message($"US earns {vpAward} {(vpAward == 1 ? "point" : "points")} from Duck and Cover");

        command.callback.Invoke(); 
    }
}
