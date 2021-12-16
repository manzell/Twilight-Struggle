using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarsawPactFormed : Card
{
    [SerializeField] List<Country> easternEurope = new List<Country>();
    [SerializeField] NATO NATO;

    public override void CardEvent(GameAction.Command command)
    {
        NATO.isPlayable = true;

        // TODO: Present the player a choice. For now, just add USSR influence
        AddUSSRInfluence(command); 
    }

    void RemoveAllUSInfluence(GameAction.Command command)
    {
        int count = 4;

        foreach (Country country in easternEurope)
            if (country.influence[Game.Faction.USA] == 0)
                easternEurope.Add(country);

        countryClickHandler = new CountryClickHandler(easternEurope, doRemoveInfluence); 

        void doRemoveInfluence(Country country)
        {
            Game.SetInfluence.Invoke(country, Game.Faction.USA, 0);
            countryClickHandler.Remove(country);
            easternEurope.Remove(country);
            count--;

            if (count == 0)
            {
                countryClickHandler.Close();
                command.callback.Invoke(); 
            }
                
        }
    }

    void AddUSSRInfluence(GameAction.Command command)
    {
        List<Country> placedCountries = new List<Country>();
        int count = 5;
        int limit = 2;

        countryClickHandler = new CountryClickHandler(easternEurope, doAddInfluence); 

        void doAddInfluence(Country country)
        {
            if(placedCountries.CountOf(country) < limit)
            {
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
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
