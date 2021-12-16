using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decolonization : Card
{
    [SerializeField] List<Country> countries = new List<Country>();

    public override void CardEvent(GameAction.Command command)
    {
        int count = 4; 

        countryClickHandler = new CountryClickHandler(countries, onCountryClick);

        void onCountryClick(Country country)
        {
            count--;

            if (countries.Contains(country))
            {
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
                countries.Remove(country);
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
