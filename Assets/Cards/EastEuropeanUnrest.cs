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

        if(eligibleCountries.Count > 3)
            countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick); 
        else // If it's a foregone choice, just jump to it and don't prompt the player
            RemoveInfluence(Game.Faction.USSR, eligibleCountries, amount); 

        void onCountryClick(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                eligibleCountries.Remove(country);
                countryClickHandler.Remove(country);
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, -amount);
                count--;
            }

            if (count == 0)
                command.callback.Invoke(); 
        }
    }


}
