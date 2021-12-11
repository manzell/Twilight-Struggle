using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentReds : Card
{
    [SerializeField] List<Country> reds;
    List<Country> eligibleCountries = new List<Country>();
    CountryClickHandler handler;
    UnityEngine.Events.UnityAction callback;

    public override void Event(UnityEngine.Events.UnityAction callback)
    {
        this.callback = callback;

        foreach(Country country in reds)
            if(country.influence[Game.Faction.USSR] > 0)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count == 1)
            Equalize(eligibleCountries[0], null); 
        else if (eligibleCountries.Count > 1)
            handler = new CountryClickHandler(eligibleCountries, Equalize); 
        else 
            callback.Invoke();
    }

    void Equalize(Country country, UnityEngine.EventSystems.PointerEventData ped)
    {
        if(eligibleCountries.Contains(country))
        {
            Game.SetInfluence.Invoke(country, Game.Faction.USA, Mathf.Max(country.stability, country.influence[Game.Faction.USA]));
            callback.Invoke();
        }
    }   
}
