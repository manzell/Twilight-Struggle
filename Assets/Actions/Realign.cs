using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq; 

public class Realign : GameAction
{
    public override Command GetCommand(Card card, GameAction action) => new RealignCommand(card, action);

    public override void ExecuteCommandAction(Command command) // This is called when we say "Use This Card for Realignments" - before specifying which countries. 
    {
        RealignCommand realign = command as RealignCommand;

        List<Country> eligibleCountries = FindObjectsOfType<Country>().ToList();
        
        foreach(Country country in eligibleCountries)
        {
            // Filter out any countries that are prohibited due to DEFCON or due to lack of opponent influence. 
            if (DEFCON.Status <= DEFCON.defconRestrictions[country.continent] || country.influence[realign.enemyPlayer] == 0 || country.GetComponent<MayNotRealign>()) 
                eligibleCountries.Remove(country);
        }

        countryClickHandler = new CountryClickHandler(eligibleCountries, SetRealignTarget); 

        void SetRealignTarget(Country country)
        {
            RealignAttempt attempt = new RealignAttempt();
            realign.realignAttempts.Add(attempt);

            realign.setTargetEvent.Invoke(attempt);

            Debug.Log($"{realign.phasingPlayer} realigning {attempt.targetCountry.countryName} ({realign.realignAttempts.Count}/{realign.cardOpsValue}) " +
                $"USA Roll: {attempt.roll[Game.Faction.USA]} + {attempt.modifiedRoll[Game.Faction.USA] - attempt.roll[Game.Faction.USA]}; " +
                $"USSR Roll: {attempt.roll[Game.Faction.USSR]} + {attempt.modifiedRoll[Game.Faction.USSR] - attempt.roll[Game.Faction.USSR]}");

            Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, attempt.influenceRemoved[Game.Faction.USSR]);
            Game.AdjustInfluence.Invoke(country, Game.Faction.USA, attempt.influenceRemoved[Game.Faction.USA]);

            if(attempt.targetCountry.influence[realign.enemyPlayer] == 0)
            {
                eligibleCountries.Remove(attempt.targetCountry); 
                countryClickHandler.Remove(country);
            }

            if (realign.realignAttempts.Count == realign.cardOpsValue || eligibleCountries.Count == 0)
            {
                countryClickHandler.Close();
                realign.callback.Invoke(); 
            }
        }
    }

    public class RealignCommand : Command
    {
        public GameEvent<RealignAttempt> setTargetEvent = new GameEvent<RealignAttempt>();  
        public List<RealignAttempt> realignAttempts = new List<RealignAttempt>();

        public RealignCommand(Card c, GameAction a) : base(c, a) { }
    }

    public class RealignAttempt
    {
        public Country targetCountry;
        public Dictionary<Game.Faction, int> roll = new Dictionary<Game.Faction, int>(); 
        public Dictionary<Game.Faction, int> modifiedRoll = new Dictionary<Game.Faction, int>();
        public Dictionary<Game.Faction, int> influenceRemoved = new Dictionary<Game.Faction, int>();

        public RealignAttempt()
        {
            // Make our rolls
            roll[Game.Faction.USSR] = Random.Range(0, 6) + 1;
            roll[Game.Faction.USA] = Random.Range(0, 6) + 1;

            modifiedRoll[Game.Faction.USA] = roll[Game.Faction.USA];
            modifiedRoll[Game.Faction.USSR] = roll[Game.Faction.USSR];

            // Calculate Realignment Bonuses 
            if (targetCountry.influence[Game.Faction.USA] > targetCountry.influence[Game.Faction.USSR])
                modifiedRoll[Game.Faction.USA]++;
            else if (targetCountry.influence[Game.Faction.USSR] > targetCountry.influence[Game.Faction.USA])
                modifiedRoll[Game.Faction.USSR]++;

            foreach (Country c in targetCountry.adjacentCountries)
                if (c.control != Game.Faction.Neutral)
                    modifiedRoll[c.control]++;

            influenceRemoved[Game.Faction.USA] = Mathf.Min(0, modifiedRoll[Game.Faction.USA] - modifiedRoll[Game.Faction.USSR]);
            influenceRemoved[Game.Faction.USA] = Mathf.Min(0, modifiedRoll[Game.Faction.USA] - modifiedRoll[Game.Faction.USSR]);
        }
    }
}
