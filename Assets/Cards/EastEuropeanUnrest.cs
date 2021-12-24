using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EastEuropeanUnrest : Card
{
    [SerializeField] List<Country> easternEurope;

    public override void CardEvent(GameAction.Command command)
    {
        int count = 3;
        int amount = Game.gamePhase == Game.GamePhase.LateWar ? 2 : 1;
        List<Country> eligibleCountries = new List<Country>();

        foreach(Country country in easternEurope)
            if(country.influence[Game.Faction.USSR] > 0) 
                eligibleCountries.Add(country);

        FindObjectOfType<UIManager>().SetButton(FindObjectOfType<UIManager>().primaryButton, "Unrest Abates", Finish); 

        if (eligibleCountries.Count > 3)
            CountryClickHandler.Setup(eligibleCountries, onCountryClick);
        else
        {
            RemoveInfluence(Game.Faction.USSR, eligibleCountries, amount);
            Finish(); 
        }

        void onCountryClick(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                eligibleCountries.Remove(country);
                CountryClickHandler.Remove(country);
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, -amount);
                count--;
            }

            if (count == 0) 
                Finish();
        }

        void Finish()
        {
            FindObjectOfType<UIManager>().UnsetButton(FindObjectOfType<UIManager>().primaryButton);
            CountryClickHandler.Close(); 
            command.callback.Invoke();
        }
    }


}
