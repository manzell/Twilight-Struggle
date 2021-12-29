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

            uiManager.SetButton(uiManager.confirmButton, "Expel Westerners", RemoveAllUSInfluence);
            uiManager.SetButton(uiManager.cancelButton, "Solidify the Eastern Bloc", AddUSSRInfluence);

            void RemoveAllUSInfluence()
            {
                uiManager.UnsetButtons();
                uiManager.SetButton(uiManager.primaryButton, "Finish purging reactionaries", Finish);
                int count = 4;

                foreach (Country country in easternEurope)
                    if (country.influence[Game.Faction.USA] == 0)
                        easternEurope.Remove(country);

                if (easternEurope.Count <= count)
                {
                    foreach (Country country in easternEurope)
                        Game.setInfluenceEvent.Invoke(country, Game.Faction.USA, 0);

                    Finish();
                }
                else
                {
                    UI.CountryClickHandler.Setup(easternEurope, onCountryClick);
                }

                void onCountryClick(Country country)
                {
                    Game.setInfluenceEvent.Invoke(country, Game.Faction.USA, 0);
                    UI.CountryClickHandler.Remove(country);
                    easternEurope.Remove(country);
                    count--;

                    if (count == 0)
                        Finish();
                }
            }

            void AddUSSRInfluence()
            {
                uiManager.UnsetButton(uiManager.confirmButton);
                uiManager.UnsetButton(uiManager.cancelButton);
                uiManager.SetButton(uiManager.primaryButton, "Finish deploying Influence", Finish);
                AddInfluence(easternEurope, Game.Faction.USSR, 5, 2, Finish);
            }

            void Finish()
            {
                UI.CountryClickHandler.Close();
                uiManager.UnsetButton(uiManager.primaryButton);
                uiManager.UnsetButton(uiManager.confirmButton);
                uiManager.UnsetButton(uiManager.cancelButton);
                command.FinishCommand();
            }
        }
    }
}