using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedNaziScientist : Card
{
    Game.Faction actingPlayer; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        FindObjectOfType<SpaceTrack>().AdvanceSpaceRace(actingPlayer); 
        callback.Invoke();
    }
}
