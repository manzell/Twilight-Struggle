using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasser : Card
{
    [SerializeField] Country egypt; 

    public override void CardEvent(GameAction.Command command)
    {
        Game.AdjustInfluence.Invoke(egypt, Game.Faction.USSR, 2);
        Game.AdjustInfluence.Invoke(egypt, Game.Faction.USA, -(int)Mathf.Ceil(egypt.influence[Game.Faction.USA] / 2));

        command.callback.Invoke(); 
    }
}
