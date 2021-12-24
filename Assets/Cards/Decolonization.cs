using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decolonization : Card
{
    [SerializeField] List<Country> countries = new List<Country>();
    public int countryCount = 4; 

    public override void CardEvent(GameAction.Command command)
    {
        int count = countryCount; 

        CountryClickHandler.Setup(countries, onCountryClick);

        Message($"Place {count} USSR Influence"); 

        uiManager.SetButton(uiManager.primaryButton, "Finish Decol", Finish);         

        void onCountryClick(Country country)
        {
            count--;

            if (countries.Contains(country))
            {
                Message($"Place {count} USSR Influence");
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
                countries.Remove(country);
                CountryClickHandler.Remove(country);
            }
            if (count == 0)
                Finish(); 
        }

        void Finish()
        {
            CountryClickHandler.Close();
            uiManager.UnsetButton(uiManager.primaryButton); 
            command.callback.Invoke();
        }
    }
}
