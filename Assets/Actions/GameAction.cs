using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public abstract class GameAction : SerializedMonoBehaviour
{
    public string actionName; 
    [ReadOnly] public CountryClickHandler countryClickHandler;

    public void SetCard(Card card)
    {
        Command command = GetCommand(card, this);

        command.executeCommand.after.AddListener(c => Debug.Log("command.executeCommand.after Invoked()"));

        // Before we Execute, we want to send an event to the Game and to the Card as well as our new command as handy hooks. 
        Game.cardCommandEvent.Invoke(command);
        card.cardCommandEvent.Invoke(command); // move this code to where it belongs... into the Command constructor
        command.executeCommand.Invoke(command); 
    }

    public abstract Command GetCommand(Card card, GameAction action); // Should I use abstractions here or an Interface?
    public abstract void onCommandExecute(Command command);

    public abstract class Command
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
            callback = Game.currentActionRound.phaseCallback;
            cardOpsValue = card.opsValue;

            Game.currentActionRound.command = this;
            executeCommand.AddListener(c => Game.cardCommandEvent.Invoke(c)); // Note that this means that Game.cardCommandEvent.before and after aren't really reliable. 
            executeCommand.AddListener(c => card.cardCommandEvent.Invoke(c));
            executeCommand.AddListener(action.onCommandExecute);

        }
    }
}