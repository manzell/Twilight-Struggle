using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class HeadlineAction : GameAction, IActionPrepare, IActionComplete
    {
        public void Prepare(GameCommand command) // Get's called each time a card is deposited
        {
            HeadlineVars headlineVars = (HeadlineVars)command.parameters;

            if(headlineVars.headlines[command.faction])
            {
                // For now do nothing, but in the future, swap the card back for your previous card
            }
            else
            {
                FindObjectOfType<Game>().playerMap[command.faction].hand.Remove(command.card); // TODO: Make this cleaner
                headlineVars.headlines[command.faction] = command.card; 
                prepareEvent.Invoke(command);
            }

            if (headlineVars.headlines.Count == 2)
                Complete(command); 
        }

        public void Complete(GameCommand command) // Gets called when both cards are placed
        {
            HeadlineVars headlineVars = (HeadlineVars)command.parameters; 
            Game.Faction initiative = headlineVars.headlines[Game.Faction.USSR].opsValue > headlineVars.headlines[Game.Faction.USA].opsValue ? Game.Faction.USSR : Game.Faction.USA;
            Game.Faction secondary = initiative == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; 

            command.callback = SecondHeadline;
            Game.SetActiveFaction(initiative); 
            headlineVars.headlines[initiative].CardEvent(command); 

            void SecondHeadline(GameCommand command)
            {
                command.callback = null;
                Game.SetActiveFaction(secondary);
                headlineVars.headlines[secondary].CardEvent(command);
            }
        }

        public class HeadlineVars : ICommandVariables
        {
            public Dictionary<Game.Faction, Card> headlines; 
        }
    }

    interface IHeadlineEvent
    {
        public void HeadlineEvent(TurnSystem.HeadlinePhase headline);
    }
}