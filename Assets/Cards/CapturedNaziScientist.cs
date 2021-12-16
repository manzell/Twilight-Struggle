using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class CapturedNaziScientist : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        FindObjectOfType<SpaceTrack>().AdvanceSpaceRace(Game.actingPlayer); 
        command.callback.Invoke();
    }
}
