using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class Turn : Phase
{
    public UnityEvent<Turn> turnStartEvent = new UnityEvent<Turn>(), 
        turnEndEvent = new UnityEvent<Turn>();

    public HeadlinePhase headline; 
    public List<ActionRound> actionRounds = new List<ActionRound>();

    private void Awake()
    {
        headline = GetComponentInChildren<HeadlinePhase>();
        actionRounds.AddRange(GetComponentsInChildren<ActionRound>()); 
    }

    public override void StartPhase(UnityAction callback)
    {
        Game.currentTurn = this;
        turnStartEvent.Invoke(this); 
        Game.turnStartEvent.Invoke(this); 
        base.StartPhase(callback);
    }

    public override void EndPhase(UnityAction callback)
    {
        Game.currentTurn = null;
        turnEndEvent.Invoke(this);
        Game.turnStartEvent.Invoke(this); 
        base.EndPhase(callback);
    }
}
