using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class TrumanDoctrine : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        List<Country> eligibleCountries = new List<Country>();

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.continent == Country.Continent.Europe && country.control == Game.Faction.Neutral && country.influence[Game.Faction.USSR] > 0)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count > 1)
        {
            Message("Truman promises to support freedom throughout Europe");
            countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick);
        }
        else
        {
            if (eligibleCountries.Count == 1)
                Game.SetInfluence.Invoke(eligibleCountries[0], Game.Faction.USSR, 0);

            command.callback.Invoke(); 
        }

        void onCountryClick(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                Game.SetInfluence.Invoke(country, Game.Faction.USSR, 0);
                countryClickHandler.Close();
                command.callback.Invoke(); 
            }
        }
    }    
}
