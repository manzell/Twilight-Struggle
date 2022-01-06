using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace TwilightStruggle
{
    public class Realign : GameAction, IActionPrepare, IActionTarget, IActionComplete
    {
        public void Prepare(GameCommand command)
        {
            if (Game.currentPhase is TurnSystem.ActionRound)
                Game.currentActionRound.command = command;

            command.parameters = new RealignVars(); 

            // Check our current Turn & Action Round for a modifier to ops value or coupStrength 
            foreach (OpsBonus opsBonus in Game.currentTurn.transform.GetComponents<OpsBonus>().Concat(Game.currentActionRound.transform.GetComponents<OpsBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    ((RealignVars)command.parameters).totalOps += opsBonus.amount;
            foreach (RealignBonus realignBonus in Game.currentTurn.transform.GetComponents<RealignBonus>().Concat(Game.currentActionRound.transform.GetComponents<RealignBonus>()))
                if (realignBonus.faction == command.faction || realignBonus.faction == Game.Faction.Neutral)
                    ((RealignVars)command.parameters).bonus += realignBonus.amount;

            prepareEvent.Invoke(command); 
        }

        public void Target(GameCommand command)
        {
            RealignAttempt attempt = ((RealignVars)command.parameters).realignAttempts.Last();
            Country targetCountry = attempt.country;

            // Reduce our available ops by 1
            ((RealignVars)command.parameters).ops--; 

            // Check the country for a Bonus
            foreach (RealignBonus realignBonus in targetCountry.transform.GetComponents<RealignBonus>())
                attempt.modifiers[realignBonus.faction == Game.Faction.Neutral ? command.faction : realignBonus.faction]++;

            // Calculate Realignment Bonuses 
            if (targetCountry.influence[Game.Faction.USA] > targetCountry.influence[Game.Faction.USSR])
                attempt.modifiers[Game.Faction.USA]++;
            else if (targetCountry.influence[Game.Faction.USSR] > targetCountry.influence[Game.Faction.USA])
                attempt.modifiers[Game.Faction.USSR]++;
            foreach (Country c in targetCountry.adjacentCountries)
                if (c.control != Game.Faction.Neutral)
                    attempt.modifiers[c.control]++;

            // Add our Turn/Round bonus
            attempt.modifiers[command.faction] += ((RealignVars)command.parameters).bonus; 

            // Make our rolls
            attempt.rolls[Game.Faction.USSR] = Random.Range(0, 6) + 1;
            attempt.rolls[Game.Faction.USSR] = Random.Range(0, 6) + 1;
            
            // Calculate our Influence To Remove
            attempt.influenceRemoved[Game.Faction.USA] = Mathf.Min(0, 
                (attempt.rolls[Game.Faction.USA] + attempt.modifiers[Game.Faction.USA]) - (attempt.rolls[Game.Faction.USSR] + attempt.modifiers[Game.Faction.USSR]));
            attempt.influenceRemoved[Game.Faction.USSR] = Mathf.Min(0,
                (attempt.rolls[Game.Faction.USSR] + attempt.modifiers[Game.Faction.USSR]) - (attempt.rolls[Game.Faction.USA] + attempt.modifiers[Game.Faction.USA]));

            // Apply the Results
            Game.AdjustInfluence(attempt.country, Game.Faction.USA, attempt.influenceRemoved[Game.Faction.USA]);
            Game.AdjustInfluence(attempt.country, Game.Faction.USSR, attempt.influenceRemoved[Game.Faction.USSR]);

            // Flag our event for the animator
            targetEvent.Invoke(command);
        }

        public void Complete(GameCommand command)
        {
            completeEvent.Invoke(command);
            command.callback = null; 
            command.FinishCommand(); 
        }


        public class RealignVars: ICommandParameters
        {
            public int totalOps = 0, ops = 0, bonus = 0;
            public List<RealignAttempt> realignAttempts = new List<RealignAttempt>();
        }

        public class RealignAttempt
        {
            public Country country;
            public Dictionary<Game.Faction, int> influenceRemoved;
            public Dictionary<Game.Faction, int> rolls;
            public Dictionary<Game.Faction, int> modifiers;

            public Dictionary<Game.Faction, vals> attempt; 

            public struct vals
            {
                public int influenceRemoved, roll, modifier; 
            }
        }

        public static List<Country> GetEligibleCountries(GameCommand command)
        {
            List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList();

            foreach (Country country in eligibleCountries)
            {
                // Filter out any countries that are prohibited due to DEFCON or due to lack of opponent influence. 
                if (DEFCONtrack.status <= DEFCONtrack.defconRestrictions[country.continent] || country.influence[command.opponent] == 0 || country.GetComponent<MayNotRealign>())
                    eligibleCountries.Remove(country);
            }

            return eligibleCountries;
        }
    }
}