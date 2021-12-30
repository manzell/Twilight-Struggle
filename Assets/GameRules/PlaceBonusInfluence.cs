using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class PlaceBonusInfluence : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public Game.Faction faction;
        public int influenceAmt;

        public void Action(TurnSystem.Phase phase, UnityAction callback)
        {
            FindObjectOfType<UI.UIMessage>().Message($"Place {influenceAmt} bonus {faction} Influence");

            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in FindObjectsOfType<Country>())
                if (country.influence[Game.Faction.USA] > 0)
                    eligibleCountries.Add(country);

            UI.CountryClickHandler.Setup(eligibleCountries, onCountryClick, Color.yellow);

            void onCountryClick(Country country)
            {
                if (eligibleCountries.Contains(country))
                {
                    Game.AdjustInfluence(country, faction, 1);
                    influenceAmt--;

                    FindObjectOfType<UI.UIMessage>().Message($"Place {influenceAmt} {faction} Influence");
                }

                if (influenceAmt == 0)
                {
                    UI.CountryClickHandler.Close();
                    callback.Invoke();
                }
            }
        }
    }
}