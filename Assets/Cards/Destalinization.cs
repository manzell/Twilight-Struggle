using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destalinization : Card
{
    CountryClickHandler countryClickHandler;
    List<Country> eligibleCountries;
    UnityEngine.Events.UnityAction callback;
    int count;

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        count = 0;
        this.callback = callback;
        eligibleCountries = new List<Country>();

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.influence[Game.Faction.USSR] > 0)
                eligibleCountries.Add(country);


        countryClickHandler = new CountryClickHandler(eligibleCountries, RemoveInfluence);


        // Prompt to PLace 4 Influence Callback to AddInfluence
    }

    void RemoveInfluence(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if (eligibleCountries.Contains(country))
        {
            count++;
            Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);

            if (country.influence[Game.Faction.USSR] == 0)
            {
                countryClickHandler.RemoveHighlight(country);
                eligibleCountries.Remove(country);
            }
        }

        if (count == 4)
            Switch(); 
    }

    void Switch()
    {
        eligibleCountries.Clear();
        placedCountries.Clear();
        countryClickHandler.Close();

        foreach (Country c in FindObjectsOfType<Country>())
            if (c.control != Game.Faction.USA)
                eligibleCountries.Add(c);

        countryClickHandler = new CountryClickHandler(eligibleCountries, AddInfluence);
    }

    List<Country> placedCountries; 

    void AddInfluence(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if(eligibleCountries.Contains(country))
        {
            int influenceLimit = 2; 

            foreach(Country c in placedCountries)
                if (c == country) 
                    influenceLimit--;

            if (placedCountries.CountOf(country) < 2)
            {
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
                placedCountries.Add(country);
                count--; 
            } 
        }

        if(count == 0)
        {
            countryClickHandler.Close();
            callback.Invoke(); 
        }
    }
}
