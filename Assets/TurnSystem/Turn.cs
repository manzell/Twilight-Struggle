using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class Turn : Phase
{
    public HeadlinePhase headlinePhase; 
    public List<ActionRound> actionRounds = new List<ActionRound>();

    private void Awake()
    {
        headlinePhase = GetComponentInChildren<HeadlinePhase>();
        actionRounds.AddRange(GetComponentsInChildren<ActionRound>()); 
    }

    public override void StartPhase(UnityAction callback)
    {
        Game.currentTurn = this;
        base.StartPhase(callback);
    }

    public override void EndPhase(UnityAction callback)
    {
        Game.currentTurn = null;
        base.EndPhase(callback);
    }
}
