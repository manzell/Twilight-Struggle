using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class ImproveDEFCON : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public void Action(TurnSystem.Phase phase, UnityAction callback)
        {
            DEFCONtrack.adjustDefconEvent.Invoke(1);
            callback.Invoke();
        }
    }
}