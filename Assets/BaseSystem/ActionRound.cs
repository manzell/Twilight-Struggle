using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;


namespace TwilightStruggle.TurnSystem
{
    public class ActionRound : Phase
    {
        public Game.Faction phasingPlayer;
        public GameCommand command;
        public bool wasEventTriggered = false;

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
            FindObjectOfType<UI.UIMessage>().Message($"Play {phasingPlayer} Action Round");

            // Sit here and wait. 
        }

        public override void EndPhase(UnityAction callback)
        {
            Game.currentActionRound = null;
            base.EndPhase(callback);
        }
    }
}