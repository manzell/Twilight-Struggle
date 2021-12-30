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
                //"TODO: Divert the callback from it's current callback to Complete(command) and re-present the card"); 
            }
            else
                command.card.CardEvent(command); 
        }

        public void Complete(GameCommand command) => command.FinishCommand();

        public override bool CanUseAction(Game.Faction actingPlayer, Card card) => !card.GetComponent<CanUseEvent>(); 
    }
}