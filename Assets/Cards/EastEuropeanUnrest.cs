using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EastEuropeanUnrest : Card
{
    [SerializeField] List<Country> easternEurope; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        int count = 3;
        int amount = Game.gamePhase == Game.GamePhase.LateWar ? 2 : 1;
        List<Country> eligibleCountries = new List<Country>();

        this.callback = callback;

        foreach(Country country in easternEurope)
            if(country.influence[Game.Faction.USSR] > 0) 
                eligibleCountries.Add(country);

        if(eligibleCountries.Count > 3)
            countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick); 
        else
            RemoveInfluence(Game.Faction.USSR, eligibleCountries, amount);

        void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
        {
            if (eligibleCountries.Contains(country))
            {
                eligibleCountries.Remove(country);
                countryClickHandler.RemoveHighlight(country);
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, -amount);
                count--;
            }

            if (count == 0)
            {
                countryClickHandler.Close();
                callback.Invoke();
            }
        }
    }


}
