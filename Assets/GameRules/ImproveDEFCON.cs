using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class ImproveDEFCON : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public void OnPhase(TurnSystem.Phase phase, UnityAction callback)
        {
            DEFCONtrack.adjustDefconEvent.Invoke(1);
            callback.Invoke();
        }
    }
}