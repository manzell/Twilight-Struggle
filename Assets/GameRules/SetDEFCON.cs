using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetDEFCON : MonoBehaviour, IPhaseAction
{
    public void OnPhase(Phase p, UnityAction callback)
    {
        Game.AdjustDEFCON.Invoke(Game.Faction.USA, 5);
        callback.Invoke();
    }
}
