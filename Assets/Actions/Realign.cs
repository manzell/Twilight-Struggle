using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Realign : Action
{
    RealignmentAttempts realignAttempts = new RealignmentAttempts();
    List<Country> eligibleCountries = new List<Country>();

    public override void Play(UnityEngine.Events.UnityAction? callback)
    {
        eligibleCountries.Clear();
        realignAttempts.Clear(); 

        foreach(Country country in FindObjectsOfType<Country>())
            if(country.influence[opponent] > 0)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count == 0) return;

        countryClickHandler = new CountryClickHandler(eligibleCountries, SetRealignTarget, Color.green);
    }

    public void SetRealignTarget(Country country, PointerEventData ped)
    {
        RealignAttempt realignAttempt = new RealignAttempt();
        realignAttempt.targetCountry = country;
        realignAttempt.faction = Game.phasingPlayer; 

        realignAttempts.Add(realignAttempt);

        // Calculate Realignment Bonuses 
        if (realignAttempt.targetCountry.influence[Game.Faction.USA] > realignAttempt.targetCountry.influence[Game.Faction.USSR])
        {
            realignAttempt.realignmentBonus[Game.Faction.USA]++;

        }
        else if (realignAttempt.targetCountry.influence[Game.Faction.USSR] > realignAttempt.targetCountry.influence[Game.Faction.USA])
            realignAttempt.realignmentBonus[Game.Faction.USSR]++;

        foreach (Country c in country.adjacentCountries)
            if (c.control != Game.Faction.Neutral)
                realignAttempt.realignmentBonus[c.control]++;

        // Make our Rolls
        realignAttempt.realignmentRolls[Game.Faction.USA] = Random.Range(0, 6) + 1;
        realignAttempt.realignmentRolls[Game.Faction.USSR] = Random.Range(0, 6) + 1;

        // Apply Realignment Results
        int USAroll = realignAttempt.realignmentBonus[Game.Faction.USA] + realignAttempt.realignmentRolls[Game.Faction.USA];
        int USSRroll = realignAttempt.realignmentBonus[Game.Faction.USSR] + realignAttempt.realignmentRolls[Game.Faction.USSR];

        Debug.Log($"{realignAttempt.faction} realigning {realignAttempt.targetCountry.countryName} ({realignAttempts.Count}/{card.opsValue}) " +
            $"USA Roll: {realignAttempt.realignmentRolls[Game.Faction.USA]} + {realignAttempt.realignmentBonus[Game.Faction.USA]}; " +
            $"USSR Roll: {realignAttempt.realignmentRolls[Game.Faction.USSR]} + {realignAttempt.realignmentBonus[Game.Faction.USSR]}");

        Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, Mathf.Min(0, USSRroll - USAroll));
        Game.AdjustInfluence.Invoke(country, Game.Faction.USA, Mathf.Min(0, USAroll - USSRroll));

        // Remove them from the list of eligible countries in some situations.
        if (country.influence[opponent] == 0)
        {
            eligibleCountries.Remove(country);
            countryClickHandler.RemoveHighlight(country); 
        }

        if (eligibleCountries.Count == 0 || realignAttempts.Count == card.opsValue)
        {
            Game.currentActionRound.gameAction = realignAttempts;
            FinishAction();
        }
    }

    public class RealignmentAttempts : List<RealignAttempt>, IGameAction { }

    public class RealignAttempt
    {
        public Country targetCountry;
        public Game.Faction faction; 
        public Dictionary<Game.Faction, int> realignmentBonus = new Dictionary<Game.Faction, int> { { Game.Faction.USA, 0 }, { Game.Faction.USSR, 0 } };
        public Dictionary<Game.Faction, int> realignmentRolls = new Dictionary<Game.Faction, int> { { Game.Faction.USA, 0 }, { Game.Faction.USSR, 0 } };
    }
}
