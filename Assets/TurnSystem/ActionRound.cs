using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events; 

public class ActionRound : Phase
{
    public Game.Faction phasingPlayer;
    public Card card;
    [ReadOnly] public Action action; 
    public IGameAction gameAction;
    public bool opponentEventTriggered = false;

    public override void StartPhase(UnityAction callback)
    {
        Game.phasingPlayer = phasingPlayer;
        Game.currentActionRound = this;
        base.StartPhase(callback);
    }

    public override void NextPhase(UnityAction callback)
    {
        // Here we wait for the Action to come back with call to EndPhase
        foreach(Action action in FindObjectsOfType<Action>())
            action.SetActionRound(this);
    }

    public override void EndPhase(UnityAction callback)
    {
        Game.currentActionRound = null; 
        base.EndPhase(callback);
    }
}