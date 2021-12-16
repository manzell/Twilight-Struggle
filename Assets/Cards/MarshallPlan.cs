using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class MarshallPlan : Card
{
    [SerializeField] List<Country> westernEurope = new List<Country>();
    [SerializeField] NATO NATO;

    public override void CardEvent(GameAction.Command command)
    {
        int count = 7;

        NATO.isPlayable = true;

        foreach (Country country in westernEurope)
            if (country.control == Game.Faction.USSR)
                westernEurope.Remove(country);

        if (westernEurope.Count <= count)
        {
            AddInfluence(faction, westernEurope, 1);
            command.callback.Invoke(); 
        }
        else
            countryClickHandler = new CountryClickHandler(westernEurope, onCountryClick, new Color(0f, .6f, .6f));

        void onCountryClick(Country country)
        {
            if (westernEurope.Contains(country))
            {
                westernEurope.Remove(country);
                countryClickHandler.Remove(country);
                Game.AdjustInfluence.Invoke(country, Game.Faction.USA, 1);
                count--;
            }

            if (count == 0)
            {
                countryClickHandler.Close();
                command.callback.Invoke(); 
            }
        }
    }
}
