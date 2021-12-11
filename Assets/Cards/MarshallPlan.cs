using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshallPlan : Card
{
    [SerializeField] List<Country> westernEurope = new List<Country>();

    List<Country> eligibleCountries;
    UnityEngine.Events.UnityAction callback;
    CountryClickHandler handler;
    int count; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        count = 7;
        List<Country> eligibleCountries = new List<Country>();

        foreach (Country country in westernEurope)
            if (country.control != Game.Faction.USSR)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count <= 7)
        {
            AddInfluence(faction, eligibleCountries, 1);
            callback.Invoke(); 
        }
        else
            handler = new CountryClickHandler(eligibleCountries, onCountryClick, new Color(0f, .6f, .6f)); 
    }

    void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if (eligibleCountries.Contains(country))
        {
            eligibleCountries.Remove(country);
            handler.RemoveHighlight(country);
            Game.AdjustInfluence.Invoke(country, Game.Faction.USA, 1);
            count--; 
        }

        if(count == 0)
        {
            handler.Close(); 
            callback.Invoke();
        }
    }
}
