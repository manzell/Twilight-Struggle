using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceBonusInfluence : MonoBehaviour, IPhaseAction
{
    public Game.Faction faction;
    public int influenceAmt;
    CountryClickHandler countryClickHandler;

    public void OnPhase(Phase phase, UnityAction callback)
    {
        FindObjectOfType<UIMessage>().Message($"Place {influenceAmt} bonus {faction} Influence");

        List<Country> eligibleCountries = new List<Country>();

        foreach (Country country in FindObjectsOfType<Country>())
            if (country.influence[Game.Faction.USA] > 0)
                eligibleCountries.Add(country);

        countryClickHandler = new CountryClickHandler(eligibleCountries, onCountryClick, Color.yellow);

        void onCountryClick(Country country)
        {
            if (eligibleCountries.Contains(country))
            {
                Game.AdjustInfluence.Invoke(country, faction, 1);
                influenceAmt--;

                FindObjectOfType<UIMessage>().Message($"Place {influenceAmt} {faction} Influence");
            }

            if (influenceAmt == 0)
            {
                countryClickHandler.Close();
                callback.Invoke();
            }
        }
    }
}
