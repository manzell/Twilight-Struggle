using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destalinization : Card
{
    public override void CardEvent(GameAction.Command command)
    {
        int count = 4;
        List<Country> eligibleCountries = new List<Country>();
        List<Country> removedFrom = new List<Country>();
        List<Country> addedTo = new List<Country>();

        int eligibleInfluence = 0; 

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.influence[Game.Faction.USSR] > 0)
            {
                eligibleCountries.Add(country);
                eligibleInfluence += country.influence[Game.Faction.USSR]; 
            }

        uiManager.SetButton(uiManager.cancelButton, "Finish Destal", Finish);
        uiManager.SetButton(uiManager.confirmButton, "Finish Removing Influence", RedeployInfluence);

        countryClickHandler = new CountryClickHandler(eligibleCountries, RemoveInfluence);

        void RemoveInfluence(Country country)
        {
            Message($"Remove {count} Stalinist influence anywhere"); 
            count--;
            eligibleInfluence--; 

            Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, -1);

            if (country.influence[Game.Faction.USSR] == 0)
            {
                countryClickHandler.Remove(country);
                eligibleCountries.Remove(country);
                removedFrom.Add(country);
            }

            if (count == 0 || eligibleInfluence == 0)
                RedeployInfluence();
        }

        void RedeployInfluence()
        {
            Message($"Place {count} USSR influence");
            countryClickHandler.Close();

            foreach (Country c in FindObjectsOfType<Country>())
                if (c.control != Game.Faction.USA)
                    eligibleCountries.Add(c);

            countryClickHandler = new CountryClickHandler(eligibleCountries, AddInfluence);

            void AddInfluence(Country country)
            {
                if (addedTo.CountOf(country) < 2)
                {
                    Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
                    addedTo.Add(country);
                    count--;
                }

                if (count == 0)
                    Finish(); 
            }
        }

        void Finish()
        {
            countryClickHandler.Close(); 
            
            uiManager.UnsetButton(uiManager.cancelButton);
            uiManager.UnsetButton(uiManager.confirmButton);
            
            command.callback.Invoke();
        }
    }
}
