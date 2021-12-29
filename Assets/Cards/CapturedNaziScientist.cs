using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class CapturedNaziScientist : Card
    {
        public override void CardEvent(GameCommand command)
        {
            FindObjectOfType<SpaceTrack>().AdvanceSpaceRace(Game.actingPlayer);
            command.FinishCommand(); 
        }
    }
}