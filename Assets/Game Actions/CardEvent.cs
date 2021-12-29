using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class CardEvent : GameAction, IActionPrepare, IActionComplete
    {
        public void Prepare(GameCommand command)
        {
            // TODO: Implement a Confirm Button. 

            if(Game.currentActionRound != null)
                Game.currentActionRound.wasEventTriggered = true;
 
            if( (command.card.faction == Game.Faction.USA || command.card.faction == Game.Faction.USSR) && command.card.faction != command.faction)
            {
                Debug.Log("TODO: Divert the callback from it's current callback to Complete(command) and re-present the card"); 
            }
            else
                command.card.CardEvent(command); 
        }

        public void Complete(GameCommand command)
        {
            command.FinishCommand();
        }

        public override bool CanUseAction(Game.Faction actingPlayer, Card card)
        {
            if(card.TryGetComponent(out CanUseEvent can)) // Some cards may become uneventable, but I don't think there's any condition whereby an event is Unplayable besides a card rule
                return can == null; 

            return true; 
        }
    }
}