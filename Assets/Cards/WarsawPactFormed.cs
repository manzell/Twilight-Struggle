using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarsawPactFormed : Card
{
    [SerializeField] List<Country> easternEurope = new List<Country>();
    List<Country> eligibleCountries;

    public override void Event(UnityEngine.Events.UnityAction? callback)
    {
        eligibleCountries = new List<Country>();
        // Prompt if players wants to add or remove Influence
    }

    public void Remove()
    {
        foreach (Country country in easternEurope)
            if (country.influence[Game.Faction.USA] > 0)
                eligibleCountries.Add(country);

        // Create a Country Picker
    }

    public void Add()
    {
        eligibleCountries = easternEurope; 

        // Create an Influence Placer
    }

    public void RemoveUSInfluence(List<Country> countries)
    {
        foreach (Country country in countries)
            Game.SetInfluence.Invoke(country, Game.Faction.USA, 0); 
    }

    public void AddUSSRInfluence(Dictionary<Country, int> influence)
    {
        foreach (Country country in influence.Keys)
            Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, influence[country]);

    }
}
