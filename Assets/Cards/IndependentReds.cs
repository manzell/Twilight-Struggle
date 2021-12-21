using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentReds : Card
{
    [SerializeField] List<Country> reds;
    List<Country> eligibleCountries = new List<Country>();

    public override void CardEvent(GameAction.Command command)
    {
        foreach(Country country in reds)
            if(country.influence[Game.Faction.USSR] > 0)
                eligibleCountries.Add(country);

        if (eligibleCountries.Count == 1)
            EqualizeInfluence(eligibleCountries[0]);
        else
        {
            if (eligibleCountries.Count > 1)
            {
                countryClickHandler = new CountryClickHandler(eligibleCountries, EqualizeInfluence);
                Message("Select country to Equalize Influence");
            }
            else
                Message("No eligible countries for Independent Reds");

            command.callback.Invoke();
        }

        void EqualizeInfluence(Country country)
        {
            Game.SetInfluence.Invoke(country, Game.Faction.USA, Mathf.Max(country.stability, country.influence[Game.Faction.USA]));
            command.callback.Invoke();
        }
    }  
}
