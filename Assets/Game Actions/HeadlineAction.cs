using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class HeadlineAction : GameAction, IActionPrepare, IActionTarget, IActionComplete
    {
        GameCommand _command;

        public new void Awake()
        {
            foreach(UI.UIDropHandler handler in GetComponentsInChildren<UI.UIDropHandler>())
            {
                handler.cardDropEvent.AddListener(HeadlineDrop);
            }
        }

        public void HeadlineDrop(Card card)
        {
            if (card.GetComponent<MayNotHeadline>() != null) return; 

            if (_command == null)
                _command = GameCommand.Create(Game.actingPlayer, card, this);
            else
                _command.card = card;

            _command.Prepare(); 
        }

        public void Prepare(GameCommand command) // Get's called each time a card is deposited
        {
            Debug.Log(command == _command); 

            HeadlineVars headlineVars;

            if(command.parameters == null)
                headlineVars = new HeadlineVars();
            else
                headlineVars = (HeadlineVars)command.parameters;

            if(headlineVars.headlines.ContainsKey(Game.actingPlayer))
            {
                // For now do nothing, TODO, swap the card back for your previous card
            }
            else
            {
                // TODO: Check if we can Headline This card
                FindObjectOfType<Game>().playerMap[Game.actingPlayer].hand.Remove(command.card); // TODO: Make this cleaner
                headlineVars.headlines.Add(Game.actingPlayer, command.card);

                prepareEvent.Invoke(command);
            }

            command.parameters = headlineVars;

            if (headlineVars.headlines.Count == 2)
                Target(command); 
        }

        public void Target(GameCommand command) // Gets called after both cards are placed
        {
            _command = null; 
            HeadlineVars headlineVars = (HeadlineVars)command.parameters; 

            Game.Faction initiative = headlineVars.headlines[Game.Faction.USSR].opsValue > headlineVars.headlines[Game.Faction.USA].opsValue ? Game.Faction.USSR : Game.Faction.USA;
            Game.Faction secondary = initiative == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA; 

            command.callback = SecondHeadline;
            Game.SetActiveFaction(initiative); 
            headlineVars.headlines[initiative].CardEvent(command); 

            void SecondHeadline(GameCommand command)
            {
                command.callback = Complete;
                Game.SetActiveFaction(secondary);
                headlineVars.headlines[secondary].CardEvent(command);
            }
        }

        public void Complete(GameCommand command)
        {
            command.FinishCommand(); 
        }

        public class HeadlineVars : ICommandVariables
        {
            public string marker; 
            public Dictionary<Game.Faction, Card> headlines = new Dictionary<Game.Faction, Card>();
        }
    }

    interface IHeadlineEvent
    {
        public void HeadlineEvent(TurnSystem.HeadlinePhase headline);
    }
}