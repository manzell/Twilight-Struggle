using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class SetInitialDefcon : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public int defcon = 5; 

        public void OnPhase(TurnSystem.Phase p, UnityAction callback)
        {
            Game.AdjustDEFCON.Invoke(Game.Faction.USA, defcon);
            callback.Invoke();
        }
    }
}