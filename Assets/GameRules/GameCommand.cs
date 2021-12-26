using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class GameCommand : MonoBehaviour
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

    public interface ICommandVariables { }
}
