using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fidel : Card
{
    [SerializeField] Country cuba; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        Game.SetInfluence.Invoke(cuba, Game.Faction.USA, 0);
        Game.SetInfluence.Invoke(cuba, Game.Faction.USSR, Mathf.Max(cuba.influence[Game.Faction.USSR], cuba.stability));

        callback.Invoke(); 
    }
}
