using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TwilightStruggle
{
    public class TrumanDoctrine : Card
    {
        public override void CardEvent(GameCommand command)
        {
            List<Country> eligibleCountries = new List<Country>();

            foreach (Country country in FindObjectsOfType<Country>())
                if (country.continent == Country.Continent.Europe && country.control == Game.Faction.Neutral && country.influence[Game.Faction.USSR] > 0)
                    eligibleCountries.Add(country);

            if (eligibleCountries.Count > 1)
            {
                Message("Truman promises to support freedom throughout Europe");
                UI.CountryClickHandler.Setup(eligibleCountries, onCountryClick);
            }
            else
            {
                if (eligibleCountries.Count == 1)
                    Game.setInfluenceEvent.Invoke(eligibleCountries[0], Game.Faction.USSR, 0);

                command.FinishCommand();
            }

            void onCountryClick(Country country)
            {
                if (eligibleCountries.Contains(country))
                {
                    Game.setInfluenceEvent.Invoke(country, Game.Faction.USSR, 0);
                    UI.CountryClickHandler.Close();
                    command.FinishCommand();
                }
            }
        }
    }
}