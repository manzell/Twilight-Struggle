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
            Game.SetActingFaction(phasingPlayer);
            base.StartPhase(callback);
        }

        public override void OnPhase(UnityAction callback)
        {
            FindObjectOfType<UI.UIMessage>().Message($"Play {phasingPlayer} Action Round");

            // Sit here and wait. 
        }

        public override void EndPhase(UnityAction callback)
        {
            if(wasEventTriggered == false && ((command.card.faction == Game.Faction.USSR && phasingPlayer == Game.Faction.USA) ||
                    (command.card.faction == Game.Faction.USA && phasingPlayer == Game.Faction.USSR)))
            {
                GameCommand newCommand = GameCommand.Create(command.opponent, command.card, FindObjectOfType<CardEvent>());
                FindObjectOfType<CardEvent>().Prepare(newCommand); 
                
            }

            Game.currentActionRound = null;
            base.EndPhase(callback);
        }
    }
}