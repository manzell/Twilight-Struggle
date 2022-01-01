using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class HeadlineAction : GameAction, IActionPrepare, IActionTarget, IActionComplete
    {
        GameCommand _command;
        public UnityEvent<GameCommand> headlineEvent, secondHeadlineEvent;

        public new void Awake()
        {
            foreach(UI.UIDropHandler handler in GetComponentsInChildren<UI.UIDropHandler>())
                handler.cardDropEvent.AddListener(HeadlineDrop);
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
            HeadlineVars headlineVars;

            if(command.parameters == null)
                headlineVars = new HeadlineVars();
            else
                headlineVars = (HeadlineVars)command.parameters;

            if(headlineVars.headlines.ContainsKey(Game.actingPlayer))
            {
                // TODO, swap the card back for your previous card
            }
            else
            {
                // TODO: Check if we can Headline This card
                FindObjectOfType<Game>().playerMap[Game.actingPlayer].hand.Remove(command.card); // TODO: Make this cleaner
                headlineVars.headlines.Add(Game.actingPlayer, command.card);

                command.parameters = headlineVars;
                prepareEvent.Invoke(command);
            }

            if (headlineVars.headlines.Count == 2)
                Target(command);
            else
                Game.SetActingFaction(command.opponent); 
        }

        public void Target(GameCommand command) // Gets called after both cards are placed
        {
            HeadlineVars headlineVars = (HeadlineVars)command.parameters;
            List<Game.Faction> headlineOrder = new List<Game.Faction>();

            _command = null;
            headlineOrder.Add(headlineVars.initiative);
            headlineOrder.Add(headlineVars.secondary);

            FirstHeadline(command); // leftover from the chained callbacks. TODO: Make this better. 
            void FirstHeadline(GameCommand command)
            {
                command.callback = SecondHeadline;
                command.faction = headlineOrder[0];
                Headline(command); 
            }

            void SecondHeadline(GameCommand command)
            {
                command.callback = Complete;
                command.faction = headlineOrder[1];
                Headline(command);
            }

            void Headline(GameCommand command)
            {
                command.card = headlineVars.headlines[command.faction];
                Game.SetPhasingFaction(command.faction);

                Debug.Log($"{command.faction} Headlining {command.card.cardName}");
                headlineEvent.Invoke(command); // Hook for Animations

                StartCoroutine(EventAfterDelay(3f)); 
                IEnumerator EventAfterDelay(float f)
                {
                    yield return new WaitForSeconds(f);
                    command.card.CardEvent(command);
                }
            }
        }

        public void Complete(GameCommand command)
        {
            completeEvent.Invoke(command);
            command.callback = null; 
            command.FinishCommand(); 
        }

        public class HeadlineVars : ICommandParameters
        {
            public string marker; 
            public Dictionary<Game.Faction, Card> headlines = new Dictionary<Game.Faction, Card>();
            
            public Game.Faction initiative => headlines[Game.Faction.USSR].opsValue > headlines[Game.Faction.USA].opsValue ? Game.Faction.USSR : Game.Faction.USA;
            public Game.Faction secondary => initiative == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
        }
    }

    interface IHeadlineEvent
    {
        public void HeadlineEvent(TurnSystem.HeadlinePhase headline);
    }
}

