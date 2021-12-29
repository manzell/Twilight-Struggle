using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class COMECON : Card
    {
        [SerializeField] List<Country> easternEurope = new List<Country>();

        public override void CardEvent(GameCommand command)
        {
            int count = 5;
            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in easternEurope)
                if (country.control != Game.Faction.USA)
                    eligibleCountries.Add(country);

            if (eligibleCountries.Count <= 5)
                foreach (Country country in eligibleCountries)
                    Game.AdjustInfluence(country, Game.Faction.USSR, 1);
            else
                UI.CountryClickHandler.Setup(eligibleCountries, onCountryClick);

            uiManager.SetButton(uiManager.primaryButton, "Finish Placing Ops", onFinish);

            void onFinish()
            {
                uiManager.UnsetButton(uiManager.primaryButton);
                UI.CountryClickHandler.Close();
                command.FinishCommand();
            }

            // TODO: Make this into generic calls to like "Add/Remove X influece to any/each of these countries"
            void onCountryClick(Country country)
            {
                Game.AdjustInfluence(country, faction, 1);
                eligibleCountries.Remove(country);
                UI.CountryClickHandler.Remove(country);

                count--;

                if (count == 0)
                    onFinish();
            }
        }
    }
}