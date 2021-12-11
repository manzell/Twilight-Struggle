using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumanDoctrine : Card
{
    [SerializeField] List<Country> eligibleCountries; 

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        this.callback = callback;

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.continent == Country.Continent.Europe && country.control == Game.Faction.Neutral && country.influence[Game.Faction.USSR] > 0)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count > 1)
            countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick);
        else
        {
            if (eligibleCountries.Count == 1)
                Game.SetInfluence.Invoke(eligibleCountries[0], Game.Faction.USSR, 0);

            callback.Invoke();
        }
    }

    void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if(eligibleCountries.Contains(country))
        {
            Game.SetInfluence.Invoke(country, Game.Faction.USSR, 0);
            countryClickHandler.Close();
            callback.Invoke(); 
        }
    }    
}
