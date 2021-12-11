using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMECON : Card
{
    [SerializeField] List<Country> easternEurope = new List<Country>();
    List<Country> eligibleCountries = new List<Country>();

    int count; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        count = 5; 
        eligibleCountries = new List<Country>();
        this.callback = callback;

        foreach(Country country in easternEurope)
            if (country.control != Game.Faction.USA)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count <= 5)
            foreach (Country country in eligibleCountries)
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
        else {
            countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick);
        }
    }

    public void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if (!eligibleCountries.Contains(country)) return; 

        Game.AdjustInfluence.Invoke(country, faction, 1);
        eligibleCountries.Remove(country);
        countryClickHandler.RemoveHighlight(country);

        count--; 

        if(count == 0)
        {
            countryClickHandler.Close();
            callback?.Invoke();  
        }
    }
}
