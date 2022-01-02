using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

namespace TwilightStruggle
{
    // Friendly Interface Definitions
    public interface ICommandParameters { }
    public interface IActionPrepare { public void Prepare(GameCommand command); }
    public interface IActionTarget { public void Target(GameCommand command); }
    public interface IActionComplete { public void Complete(GameCommand command); }
    public interface IActionUndo { public void Undo(GameCommand command); }

    public class GameCommand
    {
        // Base Variables
        public Game.Faction faction;
        public Card card;
        public GameAction gameAction;
        public TurnSystem.Phase phase;

        // Output Vars - Gamestate Changes used for Undo purposes (TODO: UNDO)
        public Dictionary<Country, Dictionary<Game.Faction, int>> influenceChange = new Dictionary<Country, Dictionary<Game.Faction, int>>();
        public Dictionary<Game.Faction, int> vpChange = new Dictionary<Game.Faction, int>();
        public Dictionary<Game.Faction, int> milOpsChange = new Dictionary<Game.Faction, int>();
        public Dictionary<Game.Faction, int> spaceRaceChange = new Dictionary<Game.Faction, int>(); 
        public List<Card> cardsDrawn = new List<Card>();
        public List<Card> cardsDiscard = new List<Card>(); 
        public int defconChange = 0;

        // Helper vars: Space Race target roll, realignment rolls, coup roll, bonus ops etc
        public ICommandParameters parameters;
        public UnityAction<GameCommand> callback;
        public UnityAction<UnityAction> phaseCallback; 
        public Game.Faction opponent => faction == Game.Faction.USA ? Game.Faction.USSR : Game.Faction.USA;
        public GameCommand subCommand; 

        //Interface Library
        public IActionPrepare prepare;
        public IActionTarget target;
        public IActionComplete complete;
        public IActionUndo undo;

        // Callback wrappers
        public void Prepare() => prepare?.Prepare(this);
        public void Target() => target?.Target(this);
        public void Complete() => complete?.Complete(this);
        public void Undo() => undo?.Undo(this);

        public static GameCommand Create(Game.Faction faction, Card card, GameAction gameAction)
        {
            GameCommand command = new GameCommand();
            command.faction = faction;
            command.card = card;
            command.gameAction = gameAction;
            command.phase = Game.currentPhase;
            command.phaseCallback = Game.currentPhase.NextPhase; 
            
            if (gameAction is IActionPrepare)
                command.prepare = (IActionPrepare)gameAction;
            if (gameAction is IActionTarget)
                command.target = (IActionTarget)gameAction;
            if (gameAction is IActionComplete)
                command.complete = (IActionComplete)gameAction;

            return command;
        }

        public void FinishCommand()
        {
            if(callback != null)
                callback.Invoke(this);
            else
                phaseCallback?.Invoke(Game.currentPhase.callback);
        }
    }
}
