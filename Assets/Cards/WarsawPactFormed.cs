using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwilightStruggle
{
    public class WarsawPactFormed : Card
    {
        [SerializeField] List<Country> easternEurope = new List<Country>();
        [SerializeField] NATO NATO;

        public override void CardEvent(GameCommand command)
        {
            NATO.isPlayable = true;

            void RemoveAllUSInfluence()
            {
                int count = 4;

                foreach (Country country in easternEurope)
                    if (country.influence[Game.Faction.USA] == 0)
                        easternEurope.Remove(country);

                if (easternEurope.Count <= count)
                {
                    foreach (Country country in easternEurope)
                        Game.SetInfluence(country, Game.Faction.USA, 0);

                    Finish();
                }
                else
                {
                    UI.CountryClickHandler.Setup(easternEurope, onCountryClick);
                }

                void onCountryClick(Country country)
                {
                    Game.SetInfluence(country, Game.Faction.USA, 0);
                    UI.CountryClickHandler.Remove(country);
                    easternEurope.Remove(country);
                    count--;

                    if (count == 0)
                        Finish();
                }
            }

            void AddUSSRInfluence()
            {
                AddInfluence(easternEurope, Game.Faction.USSR, 5, 2, Finish);
            }

            void Finish()
            {
                UI.CountryClickHandler.Close();
                command.FinishCommand();
            }
        }
    }
}