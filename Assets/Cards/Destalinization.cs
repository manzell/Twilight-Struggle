using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destalinization : Card
{
    List<Country> placedCountries = new List<Country>();

    public override void CardEvent(GameAction.Command command)
    {
        int count = 0;
        List<Country> eligibleCountries = new List<Country>();
        int eligibleInfluence = 0; 

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.influence[Game.Faction.USSR] > 0)
            {
                eligibleCountries.Add(country);
                eligibleInfluence += country.influence[Game.Faction.USSR]; 
            }

        countryClickHandler = new CountryClickHandler(eligibleCountries, RemoveInfluence);

        void RemoveInfluence(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                count++;
                eligibleInfluence--; 

                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, -1);

                if (country.influence[Game.Faction.USSR] == 0)
                {
                    countryClickHandler.Remove(country);
                    eligibleCountries.Remove(country);
                }
            }

            if (count == 4 || eligibleInfluence == 0)
                RedeployInfluence();
        }

        void RedeployInfluence()
        {
            eligibleCountries.Clear();
            placedCountries.Clear();
            countryClickHandler.Close();

            foreach (Country c in FindObjectsOfType<Country>())
                if (c.control != Game.Faction.USA)
                    eligibleCountries.Add(c);

            countryClickHandler = new CountryClickHandler(eligibleCountries, AddInfluence);

            void AddInfluence(Country country)
            {
                if (eligibleCountries.Contains(country))
                {
                    if (placedCountries.CountOf(country) < 2)
                    {
                        Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
                        placedCountries.Add(country);
                        count--;
                    }
                }

                if (count == 0)
                    command.callback.Invoke();
            }
        }
    }
}
