using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardEvent : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new EventCommand(card, action);

    public override void ExecuteCommandAction(Command command) => command.card.CardEvent(command); 

    public class EventCommand : Command
    {
        public EventCommand(Card c, GameAction a) : base(c, a) { }
    }
}
