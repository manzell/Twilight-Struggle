using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


namespace TwilightStruggle
{
    public class Coup : GameAction // Work to make this a static implementation?
    {
        public override Command GetCommand(Card card, GameAction action) => new CoupCommand(card, action);

        public override void ExecuteCommandAction(Command command)
        {
            CoupCommand coup = command as CoupCommand;
            UI.CountryClickHandler.Setup(GetEligibleCountries(coup), SetCoupTarget); // Move this to the UI

            void SetCoupTarget(Country country)
            {
                coup.SetTargetCountry(country);

                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, coup.influenceAdjusted[Game.Faction.USSR]);
                Game.AdjustInfluence.Invoke(country, Game.Faction.USA, coup.influenceAdjusted[Game.Faction.USA]);

                if (coup.targetCountry.isBattleground)
                    Game.AdjustDEFCON.Invoke(Game.phasingPlayer, -1);

                UI.CountryClickHandler.Close();
                command.callback.Invoke();
            }
        }

        public List<Country> GetEligibleCountries(CoupCommand command)
        {
            List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList();
            foreach (Country country in eligibleCountries.ToArray())
            {
                // Filter out any countries that are prohibited due to DEFCON or due to lack of opponent influence or cannot be couped for other reasons
                if (DEFCONtrack.Status <= DEFCONtrack.defconRestrictions[country.continent] || country.influence[command.enemyPlayer] == 0 || country.GetComponent<MayNotCoup>())
                    eligibleCountries.Remove(country);
            }

            return eligibleCountries;
        }

        public class CoupCommand : Command
        {
            public Country targetCountry;
            public int roll;
            public int modifiedRoll;
            public Dictionary<Game.Faction, int> influenceAdjusted = new Dictionary<Game.Faction, int>();
            public GameEvent<Command> setTargetEvent = new GameEvent<Command>();

            public CoupCommand(Card c, GameAction a) : base(c, a) =>
                roll = Random.Range(0, 6) + 1;

            public void SetTargetCountry(Country country)
            {
                targetCountry = country;
                roll = Random.Range(0, 6) + 1;
                modifiedRoll = roll + cardOpsValue;

                influenceAdjusted.Add(phasingPlayer, Mathf.Max(0, modifiedRoll - country.stability * 2 - country.influence[enemyPlayer]));
                influenceAdjusted.Add(enemyPlayer, -Mathf.Min(Mathf.Max(modifiedRoll - country.stability * 2, 0), country.influence[enemyPlayer]));

                Debug.Log($"{phasingPlayer} coups {targetCountry.countryName} with {cardOpsValue} Ops!");
                Debug.Log($"Coup Roll: {roll}. Removing {influenceAdjusted[enemyPlayer]} {enemyPlayer} influence. Adding {influenceAdjusted[phasingPlayer]} {phasingPlayer} influence in {targetCountry.countryName}");
            }
        }

        /*============
         * REFACTOR
         * ===========
         */

        // We're going to implement something I'm calling a Flyby Pattern. We encapsulate our Card and a reference to the game action here. 
        // Then we'll "dip" the command into static functions on the chose behavior which will manipulate the command object. 

        public override void Prepare(GameCommand command)
        {
            // Call this when we say "I'm goin to coup with X" 

            // coupOps = card.OpsValue; 
            // where do the coupOps get stored? 
            // They could live on an INSTANCE of a Coup? That would mean multiple copies of the logic and that seems bad.
            // We can create a lightweight class/struct to store it, 
        }

        public struct coupVars : ICommandVariables
        {
            public Country targetCountry;
            public int coupOps;
            public int roll;
            public Dictionary<Country, int> influence; // 

        }
    }
}
