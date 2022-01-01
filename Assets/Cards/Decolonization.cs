using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class Decolonization : Card
    {
        [SerializeField] List<Country> countries = new List<Country>();
        public int countryCount = 4;

        public override void CardEvent(GameCommand command)
        {
            int count = countryCount;

            UI.CountryClickHandler.Setup(countries, onCountryClick);

            Message($"Place {count} USSR Influence");

            void onCountryClick(Country country)
            {
                count--;

                if (countries.Contains(country))
                {
                    Message($"Place {count} USSR Influence");
                    Game.AdjustInfluence(country, Game.Faction.USSR, 1);
                    countries.Remove(country);
                    UI.CountryClickHandler.Remove(country);
                }
                if (count == 0)
                    command.FinishCommand();
            }
        }
    }
}