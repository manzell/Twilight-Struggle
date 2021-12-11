using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeGaulleLeadsFrance : Card
{
    [SerializeField] Country france;

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        Game.AdjustInfluence.Invoke(france, Game.Faction.USA, -2);
        Game.AdjustInfluence.Invoke(france, Game.Faction.USSR, 1);

        NATO nato = france.GetComponent<NATO>();

        if (nato) 
            Destroy(nato); 

        callback.Invoke();
    }
}
