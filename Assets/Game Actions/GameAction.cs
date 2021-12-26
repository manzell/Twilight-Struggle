using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public abstract class GameAction : SerializedMonoBehaviour
    {
        public string actionName;
        [HideInInspector] public CountryClickHandler countryClickHandler;

        public void SetCard(Card card)
        {
            Command command = GetCommand(card, this);

            Player player = FindObjectOfType<Game>().playerMap[Game.phasingPlayer];
            player.hand.Remove(card); // TODO, make sure only the Acting Player can play a card form their own hand. This is a UI check

            // Before we Execute, we want to send an event to the Game and to the Card as well as our new command as handy hooks. 
            Game.cardCommandEvent.Invoke(command);
            card.cardCommandEvent.Invoke(command); // move this code to where it belongs... into the Command constructor?

            if (this is Coup && TryGetComponent(out CoupAnimation animation)) // TODO - Make the animation the responsibility of the instance. GameAction should never know about Coup 
                // at least generalize it somehow
                animation.PrepareCoup(command as Coup.CoupCommand); // wait for the UI to to confirm/submit the command. 
            else
                command.executeCommand.Invoke(command); // This makes our Action Go - we want to NOT call this and instead make it
                                                        // This architecture is backwards. We should be calling Execute() on our GameAction and passing in the command. 
        }

        public abstract Command GetCommand(Card card, GameAction action); /* OK here is the issue
            *  We're using the strategy pattern to bind together a reference to a static implementation, the card submitted to it, and 
            *  the necessary parameters into a Command object. The command objects contain different data depending on the GameAction, 
            *  so they can't be the same type. Must research further. 
            */
        public abstract void ExecuteCommandAction(Command command);

        public abstract class Command // Eeef. Reconsider this architecture. 
        {
            public Game.Faction phasingPlayer;
            public Game.Faction actingPlayer;
            public Game.Faction enemyPlayer => actingPlayer == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
            public GameAction action;
            public Card card;
            public int cardOpsValue;

            public UnityAction callback;
            public GameEvent<Command> executeCommand = new GameEvent<Command>();

            public Command(Card card, GameAction action)
            {
                this.card = card;
                this.action = action;
                phasingPlayer = Game.phasingPlayer;
                actingPlayer = Game.phasingPlayer;
                callback = () => Game.currentPhase.NextPhase(Game.currentPhase.callback);
                cardOpsValue = card.OpsValue;

                if (Game.currentActionRound)
                    Game.currentActionRound.command = this; // This fails during the headline phase. TODO:: Fix. 
                executeCommand.AddListener(c => Game.cardCommandEvent.Invoke(c)); // Note that this means that Game.cardCommandEvent.before and after aren't really reliable. 
                executeCommand.AddListener(c => card.cardCommandEvent.Invoke(c));
                executeCommand.AddListener(action.ExecuteCommandAction);

            }
        }

        public bool CanUseAction(Game.Faction faction, Card card)
        {
            return true;
        }

        /* ===========================\
         * |                          |
         * |         REFACTOR         |
         * |                          |
         * \==========================/
         * 
         * 
         * Game Action Might Have Certain Behaviors
         * 
         * a CardPlay Behavior (Attempt Space Race, Trigger Card Event, or Prepare Influence, Prepare Coup, Prepare Realign)
         * a Target Country Behavior (Coup, Place Ops, Attempt Realign) 
         * 
         * The question remains where do attach data? 
         * 
         * In any case, 
         * 
         */

        public UnityEvent prepareGameAction = new UnityEvent(),
            targetGameAction = new UnityEvent();

        public void SetCardToo(Card card) // Start here. We get a card from the DropHandler. 
        {
            GameCommand command = new GameCommand(Game.actingPlayer, card, this); // We create our encapsulated command Event 

            Prepare(command); 
        }

        public class GameCommand
        {
            // Constructor Vars
            public Game.Faction faction; 
            public Card card;
            public GameAction gameAction; 
            public Phase commandPhase;

            // Output Vars
            public Dictionary<Country, Dictionary<Game.Faction, int>> influenceChange = new Dictionary<Country, Dictionary<Game.Faction, int>>(); 
            public Dictionary<Game.Faction, int> vpChange = new Dictionary<Game.Faction, int>();
            public Dictionary<Game.Faction, int> milOpsChange = new Dictionary<Game.Faction, int>();
            public Dictionary<Game.Faction, int> spaceRaceChange = new Dictionary<Game.Faction, int>(); // TODO: Space Race Commands? 
            public int defconChange = 0; 

            // Helper vars
            public ICommandVariables parameters; // these are helper variables: Space Race target roll, realignment rolls, coup roll, bonus ops etc

            public GameCommand(Game.Faction faction, Card card, GameAction gameAction)
            {
                this.faction = faction;
                this.card = card; 
                commandPhase = Game.currentPhase;
                // gameAction.Prepare(this); 
            }
        }


        public virtual void Undo(GameCommand command) { }
        public virtual void Prepare(GameCommand command) { }
        public virtual void Target(GameCommand command) { }
        public interface ICommandVariables { }
        public interface IActionPrepare { public void Prepare(GameCommand command); }
        public interface IActionTarget { public void Target(GameCommand command); }
    }
}