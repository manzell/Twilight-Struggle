using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuezCrisis : Card
{
    [SerializeField] List<Country> countries;
    UnityEngine.Events.UnityAction callback;
    CountryClickHandler handler;
    int count; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        count = 4; 
        this.callback = callback;

        handler = new CountryClickHandler(countries, onCountryClick); 
    }

    List<Country> removedCountries; 

    void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if(countries.Contains(country))
        {
            if(countries.CountOf(country) < 2)
            {
                removedCountries.Add(country);
                Game.AdjustInfluence.Invoke(country, Game.Faction.USA, -1);
                count--; 
            }

            if(countries.CountOf(country) == 2)
            {
                handler.RemoveHighlight(country);
            }
        }

        if(count == 0)
        {
            handler.Close();
            callback.Invoke(); 
        }
    }
}
