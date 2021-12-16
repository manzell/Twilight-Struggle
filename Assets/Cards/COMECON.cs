using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class COMECON : Card
{
    [SerializeField] List<Country> easternEurope = new List<Country>();

    public override void CardEvent(GameAction.Command command)
    {
        int count = 5;
        List<Country> eligibleCountries = new List<Country>();

        foreach(Country country in easternEurope)
            if (country.control != Game.Faction.USA)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count <= 5)
            foreach (Country country in eligibleCountries)
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
        else
            countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick);

        // TODO: Make this into generic calls to like "Add/Remove X influece to any/each of these countries"
        void onCountryClick(Country country)
        {
            // TODO: Change the country click handler to only exist on presently-eligible countries. 
            if (!eligibleCountries.Contains(country)) return;

            Game.AdjustInfluence.Invoke(country, faction, 1);
            eligibleCountries.Remove(country);
            countryClickHandler.Remove(country);

            count--;

            if (count == 0)
            {
                countryClickHandler.Close();
                command.callback.Invoke();
            }
        }
    }
}
