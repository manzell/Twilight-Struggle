using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TwilightStruggle
{
    public class SpaceRace : GameAction, IActionPrepare, IActionComplete
    {
        [SerializeField] SpaceTrack spaceTrack; 

        public void Prepare(GameCommand command)
        {
            if (Game.currentPhase is TurnSystem.ActionRound)
                Game.currentActionRound.command = command;

            SpaceVars spaceVars = new SpaceVars();
            spaceVars.roll = Random.Range(0, 6) + 1;

            command.callback = Complete;
            command.parameters = spaceVars;

            prepareEvent.Invoke(command);
        }

        public void Complete(GameCommand command)
        {
            Debug.Log("Space Race Complete");
            // Assume we've already checked if the player has more space attempts remaining && if the card has enough Ops
            // That check should occur in GameAction.CanUseAction
            int spaceRaceLevel = spaceTrack.spaceRaceLevel[command.faction];
            SpaceVars spaceVars = (SpaceVars)command.parameters;

            if (spaceVars.roll <= spaceTrack.spaceRaceTrack[spaceRaceLevel].rollRequired)
            {
                spaceVars.success = true;
                spaceTrack.AdvanceSpaceRace(command.faction);

                int vpAward = spaceTrack.spaceRaceTrack[spaceRaceLevel].vpAwards[spaceTrack.spaceRaceTrack[spaceRaceLevel].acheived.Count];

                VictoryTrack.AdjustVPs(command.faction == Game.Faction.USA ? vpAward : -vpAward);
            }

            completeEvent.Invoke(command);
            command.callback = null; 
            command.FinishCommand(); 
        }

        public class SpaceVars : ICommandParameters
        {
            public int roll;
            public bool success;
        }

        public override bool CanUseAction(Game.Faction actingPlayer, Card card)
        {
            return spaceTrack.attemptsRemaining[actingPlayer] > 0; 
            //return base.CanUseAction(actingPlayer, card);
        }
    }
}