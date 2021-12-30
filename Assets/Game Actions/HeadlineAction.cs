using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class HeadlineAction : GameAction, IActionPrepare, IActionTarget, IActionComplete
    {
        GameCommand _command;
        public UnityEvent<GameCommand> firstHeadline, secondHeadline; 

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
                // For now do nothing, TODO, swap the card back for your previous card
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
                Game.SetActiveFaction(command.opponent); 
        }

        public void Target(GameCommand command) // Gets called after both cards are placed
        {
            _command = null; 
            HeadlineVars headlineVars = (HeadlineVars)command.parameters; 

            Game.SetActiveFaction(headlineVars.initiative);
            command.card = headlineVars.headlines[headlineVars.initiative]; 
            targetEvent.Invoke(command);

            List<Game.Faction> headlineOrder = new List<Game.Faction>();

            headlineOrder.Add(headlineVars.initiative);
            headlineOrder.Add(headlineVars.secondary);

            command.callback = FirstHeadline;

            FirstHeadline(command); 

            void FirstHeadline(GameCommand command)
            {
                firstHeadline.Invoke(command); 
                command.callback = SecondHeadline;
                Game.SetPhasingFaction(headlineOrder[0]);
                command.card = headlineVars.headlines[headlineOrder[0]];

                Debug.Log($"{Game.phasingPlayer} Headlining {command.card.cardName}");
                command.card.CardEvent(command);
            }

            void SecondHeadline(GameCommand command)
            {
                StartCoroutine(SecondHeadlineDelay(3f)); 
                IEnumerator SecondHeadlineDelay(float f)
                {
                    yield return new WaitForSeconds(f);
                    secondHeadline.Invoke(command);
                    command.callback = Complete;
                    Game.SetPhasingFaction(headlineOrder[1]);
                    command.card = headlineVars.headlines[headlineOrder[1]];

                    Debug.Log($"{Game.phasingPlayer} Headlining {command.card.cardName}");
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

        public class HeadlineVars : ICommandVariables
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