using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasser : Card
{
    [SerializeField] Country egypt; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        Game.AdjustInfluence.Invoke(egypt, Game.Faction.USSR, 2);
        Game.AdjustInfluence.Invoke(egypt, Game.Faction.USA, -(int)(egypt.influence[Game.Faction.USA] / 2));

        callback.Invoke(); 
    }
}
