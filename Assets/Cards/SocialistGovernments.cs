using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialistGovernments : Card
{
    [SerializeField] List<Country> westernEurope = new List<Country>();
    List<Country> eligibleCountries;

    public override void CardEvent(GameAction.Command command)
    {
        int count = 3; 
        eligibleCountries = new List<Country>();

        foreach(Country country in westernEurope)
            if(country.influence[Game.Faction.USA] > 0) 
                eligibleCountries.Add(country);

        countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick);

        void onCountryClick(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                count--;
                Game.AdjustInfluence.Invoke(country, Game.Faction.USA, -1);
                eligibleCountries.Remove(country);
                countryClickHandler.Remove(country);
            }

            if (count == 0)
            {
                countryClickHandler.Close();
                command.callback.Invoke(); 
            }
        }
    }


}
