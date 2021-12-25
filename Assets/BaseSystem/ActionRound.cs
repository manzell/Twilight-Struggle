using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events; 

public class ActionRound : Phase
{
    public Game.Faction phasingPlayer;
    public Card card;
    [ReadOnly] public GameAction.Command command; 

    public bool opponentEventTriggered = false;

    public override void StartPhase(UnityAction callback)
    {
        Game.phasingPlayer = phasingPlayer;
        Game.currentPhase = this; 
        Game.currentActionRound = this;
        Game.setActiveFactionEvent.Invoke(phasingPlayer);
        base.StartPhase(callback);
    }

    public override void OnPhase(UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message($"{phasingPlayer} Action Round...");

        // Sit here and wait. 
    }

    public override void EndPhase(UnityAction callback)
    {
        Game.currentActionRound = null; 
        base.EndPhase(callback);
    }
}