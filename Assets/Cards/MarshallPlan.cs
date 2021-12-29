using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwilightStruggle
{
    public class MarshallPlan : Card
    {
        [SerializeField] List<Country> westernEurope = new List<Country>();
        [SerializeField] NATO NATO;

        public override void CardEvent(GameCommand command)
        {
            int count = 7;

            NATO.isPlayable = true;

            foreach (Country country in westernEurope)
                if (country.control == Game.Faction.USSR)
                    westernEurope.Remove(country);

            FindObjectOfType<UI.UIManager>().SetButton(FindObjectOfType<UI.UIManager>().primaryButton, "Finish Rebuilding Europe", Finish);

            if (westernEurope.Count <= count)
            {
                AddInfluence(faction, westernEurope, 1);
                Finish();
            }
            else
                UI.CountryClickHandler.Setup(westernEurope, onCountryClick, new Color(0f, .6f, .6f));

            void onCountryClick(Country country)
            {
                if (westernEurope.Contains(country))
                {
                    westernEurope.Remove(country);
                    UI.CountryClickHandler.Remove(country);
                    Game.adjustInfluenceEvent.Invoke(country, Game.Faction.USA, 1);
                    count--;
                }

                if (count == 0)
                    Finish();
            }

            void Finish()
            {
                UI.CountryClickHandler.Close();
                command.FinishCommand();
            }
        }
    }
}