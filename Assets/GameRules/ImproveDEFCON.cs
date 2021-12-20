using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class ImproveDEFCON : MonoBehaviour, IPhaseAction
{
    public void OnPhase(Phase phase, UnityAction callback)
    {
        DEFCON.AdjustDEFCON.Invoke(1);
        callback.Invoke();
    }
}
