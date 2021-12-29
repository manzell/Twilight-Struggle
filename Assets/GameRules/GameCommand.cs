using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

namespace TwilightStruggle
{
    public class GameCommand : MonoBehaviour
    {
        // Fauxstructor Vars
        public Game.Faction faction;
        public Card card;
        public GameAction gameAction;
        public TurnSystem.Phase phase;

        // Output Vars
        public Dictionary<Country, Dictionary<Game.Faction, int>> influenceChange = new Dictionary<Country, Dictionary<Game.Faction, int>>();
        public Dictionary<Game.Faction, int> vpChange = new Dictionary<Game.Faction, int>();
        public Dictionary<Game.Faction, int> milOpsChange = new Dictionary<Game.Faction, int>();
        public Dictionary<Game.Faction, int> spaceRaceChange = new Dictionary<Game.Faction, int>(); // TODO: Space Race Commands? 
        public int defconChange = 0;

        // Helper vars: Space Race target roll, realignment rolls, coup roll, bonus ops etc
        public ICommandVariables parameters;
        public UnityAction<GameCommand> callback;
        public UnityAction phaseCallback; 
        public Game.Faction opponent => faction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;

        public static GameCommand Create(Game.Faction faction, Card card, GameAction gameAction)
        {
            GameCommand command = new GameCommand();
            command.faction = faction;
            command.card = card;
            command.gameAction = gameAction;
            command.phase = Game.currentPhase;
            command.phaseCallback = Game.currentPhase.callback; 
            
            if (gameAction is IActionPrepare)
                command.prepare = (IActionPrepare)gameAction;
            if (gameAction is IActionTarget)
                command.target = (IActionTarget)gameAction;
            if (gameAction is IActionComplete)
                command.complete = (IActionComplete)gameAction;

            return command;
        }

        public void FinishCommand() => phaseCallback?.Invoke();

        public void Prepare() => prepare?.Prepare(this);
        public void Target() => target?.Target(this);
        public void Complete() => complete?.Complete(this);
        public void Undo() => undo?.Undo(this);

        public IActionPrepare prepare;
        public IActionTarget target; 
        public IActionComplete complete;
        public IActionUndo undo;
    }

    public interface ICommandVariables { }
    public interface IActionPrepare  { public void Prepare(GameCommand command); }   // Setup all event Variables, and Prompt for a 0, 1, or N targets
    public interface IActionTarget   { public void Target(GameCommand command); }    // Receive a Target and Execute the event and set all output variables. Recur or set callback to complete
    public interface IActionComplete { public void Complete(GameCommand command); }  // Implement whatever happened
    public interface IActionUndo { public void Undo(GameCommand command); } // TODO as long as our parameters contain Gamestate changes, undo should be* easy. 
}
