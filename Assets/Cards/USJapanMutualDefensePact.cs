using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USJapanMutualDefensePact : Card
{
    [SerializeField] Country japan;

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        if(japan.control != Game.Faction.USA) // This SHOULD prevent any Adjust-by-Zeros
            Game.SetInfluence.Invoke(japan, Game.Faction.USA, Mathf.Max(japan.influence[Game.Faction.USA], japan.stability + japan.influence[Game.Faction.USSR]));

        japan.gameObject.AddComponent<USJapanMutualDefensePact>(); 

        callback.Invoke();
    }
}
