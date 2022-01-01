using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class PlaceStartingInfluence : MonoBehaviour, TurnSystem.IPhaseAction
    {
        public Game.Faction faction;
        public int influenceAmt;
        public List<Country> eligibleCountries = new List<Country>();

        public void Action(TurnSystem.Phase phase, UnityAction callback)
        {
            Game.SetActingFaction(faction);
            FindObjectOfType<UI.UIMessage>().Message($"Place {influenceAmt} {faction} Influence");

            Card.AddInfluence(eligibleCountries, faction, influenceAmt, 0, callback); 
            //UI.CountryClickHandler.Setup(eligibleCountries, onCountryClick, Color.yellow);

            //void onCountryClick(Country country)
            //{
            //    if (eligibleCountries.Contains(country))
            //    {
            //        Game.AdjustInfluence(country, faction, 1);
            //        influenceAmt--;

            //        FindObjectOfType<UI.UIMessage>().Message($"Place {influenceAmt} {faction} Influence");
            //    }

            //    if (influenceAmt == 0)
            //    {
            //        UI.CountryClickHandler.Close();
            //        callback.Invoke();
            //    }
            //}
        }
    }
}