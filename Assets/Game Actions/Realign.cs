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
            RealignVars realignVars = new RealignVars(); 

            List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList(); 

            foreach (Country country in eligibleCountries)
            {
                // Filter out any countries that are prohibited due to DEFCON or due to lack of opponent influence. 
                if (DEFCONtrack.Status <= DEFCONtrack.defconRestrictions[country.continent] || country.influence[command.opponent] == 0 || country.GetComponent<MayNotRealign>())
                    eligibleCountries.Remove(country);
            }

            // Check our current Turn & Action Round for a modifier to ops value or coupStrength 
            foreach (OpsBonus opsBonus in Game.currentTurn.transform.GetComponents<OpsBonus>().Concat(Game.currentActionRound.transform.GetComponents<OpsBonus>()))
                if (opsBonus.faction == command.faction || opsBonus.faction == Game.Faction.Neutral)
                    realignVars.totalOps += opsBonus.amount;
            foreach (RealignBonus realignBonus in Game.currentTurn.transform.GetComponents<RealignBonus>().Concat(Game.currentActionRound.transform.GetComponents<RealignBonus>()))
                if (realignBonus.faction == command.faction || realignBonus.faction == Game.Faction.Neutral)
                    realignVars.bonus += realignBonus.amount;

            prepareEvent.Invoke(command); 
        }

        public void Target(GameCommand command)
        {
            RealignVars realignVars = (RealignVars)command.parameters;
            RealignAttempt attempt = realignVars.realignAttempts.Last();
            Country targetCountry = attempt.country;

            // Reduce our available ops by 1
            realignVars.ops--; 

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
            attempt.modifiers[command.faction] += realignVars.bonus; 

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

            if (realignVars.ops == 0)
                Complete(command); 
        }

        public void Complete(GameCommand command)
        {
            completeEvent.Invoke(command);
            command.FinishCommand(); 
        }

        public class RealignVars
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

        //public override Command GetCommand(Card card, GameAction action) => new RealignCommand(card, action);

        //public override void ExecuteCommandAction(Command command) // This is called when we say "Use This Card for Realignments" - before specifying which countries. 
        //{
        //    RealignCommand realign = command as RealignCommand;

        //    List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList();



        //    UI.CountryClickHandler.Setup(eligibleCountries, SetRealignTarget);

        //    void SetRealignTarget(Country country)
        //    {
        //        RealignAttempt attempt = new RealignAttempt();
        //        realign.realignAttempts.Add(attempt);

        //        realign.setTargetEvent.Invoke(attempt);

        //        Debug.Log($"{realign.phasingPlayer} realigning {attempt.targetCountry.countryName} ({realign.realignAttempts.Count}/{realign.cardOpsValue}) " +
        //            $"USA Roll: {attempt.roll[Game.Faction.USA]} + {attempt.modifiedRoll[Game.Faction.USA] - attempt.roll[Game.Faction.USA]}; " +
        //            $"USSR Roll: {attempt.roll[Game.Faction.USSR]} + {attempt.modifiedRoll[Game.Faction.USSR] - attempt.roll[Game.Faction.USSR]}");

        //        Game.adjustInfluenceEvent.Invoke(country, Game.Faction.USSR, attempt.influenceRemoved[Game.Faction.USSR]);
        //        Game.adjustInfluenceEvent.Invoke(country, Game.Faction.USA, attempt.influenceRemoved[Game.Faction.USA]);

        //        if (attempt.targetCountry.influence[realign.enemyPlayer] == 0)
        //        {
        //            eligibleCountries.Remove(attempt.targetCountry);
        //            UI.CountryClickHandler.Remove(country);
        //        }

        //        if (realign.realignAttempts.Count == realign.cardOpsValue || eligibleCountries.Count == 0)
        //        {
        //            UI.CountryClickHandler.Close();
        //            realign.callback.Invoke();
        //        }
        //    }
        //}

        //public class RealignCommand : Command
        //{
        //    public GameEvent<RealignAttempt> setTargetEvent = new GameEvent<RealignAttempt>();
        //    public List<RealignAttempt> realignAttempts = new List<RealignAttempt>();

        //    public RealignCommand(Card c, GameAction a) : base(c, a) { }
        //}

        //public class RealignAttempt
        //{
        //    public Country targetCountry;
        //    public Dictionary<Game.Faction, int> roll = new Dictionary<Game.Faction, int>();
        //    public Dictionary<Game.Faction, int> modifiedRoll = new Dictionary<Game.Faction, int>();
        //    public Dictionary<Game.Faction, int> influenceRemoved = new Dictionary<Game.Faction, int>();

        //    public RealignAttempt()
        //    {
        //        // Make our rolls
        //        roll[Game.Faction.USSR] = Random.Range(0, 6) + 1;
        //        roll[Game.Faction.USA] = Random.Range(0, 6) + 1;

        //        modifiedRoll[Game.Faction.USA] = roll[Game.Faction.USA];
        //        modifiedRoll[Game.Faction.USSR] = roll[Game.Faction.USSR];






        //    }
        //}
    }
}