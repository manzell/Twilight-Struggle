using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceStartingInfluence : MonoBehaviour, IPhaseAction
{
    public Game.Faction faction;
    public int influenceAmt;
    public List<Country> eligibleCountries = new List<Country>();

    public void OnPhase(Phase phase, UnityAction callback)
    {
        Game.setActiveFactionEvent.Invoke(faction); 
        FindObjectOfType<UIMessage>().Message($"Place {influenceAmt} {faction} Influence");

        CountryClickHandler.Setup(eligibleCountries, onCountryClick, Color.yellow);

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
                CountryClickHandler.Close();
                callback.Invoke();
            }
        }
    }
}
