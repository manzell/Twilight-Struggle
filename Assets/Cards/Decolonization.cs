using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decolonization : Card
{
    [SerializeField] List<Country> countries = new List<Country>();

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        this.callback = callback;
        int count = 4; 
        countryClickHandler = new CountryClickHandler(countries, onCountryClick);

        void onCountryClick(Country country, UnityEngine.EventSystems.PointerEventData ped)
        {
            count--;

            if (countries.Contains(country))
            {
                Game.AdjustInfluence.Invoke(country, Game.Faction.USSR, 1);
                countries.Remove(country);
                countryClickHandler.RemoveHighlight(country);
            }
            if (count == 0)
            {
                countryClickHandler.Close();
                callback.Invoke();
            }
        }
    }
}
