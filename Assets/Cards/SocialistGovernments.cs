using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialistGovernments : Card
{
    [SerializeField] List<Country> westernEurope = new List<Country>();
    UnityEngine.Events.UnityAction callback;
    List<Country> eligibleCountries; 
    CountryClickHandler handler;
    int count; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        this.callback = callback;
        count = 3; 
        eligibleCountries = new List<Country>();

        foreach(Country country in westernEurope)
            if(country.influence[Game.Faction.USA] > 0) 
                eligibleCountries.Add(country);

        handler = new CountryClickHandler(eligibleCountries, onCountryClick);

        // Prompt the US player to submit their list of countries
    }

    void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if(eligibleCountries.Contains(country))
        {
            count--;
            Game.AdjustInfluence.Invoke(country, Game.Faction.USA, -1);
            eligibleCountries.Remove(country);
            handler.RemoveHighlight(country);
        }

        if(count == 0)
        {
            handler.Close(); 
            callback.Invoke();
        }
    }
}
